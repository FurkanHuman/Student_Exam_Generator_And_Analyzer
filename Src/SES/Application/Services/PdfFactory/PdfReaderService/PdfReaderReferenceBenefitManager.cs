// note: perfect clean and readable code. 
// note: the code is well-structured and easy to understand.

using Application.Services.PdfFactory.PdfReaderService.Dtos;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Application.Services.PdfFactory.PdfReaderService;

internal static class PdfReaderReferenceBenefitManager
{
    private const string BenefitCodePattern = @"([A-Z]{2,3}\.)?\d+\.\d+\.\d+\.\d+\.";
    private const string SubLearningAreaCodePattern = @"[A-Z]{2,3}\.\d+\.\d+\.\d+";
    private const string LearningAreaCodePattern = @"[A-Z]{2,3}\.\d+\.\d+\.";

    private const string BenefitPattern = @$"({BenefitCodePattern})\s*([A-Za-zçÇşŞıİğĞöÖüÜ\s\-,]+(\.))?"; // group 1, group 3
    private const string BenefitPatternToTwoGroup = @"([A-Z]{2,3}\.?\d+\.\d+\.\d+\.\d+\.)\s*([A-Za-zçÇşŞıİğĞöÖüÜ\s\-,]+\.)?"; // group 1, group 2 
    private const string SubLearningAreaPattern = @$"({SubLearningAreaCodePattern})\s*-\s*(.*?)(?=\s+[A-Z]{{2,3}}\.\d+\.\d+|$)"; // group 1, group 2
    private const string LearningAreaPattern = @$"({LearningAreaCodePattern})\s*([\sA-ZÇŞİĞÜÖ\s]+)"; // group 1, group 2    
    // private const string LearningAreaPatternLineByLine = @"([A-Z]{2,3}\.\d+\.\d+\.)\s*([\sA-ZÇŞİĞÜÖ\s]+)"; // after added new line character. now not usable.

    internal static async Task ProcessPageAsync(PdfDocument pdfDocument, int pageNumber, ConcurrentBag<RawBenefitReferenceDto> result)
    {
        SimpleTextExtractionStrategy strategy = new();
        string content = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(pageNumber), strategy);
        RawBenefitReferenceDto rawBenefitReferenceDto = new()
        {
            Benefits = ExtractBenefit(content),
            SubLearningAreas = ExtractSubLearningAreas(content),
            LearningAreas = ExtractLearningAreas(content)
        };

