namespace Entity.Base;

public class Entity<I> where I : struct
{
    public I Id { get; set; }
}
