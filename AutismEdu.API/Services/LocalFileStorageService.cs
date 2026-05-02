using AutismEdu.API.Contracts;

namespace AutismEdu.API.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var folderPath = Path.Combine(webRootPath, "Uploads", "Files", subFolder);

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("Uploads", "Files", subFolder, fileName).Replace("\\", "/");
        }

        public bool DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return false;

            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            relativePath = relativePath.Replace("\\", "/").TrimStart('/');
            var fullPath = Path.Combine(webRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }

        public FileStream? GetFileStream(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            relativePath = relativePath.Replace("\\", "/").TrimStart('/');
            var fullPath = Path.Combine(webRootPath, relativePath);

            if (!File.Exists(fullPath))
                return null;

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }

        public string GetFileUrl(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            if (Uri.IsWellFormedUriString(relativePath, UriKind.Absolute))
                return relativePath;

            relativePath = relativePath.Replace("\\", "/").TrimStart('/');

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                return "/" + relativePath;

            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return $"{baseUrl}/{relativePath}";
        }
    }
}
