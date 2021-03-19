using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Data.Database;
using SamuraiApp.Domain;

namespace SamuraiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamuraisSoCController : ControllerBase
    {
        private readonly BusinessDataLogic _businessLogic;

        // private readonly SamuraiContext _context;

        public SamuraisSoCController(BusinessDataLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }

        // GET: api/Samurais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Samurai>>> GetSamurais()
        {
            var samurais = await _businessLogic.GetAllSamurais();
            return Ok(samurais);
        }

        // GET: api/Samurais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Samurai>> GetSamurai(int id)
        {
            var samurai = await _businessLogic.GetSamuraiById(id);
            if (samurai != null)
                return Ok(samurai);
            else
                return NotFound();
        }

        // PUT: api/Samurais/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Samurai object deserialized from json
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSamurai(int id, Samurai samurai)
        {
            if (id != samurai.Id)
            {
                return BadRequest();
            }

            try
            {
                bool result = await _businessLogic.UpdateSamurai(samurai);
                if (result == false)
                    return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return NoContent();
        }

        // POST: api/Samurais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Samurai>> PostSamurai(Samurai samurai)
        {
            var addedSamurai = await _businessLogic.AddSamurai(samurai);

            return CreatedAtAction("GetSamurai", new { id = addedSamurai.Id }, addedSamurai);
        }

        // DELETE: api/Samurais/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Samurai>> DeleteSamurai(int id)
        {
            return await _businessLogic.DeleteSamurai(id);
        }
    }
}
