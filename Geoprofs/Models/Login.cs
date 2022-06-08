using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Geoprofs.Models
{
    public class Login
    {   // rows voor database zetten
        [Key]
        public int loginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [ForeignKey("coworkerId")]
        public int Coworker { get; set; }

        internal object ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
