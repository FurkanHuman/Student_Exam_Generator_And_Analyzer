using Application.Features.Exams.Rules;
using AutoMapper;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;

namespace Application.Features.Exams.Commands.CreateMultipleExam;

public class CreateMultipleExamCommand : IRequest<CreateMultipleExamResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetExams"];
    
    public class CreateMultipleExamCommandHandler : IRequestHandler<CreateMultipleExamCommand, CreateMultipleExamResponse>
    {
        private readonly IMapper _mapper;
        private readonly ExamBusinessRules _examBusinessRules;

        public CreateMultipleExamCommandHandler(IMapper mapper, ExamBusinessRules examBusinessRules)
        {
            _mapper = mapper;
            _examBusinessRules = examBusinessRules;
        }

        public async Task<CreateMultipleExamResponse> Handle(CreateMultipleExamCommand request, CancellationToken cancellationToken)
        {
            CreateMultipleExamResponse response = _mapper.Map<CreateMultipleExamResponse>(null);
            return response;
        }
    }
}
