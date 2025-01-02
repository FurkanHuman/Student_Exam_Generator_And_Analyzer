using Application.Features.Semesters.Constants;
using Application.Features.Semesters.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Semesters.Constants.SemestersOperationClaims;

namespace Application.Features.Semesters.Commands.Update;

public class UpdateSemesterCommand : IRequest<UpdatedSemesterResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateOnly BeginSemesterDate { get; set; }
    public required DateOnly EndSemesterDate { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetSemesters"];

    public class UpdateSemesterCommandHandler : IRequestHandler<UpdateSemesterCommand, UpdatedSemesterResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISemesterRepository _semesterRepository;
        private readonly SemesterBusinessRules _semesterBusinessRules;

        public UpdateSemesterCommandHandler(IMapper mapper, ISemesterRepository semesterRepository,
                                         SemesterBusinessRules semesterBusinessRules)
        {
            _mapper = mapper;
            _semesterRepository = semesterRepository;
            _semesterBusinessRules = semesterBusinessRules;
        }

        public async Task<UpdatedSemesterResponse> Handle(UpdateSemesterCommand request, CancellationToken cancellationToken)
        {
            Semester? semester = await _semesterRepository.GetAsync(predicate: s => s.Id == request.Id, cancellationToken: cancellationToken);
            await _semesterBusinessRules.SemesterShouldExistWhenSelected(semester);
            semester = _mapper.Map(request, semester);

            await _semesterRepository.UpdateAsync(semester!);

            UpdatedSemesterResponse response = _mapper.Map<UpdatedSemesterResponse>(semester);
            return response;
        }
    }
}