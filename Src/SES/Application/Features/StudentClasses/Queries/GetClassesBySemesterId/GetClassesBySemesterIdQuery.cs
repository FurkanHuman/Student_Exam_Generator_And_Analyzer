using Application.Features.Semesters.Rules;
using AutoMapper;
using NArchitecture.Core.Application.Pipelines.Logging;
using MediatR;
using Domain.Entities;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentClasses.Queries.GetClassesBySemesterId;

public class GetClassesBySemesterIdQuery : IRequest<GetListResponse<GetClassesBySemesterIdResponse>>, ILoggableRequest
{
    public int SemesterId { get; set; }

    public class GetClassesBySemesterIdQueryHandler : IRequestHandler<GetClassesBySemesterIdQuery, GetListResponse< GetClassesBySemesterIdResponse>>
    {
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly IMapper _mapper;
        private readonly SemesterBusinessRules _semesterBusinessRules;

        public GetClassesBySemesterIdQueryHandler(IStudentClassRepository studentClassRepository, IMapper mapper, SemesterBusinessRules semesterBusinessRules)
        {
            _studentClassRepository = studentClassRepository;
            _mapper = mapper;
            _semesterBusinessRules = semesterBusinessRules;
        }

        public async Task<GetListResponse<GetClassesBySemesterIdResponse>> Handle(GetClassesBySemesterIdQuery request, CancellationToken cancellationToken)
        {
            await _semesterBusinessRules.SemesterIdShouldExistWhenSelected(request.SemesterId, cancellationToken);
            IPaginate<StudentClass> studentClassesList = await _studentClassRepository.GetListAsync(
                 predicate: s => s.SemesterId == request.SemesterId,
                 size: int.MaxValue,
                 cancellationToken: cancellationToken);

            GetListResponse<GetClassesBySemesterIdResponse> response = _mapper.Map<GetListResponse<GetClassesBySemesterIdResponse>>(studentClassesList);
            return response;
        }
    }
}
