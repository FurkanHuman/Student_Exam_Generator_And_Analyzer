using Application.Features.StudentClasses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.StudentClasses.Commands.Update;

public class UpdateStudentClassCommand : IRequest<UpdatedStudentClassResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
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



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetStudentClasses"];

    public class UpdateStudentClassCommandHandler : IRequestHandler<UpdateStudentClassCommand, UpdatedStudentClassResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly StudentClassBusinessRules _studentClassBusinessRules;

        public UpdateStudentClassCommandHandler(IMapper mapper, IStudentClassRepository studentClassRepository,
                                         StudentClassBusinessRules studentClassBusinessRules)
        {
            _mapper = mapper;
            _studentClassRepository = studentClassRepository;
            _studentClassBusinessRules = studentClassBusinessRules;
        }

        public async Task<UpdatedStudentClassResponse> Handle(UpdateStudentClassCommand request, CancellationToken cancellationToken)
        {
            StudentClass? studentClass = await _studentClassRepository.GetAsync(predicate: sc => sc.Id == request.Id, cancellationToken: cancellationToken);
            await _studentClassBusinessRules.StudentClassShouldExistWhenSelected(studentClass);
            studentClass = _mapper.Map(request, studentClass);

            await _studentClassRepository.UpdateAsync(studentClass!);

            UpdatedStudentClassResponse response = _mapper.Map<UpdatedStudentClassResponse>(studentClass);
            return response;
        }
    }
}