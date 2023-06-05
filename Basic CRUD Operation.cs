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
        public ActionResult<IEnumerable<Country>> GetAll()
        {
            return _dbContext.Countries.ToList();
        }
        [HttpGet("{id:int}")]
        public ActionResult<Country> GetById(int id)
        {
            return _dbContext.Countries.Find(id);
        }

        [HttpPost]
        public ActionResult<Country> Create([FromBody] Country country)
        {
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public ActionResult<Country> update([FromBody] Country country)
        {
            _dbContext.Countries.Update(country);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteById(int id)
        {
            var country = _dbContext.Countries.Find(id);
            _dbContext.Countries.Remove(country);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
