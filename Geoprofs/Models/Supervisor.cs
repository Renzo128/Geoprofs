using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Geoprofs.Models
{
    public class Supervisor
    {
        [Key]
        public int supervisorId { get; set; }
        [ForeignKey("coworkerId")]
        public int Coworker { get; set; }
    }
}
