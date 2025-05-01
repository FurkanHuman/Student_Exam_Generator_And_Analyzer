using Application.Features.ReferenceBenefits.Constants;
using Application.Features.ReferenceBenefits.Rules;
using Application.Services.Benefits;
using Application.Services.LearningAreas;
using Application.Services.PdfFactory.PdfReaderService.Dtos;
using Application.Services.Repositories;
using Application.Services.SubLearningAreas;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.ReferenceBenefits.Constants.ReferenceBenefitsOperationClaims;

namespace Application.Features.ReferenceBenefits.Commands.Create;

public class CreateReferenceBenefitCommand : IRequest<CreatedReferenceBenefitResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required ReferenceBenefitDto ReferenceBenefitDto { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetReferenceBenefits"];

    public class CreateReferenceBenefitCommandHandler : IRequestHandler<CreateReferenceBenefitCommand, CreatedReferenceBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReferenceBenefitRepository _referenceBenefitRepository;
        private readonly ReferenceBenefitBusinessRules _referenceBenefitBusinessRules;

        public CreateReferenceBenefitCommandHandler(IMapper mapper, IReferenceBenefitRepository referenceBenefitRepository, ReferenceBenefitBusinessRules referenceBenefitBusinessRules)
        {
            _mapper = mapper;
            _referenceBenefitRepository = referenceBenefitRepository;
            _referenceBenefitBusinessRules = referenceBenefitBusinessRules;
        }

        public async Task<CreatedReferenceBenefitResponse> Handle(CreateReferenceBenefitCommand request, CancellationToken cancellationToken)
        {
            ReferenceBenefit referenceBenefit = _mapper.Map<ReferenceBenefit>(request.ReferenceBenefitDto);

            ReferenceBenefit createdReferenceBenefit = await _referenceBenefitRepository.AddAsync(referenceBenefit);

            CreatedReferenceBenefitResponse response = _mapper.Map<CreatedReferenceBenefitResponse>(createdReferenceBenefit);
            return response;
        }
    }
}