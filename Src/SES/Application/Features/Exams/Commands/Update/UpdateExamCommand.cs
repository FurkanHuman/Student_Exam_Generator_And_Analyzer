using Application.Features.Exams.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.Exams.Commands.Update;

public class UpdateExamCommand : IRequest<UpdatedExamResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string LessonName { get; set; }
    public required string ExamCode { get; set; }
    public required string FooterNote { get; set; }
    public int? TotalScore { get; set; }
    public string? TotalScoreForString { get; set; }
    public required int SemesterId { get; set; }
    public required int AnalysisId { get; set; }
    public required int TeacherId { get; set; }
    public required int StudentId { get; set; }
    public required int SchoolId { get; set; }
    public required int ReferenceBenefitId { get; set; }
    public required Semester Semester { get; set; }
    public required Teacher Teacher { get; set; }
    public required Student Student { get; set; }
    public required School School { get; set; }
    public required ReferenceBenefit ReferenceBenefit { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetExams"];

    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, UpdatedExamResponse>
    {
        private readonly IMapper _mapper;
        private readonly IExamRepository _examRepository;
        private readonly ExamBusinessRules _examBusinessRules;

        public UpdateExamCommandHandler(IMapper mapper, IExamRepository examRepository,
                                         ExamBusinessRules examBusinessRules)
        {
            _mapper = mapper;
            _examRepository = examRepository;
            _examBusinessRules = examBusinessRules;
        }

        public async Task<UpdatedExamResponse> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            Exam? exam = await _examRepository.GetAsync(predicate: e => e.Id == request.Id, cancellationToken: cancellationToken);
            await _examBusinessRules.ExamShouldExistWhenSelected(exam);
            exam = _mapper.Map(request, exam);

            await _examRepository.UpdateAsync(exam!);

            UpdatedExamResponse response = _mapper.Map<UpdatedExamResponse>(exam);
            return response;
        }
    }
}