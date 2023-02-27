using Microsoft.AspNetCore.Mvc;
using healthcare.Services;
using healthcare.Repositories;
using healthcare.Entities.Database;
using healthcare.ResponseModels;

namespace healthcare.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
  private readonly ILogger<PatientsController> _logger;

  private readonly IFileService _fileService;

  private readonly IPatientDataRepository _patientDataRepository;

  private static readonly string[] _permittedExtensions = { ".docx", ".pdf" };

  public PatientsController(ILogger<PatientsController> logger, IFileService fileService, IPatientDataRepository patientDataRepository)
  {
    _logger = logger;
    _fileService = fileService;
    _patientDataRepository = patientDataRepository;
  }

  [HttpGet("{key}")]
  public async Task<IActionResult> Get(string key)
  {
    try
    {
      var patientData = await _patientDataRepository.GetByKey(key);
      if (patientData == null)
      {
        return NotFound();
      }

      var fs = _fileService.ReadAndDelete(patientData.DocumentName);
      var bytes = _fileService.StreamToByteArray(fs);
      fs.Close();

      var hash = _fileService.GetHash(bytes);

      if (hash != patientData.DocumentHash)
      {
        return BadRequest("File has been tampered with.");
      }

      _logger.LogInformation($"[{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}] Document downloaded: {patientData.DocumentName} with key: {key} and hash: {hash} by {HttpContext.Connection.RemoteIpAddress.ToString()}");

      return File(bytes, "application/octet-stream", patientData.DocumentName);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting patient data");
      return BadRequest("An error occurred.");
    }
  }

  [HttpPost]
  public async Task<IActionResult> UploadDocument(IFormFile file)
  {
    try
    {
      var stream = file.OpenReadStream();
      var bytes = _fileService.StreamToByteArray(stream);

      if (_fileService.HasValidExtension(file.FileName, _permittedExtensions))
      {
        return BadRequest("Invalid file extension.");
      }

      if (!_fileService.HasValidSignature(file.FileName, bytes))
      {
        return BadRequest("Invalid file signature.");
      }

      var hash = _fileService.GetHash(bytes);
      var newFileName = $"{Nanoid.Nanoid.Generate()}{Path.GetExtension(file.FileName)}";
      var key = $"{Nanoid.Nanoid.Generate(size: 64)}";
      _fileService.Save(bytes, newFileName);

      var patientData = new PatientData
      {
        DocumentName = newFileName,
        DocumentHash = hash,
        Key = key
      };

      await _patientDataRepository.Create(patientData);

      _logger.LogInformation($"[{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}] Document uploaded: {newFileName} with key: {key} and hash: {hash} by {HttpContext.Connection.RemoteIpAddress.ToString()}");

      return Ok(new PatientUploadResponse
      {
        Key = key,
      });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error uploading file");
      return BadRequest("An error occurred.");
    }
  }
}

