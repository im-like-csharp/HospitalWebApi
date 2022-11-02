namespace Hospital.Models;

using System.ComponentModel.DataAnnotations;
using Hospital.Enums;

public class Patient
{
    [Key]
    public int Id { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int DistrictId { get; set; }
    
    public District? District { get; set; }
}