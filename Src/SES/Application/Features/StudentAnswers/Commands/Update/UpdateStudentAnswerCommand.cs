using Application.Features.StudentAnswers.Constants;
using Application.Features.StudentAnswers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.StudentAnswers.Constants.StudentAnswersOperationClaims;

namespace Application.Features.StudentAnswers.Commands.Update;

public class UpdateStudentAnswerCommand : IRequest<UpdatedStudentAnswerResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required int StudentId { get; set; }
    public required int QuizQuestionId { get; set; }
    public Guid? QuestionOptionId { get; set; }
    public string? AnswerText { get; set; }
    public required int Score { get; set; }
    public required bool IsCorrect { get; set; }

    public string[] Roles => [Admin, Write, StudentAnswersOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetStudentAnswers"];

    public class UpdateStudentAnswerCommandHandler : IRequestHandler<UpdateStudentAnswerCommand, UpdatedStudentAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentAnswerRepository _studentAnswerRepository;
        private readonly StudentAnswerBusinessRules _studentAnswerBusinessRules;

        public UpdateStudentAnswerCommandHandler(IMapper mapper, IStudentAnswerRepository studentAnswerRepository,
                                         StudentAnswerBusinessRules studentAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentAnswerRepository = studentAnswerRepository;
            _studentAnswerBusinessRules = studentAnswerBusinessRules;
        }

        public async Task<UpdatedStudentAnswerResponse> Handle(UpdateStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            StudentAnswer? studentAnswer = await _studentAnswerRepository.GetAsync(predicate: sa => sa.Id == request.Id, cancellationToken: cancellationToken);
            await _studentAnswerBusinessRules.StudentAnswerShouldExistWhenSelected(studentAnswer);
            studentAnswer = _mapper.Map(request, studentAnswer);

            await _studentAnswerRepository.UpdateAsync(studentAnswer!);

            UpdatedStudentAnswerResponse response = _mapper.Map<UpdatedStudentAnswerResponse>(studentAnswer);
            return response;
        }
    }
}