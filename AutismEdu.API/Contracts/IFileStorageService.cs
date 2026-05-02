namespace AutismEdu.API.Contracts
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Saves an uploaded file to storage and returns the relative path.
        /// </summary>
        Task<string> SaveFileAsync(IFormFile file, string subFolder);

        /// <summary>
        /// Deletes a file from storage by its relative path.
        /// </summary>
        bool DeleteFile(string relativePath);

        /// <summary>
        /// Gets a stream for reading a file from storage.
        /// </summary>
        FileStream? GetFileStream(string relativePath);

        /// <summary>
        /// Generates a full download URL for a file.
        /// </summary>
        string GetFileUrl(string relativePath);
    }
}
