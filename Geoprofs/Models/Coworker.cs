﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Geoprofs.Models
{
    public class Coworker
    {   // rows voor database zetten
        [Key]
        public int coworkerId { get; set; }
        public string CoworkerName { get; set; }
        public string coworkerLastname { get; set; }

        public int bsn { get; set; }
        public int position { get; set; }
        public int supervisor { get; set; }
        public DateTime startDate { get; set; }

        public int vacationdays { get; set; }
        [ForeignKey("position")]

        public Position Position { get; set; }  // data uit andere databases halen
        public ICollection<AbsenceRequest> AbsenceRequest { get; set; }

    }
}
