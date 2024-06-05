using CRUDtasks.Data;
using CRUDtasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDtasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetPersons")]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _context.Persons.Include(p => p.Department).ToListAsync();
            if (!persons.Any())
            {
                return NotFound("No persons found");
            }
            return Ok(persons);
        }

        [HttpGet]
        [Route("GetPersonById/{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound($"Person with ID {id} not found");
            }
            return Ok(person);
        }

        [HttpPut]
        [Route("PutPerson/{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest("ID mismatch between route and request body");
            }

            _context.Update(person);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound($"Person with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }

            return Ok($"Person with ID {id} updated successfully");
        }

        [HttpPost]
        [Route("PostPerson")]
        public async Task<IActionResult> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
        }

        [HttpDelete]
        [Route("DeletePerson/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound($"Person with ID {id} not found");
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return Ok($"Person with ID {id} deleted successfully");
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
