namespace eHealthLink.API.Model
{
    public class PatientConsultLoop
    {
        public string ConsultationId { get; set; }
        public string PatientId { get; set; }
        public string ConsultationDate { get; set; }
        public string? ConsultationType { get; set; }
        public string? Reason { get; set; }
        public string? DoctorName { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? VisitLoop { get; set; }      
        public int? LoopCount { get; set; }

    }
}
