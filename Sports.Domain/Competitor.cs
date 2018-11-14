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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public Sport Sport { get; set; }
        

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
