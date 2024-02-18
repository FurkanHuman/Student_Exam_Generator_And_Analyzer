using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.AnalysisPageFeature.V1;

public class AnalsysPageWrittenV1 : IAnalsysPage
{
    public IDocument PageGenerate(AnalysisHeader analysisHeader)
    {        
        char[] CrayzChars = ['k', 'g'];

        return Document.Create(doc =>
        doc.Page(page =>
        {
            page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
            page.Margin(0.5f, Unit.Centimetre);
            page.Size(pageSize: PageSizes.A4);

            page.Header().Height(3.5f, Unit.Centimetre).Row(headerRow =>
            {
                AnalsysPageV1Header.SchoolAndClassInfo(analysisHeader, headerRow);
            });

            page.Content().Row(cRow =>
            {
                cRow.RelativeItem().Column(generalClassInfoForStudent =>
                {
                    AnalsysPageV1InfoTables.StudentPreTop(analysisHeader, generalClassInfoForStudent);
                    AnalsysPageV1InfoTables.TopInfo(generalClassInfoForStudent, analysisHeader.RefScorePerQuestions);
                    AnalsysPageV1InfoTables.DrawStudensAndNotes(analysisHeader, generalClassInfoForStudent);
                    AnalsysPageV1InfoTables.ClassScoreSummary(generalClassInfoForStudent, analysisHeader, CrayzChars);
                    
                    generalClassInfoForStudent.Item().PageBreak();

                    AnalsysPageV1GraphicalAnalsys.DravClassgraphicalNoteAnalsys(generalClassInfoForStudent, analysisHeader, CrayzChars);
                    AnalsysPageV1ReferenceTable.RefTable(analysisHeader, generalClassInfoForStudent, CrayzChars);
                });
            });

            page.Footer().Column(fC =>
            {
                AnalsysPageV1Footer.FooterTeacherAndPrincipal(analysisHeader, fC);
                fC.Item().AlignBottom().AlignCenter().Text(analysisHeader.FooterNote).FontSize(9).FontFamily(Fonts.Arial);
            });
        }));
    }
}
