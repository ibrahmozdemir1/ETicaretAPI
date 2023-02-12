using ETicaretAPI.Application.Abstractions.Storage.Local;
using ETicaretAPI.Infrastucture.StaticService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastucture.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
           
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream =
                      new(path, FileMode.Create,
                          FileAccess.Write, FileShare.None,
                           1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);


                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }

        public async Task DeleteAsync(string pathOrContainer, string fileName)
           => File.Delete($"{pathOrContainer}\\{fileName}");
        

        public List<string> GetFiles(string pathOrContainer)
        {
            DirectoryInfo directory = new(pathOrContainer);
            return directory.GetFiles().Select(f => f.Name).ToList();

        }

        public bool HasFile(string pathOrContainer, string fileName)
            => File.Exists($"{pathOrContainer}\\{fileName}");

        public async Task<List<(string fileName, string pathOrContainer)>> UploadAsync(string pathOrContainer, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, pathOrContainer);
            FileService fileService = new();

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();

            foreach (IFormFile file in files)
            {
                var fileName = await fileService.FileRenameAsync(uploadPath,file.Name);

                bool result = await CopyFileAsync(Path.Combine(uploadPath, fileName), file);


                datas.Add((fileName, Path.Combine(uploadPath, fileName)));
            }

            //todo Eğer ki yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception 
            // oluşturulup fırlatılması gerekiyor.

            return datas;
        }
    }
}
