using Hospital.Data;
using Hospital.DTOs;
using Hospital.Requests;
using Hospital.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers;

[ApiController]
[Route("doctors")]
public class DoctorController : Controller
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorController(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    [HttpGet("sortby/{property}&{pageNumber}&{pageSize}")]
    public async Task<ActionResult> GetSortedByPagedAsync(string? property, int? pageNumber, int? pageSize)
    {
        var doctors = await _doctorRepository.GetSortedByPropertyPagedAsync(property, pageNumber, pageSize);

        return Ok(new PagedResponse<IEnumerable<DoctorGetDto>>(doctors, pageNumber, pageSize));
    }

    [HttpGet("sortby/{property}")]
    public async Task<ActionResult> GetSortedByAsync(string? property)
    {
        var doctors = await _doctorRepository.GetSortedByPropertyAsync(property);

        return Ok(doctors);
    }
    
    [HttpGet("{pageNumber}&{pageSize}")]
    public async Task<ActionResult> GetPagedListAsync(int? pageNumber, int? pageSize)
    {
        var doctors = await _doctorRepository.GetPagedListAsync(pageNumber, pageSize);

        return Ok(new PagedResponse<IEnumerable<DoctorGetDto>>(doctors, pageNumber, pageSize));
    }
    
    [HttpGet]
    public async Task<IEnumerable<DoctorGetDto>> GetListAsync()
    {
        return await _doctorRepository.GetListAsync();
    }
    
    [HttpGet("doctorId")]
    public async Task<ActionResult<DoctorGetDto?>> GetByIdAsync(int doctorId)
    {
        try
        {
            return await _doctorRepository.GetByIdAsync(doctorId);
        }
        catch (NullReferenceException exception)
        {
            return NotFound(exception.Message);
        }
    }
    
    [HttpPost]
    public async Task<DoctorUpdateDto> PostAsync(CreateDoctorRequest request)
    {
        return await _doctorRepository.PostAsync(request);
    }

    [HttpPut]
    public async Task<DoctorUpdateDto> PutAsync(UpdateDoctorRequest request)
    {
        return await _doctorRepository.PutAsync(request);
    }

    [HttpDelete("{doctorId}")]
    public async Task<DoctorUpdateDto> DeleteAsync(int doctorId)
    {
        return await _doctorRepository.DeleteAsync(doctorId);
    }
}