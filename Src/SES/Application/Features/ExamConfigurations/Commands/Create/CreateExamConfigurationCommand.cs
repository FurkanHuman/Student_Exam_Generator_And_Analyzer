using Application.Features.ExamConfigurations.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ExamConfigurations.Commands.Create;

public class CreateExamConfigurationCommand : IRequest<CreatedExamConfigurationResponse>
{
    public required string ConfigurationHash { get; set; }

    public class CreateExamConfigurationCommandHandler : IRequestHandler<CreateExamConfigurationCommand, CreatedExamConfigurationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IExamConfigurationRepository _examConfigurationRepository;
        private readonly ExamConfigurationBusinessRules _examConfigurationBusinessRules;

        public CreateExamConfigurationCommandHandler(IMapper mapper, IExamConfigurationRepository examConfigurationRepository,
                                         ExamConfigurationBusinessRules examConfigurationBusinessRules)
        {
            _mapper = mapper;
            _examConfigurationRepository = examConfigurationRepository;
            _examConfigurationBusinessRules = examConfigurationBusinessRules;
        }

        public async Task<CreatedExamConfigurationResponse> Handle(CreateExamConfigurationCommand request, CancellationToken cancellationToken)
        {
            ExamConfiguration examConfiguration = _mapper.Map<ExamConfiguration>(request);

            await _examConfigurationRepository.AddAsync(examConfiguration);

            CreatedExamConfigurationResponse response = _mapper.Map<CreatedExamConfigurationResponse>(examConfiguration);
            return response;
        }
    }
}