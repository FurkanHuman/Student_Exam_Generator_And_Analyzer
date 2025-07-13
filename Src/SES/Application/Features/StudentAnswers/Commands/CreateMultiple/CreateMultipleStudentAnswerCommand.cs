using Application.Features.StudentAnswers.Rules;
using AutoMapper;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using Application.Services.Repositories;
using Domain.Entities;

namespace Application.Features.StudentAnswers.Commands.CreateMultiple;

public class CreateMultipleStudentAnswerCommand : IRequest<CreateMultipleStudentAnswerResponse>, ILoggableRequest, ITransactionalRequest
{
    public IList<MultipleStudentAnswer> StudentAnswers { get; set; }

    public class CreateMultipleStudentAnswerCommandHandler : IRequestHandler<CreateMultipleStudentAnswerCommand, CreateMultipleStudentAnswerResponse>
    {

        private readonly StudentAnswerBusinessRules _studentAnswerBusinessRules;
        private readonly IStudentAnswerRepository _studentAnswerRepository;
        private readonly IMapper _mapper;

        public CreateMultipleStudentAnswerCommandHandler(StudentAnswerBusinessRules studentAnswerBusinessRules, IStudentAnswerRepository studentAnswerRepository, IMapper mapper)
        {
            _studentAnswerBusinessRules = studentAnswerBusinessRules;
            _studentAnswerRepository = studentAnswerRepository;
            _mapper = mapper;
        }

        public async Task<CreateMultipleStudentAnswerResponse> Handle(CreateMultipleStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            await _studentAnswerBusinessRules.CheckIfStudentAnswersExistAsync(request.StudentAnswers);

            IList<StudentAnswer> studentAnswers = _mapper.Map<IList<StudentAnswer>>(request.StudentAnswers);

            await _studentAnswerRepository.AddRangeAsync(studentAnswers,cancellationToken);

            return new();
        }
    }
}
