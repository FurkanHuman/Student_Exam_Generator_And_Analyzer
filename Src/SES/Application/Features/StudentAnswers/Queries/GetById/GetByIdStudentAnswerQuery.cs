using Application.Features.StudentAnswers.Constants;
using Application.Features.StudentAnswers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.StudentAnswers.Constants.StudentAnswersOperationClaims;

namespace Application.Features.StudentAnswers.Queries.GetById;

public class GetByIdStudentAnswerQuery : IRequest<GetByIdStudentAnswerResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdStudentAnswerQueryHandler : IRequestHandler<GetByIdStudentAnswerQuery, GetByIdStudentAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentAnswerRepository _studentAnswerRepository;
        private readonly StudentAnswerBusinessRules _studentAnswerBusinessRules;

        public GetByIdStudentAnswerQueryHandler(IMapper mapper, IStudentAnswerRepository studentAnswerRepository, StudentAnswerBusinessRules studentAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentAnswerRepository = studentAnswerRepository;
            _studentAnswerBusinessRules = studentAnswerBusinessRules;
        }

        public async Task<GetByIdStudentAnswerResponse> Handle(GetByIdStudentAnswerQuery request, CancellationToken cancellationToken)
        {
            StudentAnswer? studentAnswer = await _studentAnswerRepository.GetAsync(predicate: sa => sa.Id == request.Id, cancellationToken: cancellationToken);
            await _studentAnswerBusinessRules.StudentAnswerShouldExistWhenSelected(studentAnswer);

            GetByIdStudentAnswerResponse response = _mapper.Map<GetByIdStudentAnswerResponse>(studentAnswer);
            return response;
        }
    }
}