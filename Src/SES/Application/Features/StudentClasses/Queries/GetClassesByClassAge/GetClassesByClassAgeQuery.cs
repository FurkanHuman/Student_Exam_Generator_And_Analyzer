using Application.Features.StudentClasses.Rules;
using AutoMapper;
using NArchitecture.Core.Application.Pipelines.Logging;
using MediatR;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentClasses.Queries.GetClassesByClassAge;

public class GetClassesByClassAgeQuery : IRequest<GetListResponse<GetClassesByClassAgeResponse>>, ILoggableRequest
{
    public int ClassAge { get; set; }
    public int SemesterId { get; set; }

    public class GetClassesByClassAgeQueryHandler : IRequestHandler<GetClassesByClassAgeQuery, GetListResponse<GetClassesByClassAgeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly StudentClassBusinessRules _studentClassBusinessRules;

        public GetClassesByClassAgeQueryHandler(IMapper mapper, IStudentClassRepository studentClassRepository, StudentClassBusinessRules studentClassBusinessRules)
        {
            _mapper = mapper;
            _studentClassRepository = studentClassRepository;
            _studentClassBusinessRules = studentClassBusinessRules;
        }

        public async Task<GetListResponse<GetClassesByClassAgeResponse>> Handle(GetClassesByClassAgeQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Domain.Entities.StudentClass> studentClasses = await _studentClassRepository.GetListAsync(
                predicate: sc => sc.ClassAge == request.ClassAge && sc.SemesterId == request.SemesterId,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetClassesByClassAgeResponse> response = _mapper.Map<GetListResponse<GetClassesByClassAgeResponse>>(studentClasses);
            return response;
        }
    }
}