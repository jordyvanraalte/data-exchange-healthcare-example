using healthcare.Entities.Database;

namespace healthcare.Repositories;

public interface IPatientDataRepository
{
    Task<PatientData?> GetByKey(string key);
    Task<PatientData> Create(PatientData patientData);
}