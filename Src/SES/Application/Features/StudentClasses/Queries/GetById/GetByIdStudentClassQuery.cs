using Application.Features.StudentClasses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentClasses.Queries.GetById;

public class GetByIdStudentClassQuery : IRequest<GetByIdStudentClassResponse>
{
    public int Id { get; set; }



    public class GetByIdStudentClassQueryHandler : IRequestHandler<GetByIdStudentClassQuery, GetByIdStudentClassResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly StudentClassBusinessRules _studentClassBusinessRules;

        public GetByIdStudentClassQueryHandler(IMapper mapper, IStudentClassRepository studentClassRepository, StudentClassBusinessRules studentClassBusinessRules)
        {
            _mapper = mapper;
            _studentClassRepository = studentClassRepository;
            _studentClassBusinessRules = studentClassBusinessRules;
        }

        public async Task<GetByIdStudentClassResponse> Handle(GetByIdStudentClassQuery request, CancellationToken cancellationToken)
        {
            StudentClass? studentClass = await _studentClassRepository.GetAsync(predicate: sc => sc.Id == request.Id, cancellationToken: cancellationToken);
            await _studentClassBusinessRules.StudentClassShouldExistWhenSelected(studentClass);

            GetByIdStudentClassResponse response = _mapper.Map<GetByIdStudentClassResponse>(studentClass);
            return response;
        }
    }
}