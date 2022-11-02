using System.ComponentModel.DataAnnotations;

namespace Hospital.Models;

public class Doctor
{
    [Key]
    public int Id { get; set; }
    public string Fio { get; set; } = string.Empty;
    public int CabinetId { get; set; }
    public int SpecializationId { get; set; }
    public int? DistrictId { get; set; }
    
    public Cabinet? Cabinet { get; set; }
    public Specialization? Specialization { get; set; }
    public District? District { get; set; }
}