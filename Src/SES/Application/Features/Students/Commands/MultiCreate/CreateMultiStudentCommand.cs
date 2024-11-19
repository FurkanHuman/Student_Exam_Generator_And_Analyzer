using Application.Features.Students.Constants;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Students.Constants.StudentsOperationClaims;

namespace Application.Features.Students.Commands.MultiCreate;
public class CreateMultiStudentCommand : IRequest<ICollection<CreatedMultiStudentResponse>>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int SchoolId { get; set; }
    public required int SemesterId { get; set; }
    public required int RefTeacherId { get; set; }
    public required byte[] PdfFile { get; set; }

    public string[] Roles => [Admin, Write, StudentsOperationClaims.Create];

    public bool BypassCache { get; }

    public string? CacheKey { get; }

    public string[]? CacheGroupKey => ["GetStudents"];

}
