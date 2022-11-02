namespace Hospital.DTOs;

public class DoctorGetDto
{
    public int? DoctorId { get; set; }
    public string Fio { get; set; } = string.Empty;
    public int Cabinet { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public int? District { get; set; }
}