using Application.Features.Analyses.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Analyses.Queries.GetByIdAnalysisGetResponsiblePersonnel;

public class GetByIdAnalysisGetResponsiblePersonnelQuery : IRequest<GetByIdAnalysisGetResponsiblePersonnelResponse>
{
    public int Id { get; set; }

    public class GetByIdAnalysisGetResponsiblePersonnelQueryHandler : IRequestHandler<GetByIdAnalysisGetResponsiblePersonnelQuery, GetByIdAnalysisGetResponsiblePersonnelResponse>
    {
        private readonly IAnalysisRepository _analysisRepository;
        private readonly AnalysisBusinessRules _analysisBusinessRules;

        public GetByIdAnalysisGetResponsiblePersonnelQueryHandler(IAnalysisRepository analysisRepository, AnalysisBusinessRules analysisBusinessRules)
        {
            _analysisRepository = analysisRepository;
            _analysisBusinessRules = analysisBusinessRules;
        }

        public async Task<GetByIdAnalysisGetResponsiblePersonnelResponse> Handle(GetByIdAnalysisGetResponsiblePersonnelQuery request, CancellationToken cancellationToken)
        {
            await _analysisBusinessRules.AnalysisIdShouldExistWhenSelected(request.Id, cancellationToken);
            Analysis? analysis = await _analysisRepository.GetAsync(
                                                                    predicate: a => a.Id == request.Id,
                                                                    include: a => a.Include(a => a.Principal).ThenInclude(p => p.Personel)
                                                                                   .Include(a => a.Teachers)
                                                                                   .Include(a => a.Exams).ThenInclude(e => e.ExamAuthor)
                                                                                                         .ThenInclude(p => p.Personel)
                                                                                   .Include(a => a.StudentExamAnswers).ThenInclude(sea => sea.ReviewerTeacher)
                                                                                                                      .ThenInclude(p => p.Personel),
                                                                    cancellationToken: cancellationToken);

            await _analysisBusinessRules.AnalysisShouldExistWhenSelected(analysis);

            Personel principalPersonel = analysis!.Principal.Personel;
            string principalName =$"{principalPersonel.Name} {principalPersonel.SurName}";

            Personel examAuthor = analysis.Exams[0].ExamAuthor.Personel;
            string examAuthorName = $"{examAuthor.Name} {examAuthor.SurName}";

            string[] reviewingTeachersNames = [.. analysis.StudentExamAnswers
                .Select(sea => sea.ReviewerTeacher.Personel)
                .Distinct()
                .Select(pt => $"{pt.Name} {pt.SurName}")];

            return new ()
            {
                Principal = principalName,
                ExamAuthorTeacher = examAuthorName,
                ReviewingTeachersNames = reviewingTeachersNames
            };
        }
    }
}
