using NArchitecture.Core.Application.Responses;

namespace Application.Features.Exams.Commands.CreateMultipleExam;

public class CreateMultipleExamResponse : IResponse 
{
    public string FileName { get; set; } = string.Empty;
    public MemoryStream ZipFileMemStream { get; set; }
}
