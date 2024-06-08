using Application.Features.Teachers.Constants;
using Application.Features.Teachers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Teachers.Constants.TeachersOperationClaims;

namespace Application.Features.Teachers.Commands.Create;

public class CreateTeacherCommand : IRequest<CreatedTeacherResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string Name { get; set; }
    public required string SurName { get; set; }
    public required int SemesterId { get; set; }
    public required int ExamId { get; set; }
    public required int StudentId { get; set; }
    public required int SchoolId { get; set; }
    public required int UserId { get; set; }
    public required User User { get; set; }
    public required School School { get; set; }
    public required Semester Semester { get; set; }

    public string[] Roles => [Admin, Write, TeachersOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetTeachers"];

    public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, CreatedTeacherResponse>
    {
        private readonly IMapper _mapper;
        private readonly ITeacherRepository _teacherRepository;
        private readonly TeacherBusinessRules _teacherBusinessRules;

        public CreateTeacherCommandHandler(IMapper mapper, ITeacherRepository teacherRepository,
                                         TeacherBusinessRules teacherBusinessRules)
        {
            _mapper = mapper;
            _teacherRepository = teacherRepository;
            _teacherBusinessRules = teacherBusinessRules;
        }

        public async Task<CreatedTeacherResponse> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            Teacher teacher = _mapper.Map<Teacher>(request);

            await _teacherRepository.AddAsync(teacher);

            CreatedTeacherResponse response = _mapper.Map<CreatedTeacherResponse>(teacher);
            return response;
        }
    }
}