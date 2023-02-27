using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace healthcare.Entities.Database;

public class PatientData {
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; } = 0;

  public string Key { get; set; }

  public string DocumentName { get; set; }

  public string DocumentHash { get; set; }
}