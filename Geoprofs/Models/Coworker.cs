using System;
using System.Collections.Generic;
namespace Geoprofs.Models
{
    public class Coworker
    {
        public int medewerkerId { get; set; }
        public string medewerkerName { get; set; }
        public string medewerkerLastName { get; set; }

        public int bsn { get; set; }
        public Position position { get; set; }
        public Supervisor supervisor { get; set; }
        public string startDate { get; set; }
        public Absence absence { get; set; }
        public int vacationdays { get; set; }

    }
}
