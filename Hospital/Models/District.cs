using System.ComponentModel.DataAnnotations;

namespace Hospital.Models;

public class District
{
    [Key]
    public int Id { get; set; }
    public int Number { get; set; }
}