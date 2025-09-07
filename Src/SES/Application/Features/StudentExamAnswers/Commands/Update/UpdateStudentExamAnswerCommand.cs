using Application.Features.StudentExamAnswers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using Domain.Enums;
using Domain.Enums;

namespace Application.Features.StudentExamAnswers.Commands.Update;

public class UpdateStudentExamAnswerCommand : IRequest<UpdatedStudentExamAnswerResponse>, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required int ReviewerTeacherId { get; set; }
    public required int StudentId { get; set; }
    public required int ExamId { get; set; }
    public required EvaluationOrigin EvaluationOrigin { get; set; }
    public required ExamEvaluationStatus ExamEvaluationStatus { get; set; }

    public class UpdateStudentExamAnswerCommandHandler : IRequestHandler<UpdateStudentExamAnswerCommand, UpdatedStudentExamAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentExamAnswerRepository _studentExamAnswerRepository;
        private readonly StudentExamAnswerBusinessRules _studentExamAnswerBusinessRules;

        public UpdateStudentExamAnswerCommandHandler(IMapper mapper, IStudentExamAnswerRepository studentExamAnswerRepository,
                                         StudentExamAnswerBusinessRules studentExamAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentExamAnswerRepository = studentExamAnswerRepository;
            _studentExamAnswerBusinessRules = studentExamAnswerBusinessRules;
        }

        public async Task<UpdatedStudentExamAnswerResponse> Handle(UpdateStudentExamAnswerCommand request, CancellationToken cancellationToken)
        {
            StudentExamAnswer? studentExamAnswer = await _studentExamAnswerRepository.GetAsync(predicate: sea => sea.Id == request.Id, cancellationToken: cancellationToken);
            await _studentExamAnswerBusinessRules.StudentExamAnswerShouldExistWhenSelected(studentExamAnswer);
            studentExamAnswer = _mapper.Map(request, studentExamAnswer);

            await _studentExamAnswerRepository.UpdateAsync(studentExamAnswer!);

            UpdatedStudentExamAnswerResponse response = _mapper.Map<UpdatedStudentExamAnswerResponse>(studentExamAnswer);
            return response;
        }
    }
}