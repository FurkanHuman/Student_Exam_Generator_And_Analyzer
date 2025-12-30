using Application.Features.ExamConfigurations.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ExamConfigurations.Commands.Delete;

public class DeleteExamConfigurationCommand : IRequest<DeletedExamConfigurationResponse>
{
    public int Id { get; set; }

    public class DeleteExamConfigurationCommandHandler : IRequestHandler<DeleteExamConfigurationCommand, DeletedExamConfigurationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IExamConfigurationRepository _examConfigurationRepository;
        private readonly ExamConfigurationBusinessRules _examConfigurationBusinessRules;

        public DeleteExamConfigurationCommandHandler(IMapper mapper, IExamConfigurationRepository examConfigurationRepository,
                                         ExamConfigurationBusinessRules examConfigurationBusinessRules)
        {
            _mapper = mapper;
            _examConfigurationRepository = examConfigurationRepository;
            _examConfigurationBusinessRules = examConfigurationBusinessRules;
        }

        public async Task<DeletedExamConfigurationResponse> Handle(DeleteExamConfigurationCommand request, CancellationToken cancellationToken)
        {
            ExamConfiguration? examConfiguration = await _examConfigurationRepository.GetAsync(predicate: ec => ec.Id == request.Id, cancellationToken: cancellationToken);
            await _examConfigurationBusinessRules.ExamConfigurationShouldExistWhenSelected(examConfiguration);

            await _examConfigurationRepository.DeleteAsync(examConfiguration!);

            DeletedExamConfigurationResponse response = _mapper.Map<DeletedExamConfigurationResponse>(examConfiguration);
            return response;
        }
    }
}