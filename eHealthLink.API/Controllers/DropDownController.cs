using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using eHealthLink.API.Model;
using Microsoft.Extensions.Options;

namespace eHealthLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IOptions<DatabaseSetting> dbsetting;
        private readonly string _connectionString;
        private readonly string _schema;


        //Constructor 
        public DropDownController(IConfiguration configuration, IOptions<DatabaseSetting> dbsettings)
        {
            _schema = dbsettings.Value.Schema;
            this.configuration = configuration;
            this.dbsetting = dbsetting;
            _connectionString = configuration.GetConnectionString("eHealthLinkConnectionStrings");
        }

        [HttpGet("SymptomsChange")]
        public async Task<IActionResult> GetSymptomsChange()
        {
            try
            {
                var symptoms = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "SymptomsChange");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            symptoms.Add(reader.GetString(0)); // first column
                        }
                    }
                }

                if (symptoms.Any())
                {
                    return Ok(symptoms); // returns as JSON array in Swagger
                }
                else
                {
                    return NotFound("No symptoms found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet("PrescribedMedications")]
        public async Task<IActionResult> GetPrescribedMedications()
        {
            try
            {
                var medications = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "PrescribedMedications");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            medications.Add(reader.GetString(0)); // first column "Medication"
                        }
                    }
                }

                if (medications.Any())
                {
                    return Ok(medications); // JSON array in Swagger
                }
                else
                {
                    return NotFound("No prescribed medications found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("TestsOrdered")]
        public async Task<IActionResult> GetTestsOrdered()
        {
            try
            {
                var tests = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "TestsOrdered");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tests.Add(reader.GetString(0)); // first column "TestName"
                        }
                    }
                }

                if (tests.Any())
                {
                    return Ok(tests); // JSON array for dropdown
                }
                else
                {
                    return NotFound("No tests found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("TreatmentResponse")]
        public async Task<IActionResult> GetTreatmentResponse()
        {
            try
            {
                var responses = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "TreatmentResponse");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            responses.Add(reader.GetString(0)); // first column "TreatmentResponse"
                        }
                    }
                }

                if (responses.Any())
                {
                    return Ok(responses); // JSON array in Swagger
                }
                else
                {
                    return NotFound("No treatment responses found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet("SideEffectsReported")]
        public async Task<IActionResult> GetSideEffectsReported()
        {
            try
            {
                var sideEffects = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "SideEffectsReported");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            sideEffects.Add(reader.GetString(0)); // first column "SideEffect"
                        }
                    }
                }

                if (sideEffects.Any())
                {
                    return Ok(sideEffects); // returns JSON array in Swagger
                }
                else
                {
                    return NotFound("No side effects found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("LifestyleChangesSuggested")]
        public async Task<IActionResult> GetLifestyleChangesSuggested()
        {
            try
            {
                var lifestyleChanges = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "LifestyleChangesSuggested");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lifestyleChanges.Add(reader.GetString(0)); // first column "LifestyleChange"
                        }
                    }
                }

                if (lifestyleChanges.Any())
                {
                    return Ok(lifestyleChanges); // JSON array for dropdown
                }
                else
                {
                    return NotFound("No lifestyle changes found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("ReferralMade")]
        public async Task<IActionResult> GetReferralMade()
        {
            try
            {
                var referrals = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand($"{_schema}.Prc_PersonalData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Operation", "ReferralMade");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            referrals.Add(reader.GetString(0)); // first column "Referral"
                        }
                    }
                }

                if (referrals.Any())
                {
                    return Ok(referrals); // JSON array in Swagger
                }
                else
                {
                    return NotFound("No referrals found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


    }
}
