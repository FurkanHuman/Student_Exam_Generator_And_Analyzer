namespace Application.Services.PdfFactory.PdfReaderService.Dtos;

public record ClassWithStudentsDto
{
    public int Age { get; set; }
    public char Section { get; set; }
    public required ICollection<string[]> Students { get; set; }
}