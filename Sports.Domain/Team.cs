using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sports.Domain
{
    public class Team
    {
        const int MAX_FILE_SIZE = 2000000;
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Picture { get; private set; }

        public void IsValid()
        {
            IsValidName();
        }

        private void IsValidName()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                throw new Exception();
            }
        }

        public void AddPictureFromPath(string filePath)
        {
            ValidateFile(filePath);
            byte[] bytePicture = File.ReadAllBytes(filePath);
            this.Picture = System.Text.Encoding.UTF8.GetString(bytePicture);
        }

        public void ValidateFile(string filePath)
        {
            ValidatePath(filePath);
            ValidateImage(filePath);
            ValidateFileSize(filePath);
        }

        private void ValidateImage(string filePath)
        {
            if (!filePath.EndsWith(".png") && !filePath.EndsWith(".jpg"))
            {
                throw new Exception();
            }
        }

        private void ValidatePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception();
            }
        }

        public void ValidateFileSize(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Length > MAX_FILE_SIZE)
            {
                throw new Exception();
            }
        }

    }
}
