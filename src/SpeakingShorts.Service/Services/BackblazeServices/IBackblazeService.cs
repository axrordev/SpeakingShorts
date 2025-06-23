namespace SpeakingShorts.Service.Services.BackblazeServices;

public interface IBackblazeService
{
Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
Task<Stream> DownloadFileAsync(string fileName);
Task<string> DownloadFileToPathAsync(string fileName, string destinationPath);
Task<bool> DeleteFileAsync(string fileName);
Task<IEnumerable<string>> ListFilesAsync();
}
