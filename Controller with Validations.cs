using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldApi.Data;
using WorldApi.Models;

namespace WorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationDbContextcs _dbContext;

        public CountryController(ApplicationDbContextcs dbContextcs)
        {
            _dbContext = dbContextcs;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Country>> GetAll()
        {
            var countries = _dbContext.Countries.ToList();
            if (countries == null)
            {
                return NoContent();
            }
            return Ok(countries);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Country> GetById(int id)
        {
            var country = _dbContext.Countries.Find(id);
            if (country == null)
            {
                return NoContent();
            }
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Country> Create([FromBody] Country country)
        {
            var result = _dbContext.Countries.AsQueryable().Where(x => x.Name.ToLower().Trim() == country.Name.ToLower().Trim()).Any();
            if (result)
            {
                return Conflict("Data exist in Database ");
            }

            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
            return CreatedAtAction("GetById",new {id = country.Id,country}); // creates and then displays the data
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Country> Update(int id,[FromBody] Country country)
        {
            if(country == null || id != country.Id)
            {
                return BadRequest();
            }

            var countryFromDb = _dbContext.Countries.Find(id);
            if(countryFromDb == null)
            {
                return NotFound();
            }
            countryFromDb.Name = country.Name;
            countryFromDb.SubName = country.SubName;
            countryFromDb.CountryCode = country.CountryCode;
            
            _dbContext.Countries.Update(countryFromDb);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var country = _dbContext.Countries.Find(id);
            if(country == null)
            {
                return NotFound();
            }
            _dbContext.Countries.Remove(country);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
