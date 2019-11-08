using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Service.Services
{
    class FileService
    {
        private byte[] file;
        private string path;
        private string typeFile;
        private string oldTypeFile;

        private string GetTypeFile(string nameFile)
        {
            if (nameFile == null || nameFile == string.Empty) return string.Empty;
            var listWords = nameFile.Split('.');
            string retVal = string.Empty;
            if (listWords?.Length > 0)
            {
                retVal = listWords[listWords.Length - 1];
            }

            return retVal;
        }

        public string GetTypeFile() => typeFile;

        public FileService(byte[] file, string path, string nameFile)
        {
            this.file = file;
            this.path = path;
            this.typeFile = GetTypeFile(nameFile);
        }
        
        public FileService(byte[] file, string path, string nameFile, string oldTypeFile) : 
                          this(file, path, nameFile)
        {
            this.oldTypeFile = oldTypeFile;
        }

        public void Add()
        {
            if (file?.Length > 0)
            {
                System.IO.File.WriteAllBytes(path + "." + typeFile, file);
            }
        }

        public void AddOrDelete(bool isDelete)
        {
            if(isDelete && System.IO.File.Exists(path + "." + oldTypeFile))
            {
                System.IO.File.Delete(path + "." + oldTypeFile);
            }
            else if (file?.Length > 0)
            {
                if (oldTypeFile != typeFile)
                    System.IO.File.Delete(path + "." + oldTypeFile);
                System.IO.File.WriteAllBytes(path + "." + typeFile, file);
            }

            
        }

    }
}
