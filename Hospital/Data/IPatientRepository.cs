using Hospital.DTOs;
using Hospital.Models;
using Hospital.Requests;

namespace Hospital.Data;

public interface IPatientRepository
{
    Task<IEnumerable<PatientGetDto>> GetSortedByPropertyPagedAsync(string? property, int? pageNumber, int? pageSize);
    Task<IEnumerable<PatientGetDto>> GetSortedByPropertyAsync(string property); 
    Task<IEnumerable<PatientGetDto>> GetPagedListAsync(int? pageNumber, int? pageSize);
    Task<IEnumerable<PatientGetDto>> GetListAsync();
    Task<PatientGetDto?> GetByIdAsync(int patientId);
    Task<PatientUpdateDto> PostAsync(CreatePatientRequest request);
    Task<PatientUpdateDto> PutAsync(UpdatePatientRequest request);
    Task<PatientUpdateDto> DeleteAsync(int patientId);
}