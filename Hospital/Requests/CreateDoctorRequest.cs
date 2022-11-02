namespace Hospital.Requests;

public class CreateDoctorRequest
{
    public string Fio { get; set; } = string.Empty;
    public int CabinetId { get; set; }
    public int SpecializationId { get; set; }
    public int? DistrictId { get; set; }
}