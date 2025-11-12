using Application.Features.StudentAnswers.Rules;
using Application.Services.Repositories;
using Application.Services.Teachers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.StudentAnswers.Commands.CreateMultiple;

public class CreateMultipleStudentAnswerCommand : IRequest<CreateMultipleStudentAnswerResponse>, ILoggableRequest, ITransactionalRequest
{
    public Guid ReviewerPersonelId { get; set; }
    public IList<MultipleStudentAnswer> StudentAnswers { get; set; }

    public class CreateMultipleStudentAnswerCommandHandler : IRequestHandler<CreateMultipleStudentAnswerCommand, CreateMultipleStudentAnswerResponse>
    {

        private readonly StudentAnswerBusinessRules _studentAnswerBusinessRules;
        private readonly IStudentAnswerRepository _studentAnswerRepository;
        private readonly ITeacherService _teacherService;
        private readonly IMapper _mapper;

        public CreateMultipleStudentAnswerCommandHandler(StudentAnswerBusinessRules studentAnswerBusinessRules, IStudentAnswerRepository studentAnswerRepository, ITeacherService teacherService, IMapper mapper)
        {
            _studentAnswerBusinessRules = studentAnswerBusinessRules;
            _studentAnswerRepository = studentAnswerRepository;
            _teacherService = teacherService;
            _mapper = mapper;
        }

        public async Task<CreateMultipleStudentAnswerResponse> Handle(CreateMultipleStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            await _studentAnswerBusinessRules.CheckIfStudentAnswersExistAsync(request.StudentAnswers);

            IList<StudentAnswer> studentAnswers = _mapper.Map<IList<StudentAnswer>>(request.StudentAnswers);

            Teacher? teacher = await _teacherService.GetAsync(
            predicate: t => t.PersonelId == request.ReviewerPersonelId,
            cancellationToken: cancellationToken);

            await _studentAnswerBusinessRules.CheckIfReviewerTeacherExistsAsync(teacher);

            // foreach (StudentAnswer studentAnswer in studentAnswers)
            //     studentAnswer.ReviewerTeacherId = teacher!.Id;

            await _studentAnswerRepository.AddRangeAsync(studentAnswers, cancellationToken);

            return new();
        }
    }
}
