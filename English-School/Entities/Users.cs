using System;
using System.Collections.Generic;

namespace EnglishSchool.Entities;

public class User
{
    public int Id { get; set; }

    public short RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string FullName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsBlocked { get; set; }

    public DateTime CreatedAt { get; set; }

    public Role Role { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}