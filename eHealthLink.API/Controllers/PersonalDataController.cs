using eHealthLink.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

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
        public async Task<IActionResult> GenerateId(string ID)
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
                return StatusCode (500, "Internal Server Error" + ex.Message);
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
            model.BirthDate = model.BirthDate == "null" ? null : DateTime.TryParse(model.BirthDate, out var bd) ? bd : null;
            model.Age = model.Age == "null" ? null : int.age : null;
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
            model.CreatedAt = string.Equals(model.CreatedAt, "null", StringComparison.OrdinalIgnoreCase) ? string.Empty : model.CreatedAt;
            model.PatientId = string.Equals(model.PatientId, "null", StringComparison.OrdinalIgnoreCase) ? string.Empty : model.PatientId;



        }















    }
}











