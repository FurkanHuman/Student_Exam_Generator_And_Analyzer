namespace Application.Features.SemesterFeature.CRUD;

public class RequestSemesterDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BeginSemesterDate { get; set; }

    public DateOnly EndSemesterDate { get; set; }
}