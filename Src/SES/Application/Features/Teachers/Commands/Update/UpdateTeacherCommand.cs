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

namespace Application.Features.Teachers.Commands.Update;

public class UpdateTeacherCommand : IRequest<UpdatedTeacherResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SurName { get; set; }
    public required int SemesterId { get; set; }
    public required int ExamId { get; set; }
    public required int StudentId { get; set; }
    public required int SchoolId { get; set; }
    public required School School { get; set; }
    public required Semester Semester { get; set; }

    public string[] Roles => [Admin, Write, TeachersOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetTeachers"];

    public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, UpdatedTeacherResponse>
    {
        private readonly IMapper _mapper;
        private readonly ITeacherRepository _teacherRepository;
        private readonly TeacherBusinessRules _teacherBusinessRules;

        public UpdateTeacherCommandHandler(IMapper mapper, ITeacherRepository teacherRepository,
                                         TeacherBusinessRules teacherBusinessRules)
        {
            _mapper = mapper;
            _teacherRepository = teacherRepository;
            _teacherBusinessRules = teacherBusinessRules;
        }

        public async Task<UpdatedTeacherResponse> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            Teacher? teacher = await _teacherRepository.GetAsync(predicate: t => t.Id == request.Id, cancellationToken: cancellationToken);
            await _teacherBusinessRules.TeacherShouldExistWhenSelected(teacher);
            teacher = _mapper.Map(request, teacher);

            await _teacherRepository.UpdateAsync(teacher!);

            UpdatedTeacherResponse response = _mapper.Map<UpdatedTeacherResponse>(teacher);
            return response;
        }
    }
}