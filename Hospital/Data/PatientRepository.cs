using System.ComponentModel;
using Hospital.DTOs;
using Hospital.Extensions;
using Hospital.Models;
using Hospital.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Data;

public class PatientRepository : IPatientRepository
{
    private readonly HospitalContext _context;

    public PatientRepository(HospitalContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientGetDto>> GetSortedByPropertyPagedAsync(string? property, int? pageNumber, int? pageSize)
    {
        return (await GetSortedByPropertyAsync(property))
            .AsQueryable()
            .TakePage(pageNumber, pageSize);
    }

    public async Task<IEnumerable<PatientGetDto>> GetSortedByPropertyAsync(string? property)
    {
        var patients = (await GetListAsync()).AsQueryable();

        if (string.IsNullOrWhiteSpace(property))
        {
            return patients;
        }

        PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(PatientGetDto)).Find(property, true)
                                  ?? TypeDescriptor.GetProperties(typeof(PatientGetDto)).Find("PatientId", true)!;
        
        patients = patients.OrderBy(x => prop.GetValue(x));

        return patients;
    }

    public async Task<IEnumerable<PatientGetDto>> GetPagedListAsync(int? pageNumber, int? pageSize)
    {
        return await _context.Set<Patient>()
            .AsQueryable()
            .TakePage(pageNumber, pageSize)
            .Include(x => x.District)
            .Select(x => new PatientGetDto
            {
                Surname = x.Surname,
                Name = x.Name,
                MiddleName = x.MiddleName,
                Address = x.Address,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                District = x.District!.Number
            }).ToListAsync();
    }

    public async Task<IEnumerable<PatientGetDto>> GetListAsync()
    {
        return await _context.Set<Patient>()
            .AsQueryable()
            .Include(x => x.District)
            .Select(x => new PatientGetDto
            {
                Surname = x.Surname,
                Name = x.Name,
                MiddleName = x.MiddleName,
                Address = x.Address,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                District = x.District!.Number
            }).ToListAsync();
    }

    public async Task<PatientGetDto?> GetByIdAsync(int patientId)
    {
        var patient = await GetPatientByIdAsync(patientId);
        
        if (patient is null)
        {
            throw new NullReferenceException("Patient not found");
        }

        return new PatientGetDto
        {
            Surname = patient.Surname,
            Name = patient.Name,
            MiddleName = patient.MiddleName,
            Address = patient.Address,
            BirthDate = patient.BirthDate,
            Gender = patient.Gender,
            District = patient.District!.Number
        };
    }

    public async Task<PatientUpdateDto> PostAsync(CreatePatientRequest request)
    {
        if (!await _context.Set<District>().AnyAsync(x => x.Id == request.DistrictId))
        {
            return new PatientUpdateDto
            {
                PatientId = null,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"District not found"
                }
            };
        }

        var patient = await _context.Set<Patient>().AddAsync(new Patient
        {
            Surname = request.Surname,
            Name = request.Name,
            MiddleName = request.MiddleName,
            Address = request.Address,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            DistrictId = request.DistrictId
        });

        await _context.SaveChangesAsync();

        return new PatientUpdateDto
        {
            PatientId = patient.Entity.Id, 
            OperationResult = new OperationResult
            {
                Code = 200, 
                Message = $"Patient {BuildFio(patient.Entity.Surname, patient.Entity.Name, patient.Entity.MiddleName)} created"
            }
        };
    }

    public async Task<PatientUpdateDto> PutAsync(UpdatePatientRequest request)
    {
        var patient = await GetPatientByIdAsync(request.Id);

        if (patient is null)
        {
            return new PatientUpdateDto
            {
                PatientId = request.Id,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"Patient not found"
                }
            };
        }

        if (!await _context.Set<District>().AnyAsync(x => x.Id == request.DistrictId))
        {
            return new PatientUpdateDto
            {
                PatientId = request.Id,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"District not found"
                }
            };
        }

        patient.Surname = request.Surname;
        patient.Name = request.Name;
        patient.MiddleName = request.MiddleName;
        patient.Address = request.Address;
        patient.BirthDate = request.BirthDate;
        patient.Gender = request.Gender;
        patient.DistrictId = request.DistrictId;

        await _context.SaveChangesAsync();

        return new PatientUpdateDto
        {
            PatientId = patient.Id,
            OperationResult = new OperationResult
            {
                Code = 200,
                Message = $"Patient {BuildFio(patient.Surname, patient.Name, patient.MiddleName)} updated"
            }
        };
    }

    public async Task<PatientUpdateDto> DeleteAsync(int patientId)
    {
        var patient = await GetPatientByIdAsync(patientId);

        if (patient is null)
        {
            return new PatientUpdateDto
            {
                PatientId = patientId,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"Patient not found"
                }
            };
        }

        _context.Set<Patient>().Remove(patient);
        await _context.SaveChangesAsync();

        return new PatientUpdateDto
        {
            PatientId = patient.Id,
            OperationResult = new OperationResult
            {
                Code = 200,
                Message = $"Patient {BuildFio(patient.Surname, patient.Name, patient.MiddleName)} deleted"
            }
        };
    }

    private async Task<Patient?> GetPatientByIdAsync(int patientId)
    {
        return await _context.Set<Patient>()
            .AsQueryable()
            .Where(x => x.Id == patientId)
            .Include(x => x.District)
            .FirstOrDefaultAsync();
    }

    private string BuildFio(string surname, string name, string middleName) => $"{surname} {name} {middleName}";
}