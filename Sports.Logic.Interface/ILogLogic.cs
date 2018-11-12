using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Logic.Interface
{
    public interface ILogLogic
    {
        ICollection<string> GetBetweenDates(DateTime startDate, DateTime endDate);
        void AddEntry(string entry, string username, DateTime date);
        void CleanLog();
    }
}
