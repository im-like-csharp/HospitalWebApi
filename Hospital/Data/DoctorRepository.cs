using System.ComponentModel;
using Hospital.Models;
using Hospital.DTOs;
using Hospital.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Data;

public class DoctorRepository : IDoctorRepository
{
    private readonly HospitalContext _context;

    public DoctorRepository(HospitalContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorGetDto>> GetSortedByPropertyAsync(string? property, int? pageNumber, int? pageSize)
    {
        var doctors = await GetSortedByPropertyAsync(property);
        
        return TakePage(doctors, pageNumber, pageSize);
    }

    public async Task<IEnumerable<DoctorGetDto>> GetSortedByPropertyAsync(string property)
    {
        var doctors = await GetListAsync();

        if (string.IsNullOrWhiteSpace(property))
        {
            return doctors;
        }

        PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(DoctorGetDto)).Find((string)property, true);

        if (prop is not null)
        {
            doctors = doctors.OrderBy(x => prop.GetValue(x));
        }

        return doctors;
    }

    public async Task<IEnumerable<DoctorGetDto>> GetPagedListAsync(int? pageNumber, int? pageSize)
    {
        var doctors = await GetListAsync();

        return TakePage(doctors, pageNumber, pageSize);
    }

    public async Task<IEnumerable<DoctorGetDto>> GetListAsync()
    {
        return await _context.Set<Doctor>()
            .Include(x => x.Cabinet)
            .Include(x => x.Specialization)
            .Include(x => x.District)
            .Select(x => new DoctorGetDto
            {
                DoctorId = x.Id,
                Fio = x.Fio,
                Cabinet = x.Cabinet.Number,
                Specialization = x.Specialization.Name,
                District = x.District.Number
            }).ToListAsync();
    }
    
    public async Task<DoctorGetDto?> GetByIdAsync(int doctorId)
    {
        var doctor = await GetDoctorByIdAsync(doctorId);

        return new DoctorGetDto
        {
            DoctorId = doctor.Id,
            Fio = doctor.Fio,
            Cabinet = doctor.Cabinet.Number,
            Specialization = doctor.Specialization.Name,
            District = doctor.District.Number
        };
    }
    
    public async Task<DoctorUpdateDto> PostAsync(CreateDoctorRequest request)
    {
        if (!await _context.Set<District>().AnyAsync(x => x.Id == request.DistrictId))
        {
            return new DoctorUpdateDto
            {
                DoctorId = null,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"District not found"
                }
            };
        }

        var doctor = await _context.Doctors.AddAsync(new Doctor
        {
            Fio = request.Fio,
            CabinetId = request.CabinetId,
            SpecializationId = request.SpecializationId,
            DistrictId = request.DistrictId
        });

        await _context.SaveChangesAsync();

        return new DoctorUpdateDto
        {
            DoctorId = doctor.Entity.Id, 
            OperationResult = new OperationResult
            {
                Code = 200, 
                Message = $"Doctor {doctor.Entity.Fio} created"
            }
        };
    }

    public async Task<DoctorUpdateDto> PutAsync(UpdateDoctorRequest request)
    {
        var doctor = await GetDoctorByIdAsync(request.Id);

        if (doctor is null)
        {
            return new DoctorUpdateDto
            {
                DoctorId = request.Id,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"Doctor not found"
                }
            };
        }

        if (!await _context.Set<District>().AnyAsync(x => x.Id == request.DistrictId))
        {
            return new DoctorUpdateDto
            {
                DoctorId = request.Id,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"District not found"
                }
            };
        }

        doctor.Fio = request.Fio;
        doctor.CabinetId = request.CabinetId;
        doctor.SpecializationId = request.SpecializationId;
        doctor.DistrictId = request.DistrictId;

        await _context.SaveChangesAsync();

        return new DoctorUpdateDto
        {
            DoctorId = doctor.Id,
            OperationResult = new OperationResult
            {
                Code = 200,
                Message = $"Doctor {doctor.Fio} updated"
            }
        };
    }

    public async Task<DoctorUpdateDto> DeleteAsync(int doctorId)
    {
        var doctor = await GetDoctorByIdAsync(doctorId);

        if (doctor is null)
        {
            return new DoctorUpdateDto
            {
                DoctorId = doctorId,
                OperationResult = new OperationResult
                {
                    Code = 400,
                    Message = $"Doctor not found"
                }
            };
        }

        _context.Set<Doctor>().Remove(doctor);
        await _context.SaveChangesAsync();

        return new DoctorUpdateDto
        {
            DoctorId = doctor.Id,
            OperationResult = new OperationResult
            {
                Code = 200,
                Message = $"Doctor {doctor.Fio} deleted"
            }
        };
    }

    private IEnumerable<DoctorGetDto> TakePage(IEnumerable<DoctorGetDto> doctors, int? pageNumber, int? pageSize)
    {
        if (pageNumber is null || pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize is null || pageSize < 1)
        {
            pageSize = 100;
        }

        return doctors.Skip(((int)pageNumber - 1) * (int)pageSize).Take((int)pageSize);
    }

    private async Task<Doctor?> GetDoctorByIdAsync(int doctorId)
    {
        return await _context.Set<Doctor>()
            .Include(x => x.Cabinet)
            .Include(x => x.Specialization)
            .Include(x => x.District)
            .FirstOrDefaultAsync(x => x.Id == doctorId);
    }
}