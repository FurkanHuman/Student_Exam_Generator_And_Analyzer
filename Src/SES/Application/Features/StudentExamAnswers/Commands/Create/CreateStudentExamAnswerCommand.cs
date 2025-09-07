using Application.Features.StudentExamAnswers.Rules;
using Application.Services.Repositories;
using Application.Services.Teachers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.StudentExamAnswers.Commands.Create;

public class CreateStudentExamAnswerCommand : IRequest<CreatedStudentExamAnswerResponse>, ILoggableRequest, ITransactionalRequest
{
    public Guid ReviewerPersonelId { get; set; }
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public byte EvaluationOrigin { get; set; }
    public byte ExamEvaluationStatus { get; set; }
    public IList<StudentQuestionAnswerDto> StudentQuestionAnswers { get; set; } = [];

    public class CreateStudentExamAnswerCommandHandler : IRequestHandler<CreateStudentExamAnswerCommand, CreatedStudentExamAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentExamAnswerRepository _studentExamAnswerRepository;
        private readonly ITeacherService _teacherService;
        private readonly StudentExamAnswerBusinessRules _studentExamAnswerBusinessRules;

        public CreateStudentExamAnswerCommandHandler(IMapper mapper, IStudentExamAnswerRepository studentExamAnswerRepository, ITeacherService teacherService, StudentExamAnswerBusinessRules studentExamAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentExamAnswerRepository = studentExamAnswerRepository;
            _teacherService = teacherService;
            _studentExamAnswerBusinessRules = studentExamAnswerBusinessRules;
        }

        public async Task<CreatedStudentExamAnswerResponse> Handle(CreateStudentExamAnswerCommand request, CancellationToken cancellationToken)
        {
            Teacher? teacher = await _teacherService.GetAsync(
            predicate: t => t.PersonelId == request.ReviewerPersonelId,
            cancellationToken: cancellationToken);

            await _studentExamAnswerBusinessRules.CheckIfReviewerTeacherExistsAsync(teacher);

            StudentExamAnswer studentExamAnswer = _mapper.Map<StudentExamAnswer>(request);

            studentExamAnswer.ReviewerTeacherId = teacher!.Id;
            studentExamAnswer.ReviewerTeacher = teacher;

            await _studentExamAnswerRepository.AddAsync(studentExamAnswer);

            CreatedStudentExamAnswerResponse response = _mapper.Map<CreatedStudentExamAnswerResponse>(studentExamAnswer);
            return response;
        }
    }
}