using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;


namespace Sports.Domain
{
    public class Competitor
    {
        const int MAX_FILE_SIZE = 2000000;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; private set; }
        public Sport Sport { get; set; }
        public int Score { get; set; }
        

        public void IsValid()
        {
            IsValidName();
            ValidScore();
        }

        private void ValidScore()
        {
            if (Score < 0)
            {
                throw new InvalidCompetitorScoreException("");
            }
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
            if (filePath != null)
            {
                ValidateFile(filePath);
                byte[] bytePicture = File.ReadAllBytes(filePath);
                this.Picture = System.Text.Encoding.UTF8.GetString(bytePicture);
            }
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
                throw new InvalidCompetitorImageException(ImageCompetitorValidation.INVALID_FILE_EXTENSION_MESSAGE);
            }
        }

        private void ValidatePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new InvalidCompetitorImageException(ImageCompetitorValidation.INVALID_FILE_PATH_MESSAGE);
            }
        }

        public void ValidateFileSize(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Length > MAX_FILE_SIZE)
            {
                throw new InvalidCompetitorImageException(ImageCompetitorValidation.INVALID_FILE_PATH_MESSAGE);
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
                return this.Name.Equals(((Competitor)obj).Name);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void UpdateData(Competitor competitor)
        {
            this.Name = IgnoreWhiteSpace(this.Name, competitor.Name);
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
