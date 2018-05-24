using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoWebAPI.Models;

namespace DemoWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly SampledatabaseContext _context;

        public EmployeesController(SampledatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<Employee> GetEmployee()
        {
            return _context.Employee;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.EmpId == id);
            var sampledatabaseContext = _context.Bank.Include(b => b.Emp).Where(m => m.EmpId == id);
            employee.Bank = await sampledatabaseContext.ToListAsync();
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmpId)
            {
                return BadRequest();
            }

            _context.Employee.Update(employee);
            foreach (var item in employee.Bank)
            {
                if (item.BankId <= 0) _context.Bank.Add(item);
                else
                    _context.Bank.Update(item);

            }



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employee.Add(employee);
            if (employee.Bank != null)
            {
                foreach (var item in employee.Bank)
                {
                    item.EmpId = employee.EmpId;
                    _context.Bank.Add(item);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(employee.EmpId);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }
            var bankContext = _context.Bank.Include(b => b.Emp);
            var bank = bankContext.Where(e => e.EmpId == id);
            if (bank != null)
            {
                foreach (var item in bank)
                {
                    _context.Bank.Remove(item);
                }
            }
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmpId == id);
        }

        [HttpGet]
        public IActionResult Error() {
            return BadRequest();
        }
    }
}