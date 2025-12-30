using Application.Services.PdfFactory.CreateExamPdf.Constants;
using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

internal static class ExamPageHeader
{
    private const int ShowPageNumber = 1;
    internal static void ExamHeader(this PageDescriptor page, Exam exam, ExamInfo examInfo)
    {
        ExamConstants.CurrentLanguage = examInfo.Language;

        page.Header().ShowIf(h => h.PageNumber == ShowPageNumber).PaddingBottom(0.5F, Unit.Centimetre).Row(headerRow =>
        {
            headerRow.Spacing(1.15f, Unit.Point);
            headerRow.RelativeItem(1).ShowIf(!examInfo.IsGhostExam).AlignLeft().AlignCenter().AlignTop().Column(nameBox =>
            {
                if (!examInfo.IsAnonymousExamMode)
                {
                    nameBox.Item().AlignMiddle().Border(0, Unit.Point).Height(55).Column(col =>
                    {
                        col.Item().AlignLeft().Text(exam.Student.Name).Bold();
                        col.Item().AlignLeft().Text(exam.Student.SurName).Bold();
                        col.Item().AlignLeft().Text($"{exam.Student.StudentClass.ClassAge}/{exam.Student.StudentClass.ClassBranch}       {exam.Student.SchoolNumber}").Bold();
                    });
                }

                else
                {
                    nameBox.Item().AlignMiddle().Border(0, Unit.Point).Height(55).Column(col =>
                    {
                        col.Item().AlignLeft().Text($"{ExamConstants.Get("name")}:").Bold();
                        col.Item().AlignLeft().Text($"{ExamConstants.Get("surname")}:").Bold();
                        col.Item().AlignLeft().Row(iRow =>
                        {
                            iRow.RelativeItem().AlignLeft().Text($"{ExamConstants.Get("class_branch")}:").Bold();
                            iRow.RelativeItem().AlignLeft().Text($"{ExamConstants.Get("number")}:").Bold();

                        });
                    });
                }
            });

            headerRow.RelativeItem(2).AlignCenter().AlignTop().Column(generalInfo =>
            {
                generalInfo.Item().AlignCenter().Text($"{exam.Semester.BeginSemesterDate.Year} - {exam.Semester.EndSemesterDate.Year}").Bold();
                generalInfo.Item().AlignCenter().Text(exam.School.Name).Bold();
                generalInfo.Item().AlignCenter().Text($"{exam.Lesson.LessonName} {ExamConstants.Get("lesson")}").Bold();
                generalInfo.Item().AlignCenter().Text($"{examInfo.SelectedClass}. {ExamConstants.Get("clasess")} {exam.Semester.Name}. {ExamConstants.Get("period")} {examInfo.CurrentExamNumber}. {ExamConstants.Get("written_exam")}").Bold();
            });

            headerRow.RelativeItem(1).AlignRight().AlignTop().Column(examSummary =>
            {
                examSummary.Item().AlignCenter().Text(QuizQuestionHelpers.InsertDashInString(exam.ExamTrackingCode)).Bold();
                examSummary.Item().AlignCenter().Text(exam.ExamDate.ToShortDateString()).Bold();
                examSummary.Item().AlignCenter().Text(ExamConstants.Get("score")).Bold();
                examSummary.Item().AlignCenter().Width(2, Unit.Centimetre).Column(col =>
                {
                    col.Item().AlignMiddle().AlignRight().Text($"/ {examInfo.ExamScore}").Bold();
                    col.Item().AlignMiddle().AlignRight().Text($"/ {examInfo.ExamScoreStr}").Bold();
                });
            });
        });
    }
}
