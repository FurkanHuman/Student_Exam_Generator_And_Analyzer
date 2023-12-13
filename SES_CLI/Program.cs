// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using App;
using QuestPDF.Fluent;
using QuestPDF.Previewer;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


Console.WriteLine("Hello, World!");
ServiceProvider serviceProvider = new ServiceCollection()
    .AddAppServiceRegistration()
    .BuildServiceProvider();
Document.Create(document =>
{
    document.Page(page =>
    {
        page.Margin(0.5f, Unit.Centimetre);
        page.Size(pageSize: PageSizes.A4);
        page.Header().Background(Colors.Cyan.Darken2).Height(3, Unit.Centimetre).Row(headerRow =>
        {
            headerRow.RelativeItem(2).Padding(0.2f, Unit.Centimetre).Background(Colors.Brown.Darken2).Column(nametag =>
            {
                    nametag.Item().AlignCenter().Text("name of Student").FontSize(11);
                    nametag.Item().AlignCenter().Text("surname of Student").FontSize(11);
                    nametag.Item().AlignCenter().Text("class of str").FontSize(11);
                    nametag.Item().AlignCenter().Text("Student number").FontSize(11);
            });

            headerRow.RelativeItem(5).Background(Colors.LightBlue.Darken2).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
            {
                generalInfo.Item().AlignCenter().Text("2023 - 2024").FontSize(13).FontFamily(Fonts.Arial);
                generalInfo.Item().AlignCenter().Text("HACIİLBEY MENSUCAT SANTRAL ORTAOKULU").FontSize(13).FontFamily(Fonts.Arial);
                generalInfo.Item().AlignCenter().Text("BİLİŞİM TEKNOLOJİLERİ ve YAZILIM ").FontSize(13).FontFamily(Fonts.Arial);
                generalInfo.Item().AlignCenter().Text("1. dönem 1. yazılı sınavı").FontSize(12).FontFamily(Fonts.Arial);
            });

            headerRow.RelativeItem(1.5f).Padding(0.2f, Unit.Centimetre).Background(Colors.Green.Darken2).Column(examSummary => 
            {
                examSummary.Item().AlignCenter().Text("Code: F09");
                examSummary.Item().AlignCenter().Text("Score").Underline();
                examSummary.Item().AlignCenter().Text("Digital score this is visible text").FontSize(5);
            });
        });

        page.Content().Background(Colors.Blue.Darken2).PaddingTop(0.4f, Unit.Centimetre).PaddingBottom(0.4f, Unit.Centimetre)
        .Column(column =>
        {
            column.Spacing(0.4f, Unit.Centimetre);
            for (int i = 0; i < 10; i++)
                column.Item().DebugArea($"box {i}", Colors.Grey.Medium).Background(Colors.DeepOrange.Accent4).Height(2, Unit.Centimetre).Text($"{i + 1}) Soru");
        });

        page.Footer().Background(Colors.Lime.Darken2).Height(1, Unit.Centimetre).Column(footerNotes =>
        {
            footerNotes.Item().Text("this area other notes in the footer").FontSize(9).LineHeight(2);
        });
    });
}).ShowInPreviewer();
//var examCodes = serviceProvider.GetService<IExamCodeGenerator>().GenerateExamCodes(-1);
