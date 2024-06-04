using CRUDtasks.Data;
using CRUDtasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// GET: Get all persons
        /// </summary>
        [HttpGet]
        [Route("GetPersons")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var persons = await _context.Persons.Include(p => p.Department).ToListAsync();
            if (!persons.Any())
            {
                return NotFound("No persons found");
            }
            return Ok(persons);
        }

        /// <summary>
        /// GET: Get a person by ID
        /// </summary>
        [HttpGet]
        [Route("GetPersonById/{id}")]
        public async Task<ActionResult<Person>> GetPersonById(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return person;
        }

        /// <summary>
        /// PUT: Update an existing person
        /// </summary>
        [HttpPut]
        [Route("PutPerson/{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// POST: Create a new person
        /// </summary>
        [HttpPost]
        [Route("PostPerson")]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetPerson", new { id = person.Id }, person);
        }

        /// <summary>
        /// DELETE: Delete a person by ID
        /// </summary>
        [HttpDelete]
        [Route("DeletePerson/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
