using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Geoprofs.Models
{
    public class Position
    {
        [Key]   // rows voor database zetten
        public int positionId { get; set; }
        public string positionName { get; set; }
    }
}
