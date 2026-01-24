using Domain.Common;

namespace Domain.Exam.ValueObjects;

public sealed record ExamMetadata : ValueObject
{
    public string LessonName { get; init; }
    public string TrackingCode { get; init; }
    public byte[] RandomizerSeed { get; init; }
    public string? FooterNote { get; init; }

    private ExamMetadata()
    {
        LessonName = string.Empty;
        TrackingCode = string.Empty;
        RandomizerSeed = [];
    }

    private ExamMetadata(string lessonName, string trackingCode)
    {
        if (string.IsNullOrWhiteSpace(lessonName))
            throw new DomainException("Lesson name cannot be empty");

        if (string.IsNullOrWhiteSpace(trackingCode))
            throw new DomainException("Tracking code cannot be empty");

        LessonName = lessonName.Trim();
        TrackingCode = trackingCode.Trim().ToUpper();
        RandomizerSeed = Guid.NewGuid().ToByteArray();
    }

    public static ExamMetadata Create(string lessonName, string trackingCode) => new(lessonName, trackingCode);

    public ExamMetadata WithFooterNote(string note) => this with { FooterNote = note };

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return LessonName;
        yield return TrackingCode;
        yield return FooterNote;
    }
}
