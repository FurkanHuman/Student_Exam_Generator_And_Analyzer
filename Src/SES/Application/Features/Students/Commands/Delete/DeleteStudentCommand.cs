using Application.Features.Students.Constants;
using Application.Features.Students.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Students.Constants.StudentsOperationClaims;

namespace Application.Features.Students.Commands.Delete;

public class DeleteStudentCommand : IRequest<DeletedStudentResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public bool PermanentDelete { get; set; }


    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetStudents"];

    public class DeleteStudentCommandHandler(IMapper mapper, IStudentRepository studentRepository,
                                     StudentBusinessRules studentBusinessRules) : IRequestHandler<DeleteStudentCommand, DeletedStudentResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IStudentRepository _studentRepository = studentRepository;
        private readonly StudentBusinessRules _studentBusinessRules = studentBusinessRules;

        public async Task<DeletedStudentResponse> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            Student? student = await _studentRepository.GetAsync(predicate: s => s.Id == request.Id, cancellationToken: cancellationToken);
            await _studentBusinessRules.StudentShouldExistWhenSelected(student);

            Student? deleted = await _studentRepository.DeleteAsync(entity:student!, permanent:request.PermanentDelete, cancellationToken: cancellationToken);

            DeletedStudentResponse response = _mapper.Map<DeletedStudentResponse>(deleted);
            return response;
        }
    }
}