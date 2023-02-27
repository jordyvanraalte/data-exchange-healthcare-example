using System.Security.Cryptography;

namespace healthcare.Services;

public class FileService : IFileService
{
  private static readonly Dictionary<string, List<byte[]>> _fileSignatures =
  new Dictionary<string, List<byte[]>>
  {
        {
            ".pdf", new List<byte[]>
            {
                new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D },
            }
        },
        {
            ".docx", new List<byte[]>
            {
                new byte[] { 0x50, 0x4B, 0x03, 0x04 },
            }
        }
  };

  public bool HasValidExtension(string fileName, string[] permittedExtensions)
  {
    var ext = Path.GetExtension(fileName).ToLowerInvariant();
    return string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext);
  }

  public bool HasValidSignature(string fileName, byte[] bytes)
  {
    var ext = Path.GetExtension(fileName).ToLowerInvariant();
    var signatures = _fileSignatures[ext];
    var headerBytes = new byte[signatures.Max(m => m.Length)];
    Array.Copy(bytes, headerBytes, headerBytes.Length);
    return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
  }

  public string GetHash(byte[] bytes)
  {
    using var sha256 = SHA256.Create();
    var hash = sha256.ComputeHash(bytes);
    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
  }

  public string GetHash(Stream stream)
  {
    using var sha256 = SHA256.Create();
    var hash = sha256.ComputeHash(stream);
    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
  }

  public void Save(byte[] bytes, string fileName)
  {
    if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "uploads")))
      Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "uploads"));

    var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);
    using (var fileStream = new FileStream(path, FileMode.Create))
    {
      fileStream.Write(bytes, 0, bytes.Length);
    }
  }

  public FileStream ReadAndDelete(string fileName)
  {
    var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);
    var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
    return fs;
  }

  public byte[] StreamToByteArray(Stream input)
  {
    using (MemoryStream ms = new MemoryStream())
    {
      input.CopyTo(ms);
      return ms.ToArray();
    }
  }
}