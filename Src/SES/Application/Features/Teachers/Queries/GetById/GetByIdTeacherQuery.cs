using Application.Features.Teachers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Teachers.Constants.TeachersOperationClaims;

namespace Application.Features.Teachers.Queries.GetById;

public class GetByIdTeacherQuery : IRequest<GetByIdTeacherResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdTeacherQueryHandler : IRequestHandler<GetByIdTeacherQuery, GetByIdTeacherResponse>
    {
        private readonly IMapper _mapper;
        private readonly ITeacherRepository _teacherRepository;
        private readonly TeacherBusinessRules _teacherBusinessRules;

        public GetByIdTeacherQueryHandler(IMapper mapper, ITeacherRepository teacherRepository, TeacherBusinessRules teacherBusinessRules)
        {
            _mapper = mapper;
            _teacherRepository = teacherRepository;
            _teacherBusinessRules = teacherBusinessRules;
        }

        public async Task<GetByIdTeacherResponse> Handle(GetByIdTeacherQuery request, CancellationToken cancellationToken)
        {
            Teacher? teacher = await _teacherRepository.GetAsync(predicate: t => t.Id == request.Id, cancellationToken: cancellationToken);
            await _teacherBusinessRules.TeacherShouldExistWhenSelected(teacher);

            GetByIdTeacherResponse response = _mapper.Map<GetByIdTeacherResponse>(teacher);
            return response;
        }
    }
}