        result.Add(rawBenefitReferenceDto);
        await Task.Yield();
    }

    // todo: future plan to use this method for bypass pdf file.
    internal static async Task ProcessBypassPdfFileAsync(ICollection<string> lines, ConcurrentBag<RawBenefitReferenceDto> result)
    {
        string content = string.Join("\n", lines); // now this code is not used. placeholder code

        RawBenefitReferenceDto rawBenefitReferenceDto = new()
        {
            Benefits = ExtractBenefit(content),
            SubLearningAreas = ExtractSubLearningAreas(content),
            LearningAreas = ExtractLearningAreas(content)
        };

        result.Add(rawBenefitReferenceDto);
        await Task.Yield();
    }

    internal static ReferenceBenefitDto ProcessReferenceBenefit(ICollection<string> lines)
    {
        ICollection<LearningAreaDto> learningAreas = [];
        ICollection<SubLearningDto> subLearningAreas = [];
        ICollection<BenefitDto> benefitDtos = [];

        ExtractDetails(lines, learningAreas, subLearningAreas, benefitDtos);
        MapSubLearningAreasToBenefits(subLearningAreas, benefitDtos);
        MapLearningAreasToSubLearningAreas(learningAreas, subLearningAreas);

        ReferenceBenefitDto referenceBenefitDto = new()
        {
            RBName = string.Empty,
            LearningAreas = learningAreas,
        };

        return referenceBenefitDto;
    }

    internal static ICollection<RawBenefitReferenceDto> DistinctByAllItems(ConcurrentBag<RawBenefitReferenceDto> rawBenefitReferencesBag)
    {
        List<string>? benefitts = [];
        List<string>? subLearningAreas = [];
        List<string>? learningAreas = [];

        foreach (RawBenefitReferenceDto raw in rawBenefitReferencesBag)
        {
            if (raw.Benefits != null)
                benefitts.AddRange(raw.Benefits);

            if (raw.SubLearningAreas != null)
                subLearningAreas.AddRange(raw.SubLearningAreas);

            if (raw.LearningAreas != null)
                learningAreas.AddRange(raw.LearningAreas);
        }

        benefitts = benefitts.Distinct().ToList();
        subLearningAreas = subLearningAreas.Distinct().ToList();
        learningAreas = learningAreas.Distinct().ToList();

        HandleUnmatchedCodes(benefitts, subLearningAreas, learningAreas);

        return [new RawBenefitReferenceDto
        {
            Benefits = benefitts,
            SubLearningAreas = subLearningAreas,
            LearningAreas = learningAreas
        }];
    }

    private static ICollection<string>? ExtractBenefit(string content)
    {
        Regex _benefitRegex = new(BenefitPattern, RegexOptions.Compiled);

        ICollection<string>? benefits = [];

        string cleanedContent = CleanInContentToBenefitPatern(content);

        cleanedContent = Regex.Replace(cleanedContent, @"[A-Z]{2,3}$", "").Trim();

        MatchCollection matchesBenefits = _benefitRegex.Matches(cleanedContent);
        foreach (Match match in matchesBenefits)
            benefits.Add(match.Value);

        return benefits;
    }

    private static ICollection<string>? ExtractSubLearningAreas(string content)
    {
        Regex _subLearningAreaRegex = new(SubLearningAreaPattern, RegexOptions.Compiled);
        ICollection<string>? subLearningAreas = [];
        string cleanedContent = CleanInContentToSubLearningPatern(content);

        MatchCollection matchesSubLearningAreas = _subLearningAreaRegex.Matches(cleanedContent);

        foreach (Match match in matchesSubLearningAreas)
            subLearningAreas.Add(match.Value);

        return subLearningAreas;
    }

    private static ICollection<string>? ExtractLearningAreas(string content)
    {
        Regex _learningAreaRegex = new(LearningAreaPattern, RegexOptions.Compiled);
        ICollection<string>? learningAreas = [];
        string cleanedContent = CleanInContentToLearningAreaPattern(content);

        MatchCollection matchesLearningAreas = _learningAreaRegex.Matches(cleanedContent);

        foreach (Match match in matchesLearningAreas)
            learningAreas.Add(match.Value);


        return learningAreas;
    }

    private static string CleanInContentToBenefitPatern(string content)
    {
        content = content.Replace("\n", " "); // Converts new lines to spaces.
        content = Regex.Replace(content, @"\b[A-ZİĞÜŞÖÇ]{3,}\b", ""); // Removes uppercase words.
        content = Regex.Replace(content, @"\s+", " ");

        return content;
    }

    private static string CleanInContentToSubLearningPatern(string content)
    {
        content = content.Replace("\n", " ");
        content = Regex.Replace(content, @"\b[a-z]{3,}\b", ""); // Removes lowercase words.
        content = Regex.Replace(content, BenefitPattern, ""); // Removes benefits.
        content = Regex.Replace(content, @"\s+", " ");
        return content;
    }

    private static string CleanInContentToLearningAreaPattern(string content)
    {
        content = Regex.Replace(content, SubLearningAreaPattern, "");
        content = Regex.Replace(content, @"\s+", " ");
        content = Regex.Replace(content, BenefitPattern, "");
        content = Regex.Replace(content, @"\s+", " ");

        return content;

    }

    private static void ExtractDetails(ICollection<string> lines, ICollection<LearningAreaDto> learningAreas, ICollection<SubLearningDto> subLearningAreas, ICollection<BenefitDto> benefitDtos)
    {
        foreach (string line in lines)
        {
            BenefitDto? benefitDetail = ExtractBenefitDetails(line);
            if (benefitDetail != null)
                benefitDtos.Add(benefitDetail);

            SubLearningDto? subLearningArea = ExtractSubLearningArea(line);
            if (subLearningArea != null)
                subLearningAreas.Add(subLearningArea);

            LearningAreaDto? learningArea = ExtractLearningArea(line);
            if (learningArea != null)
                learningAreas.Add(learningArea);
        }
    }

    private static void MapLearningAreasToSubLearningAreas(ICollection<LearningAreaDto> learningAreas, ICollection<SubLearningDto> subLearningAreas)
    {
        foreach (LearningAreaDto learningAreaDto in learningAreas)
            foreach (SubLearningDto subLearningDto in subLearningAreas)
                if (subLearningDto.SLCode.StartsWith(learningAreaDto.LACode))
                    learningAreaDto.SubLearningAreas.Add(subLearningDto);
    }

    private static void MapSubLearningAreasToBenefits(ICollection<SubLearningDto> subLearningAreas, ICollection<BenefitDto> benefitDtos)
    {
        foreach (SubLearningDto subLearningDto in subLearningAreas)
            foreach (BenefitDto benefitDto in benefitDtos)
                if (benefitDto.BCode.StartsWith(subLearningDto.SLCode))
                    subLearningDto.Benefits.Add(benefitDto);
    }

    private static LearningAreaDto? ExtractLearningArea(string line)
    {
        Regex _learningAreaRegex = new(LearningAreaPattern, RegexOptions.Compiled);
        Match match = _learningAreaRegex.Match(line);
        if (!match.Success)
            return null;
        return new LearningAreaDto
        {
            LACode = match.Groups[1].Value,
            Description = match.Groups[2].Value,
            SubLearningAreas = []
        };
    }

    private static SubLearningDto? ExtractSubLearningArea(string line)
    {
        Regex _subLearningAreaRegex = new(SubLearningAreaPattern, RegexOptions.Compiled);
        Match match = _subLearningAreaRegex.Match(line);
        if (!match.Success)
            return null;
        return new SubLearningDto
        {
            SLCode = match.Groups[1].Value,
            Description = match.Groups[2].Value,
            Benefits = []
        };
    }

    private static BenefitDto? ExtractBenefitDetails(string line)
    {
        Regex _learningAreaRegex = new(BenefitPatternToTwoGroup, RegexOptions.Compiled);
        Match match = _learningAreaRegex.Match(line);
        if (!match.Success)
            return null;
        return new BenefitDto
        {
            BCode = match.Groups[1].Value,
            Description = match.Groups[2].Value
        };
    }

    // huge thanks ChatGPT And Copilot
    private static void HandleUnmatchedCodes(ICollection<string> benefits, ICollection<string> subLearningAreas, ICollection<string> learningAreas)
    {
        Regex benefitPatternRegex = new(BenefitPatternToTwoGroup, RegexOptions.Compiled);
        Regex slCodeRegex = new(SubLearningAreaCodePattern, RegexOptions.Compiled);
        Regex laCodeRegex = new(LearningAreaCodePattern, RegexOptions.Compiled);

        foreach (string benefit in benefits)
        {
            Match benefitMatch = benefitPatternRegex.Match(benefit);
            if (!benefitMatch.Success)
                continue;

            string bCode = benefitMatch.Groups[1].Value;

            bool isMatchedSubLearning = subLearningAreas.Any(sla => slCodeRegex.IsMatch(sla) && bCode.StartsWith(slCodeRegex.Match(sla).Value));
            bool isMatchedLearningArea = learningAreas.Any(la => laCodeRegex.IsMatch(la) && bCode.StartsWith(laCodeRegex.Match(la).Value));

            IsNotMatchedCode(bCode, slCodeRegex, subLearningAreas, "- Bilinmeyen Alt Konu");
            IsNotMatchedCode(bCode, laCodeRegex, learningAreas, "Bilinmeyen Öğrenme Alanı");
        }
    }

    private static void IsNotMatchedCode(string code, Regex regex, ICollection<string> referenceList, string topic)
    {
        Match match = regex.Match(code);
        if (!match.Success)
            return;

        string matchedCode = match.Value;
        if (!referenceList.Any(i => i.StartsWith(matchedCode)))
            referenceList.Add($"{matchedCode} {topic}");
    }
}