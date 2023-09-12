using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Service
{
    public class FileService
    {
        private readonly IWebHostEnvironment webHostEnvironment; //1
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }//2
        public async Task<Tuple<int, string>> UploadImage(IFormFile imageFile) //3
        {
            try
            {
                var contentPath = webHostEnvironment.ContentRootPath; 
                var path = Path.Combine(contentPath, "Uploads"); 
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path); 

               
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" }; 
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string> ( 0, msg );
                } 
                string uniqueString = Guid.NewGuid().ToString();
                string fileName = uniqueString + ext;
                string fullPath = Path.Combine(path, fileName);
                using (var filStream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(filStream);
                } 
                return new Tuple<int, string>(1, "Image saved successfully");
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>( 0, "An error occurred: " +  ex.Message );
            }
        }
    }
}
