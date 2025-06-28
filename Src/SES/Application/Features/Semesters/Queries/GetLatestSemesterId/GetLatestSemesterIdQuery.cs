using Application.Features.Semesters.Rules;
using NArchitecture.Core.Application.Pipelines.Caching;
using MediatR;
using Application.Services.Repositories;
using Domain.Entities;

namespace Application.Features.Semesters.Queries.GetLatestSemesterId;

public class GetLatestSemesterIdQuery : IRequest<int>, ICachableRequest
{
    public bool BypassCache { get; }
    public string? CacheKey => $"GetLatestSemesterId";
    public string? CacheGroupKey => "GetSemesters";
    public TimeSpan? SlidingExpiration { get; }

    public class GetLatestSemesterIdQueryHandler : IRequestHandler<GetLatestSemesterIdQuery, int>
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly SemesterBusinessRules _semesterBusinessRules;

        public GetLatestSemesterIdQueryHandler(ISemesterRepository semesterRepository, SemesterBusinessRules semesterBusinessRules)
        {
            _semesterRepository = semesterRepository;
            _semesterBusinessRules = semesterBusinessRules;
        }

        public async Task<int> Handle(GetLatestSemesterIdQuery request, CancellationToken cancellationToken)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            // it only works within the time zone
            Semester? _latesSemester = await _semesterRepository.GetAsync(
                predicate: s => s.BeginSemesterDate <= currentDate && s.EndSemesterDate >= currentDate,
                enableTracking: false,
                cancellationToken: cancellationToken
            );

            await _semesterBusinessRules.SemesterShouldExistWhenSelected(_latesSemester);

            return _latesSemester!.Id;
        }
    }
}
