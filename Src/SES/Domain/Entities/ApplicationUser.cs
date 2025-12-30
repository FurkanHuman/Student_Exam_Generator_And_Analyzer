using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public virtual Guid PersonelId { get; set; }
    public virtual Personel? Personel { get; set; }
}
