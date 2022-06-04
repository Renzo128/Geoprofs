namespace Geoprofs.Models
{
    public class AbsenceRequest
    {
        public int Id { get; set; }
        public Coworker Coworker { get; set; }
        public string AbsenceStart { get; set; }
        public string AbsenceEnd { get; set; }
        public string Note { get; set; }
        public AbsenceType absenceType { get; set; }
    }
}
