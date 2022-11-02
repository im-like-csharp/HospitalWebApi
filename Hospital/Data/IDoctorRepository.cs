using Hospital.DTOs;
using Hospital.Requests;

namespace Hospital.Data;

public interface IDoctorRepository
{
    Task<IEnumerable<DoctorGetDto>> GetSortedByPropertyAsync(string? property, int? pageNumber, int? pageSize);
    Task<IEnumerable<DoctorGetDto>> GetSortedByPropertyAsync(string property); 
    Task<IEnumerable<DoctorGetDto>> GetPagedListAsync(int? pageNumber, int? pageSize);
    Task<IEnumerable<DoctorGetDto>> GetListAsync();
    Task<DoctorGetDto?> GetByIdAsync(int doctorId);
    Task<DoctorUpdateDto> PostAsync(CreateDoctorRequest request);
    Task<DoctorUpdateDto> PutAsync(UpdateDoctorRequest request);
    Task<DoctorUpdateDto> DeleteAsync(int doctorId);
}