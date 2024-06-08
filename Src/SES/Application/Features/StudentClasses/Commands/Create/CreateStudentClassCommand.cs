using Application.Features.StudentClasses.Constants;
using Application.Features.StudentClasses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.StudentClasses.Constants.StudentClassesOperationClaims;

namespace Application.Features.StudentClasses.Commands.Create;

public class CreateStudentClassCommand : IRequest<CreatedStudentClassResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public string? Name { get; set; }
    public required int ClassAge { get; set; }
    public required char ClassBranch { get; set; }
    public string? Decription { get; set; }
    public required int SchoolId { get; set; }
    public required int SemesterId { get; set; }
    public required int RefTeacherId { get; set; }
    public required School School { get; set; }
    public required Semester Semester { get; set; }
    public required Teacher RefTeacher { get; set; }

    public string[] Roles => [Admin, Write, StudentClassesOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetStudentClasses"];

    public class CreateStudentClassCommandHandler : IRequestHandler<CreateStudentClassCommand, CreatedStudentClassResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly StudentClassBusinessRules _studentClassBusinessRules;

        public CreateStudentClassCommandHandler(IMapper mapper, IStudentClassRepository studentClassRepository,
                                         StudentClassBusinessRules studentClassBusinessRules)
        {
            _mapper = mapper;
            _studentClassRepository = studentClassRepository;
            _studentClassBusinessRules = studentClassBusinessRules;
        }

        public async Task<CreatedStudentClassResponse> Handle(CreateStudentClassCommand request, CancellationToken cancellationToken)
        {
            StudentClass studentClass = _mapper.Map<StudentClass>(request);

            await _studentClassRepository.AddAsync(studentClass);

            CreatedStudentClassResponse response = _mapper.Map<CreatedStudentClassResponse>(studentClass);
            return response;
        }
    }
}