using Application.Features.Exams.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using Domain.Enums;

namespace Application.Features.Exams.Queries.GetListExamByLessonId;

public class GetListExamByLessonIdQuery : IRequest<GetListResponse<GetListExamByLessonIdDto>>
{
    public int LessonId { get; set; }

    public class GetListExamByLessonIdQueryHandler : IRequestHandler<GetListExamByLessonIdQuery, GetListResponse<GetListExamByLessonIdDto>>
    {
        private readonly IMapper _mapper;
        private readonly IExamRepository _examRepository;
        private readonly ExamBusinessRules _examBusinessRules;

        public GetListExamByLessonIdQueryHandler(IMapper mapper, IExamRepository examRepository, ExamBusinessRules examBusinessRules)
        {
            _mapper = mapper;
            _examRepository = examRepository;
            _examBusinessRules = examBusinessRules;
        }

        public async Task<GetListResponse<GetListExamByLessonIdDto>> Handle(GetListExamByLessonIdQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Exam> examList = await _examRepository.GetListAsync(
                predicate: e => e.LessonId == request.LessonId && e.EvaluationOrigin == EvaluationOrigin.NotEvaluated,
                include: e => e.Include(e => e.Student)
                               .Include(e => e.QuizQuestions),
                index: 0,
                size: int.MaxValue,
                cancellationToken: cancellationToken);
            await _examBusinessRules.ExamListShouldExistWhenSelected(examList);

            GetListResponse<GetListExamByLessonIdDto> response = _mapper.Map<GetListResponse<GetListExamByLessonIdDto>>(examList);
            return response;
        }

    }
}
