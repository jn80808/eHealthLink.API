using eHealthLink.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;

namespace eHealthLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalDataController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IOptions<DatabaseSetting> dbsetting;
        private readonly string _connectionString;
        private readonly string _schema;


        //Constructor 
        public PersonalDataController(IConfiguration configuration, IOptions<DatabaseSetting> dbsettings)
        {
            _schema = dbsettings.Value.Schema;
            this.configuration = configuration;
            this.dbsetting = dbsetting;
            _connectionString = configuration.GetConnectionString("eHealthLinkConnectionStrings");
        }



        //HttpGet
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Operation", "GetAll");

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                var results = new List<Dictionary<string, object>>();
                                while (await reader.ReadAsync())
                                {
                                    var row = new Dictionary<string, object>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    }
                                    results.Add(row);
                                }
                                return Ok(results);
                            }
                            else
                            {
                                return Ok(new { message = "No records found" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        //HttpGet ID
        [HttpGet("GenerateId")]
        public async Task<IActionResult> GenerateId()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "GENERATEID"); 

                    await connection.OpenAsync();

                    object result = await command.ExecuteScalarAsync(); // retrieve single value

                    if (result != null && result != DBNull.Value)
                    {
                        return Ok(result.ToString());
                    }
                    else
                    {
                        return NotFound("No generated ID found");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }



        //HttpPost 
        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] PersonalData model)
        {
            model.FirstName = model.FirstName == "null" ? string.Empty : model.FirstName;
            model.MiddleName = model.MiddleName == "null" ? string.Empty : model.MiddleName;
            model.LastName = model.LastName == "null" ? string.Empty : model.LastName;
            model.PreferredName = model.PreferredName == "null" ? string.Empty : model.PreferredName;
            if (model.BirthDate == null || model.BirthDate == DateTime.MinValue) model.BirthDate = null;
            model.Age = model.Age ?? 0;
            model.Sex = model.Sex == "null" ? string.Empty : model.Sex;
            model.SocialSecurityNumber = model.SocialSecurityNumber == "null" ? string.Empty : model.SocialSecurityNumber;
            model.PreferredLanguage = model.PreferredLanguage == "null" ? string.Empty : model.PreferredLanguage;
            model.Ethnicity = model.Ethnicity == "null" ? string.Empty : model.Ethnicity;
            model.MaritalStatus = model.MaritalStatus == "null" ? string.Empty : model.MaritalStatus;
            model.Occupation = model.Occupation == "null" ? string.Empty : model.Occupation;
            model.Employer = model.Employer == "null" ? string.Empty : model.Employer;
            model.Email = model.Email == "null" ? string.Empty : model.Email;
            model.PhoneHome = model.PhoneHome == "null" ? string.Empty : model.PhoneHome;
            model.PhoneWork = model.PhoneWork == "null" ? string.Empty : model.PhoneWork;
            model.PhoneCell = model.PhoneCell == "null" ? string.Empty : model.PhoneCell;
            model.PreferredContact = model.PreferredContact == "null" ? string.Empty : model.PreferredContact;
            model.AddressStreet = model.AddressStreet == "null" ? string.Empty : model.AddressStreet;
            model.AddressCity = model.AddressCity == "null" ? string.Empty : model.AddressCity;
            model.AddressState = model.AddressState == "null" ? string.Empty : model.AddressState;
            model.AddressZip = model.AddressZip == "null" ? string.Empty : model.AddressZip;
            model.BillingStreet = model.BillingStreet == "null" ? string.Empty : model.BillingStreet;
            model.BillingCity = model.BillingCity == "null" ? string.Empty : model.BillingCity;
            model.BillingState = model.BillingState == "null" ? string.Empty : model.BillingState;
            model.BillingZip = model.BillingZip == "null" ? string.Empty : model.BillingZip;
            model.Operation = model.Operation == "null" ? string.Empty : model.Operation;
            if (model.CreatedAt == null || model.CreatedAt == DateTime.MinValue) model.CreatedAt = null;
            model.PatientId = model.PatientId == "null" ? string.Empty : model.PatientId;


            using (var connection = new SqlConnection(_connectionString))

                try
                {
                    using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                    {
                        await connection.OpenAsync();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Operation", "INSERT");
                        command.Parameters.AddWithValue("@FirstName", model.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", model.MiddleName);
                        command.Parameters.AddWithValue("@LastName", model.LastName);
                        command.Parameters.AddWithValue("@PreferredName", model.PreferredName);
                        command.Parameters.AddWithValue("@BirthDate", model.BirthDate);
                        command.Parameters.AddWithValue("@Age", model.Age);
                        command.Parameters.AddWithValue("@Sex", model.Sex);
                        command.Parameters.AddWithValue("@SocialSecurityNumber", model.SocialSecurityNumber);
                        command.Parameters.AddWithValue("@PreferredLanguage", model.PreferredLanguage);
                        command.Parameters.AddWithValue("@Ethnicity", model.Ethnicity);
                        command.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
                        command.Parameters.AddWithValue("@Occupation", model.Occupation);
                        command.Parameters.AddWithValue("@Employer", model.Employer);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@PhoneHome", model.PhoneHome);
                        command.Parameters.AddWithValue("@PhoneWork", model.PhoneWork);
                        command.Parameters.AddWithValue("@PhoneCell", model.PhoneCell);
                        command.Parameters.AddWithValue("@PreferredContact", model.PreferredContact);
                        command.Parameters.AddWithValue("@AddressStreet", model.AddressStreet);
                        command.Parameters.AddWithValue("@AddressCity", model.AddressCity);
                        command.Parameters.AddWithValue("@AddressState", model.AddressState);
                        command.Parameters.AddWithValue("@AddressZip", model.AddressZip);
                        command.Parameters.AddWithValue("@BillingStreet", model.BillingStreet);
                        command.Parameters.AddWithValue("@BillingCity", model.BillingCity);
                        command.Parameters.AddWithValue("@BillingState", model.BillingState);
                        command.Parameters.AddWithValue("@BillingZip", model.BillingZip);
                        command.Parameters.AddWithValue("@Operation", model.Operation);
                        command.Parameters.AddWithValue("@CreatedAt", model.CreatedAt);
                        command.Parameters.AddWithValue("@PatientId", model.PatientId);

                        await command.ExecuteNonQueryAsync();

                    }
                    return Ok("Data Inserted Successfully");
                }

                catch (Exception ex)
                {
                    //Handle execeptions appropiately (logging,error response,etc.)
                    return StatusCode(500, "Internal Server Error" + ex.Message);
                }

        }



        [HttpPut("Update/{PatientId}")]
        public async Task<IActionResult> UpdatePatientData(string PatientId, [FromBody] PersonalData model)
        {
            // Default value mapping
            model.FirstName = model.FirstName == "null" ? string.Empty : model.FirstName;
            model.MiddleName = model.MiddleName == "null" ? string.Empty : model.MiddleName;
            model.LastName = model.LastName == "null" ? string.Empty : model.LastName;
            model.PreferredName = model.PreferredName == "null" ? string.Empty : model.PreferredName;
            if (model.BirthDate == null || model.BirthDate == DateTime.MinValue) model.BirthDate = null;
            model.Age = model.Age ?? 0;
            model.Sex = model.Sex == "null" ? string.Empty : model.Sex;
            model.SocialSecurityNumber = model.SocialSecurityNumber == "null" ? string.Empty : model.SocialSecurityNumber;
            model.PreferredLanguage = model.PreferredLanguage == "null" ? string.Empty : model.PreferredLanguage;
            model.Ethnicity = model.Ethnicity == "null" ? string.Empty : model.Ethnicity;
            model.MaritalStatus = model.MaritalStatus == "null" ? string.Empty : model.MaritalStatus;
            model.Occupation = model.Occupation == "null" ? string.Empty : model.Occupation;
            model.Employer = model.Employer == "null" ? string.Empty : model.Employer;
            model.Email = model.Email == "null" ? string.Empty : model.Email;
            model.PhoneHome = model.PhoneHome == "null" ? string.Empty : model.PhoneHome;
            model.PhoneWork = model.PhoneWork == "null" ? string.Empty : model.PhoneWork;
            model.PhoneCell = model.PhoneCell == "null" ? string.Empty : model.PhoneCell;
            model.PreferredContact = model.PreferredContact == "null" ? string.Empty : model.PreferredContact;
            model.AddressStreet = model.AddressStreet == "null" ? string.Empty : model.AddressStreet;
            model.AddressCity = model.AddressCity == "null" ? string.Empty : model.AddressCity;
            model.AddressState = model.AddressState == "null" ? string.Empty : model.AddressState;
            model.AddressZip = model.AddressZip == "null" ? string.Empty : model.AddressZip;
            model.BillingStreet = model.BillingStreet == "null" ? string.Empty : model.BillingStreet;
            model.BillingCity = model.BillingCity == "null" ? string.Empty : model.BillingCity;
            model.BillingState = model.BillingState == "null" ? string.Empty : model.BillingState;
            model.BillingZip = model.BillingZip == "null" ? string.Empty : model.BillingZip;
            model.Operation = model.Operation == "null" ? "UPDATE" : model.Operation;
            if (model.CreatedAt == null || model.CreatedAt == DateTime.MinValue) model.CreatedAt = null;
            model.PatientId = string.IsNullOrWhiteSpace(model.PatientId) ? PatientId : model.PatientId;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Operation", model.Operation);
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", model.MiddleName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@PreferredName", model.PreferredName);
                    command.Parameters.AddWithValue("@BirthDate", model.BirthDate);
                    command.Parameters.AddWithValue("@Age", model.Age);
                    command.Parameters.AddWithValue("@Sex", model.Sex);
                    command.Parameters.AddWithValue("@SocialSecurityNumber", model.SocialSecurityNumber);
                    command.Parameters.AddWithValue("@PreferredLanguage", model.PreferredLanguage);
                    command.Parameters.AddWithValue("@Ethnicity", model.Ethnicity);
                    command.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
                    command.Parameters.AddWithValue("@Occupation", model.Occupation);
                    command.Parameters.AddWithValue("@Employer", model.Employer);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@PhoneHome", model.PhoneHome);
                    command.Parameters.AddWithValue("@PhoneWork", model.PhoneWork);
                    command.Parameters.AddWithValue("@PhoneCell", model.PhoneCell);
                    command.Parameters.AddWithValue("@PreferredContact", model.PreferredContact);
                    command.Parameters.AddWithValue("@AddressStreet", model.AddressStreet);
                    command.Parameters.AddWithValue("@AddressCity", model.AddressCity);
                    command.Parameters.AddWithValue("@AddressState", model.AddressState);
                    command.Parameters.AddWithValue("@AddressZip", model.AddressZip);
                    command.Parameters.AddWithValue("@BillingStreet", model.BillingStreet);
                    command.Parameters.AddWithValue("@BillingCity", model.BillingCity);
                    command.Parameters.AddWithValue("@BillingState", model.BillingState);
                    command.Parameters.AddWithValue("@BillingZip", model.BillingZip);
                    command.Parameters.AddWithValue("@CreatedAt", model.CreatedAt);
                    command.Parameters.AddWithValue("@PatientId", model.PatientId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                return Ok("Data Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }



        [HttpDelete("Delete/{PatientId}")]
        public async Task<IActionResult> DeletePatientRecord(string PatientId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                    {
                        await connection.OpenAsync();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Operation", "Delete");
                        command.Parameters.AddWithValue("@PatientId", PatientId);

                        await command.ExecuteNonQueryAsync();
                    }

                    return Ok("Data Deleted Successfully");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal Server Error: " + ex.Message);
                }
            }
        }

        //at LOOP 
        [HttpPost("PostDataFull")]
        public async Task<IActionResult> PostDataFull([FromBody] PersonalData model)
        {
            // Replace "null" strings with empty strings
            model.FirstName = model.FirstName == "null" ? string.Empty : model.FirstName;
            model.MiddleName = model.MiddleName == "null" ? string.Empty : model.MiddleName;
            model.LastName = model.LastName == "null" ? string.Empty : model.LastName;
            model.PreferredName = model.PreferredName == "null" ? string.Empty : model.PreferredName;
            if (model.BirthDate == null || model.BirthDate == DateTime.MinValue) model.BirthDate = null;
            model.Age ??= 0;
            model.Sex = model.Sex == "null" ? string.Empty : model.Sex;
            model.SocialSecurityNumber = model.SocialSecurityNumber == "null" ? string.Empty : model.SocialSecurityNumber;
            model.PreferredLanguage = model.PreferredLanguage == "null" ? string.Empty : model.PreferredLanguage;
            model.Ethnicity = model.Ethnicity == "null" ? string.Empty : model.Ethnicity;
            model.MaritalStatus = model.MaritalStatus == "null" ? string.Empty : model.MaritalStatus;
            model.Occupation = model.Occupation == "null" ? string.Empty : model.Occupation;
            model.Employer = model.Employer == "null" ? string.Empty : model.Employer;
            model.Email = model.Email == "null" ? string.Empty : model.Email;
            model.PhoneHome = model.PhoneHome == "null" ? string.Empty : model.PhoneHome;
            model.PhoneWork = model.PhoneWork == "null" ? string.Empty : model.PhoneWork;
            model.PhoneCell = model.PhoneCell == "null" ? string.Empty : model.PhoneCell;
            model.PreferredContact = model.PreferredContact == "null" ? string.Empty : model.PreferredContact;
            model.AddressStreet = model.AddressStreet == "null" ? string.Empty : model.AddressStreet;
            model.AddressCity = model.AddressCity == "null" ? string.Empty : model.AddressCity;
            model.AddressState = model.AddressState == "null" ? string.Empty : model.AddressState;
            model.AddressZip = model.AddressZip == "null" ? string.Empty : model.AddressZip;
            model.BillingStreet = model.BillingStreet == "null" ? string.Empty : model.BillingStreet;
            model.BillingCity = model.BillingCity == "null" ? string.Empty : model.BillingCity;
            model.BillingState = model.BillingState == "null" ? string.Empty : model.BillingState;
            model.BillingZip = model.BillingZip == "null" ? string.Empty : model.BillingZip;
            model.Operation = model.Operation == "null" ? string.Empty : model.Operation;
            if (model.CreatedAt == null || model.CreatedAt == DateTime.MinValue) model.CreatedAt = null;
            model.PatientId = model.PatientId == "null" ? string.Empty : model.PatientId;

            string insertedPatientId = string.Empty;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Operation", "INSERT");
                        command.Parameters.AddWithValue("@FirstName", model.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", model.MiddleName);
                        command.Parameters.AddWithValue("@LastName", model.LastName);
                        command.Parameters.AddWithValue("@PreferredName", model.PreferredName);
                        command.Parameters.AddWithValue("@BirthDate", (object?)model.BirthDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Age", model.Age ?? 0);
                        command.Parameters.AddWithValue("@Sex", model.Sex);
                        command.Parameters.AddWithValue("@SocialSecurityNumber", model.SocialSecurityNumber);
                        command.Parameters.AddWithValue("@PreferredLanguage", model.PreferredLanguage);
                        command.Parameters.AddWithValue("@Ethnicity", model.Ethnicity);
                        command.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
                        command.Parameters.AddWithValue("@Occupation", model.Occupation);
                        command.Parameters.AddWithValue("@Employer", model.Employer);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@PhoneHome", model.PhoneHome);
                        command.Parameters.AddWithValue("@PhoneWork", model.PhoneWork);
                        command.Parameters.AddWithValue("@PhoneCell", model.PhoneCell);
                        command.Parameters.AddWithValue("@PreferredContact", model.PreferredContact);
                        command.Parameters.AddWithValue("@AddressStreet", model.AddressStreet);
                        command.Parameters.AddWithValue("@AddressCity", model.AddressCity);
                        command.Parameters.AddWithValue("@AddressState", model.AddressState);
                        command.Parameters.AddWithValue("@AddressZip", model.AddressZip);
                        command.Parameters.AddWithValue("@BillingStreet", model.BillingStreet);
                        command.Parameters.AddWithValue("@BillingCity", model.BillingCity);
                        command.Parameters.AddWithValue("@BillingState", model.BillingState);
                        command.Parameters.AddWithValue("@BillingZip", model.BillingZip);
                        command.Parameters.AddWithValue("@CreatedAt", (object?)model.CreatedAt ?? DBNull.Value);
                        //command.Parameters.AddWithValue("@PatientId", model.PatientId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                insertedPatientId = reader["PatientId"]?.ToString() ?? string.Empty;
                            }
                        }
                    }

                    // Insert Loop records if present
                    if (model.loopData != null && model.loopData.Count > 0)
                    {
                        foreach (var loop in model.loopData)
                        {
                            using (var loopCommand = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                            {
                                loopCommand.CommandType = CommandType.StoredProcedure;

                                loopCommand.Parameters.AddWithValue("@Operation", "INSERT_CONSULTATION");
                                loopCommand.Parameters.AddWithValue("@ConsultationId", loop.ConsultationId ?? string.Empty);
                                //loopCommand.Parameters.AddWithValue("@PatientId", insertedPatientId);
                                loopCommand.Parameters.AddWithValue("@ConsultationDate", (object?)loop.ConsultationDate ?? DBNull.Value);
                                loopCommand.Parameters.AddWithValue("@ConsultationType", loop.ConsultationType ?? string.Empty);
                                loopCommand.Parameters.AddWithValue("@Reason", loop.Reason ?? string.Empty);
                                loopCommand.Parameters.AddWithValue("@DoctorName", loop.DoctorName ?? string.Empty);
                                loopCommand.Parameters.AddWithValue("@Notes", loop.Notes ?? string.Empty);
                                loopCommand.Parameters.AddWithValue("@CreatedAt", (object?)loop.CreatedAt ?? DBNull.Value);
                                loopCommand.Parameters.AddWithValue("@VisitLoop", loop.VisitLoop ?? string.Empty);
                                loopCommand.Parameters.AddWithValue("@LoopCount", loop.LoopCount ?? 0);


                                await loopCommand.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }

                return Ok("Data Inserted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("GetDataFull/{patientId}")]
        public async Task<IActionResult> GetDataFull(string? patientId)
        {
            try
            {
                PersonalData personalData = null;
                List<PatientConsultLoop> loopList = new List<PatientConsultLoop>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Get main patient record
                    string patientQuery = $"SELECT * FROM {_schema}.Tbl_Patients WHERE PatientId = @PatientId";
                    using (var cmd = new SqlCommand(patientQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@PatientId", patientId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                personalData = new PersonalData
                                {
                                    PatientId = reader["PatientId"]?.ToString(),
                                    FirstName = reader["FirstName"]?.ToString(),
                                    MiddleName = reader["MiddleName"]?.ToString(),
                                    LastName = reader["LastName"]?.ToString(),
                                    PreferredName = reader["PreferredName"]?.ToString(),
                                    BirthDate = reader["BirthDate"] != DBNull.Value
                                            ? Convert.ToDateTime(reader["BirthDate"])
                                            : (DateTime?)null,
                                    Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0,
                                    Sex = reader["Sex"]?.ToString(),
                                    SocialSecurityNumber = reader["SocialSecurityNumber"]?.ToString(),
                                    PreferredLanguage = reader["PreferredLanguage"]?.ToString(),
                                    Ethnicity = reader["Ethnicity"]?.ToString(),
                                    MaritalStatus = reader["MaritalStatus"]?.ToString(),
                                    Occupation = reader["Occupation"]?.ToString(),
                                    Employer = reader["Employer"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    PhoneHome = reader["PhoneHome"]?.ToString(),
                                    PhoneWork = reader["PhoneWork"]?.ToString(),
                                    PhoneCell = reader["PhoneCell"]?.ToString(),
                                    PreferredContact = reader["PreferredContact"]?.ToString(),
                                    AddressStreet = reader["AddressStreet"]?.ToString(),
                                    AddressCity = reader["AddressCity"]?.ToString(),
                                    AddressState = reader["AddressState"]?.ToString(),
                                    AddressZip = reader["AddressZip"]?.ToString(),
                                    BillingStreet = reader["BillingStreet"]?.ToString(),
                                    BillingCity = reader["BillingCity"]?.ToString(),
                                    BillingState = reader["BillingState"]?.ToString(),
                                    BillingZip = reader["BillingZip"]?.ToString(),
                                    Operation = reader["Operation"]?.ToString(),
                                    CreatedAt = reader["CreatedAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(reader["CreatedAt"]),

                                    PatientIdNumber = reader["PatientIdNumber"]?.ToString()
                                };
                            }
                        }
                    }

                    //  Get consultations for the patient
                    if (personalData != null)
                    {
                        string consultQuery = $"SELECT * FROM {_schema}.Tbl_PatientConsultations WHERE PatientId = @PatientId";
                        using (var cmd = new SqlCommand(consultQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@PatientId", patientId);

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    loopList.Add(new PatientConsultLoop
                                    {
                                        ConsultationId = reader["ConsultationId"]?.ToString(),
                                        PatientId = reader["PatientId"]?.ToString(),
                                        ConsultationDate = reader["ConsultationDate"] != DBNull.Value ? Convert.ToDateTime(reader["ConsultationDate"]): (DateTime?)null,
                                        ConsultationType = reader["ConsultationType"]?.ToString(),
                                        Reason = reader["Reason"]?.ToString(),
                                        DoctorName = reader["DoctorName"]?.ToString(),
                                        Notes = reader["Notes"]?.ToString(),
                                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : (DateTime?)null,
                                        VisitLoop = reader["VisitLoop"]?.ToString(),
                                        LoopCount = reader["LoopCount"] != DBNull.Value ? Convert.ToInt32(reader["LoopCount"]) : 0
                                    });
                                }
                            }
                        }
                    }
                }

                // Return combined result
                if (personalData != null)
                {
                    personalData.loopData = loopList;
                    return Ok(personalData);
                }
                else
                {
                    return NotFound($"No patient found with ID {patientId}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


    }
}











