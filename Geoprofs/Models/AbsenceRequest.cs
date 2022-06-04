using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Geoprofs.Models
{
    public class AbsenceRequest
    {
        [Key]
        public int absenceId { get; set; }
        public int Coworker { get; set; }
        public DateTime AbsenceStart { get; set; }
        public DateTime AbsenceEnd { get; set; }
        public string Note { get; set; }
        public int absenceType { get; set; }

        public string absenceStatus { get; set; }
    }
}
