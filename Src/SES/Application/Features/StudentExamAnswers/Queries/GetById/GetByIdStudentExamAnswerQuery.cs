using Application.Features.StudentExamAnswers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentExamAnswers.Queries.GetById;

public class GetByIdStudentExamAnswerQuery : IRequest<GetByIdStudentExamAnswerResponse>
{
    public Guid Id { get; set; }

    public class GetByIdStudentExamAnswerQueryHandler : IRequestHandler<GetByIdStudentExamAnswerQuery, GetByIdStudentExamAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentExamAnswerRepository _studentExamAnswerRepository;
        private readonly StudentExamAnswerBusinessRules _studentExamAnswerBusinessRules;

        public GetByIdStudentExamAnswerQueryHandler(IMapper mapper, IStudentExamAnswerRepository studentExamAnswerRepository, StudentExamAnswerBusinessRules studentExamAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentExamAnswerRepository = studentExamAnswerRepository;
            _studentExamAnswerBusinessRules = studentExamAnswerBusinessRules;
        }

        public async Task<GetByIdStudentExamAnswerResponse> Handle(GetByIdStudentExamAnswerQuery request, CancellationToken cancellationToken)
        {
            StudentExamAnswer? studentExamAnswer = await _studentExamAnswerRepository.GetAsync(predicate: sea => sea.Id == request.Id, cancellationToken: cancellationToken);
            await _studentExamAnswerBusinessRules.StudentExamAnswerShouldExistWhenSelected(studentExamAnswer);

            GetByIdStudentExamAnswerResponse response = _mapper.Map<GetByIdStudentExamAnswerResponse>(studentExamAnswer);
            return response;
        }
    }
}