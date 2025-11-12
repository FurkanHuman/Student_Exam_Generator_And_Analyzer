using Application.Features.StudentExamAnswers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.StudentExamAnswers.Commands.Delete;

public class DeleteStudentExamAnswerCommand : IRequest<DeletedStudentExamAnswerResponse>, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public class DeleteStudentExamAnswerCommandHandler : IRequestHandler<DeleteStudentExamAnswerCommand, DeletedStudentExamAnswerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentExamAnswerRepository _studentExamAnswerRepository;
        private readonly StudentExamAnswerBusinessRules _studentExamAnswerBusinessRules;

        public DeleteStudentExamAnswerCommandHandler(IMapper mapper, IStudentExamAnswerRepository studentExamAnswerRepository,
                                         StudentExamAnswerBusinessRules studentExamAnswerBusinessRules)
        {
            _mapper = mapper;
            _studentExamAnswerRepository = studentExamAnswerRepository;
            _studentExamAnswerBusinessRules = studentExamAnswerBusinessRules;
        }

        public async Task<DeletedStudentExamAnswerResponse> Handle(DeleteStudentExamAnswerCommand request, CancellationToken cancellationToken)
        {
            StudentExamAnswer? studentExamAnswer = await _studentExamAnswerRepository.GetAsync(predicate: sea => sea.Id == request.Id, cancellationToken: cancellationToken);
            await _studentExamAnswerBusinessRules.StudentExamAnswerShouldExistWhenSelected(studentExamAnswer);

            await _studentExamAnswerRepository.DeleteAsync(studentExamAnswer!);

            DeletedStudentExamAnswerResponse response = _mapper.Map<DeletedStudentExamAnswerResponse>(studentExamAnswer);
            return response;
        }
    }
}