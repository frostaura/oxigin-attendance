using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class JobAllocation
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public string Location { get; set; }
        public List<Employee> AllocatedEmployees { get; set; }
        public string EventDetails { get; set; }
        public string EventCoordinator { get; set; }
    }
}
