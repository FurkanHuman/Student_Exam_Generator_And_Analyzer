// todo: the code here will be heavily refactored and the 'single responsibility principle' will be applied
using Application.Features.Exams.Rules;
using Application.Services.Lessons;
using Application.Services.PdfFactory.CreateExamPdf;
using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Application.Services.QuizQuestions;
using Application.Services.ReferenceBenefits;
using Application.Services.Repositories;
using Application.Services.Schools;
using Application.Services.Semesters;
using Application.Services.Students;
using Application.Services.Teachers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using System.IO.Compression;

namespace Application.Features.Exams.Commands.CreateMultipleExam;

public class CreateMultipleExamCommand : IRequest<CreateMultipleExamResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public ExamInfo ExamInfo { get; set; }
    public Guid ExamAuthorIdByPersonelId { get; set; }
    public int SemesterId { get; set; }
    public int LessonId { get; set; }
    public int[] ClassIds { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetExams"];


    public class CreateMultipleExamCommandHandler : IRequestHandler<CreateMultipleExamCommand, CreateMultipleExamResponse>
    {
        private readonly IMapper _mapper;
        private readonly IExamRepository _examRepository;
        private readonly IStudentService _studentService;
        private readonly ILessonService _lessonService;
        private readonly ISemesterService _semesterService;
        private readonly ISchoolService _schoolService;
        private readonly IReferenceBenefitService _referenceBenefitService;
        private readonly ITeacherService _teacherService;
        private readonly IQuizQuestionService _quizQuestionService;
        private readonly IExamPageGenerator _examPageGenerator;
        private readonly ExamBusinessRules _examBusinessRules;

        public CreateMultipleExamCommandHandler(IMapper mapper, IExamRepository examRepository, IStudentService studentService, ILessonService lessonService, ISemesterService semesterService, ISchoolService schoolService, IReferenceBenefitService referenceBenefitService, ITeacherService teacherService, IQuizQuestionService quizQuestionService, IExamPageGenerator examPageGenerator, ExamBusinessRules examBusinessRules)
        {
            _mapper = mapper;
            _examRepository = examRepository;
            _studentService = studentService;
            _lessonService = lessonService;
            _semesterService = semesterService;
            _schoolService = schoolService;
            _referenceBenefitService = referenceBenefitService;
            _teacherService = teacherService;
            _quizQuestionService = quizQuestionService;
            _examPageGenerator = examPageGenerator;
            _examBusinessRules = examBusinessRules;
        }

        public async Task<CreateMultipleExamResponse> Handle(CreateMultipleExamCommand request, CancellationToken cancellationToken)
        {
            IList<Exam> exams = [];
            IList<byte[]> pdfBytes = [];

            string examCode = QuizQuestionHelpers.GenerateBase32String();

            Lesson? lesson = await _lessonService.GetAsync(
                predicate: l => l.Id == request.LessonId,
                cancellationToken: cancellationToken);

            Semester? semester = await _semesterService.GetAsync(
                predicate: s => s.Id == request.SemesterId,
                cancellationToken: cancellationToken);

            Teacher? examAuthor = await _teacherService.GetAsync(
                predicate: t => t.PersonelId == request.ExamAuthorIdByPersonelId,
                include: t => t.Include(x => x.Personel),
                cancellationToken: cancellationToken);

            IPaginate<School>? schools = await _schoolService.GetListAsync(cancellationToken: cancellationToken);
            School school = schools!.Items.Last();

            IPaginate<ReferenceBenefit>? referenceBenefitList = await _referenceBenefitService.GetListAsync(cancellationToken: cancellationToken);
            ReferenceBenefit referenceBenefit = referenceBenefitList!.Items.First(rb => rb.Lesson == lesson && rb.School == school && rb.Semester == semester);

            IPaginate<Student>? students = await _studentService.GetListAsync(
                predicate: s => request.ClassIds.Contains(s.StudentClassId),
                size: int.MaxValue,
                index: 0,
                include: s => s.Include(x => x.StudentClass),
                cancellationToken: cancellationToken);

            int[] qqIds = request.ExamInfo.QQOrder.Keys.ToArray();
            IPaginate<QuizQuestion>? quizQuestions = await _quizQuestionService.GetListAsync(
                predicate: q => qqIds.Contains(q.Id),
                size: int.MaxValue,
                index: 0,
                include: q => q.Include(x => x.Options),
                cancellationToken: cancellationToken);

            _examBusinessRules.CheckStudentAvailability(students);

            foreach (Student student in students!.Items)
            {
                if (request.ExamInfo.IsRandomizeQuestions)
                    examCode = QuizQuestionHelpers.GenerateBase32String();

                Exam exam = new()
                {
                    ExamDate = request.ExamInfo.ExamScheduledDate,
                    ExamLessonName = request.ExamInfo.ExamName,
                    ExamCode = examCode,
                    FooterNote = request.ExamInfo.FooterNote,
                    TotalScoreForString = request.ExamInfo.ExamScoreStr,
                    TotalScore = request.ExamInfo.ExamScore,

                    StudentId = student.Id,
                    SemesterId = request.SemesterId,
                    LessonId = request.LessonId,
                    SchoolId = school.Id,
                    ReferenceBenefitId = referenceBenefit.Id,
                    ExamAuthorId = examAuthor!.Id,

                    Lesson = lesson!,
                    Semester = semester!,
                    Student = student!,
                    School = school!,
                    ReferenceBenefit = referenceBenefit!,
                    ExamAuthor = examAuthor!,

                    QuizQuestions = quizQuestions!.Items
                };
                exams.Add(exam);
            }

            ExamInfo examInfo = request.ExamInfo;

            using MemoryStream memoryStream = new();
            using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (Exam exam in exams)
                {
                    byte[] examPdf = _examPageGenerator.PageGenerate(exam, ref examInfo);
                    pdfBytes.Add(examPdf);

                    string pdfFileName = $"{exam.Student.SchoolNumber}_{exam.Student.StudentClass.ClassAge}-{exam.Student.StudentClass.ClassBranch}.pdf";
                    ZipArchiveEntry pdfEntry = archive.CreateEntry(pdfFileName);

                    using (Stream entryStream = pdfEntry.Open())
                    {
                        await entryStream.WriteAsync(examPdf, 0, examPdf.Length, cancellationToken);
                    }
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            byte[] zipFileBytes = memoryStream.ToArray();

            await _examRepository.AddRangeAsync(exams);

            CreateMultipleExamResponse response = new();
            response.FileName = $"{lesson!.LessonName}-{examInfo.SelectedClass}-s�n�flar-{examInfo.CurrentExamNumber}-yaz�l�.zip";
            response.ZipFileMemStream = new MemoryStream(zipFileBytes);
            return response;
        }
    }
}
