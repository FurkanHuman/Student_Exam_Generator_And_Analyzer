using Application.Features.ExamConfigurations.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ExamConfigurations.Commands.Update;

public class UpdateExamConfigurationCommand : IRequest<UpdatedExamConfigurationResponse>
{
    public int Id { get; set; }
    public required string ConfigurationHash { get; set; }

    public class UpdateExamConfigurationCommandHandler : IRequestHandler<UpdateExamConfigurationCommand, UpdatedExamConfigurationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IExamConfigurationRepository _examConfigurationRepository;
        private readonly ExamConfigurationBusinessRules _examConfigurationBusinessRules;

        public UpdateExamConfigurationCommandHandler(IMapper mapper, IExamConfigurationRepository examConfigurationRepository,
                                         ExamConfigurationBusinessRules examConfigurationBusinessRules)
        {
            _mapper = mapper;
            _examConfigurationRepository = examConfigurationRepository;
            _examConfigurationBusinessRules = examConfigurationBusinessRules;
        }

        public async Task<UpdatedExamConfigurationResponse> Handle(UpdateExamConfigurationCommand request, CancellationToken cancellationToken)
        {
            ExamConfiguration? examConfiguration = await _examConfigurationRepository.GetAsync(predicate: ec => ec.Id == request.Id, cancellationToken: cancellationToken);
            await _examConfigurationBusinessRules.ExamConfigurationShouldExistWhenSelected(examConfiguration);
            examConfiguration = _mapper.Map(request, examConfiguration);

            await _examConfigurationRepository.UpdateAsync(examConfiguration!);

            UpdatedExamConfigurationResponse response = _mapper.Map<UpdatedExamConfigurationResponse>(examConfiguration);
            return response;
        }
    }
}