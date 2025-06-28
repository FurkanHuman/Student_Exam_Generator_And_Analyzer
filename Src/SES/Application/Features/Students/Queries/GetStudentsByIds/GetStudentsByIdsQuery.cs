using Application.Features.Students.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Students.Queries.GetStudentsByIds;

public class GetStudentsByIdsQuery : IRequest<GetListResponse<GetStudentsByIdsListItemDto>>
{
    public IEnumerable<int> Ids { get; set; }
    public class GetStudentsByIdsQueryHandler : IRequestHandler<GetStudentsByIdsQuery, GetListResponse<GetStudentsByIdsListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly StudentBusinessRules _studentBusinessRules;

        public GetStudentsByIdsQueryHandler(IMapper mapper, IStudentRepository studentRepository, StudentBusinessRules studentBusinessRules)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _studentBusinessRules = studentBusinessRules;
        }

        public async Task<GetListResponse<GetStudentsByIdsListItemDto>> Handle(GetStudentsByIdsQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Student> students = await _studentRepository.GetListAsync(
                predicate: s => request.Ids.Contains(s.Id),
                size: int.MaxValue,
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            await _studentBusinessRules.StudentsShouldExistWhenSelected(students);

            GetListResponse<GetStudentsByIdsListItemDto> response = _mapper.Map<GetListResponse<GetStudentsByIdsListItemDto>>(students);
            return response;
        }
    }
}
