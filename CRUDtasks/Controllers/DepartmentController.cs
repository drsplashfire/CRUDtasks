using CRUDtasks.Data;
using CRUDtasks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace CRUDtasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET: api/Department - Get all departments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            if (!departments.Any())
            {
                return NotFound("No departments found");
            }
            return Ok(departments);
        }

        /// <summary>
        ///  GET: api/Department/{id} - Get a department by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}", Name = "GetDepartment")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        /// <summary>
        /// POST: api/Department - Create a new department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Include validation errors in response
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetDepartment", new { id = department.Id }, department);
        }

        /// <summary>
        /// PUT: api/Department/{id} - Update an existing department
        /// </summary>
        /// <param name="id"></param>
        /// <param name="department"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Include validation errors in response
            }

            if (id != department.Id)
            {
                return BadRequest("ID in request body doesn't match ID in URL");
            }

            var existingDepartment = await _context.Departments.Include(d => d.Persons).FirstOrDefaultAsync(d => d.Id == id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            // Update department properties
            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.Address = department.Address;

            // Update associated persons (assuming the Persons collection is replaced entirely)
            existingDepartment.Persons.Clear(); // Clear existing persons
            existingDepartment.Persons.AddRange(department.Persons); // Add new persons

            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        /// <summary>
        /// DELETE: api/Department/{id} - Delete a department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
