using Application.Services.PdfFactory.CreateExamPdf.Constants;
using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

internal static class ExamPageHeader
{
    internal static PageDescriptor ExamHeader(this PageDescriptor page, Exam exam, ExamInfo examInfo)
    {
        ExamConstants.CurrentLanguage = examInfo.Language;

        page.Header().Height(3, Unit.Centimetre).Row(headerRow =>
        {
            headerRow.RelativeItem(2).Padding(0.2f, Unit.Centimetre).Column(nametag =>
            {
                nametag.Item().DefaultTextStyle(ts => ts.FontSize(9));

                nametag.Item().AlignCenter().Text(exam.Student.Name);
                nametag.Item().AlignCenter().Text(exam.Student.SurName);
                nametag.Item().AlignCenter().Text($"{exam.Student.StudentClass.ClassAge}/{exam.Student.StudentClass.ClassBranch} | {exam.Student.SchoolNumber}");
            });

            headerRow.RelativeItem(5).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
            {
                generalInfo.Item().DefaultTextStyle(ts => ts.FontSize(15));

                generalInfo.Item().AlignCenter().Text($"{exam.Semester.BeginSemesterDate.Year}/{exam.Semester.EndSemesterDate.Year}").FontSize(12).Bold();
                generalInfo.Item().AlignCenter().Text(exam.School.Name).Bold();
                generalInfo.Item().AlignCenter().Text($"{exam.Lesson.LessonName} {ExamConstants.Get("lesson")}").Bold();
                generalInfo.Item().AlignCenter().Text($"{exam.Student.StudentClass.ClassAge}. {ExamConstants.Get("clasess")} {examInfo.ExamTerm}. {ExamConstants.Get("period")} {examInfo.CurrentExamNumber}. {ExamConstants.Get("written_exam")}").Bold();
            });

            headerRow.RelativeItem(1.5f).Padding(0.2f, Unit.Centimetre).Column(examSummary =>
            {
                examSummary.Item().AlignCenter().Text(QuizQuestionHelpers.InsertDashInString(exam.ExamCode)).FontSize(9);
                examSummary.Item().AlignCenter().Text(ExamConstants.Get("score")).FontSize(14);
                examSummary.Item().AlignCenter().Border(1).Width(75).Height(25).Column(col =>
                {
                    col.Item().PaddingRight(5).PaddingTop(3.5F).AlignMiddle().AlignRight().Text($"/ {examInfo.ExamScore}").FontSize(15).Bold();
                });

            });
        });
        return page;
    }
}
