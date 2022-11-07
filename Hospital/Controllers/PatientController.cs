using Hospital.Data;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Requests;
using Hospital.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers;

[ApiController]
[Route("patients")]
public class PatientController : Controller
{
    private readonly IPatientRepository _patientRepository;

    public PatientController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    
    [HttpGet("sortby/{property}&{pageNumber}&{pageSize}")]
    public async Task<ActionResult> GetSortByAsync(string? property, int? pageNumber, int? pageSize)
    {
        var patients = await _patientRepository.GetSortedByPropertyAsync(property, pageNumber, pageSize);

        return Ok(patients);
    }

    [HttpGet("sortby/{property}")]
    public async Task<ActionResult> GetSortByAsync(string property)
    {
        var patients = await _patientRepository.GetSortedByPropertyAsync(property);

        return Ok(patients);
    }
    
    [HttpGet("{pageNumber}&{pageSize}")]
    public async Task<ActionResult> GetPagedListAsync(int pageNumber, int pageSize)
    {
        var patients = await _patientRepository.GetPagedListAsync(pageNumber, pageSize);

        return Ok(new PagedResponse<IEnumerable<PatientGetDto>>(patients, pageNumber, pageSize));
    }
    
    [HttpGet]
    public async Task<IEnumerable<PatientGetDto>> GetListAsync()
    {
        return await _patientRepository.GetListAsync();
    }

    [HttpGet("patientId")]
    public async Task<ActionResult<PatientGetDto?>> GetByIdAsync(int patientId)
    {
        PatientGetDto patientGetDto;
        
        try
        {
            patientGetDto = await _patientRepository.GetByIdAsync(patientId);
        }
        catch (NullReferenceException exception)
        {
            return NotFound(exception.Message);
        }

        return patientGetDto;
    }
    
    [HttpPost]
    public async Task<PatientUpdateDto> PostAsync(CreatePatientRequest request)
    {
        return await _patientRepository.PostAsync(request);
    }

    [HttpPut]
    public async Task<PatientUpdateDto> PutAsync(UpdatePatientRequest request)
    {
        return await _patientRepository.PutAsync(request);
    }

    [HttpDelete("{patientId}")]
    public async Task<PatientUpdateDto> DeleteAsync(int patientId)
    {
        return await _patientRepository.DeleteAsync(patientId);
    }
}