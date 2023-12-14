// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using App;
using QuestPDF.Fluent;
using QuestPDF.Previewer;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using App.ExamPage;
using Entity.Entities.Mains;


Console.WriteLine("Hello, World!");
ServiceProvider serviceProvider = new ServiceCollection()
    .AddAppServiceRegistration()
    .BuildServiceProvider();
Random r = new(1234);
for (int i = 0; i < 5; i++)
    Console.WriteLine(r.Next(0xFFFF).ToString("X3"));

QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;
IDocument document = Document.Create(document =>
{
    document.Page(page =>
    {
        page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
        page.Margin(0.5f, Unit.Centimetre);
        page.Size(pageSize: PageSizes.A4);
        page.Header().DebugArea($"Header", Colors.Black).Height(3, Unit.Centimetre).Row(headerRow =>
        {
            headerRow.RelativeItem(2).Padding(0.2f, Unit.Centimetre).Column(nametag =>
            {
                nametag.Item().DefaultTextStyle(ts => ts.FontSize(11));
                nametag.Item().AlignCenter().Text("Furkan");
                nametag.Item().AlignCenter().Text("Bozkurt");
                nametag.Item().AlignCenter().Text("1:D");
                nametag.Item().AlignCenter().Text("9999");
            });

            headerRow.RelativeItem(5).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
            {
                generalInfo.Item().DefaultTextStyle(ts => ts.FontSize(13));
                generalInfo.Item().AlignCenter().Text("2023 - 2024");
                generalInfo.Item().AlignCenter().Text("DEMODEMODEMODEMODEMO  ORTAOKULU");
                generalInfo.Item().AlignCenter().Text("DEMO BILIŞIM TEKNOLOJİLERI ve YAZILIM 159");
                generalInfo.Item().AlignCenter().Text("DEMO 1. dönem 1. yazılı sınavı").FontSize(12);
            });

            headerRow.RelativeItem(1.5f).Padding(0.2f, Unit.Centimetre).Column(examSummary =>
            {
                examSummary.Item().AlignCenter().Text("Code: F09").FontSize(10);
                examSummary.Item().AlignCenter().Text("Score").Underline().FontSize(14);
                examSummary.Item().AlignCenter().Text("100").FontSize(25);
            });
        });

        page.Content().PaddingTop(0.4f, Unit.Centimetre).PaddingBottom(0.4f, Unit.Centimetre)
        .Column(column =>
        {
            column.Spacing(0.4f, Unit.Centimetre);
            for (int i = 0; i < 10; i++)
                column.Item().DebugArea($"box {i}", Colors.Black).Height(2, Unit.Centimetre).Text($"{i + 1}) " + Placeholders.Question());
        });

        page.Footer().Height(1, Unit.Centimetre).Row(footerNotes =>
        {
            footerNotes.RelativeItem(16).DebugArea("Footer", Colors.Black).Text("this area other notes in the footer").FontSize(9);
            footerNotes.RelativeItem(1).DebugArea("c a", Colors.Black).AlignMiddle().AlignCenter().Text("FFF").FontSize(10);
        });
    });

    document.Page(page =>
    {
        page.Margin(0.5f, Unit.Centimetre);
        page.Size(pageSize: PageSizes.A4);

        page.Content().PaddingTop(0.4f, Unit.Centimetre).PaddingBottom(0.4f, Unit.Centimetre)
        .Column(column =>
        {
            column.Spacing(0.4f, Unit.Centimetre);
            for (int i = 10; i < 20; i++)
                column.Item().DebugArea($"box {i}", Colors.Black).Height(2, Unit.Centimetre).Text($"{i + 1}) " + Placeholders.Question());
        });

        page.Footer().Height(1, Unit.Centimetre).Row(footerNotes =>
        {
                footerNotes.RelativeItem(16).DebugArea("Footer", Colors.Black).Text("this area other notes in the footer").FontSize(9);
                footerNotes.RelativeItem(1).DebugArea("c a",Colors.Black).AlignMiddle().AlignCenter().Text("FFF").FontSize(10);
        });
    });
});

//document.ShowInPreviewer();



//Exam exam=new() 
//{


//};


//IPage page = serviceProvider.GetRequiredService<IPage>();
//IList<IDocument> documents = new List<IDocument>()
//{
//    page.FrontPageGenerate(),
//    page.BackPageGenerate()
//};

//Document.Merge(documents).ShowInPreviewer();