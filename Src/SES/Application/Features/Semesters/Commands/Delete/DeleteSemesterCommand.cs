using Application.Features.Semesters.Constants;
using Application.Features.Semesters.Constants;
using Application.Features.Semesters.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Semesters.Constants.SemestersOperationClaims;

namespace Application.Features.Semesters.Commands.Delete;

public class DeleteSemesterCommand : IRequest<DeletedSemesterResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Write, SemestersOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetSemesters"];

    public class DeleteSemesterCommandHandler : IRequestHandler<DeleteSemesterCommand, DeletedSemesterResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISemesterRepository _semesterRepository;
        private readonly SemesterBusinessRules _semesterBusinessRules;

        public DeleteSemesterCommandHandler(IMapper mapper, ISemesterRepository semesterRepository,
                                         SemesterBusinessRules semesterBusinessRules)
        {
            _mapper = mapper;
            _semesterRepository = semesterRepository;
            _semesterBusinessRules = semesterBusinessRules;
        }

        public async Task<DeletedSemesterResponse> Handle(DeleteSemesterCommand request, CancellationToken cancellationToken)
        {
            Semester? semester = await _semesterRepository.GetAsync(predicate: s => s.Id == request.Id, cancellationToken: cancellationToken);
            await _semesterBusinessRules.SemesterShouldExistWhenSelected(semester);

            await _semesterRepository.DeleteAsync(semester!);

            DeletedSemesterResponse response = _mapper.Map<DeletedSemesterResponse>(semester);
            return response;
        }
    }
}