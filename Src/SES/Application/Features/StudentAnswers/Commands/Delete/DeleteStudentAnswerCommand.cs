using Application.Features.StudentAnswers.Constants;
using Application.Features.StudentAnswers.Constants;
using Application.Features.StudentAnswers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.StudentAnswers.Constants.StudentAnswersOperationClaims;

namespace Application.Features.StudentAnswers.Commands.Delete;

public class DeleteStudentAnswerCommand : IRequest<DeletedStudentAnswerResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write, StudentAnswersOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetStudentAnswers"];

    public class DeleteStudentAnswerCommandHandler : IRequestHandler<DeleteStudentAnswerCommand, DeletedStudentAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentAnswerRepository _studentAnswerRepository;
        private readonly StudentAnswerBusinessRules _studentAnswerBusinessRules;

        public DeleteStudentAnswerCommandHandler(IMapper mapper, IStudentAnswerRepository studentAnswerRepository,
                                         StudentAnswerBusinessRules studentAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentAnswerRepository = studentAnswerRepository;
            _studentAnswerBusinessRules = studentAnswerBusinessRules;
        }

        public async Task<DeletedStudentAnswerResponse> Handle(DeleteStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            StudentAnswer? studentAnswer = await _studentAnswerRepository.GetAsync(predicate: sa => sa.Id == request.Id, cancellationToken: cancellationToken);
            await _studentAnswerBusinessRules.StudentAnswerShouldExistWhenSelected(studentAnswer);

            await _studentAnswerRepository.DeleteAsync(studentAnswer!);

            DeletedStudentAnswerResponse response = _mapper.Map<DeletedStudentAnswerResponse>(studentAnswer);
            return response;
        }
    }
}