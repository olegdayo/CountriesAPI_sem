using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountriesAPI_sem.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private const string ConnectionString =
            "Host=localhost;Port=5432;User Id=postgres;Password=postgres;Database=postgres;";

        [HttpGet("countries")]
        public async Task<IEnumerable<string>> GetCountries()
        {
            using (var dbConnection = new NpgsqlConnection(ConnectionString))
            {
                return await dbConnection.QueryAsync<string>(
                    @"SELECT DISTINCT name
                          FROM countries"
                    );
            }
        }

        [HttpGet("regions")]
        public async Task<IEnumerable<string>> GetRegions()
        {
            using (var dbConnection = new NpgsqlConnection(ConnectionString))
            {
                return await dbConnection.QueryAsync<string>(
                    @"SELECT DISTINCT region
                          FROM countries"
                );
            }
        }

        [HttpGet("countries/{name}")]
        public async Task<Country> GetCountryInfo([FromRoute] string name)
        {
            using (var dbConnection = new NpgsqlConnection(ConnectionString))
            {
                return (await dbConnection.QueryAsync<Country>(
                    @"SELECT name
                                , region
                                , subregion
                                , capital
                          FROM countries
                          WHERE name = @Name", new
                    {
                        Name = name
                    }
                )).First();
            }
        }

        [HttpGet("regions/{region}")]
        public async Task<IEnumerable<Country>> GetRegionInfo([FromRoute] string region)
        {
            using (var dbConnection = new NpgsqlConnection(ConnectionString))
            {
                return (await dbConnection.QueryAsync<Country>(
                    @"SELECT name
                                , region
                                , subregion
                                , capital
                          FROM countries
                          WHERE region = @Region", new
                    {
                        Region = region
                    }
                    )).ToList();
            }
        }
    }
}
