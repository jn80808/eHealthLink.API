using eHealthLink.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
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
        [HttpGet]
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
        [HttpGet("GenerateId/{ID}")]
        public async Task<IActionResult> GenerateId(string? ID)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "GENERATEDID");
                    command.Parameters.AddWithValue("@ID", string.IsNullOrWhiteSpace(ID) ? DBNull.Value : ID);

                    await connection.OpenAsync();

                    object result = await command.ExecuteScalarAsync(); //Retrive Single Value 

                    if (result != null)
                    {
                        return Ok(result.ToString);
                    }
                    else
                    {
                        return NotFound("No generated ID Found");
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error" + ex.Message);
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
            model.BirthDate = model.BirthDate == "null" ? string.Empty : model.BirthDate;
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
            model.CreatedAt = model.CreatedAt == "null" ? string.Empty : model.CreatedAt;
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



        [HttpPut("{PatientId}")]
        public async Task<IActionResult> UpdatePatientData(string PatientId, [FromBody] PersonalData model)
        {
            // Default value mapping
            model.FirstName = model.FirstName == "null" ? string.Empty : model.FirstName;
            model.MiddleName = model.MiddleName == "null" ? string.Empty : model.MiddleName;
            model.LastName = model.LastName == "null" ? string.Empty : model.LastName;
            model.PreferredName = model.PreferredName == "null" ? string.Empty : model.PreferredName;
            model.BirthDate = model.BirthDate == "null" ? string.Empty : model.BirthDate;
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
            model.CreatedAt = model.CreatedAt == "null" ? string.Empty : model.CreatedAt;
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



        [HttpDelete("{PatientId}")]
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





    }
}











