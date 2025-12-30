using Application.Features.ExamConfigurations.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ExamConfigurations.Queries.GetById;

public class GetByIdExamConfigurationQuery : IRequest<GetByIdExamConfigurationResponse>
{
    public int Id { get; set; }

    public class GetByIdExamConfigurationQueryHandler : IRequestHandler<GetByIdExamConfigurationQuery, GetByIdExamConfigurationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IExamConfigurationRepository _examConfigurationRepository;
        private readonly ExamConfigurationBusinessRules _examConfigurationBusinessRules;

        public GetByIdExamConfigurationQueryHandler(IMapper mapper, IExamConfigurationRepository examConfigurationRepository, ExamConfigurationBusinessRules examConfigurationBusinessRules)
        {
            _mapper = mapper;
            _examConfigurationRepository = examConfigurationRepository;
            _examConfigurationBusinessRules = examConfigurationBusinessRules;
        }

        public async Task<GetByIdExamConfigurationResponse> Handle(GetByIdExamConfigurationQuery request, CancellationToken cancellationToken)
        {
            ExamConfiguration? examConfiguration = await _examConfigurationRepository.GetAsync(predicate: ec => ec.Id == request.Id, cancellationToken: cancellationToken);
            await _examConfigurationBusinessRules.ExamConfigurationShouldExistWhenSelected(examConfiguration);

            GetByIdExamConfigurationResponse response = _mapper.Map<GetByIdExamConfigurationResponse>(examConfiguration);
            return response;
        }
    }
}