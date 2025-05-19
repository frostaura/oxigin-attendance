using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface ITimesheetService
    {
        Task<IEnumerable<Timesheet>> GenerateTimesheetsAsync(DateTime startDate, DateTime endDate);
    }

    public class TimesheetService : ITimesheetService
    {
        private readonly List<Timesheet> _timesheets = new List<Timesheet>();

        public async Task<IEnumerable<Timesheet>> GenerateTimesheetsAsync(DateTime startDate, DateTime endDate)
        {
            var timesheets = _timesheets.Where(t => t.Date >= startDate && t.Date <= endDate);
            return await Task.FromResult(timesheets);
        }

        // Method to add a timesheet entry
        public void AddTimesheet(Timesheet timesheet)
        {
            _timesheets.Add(timesheet);
        }
    }
}
