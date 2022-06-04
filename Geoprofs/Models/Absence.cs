using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Geoprofs.Models
{
    public class Absence
    {
        [Key]
        public int absenceId { get; set; }
        public int totalVacation { get; set; }
    }
}
