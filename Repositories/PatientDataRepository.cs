using healthcare.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace healthcare.Repositories;

public class PatientDataRepository : IPatientDataRepository
{
  private readonly ApplicationDbContext _context;
  public PatientDataRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<PatientData?> GetByKey(string key)
  {
    return await _context.PatientDatas.FirstOrDefaultAsync(x => x.Key == key);
  }

  public async Task<PatientData> Create(PatientData patientData)
  {
    _context.PatientDatas.Add(patientData);
    await _context.SaveChangesAsync();
    return patientData;
  }

}