using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Geoprofs.Models
{
    public class AbsenceType
    {
        [Key]
        public int absenceTypeId { get; set; }
        public string absenceName { get; set; }
    }
}
