using Application.Features.Teachers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.Teachers.Commands.Create;

public class CreateTeacherCommand : IRequest<CreatedTeacherResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int SemesterId { get; set; }

    public required Guid PersonelId { get; set; }
    public required int SchoolId { get; set; }




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