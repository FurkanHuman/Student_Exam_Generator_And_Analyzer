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

namespace Application.Features.Semesters.Commands.Create;

public class CreateSemesterCommand : IRequest<CreatedSemesterResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string Name { get; set; }
    public required DateOnly BeginSemesterDate { get; set; }
    public required DateOnly EndSemesterDate { get; set; }

    public string[] Roles => [Admin, Write, SemestersOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetSemesters"];

    public class CreateSemesterCommandHandler : IRequestHandler<CreateSemesterCommand, CreatedSemesterResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISemesterRepository _semesterRepository;
        private readonly SemesterBusinessRules _semesterBusinessRules;

        public CreateSemesterCommandHandler(IMapper mapper, ISemesterRepository semesterRepository,
                                         SemesterBusinessRules semesterBusinessRules)
        {
            _mapper = mapper;
            _semesterRepository = semesterRepository;
            _semesterBusinessRules = semesterBusinessRules;
        }

        public async Task<CreatedSemesterResponse> Handle(CreateSemesterCommand request, CancellationToken cancellationToken)
        {
            Semester semester = _mapper.Map<Semester>(request);

            await _semesterRepository.AddAsync(semester);

            CreatedSemesterResponse response = _mapper.Map<CreatedSemesterResponse>(semester);
            return response;
        }
    }
}