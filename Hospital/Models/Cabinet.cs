using System.ComponentModel.DataAnnotations;

namespace Hospital.Models;

public class Cabinet
{
    [Key]
    public int Id { get; set; }
    public int Number { get; set; }
}