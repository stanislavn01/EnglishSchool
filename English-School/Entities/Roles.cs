using System.Collections.Generic;

namespace EnglishSchool.Entities;

public class Role
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}