using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Front_Back.Extension
{
    public static class Extension
    {
        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }

        public static bool CheckSize(this IFormFile file, int kb)
        {
            return file.Length / 1024 <= kb;
        }

        public async static Task<string> SaveFile(this IFormFile file, string folder, string root)
        {
            try
            {

                string fileName = Guid.NewGuid().ToString() + file.FileName;
                string resultPath = Path.Combine(root, folder, fileName);
                using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return fileName;
            }
            catch (Exception)
            {

                return null;
            }

                                        }
    }
}
