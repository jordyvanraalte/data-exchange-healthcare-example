namespace healthcare.Services;

public interface IFileService {
    bool HasValidExtension(string fileName, string[] permittedExtensions);
    bool HasValidSignature(string fileName, byte[] bytes);
    string GetHash(Stream stream);
    string GetHash(byte[] bytes);
    void Save(byte[] bytes, string fileName);
    FileStream ReadAndDelete(string fileName);

    byte[] StreamToByteArray(Stream input);
}