namespace Application.Features.SemesterFeature.CRUD;

public class CreateSemesterDTO
{
    public string Name { get; set; }
    public DateOnly BeginSemesterDate { get; set; }

    public DateOnly EndSemesterDate { get; set; }

}