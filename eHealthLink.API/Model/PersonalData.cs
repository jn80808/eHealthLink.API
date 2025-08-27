using System;

namespace eHealthLink.API.Model
{
    public class PersonalData
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PreferredName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? Ethnicity { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Occupation { get; set; }
        public string? Employer { get; set; }
        public string? Email { get; set; }
        public string? PhoneHome { get; set; }
        public string? PhoneWork { get; set; }
        public string? PhoneCell { get; set; }
        public string? PreferredContact { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressZip { get; set; }
        public string? BillingStreet { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingState { get; set; }
        public string? BillingZip { get; set; }
        public string? Operation { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? PatientId { get; set; }
        public string? PatientIdNumber { get; set; }

        public List<PatientConsultLoop>loopData { get; set; }
    }
}
