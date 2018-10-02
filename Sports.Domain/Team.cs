using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;


namespace Sports.Domain
{
    public class Team
    {
        const int MAX_FILE_SIZE = 2000000;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; private set; }

        public Sport Sport { get; set; }
        public ICollection<Match> LocalMatches { get; set; }
        public ICollection<Match> VisitorMatches { get; set; }
        public ICollection<Favorite> Favorites { get; set; }

        public void IsValid()
        {
            IsValidName();
        }

        private void IsValidName()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                throw new InvalidEmptyTextFieldException(EmptyField.EMPTY_NAME_MESSAGE);
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
                throw new InvalidTeamImageException(ImageTeamValidation.INVALID_FILE_EXTENSION_MESSAGE);
            }
        }

        private void ValidatePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new InvalidTeamImageException(ImageTeamValidation.INVALID_FILE_PATH_MESSAGE);
            }
        }

        public void ValidateFileSize(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Length > MAX_FILE_SIZE)
            {
                throw new InvalidTeamImageException(ImageTeamValidation.INVALID_FILE_PATH_MESSAGE);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }
            else
            {
                return this.Name.Equals(((Team)obj).Name);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void UpdateData(Team team)
        {
            this.Name = IgnoreWhiteSpace(this.Name, team.Name);
            this.IsValid();
        }

        private string IgnoreWhiteSpace(string originalText, string updatedText)
        {
            if (string.IsNullOrWhiteSpace(updatedText))
            {
                return originalText;
            }
            return updatedText;
        }

    }
}
