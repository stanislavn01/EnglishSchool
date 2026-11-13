namespace EnglishSchool.Entities;

public class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public short DurationWeeks { get; set; }

    public bool IsHidden { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}