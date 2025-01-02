using Application.Features.Schools.Constants;
using Application.Features.Schools.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Schools.Constants.SchoolsOperationClaims;

namespace Application.Features.Schools.Commands.Update;

public class UpdateSchoolCommand : IRequest<UpdatedSchoolResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Principal Principal { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetSchools"];

    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, UpdatedSchoolResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolRepository _schoolRepository;
        private readonly SchoolBusinessRules _schoolBusinessRules;

        public UpdateSchoolCommandHandler(IMapper mapper, ISchoolRepository schoolRepository,
                                         SchoolBusinessRules schoolBusinessRules)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _schoolBusinessRules = schoolBusinessRules;
        }

        public async Task<UpdatedSchoolResponse> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            School? school = await _schoolRepository.GetAsync(predicate: s => s.Id == request.Id, cancellationToken: cancellationToken);
            await _schoolBusinessRules.SchoolShouldExistWhenSelected(school);
            school = _mapper.Map(request, school);

            await _schoolRepository.UpdateAsync(school!);

            UpdatedSchoolResponse response = _mapper.Map<UpdatedSchoolResponse>(school);
            return response;
        }
    }
}