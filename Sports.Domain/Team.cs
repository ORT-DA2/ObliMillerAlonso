using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sports.Domain
{
    public class Team
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public byte[] Picture { get; private set; }

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
            this.Picture = File.ReadAllBytes(filePath);
        }

    }
}
