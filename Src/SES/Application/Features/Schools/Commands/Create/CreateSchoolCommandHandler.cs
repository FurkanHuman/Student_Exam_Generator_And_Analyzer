using Application.Features.Schools.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Schools.Commands.Create;

public partial class CreateSchoolCommand
{
    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, CreatedSchoolResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolRepository _schoolRepository;
        private readonly SchoolBusinessRules _schoolBusinessRules;

        public CreateSchoolCommandHandler(IMapper mapper, ISchoolRepository schoolRepository,
                                         SchoolBusinessRules schoolBusinessRules)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _schoolBusinessRules = schoolBusinessRules;
        }

        public async Task<CreatedSchoolResponse> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            School school = _mapper.Map<School>(request);

            await _schoolRepository.AddAsync(school);

            CreatedSchoolResponse response = _mapper.Map<CreatedSchoolResponse>(school);
            return response;
        }
    }
}