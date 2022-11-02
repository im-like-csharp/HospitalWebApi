using System.ComponentModel.DataAnnotations;

namespace Hospital.Models;

public class Specialization
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
}