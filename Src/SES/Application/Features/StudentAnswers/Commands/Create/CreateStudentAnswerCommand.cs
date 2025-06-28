using Application.Features.StudentAnswers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.StudentAnswers.Commands.Create;

public class CreateStudentAnswerCommand : IRequest<CreatedStudentAnswerResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int StudentId { get; set; }
    public required int QuizQuestionId { get; set; }
    public Guid? QuestionOptionId { get; set; }
    public string? AnswerText { get; set; }
    public required int Score { get; set; }
    public required bool IsCorrect { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetStudentAnswers"];

    public class CreateStudentAnswerCommandHandler : IRequestHandler<CreateStudentAnswerCommand, CreatedStudentAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentAnswerRepository _studentAnswerRepository;
        private readonly StudentAnswerBusinessRules _studentAnswerBusinessRules;

        public CreateStudentAnswerCommandHandler(IMapper mapper, IStudentAnswerRepository studentAnswerRepository,
                                         StudentAnswerBusinessRules studentAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentAnswerRepository = studentAnswerRepository;
            _studentAnswerBusinessRules = studentAnswerBusinessRules;
        }

        public async Task<CreatedStudentAnswerResponse> Handle(CreateStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            StudentAnswer studentAnswer = _mapper.Map<StudentAnswer>(request);

            await _studentAnswerRepository.AddAsync(studentAnswer);

            CreatedStudentAnswerResponse response = _mapper.Map<CreatedStudentAnswerResponse>(studentAnswer);
            return response;
        }
    }
}