using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models.EntityFramework;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly SeriesDbContext _context;

        public UtilisateursController(SeriesDbContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        //[ActionName("GetAllUtilisateurs")]
        [ProducesResponseType(typeof(IEnumerable<Utilisateur>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

        // GET: api/Utilisateurs/5
        [HttpGet]
        [ActionName("GetById")]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(Utilisateur), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetById(int id)
        {
            //var utilisateur = await _context.Utilisateurs.FindAsync(id);
            var utilisateur = await _context.Utilisateurs.SingleOrDefaultAsync(u => u.UtilisateurId == id);

            if (utilisateur == null)
            {
                return NotFound("Id utilisateur inconnu");
            }

            return utilisateur;
        }

        [HttpGet]
        [ActionName("GetByEmail")]
        [Route("[action]/{email}")]
        [ProducesResponseType(typeof(Utilisateur), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Utilisateur>> GetByEmail(string email)
        {
            var utilisateur = await _context.Utilisateurs.SingleOrDefaultAsync(u => u.Mail == email);

            if (utilisateur == null)
            {
                return NotFound("Id utilisateur inconnu");
            }

            return utilisateur;
        }
        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != utilisateur.UtilisateurId)
            {
                return BadRequest();
            }

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
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

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(Utilisateur), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.UtilisateurId }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUtilisateur(int id)
        //{
        //    var utilisateur = await _context.Utilisateurs.FindAsync(id);
        //    if (utilisateur == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Utilisateurs.Remove(utilisateur);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Utilisateur>> Patch(int id, [FromBody] JsonPatchDocument<Utilisateur> patchEntity)
        {
            //var utilisateur = await _context.Utilisateurs.SingleOrDefaultAsync(u => u.UtilisateurId == id);

            var entity = await _context.Utilisateurs.FirstOrDefaultAsync(user => user.UtilisateurId == id);

            if (entity == null)
            {
                return NotFound();
            }

            patchEntity.ApplyTo(entity, ModelState); // Must have Microsoft.AspNetCore.Mvc.NewtonsoftJson installed

            return entity;
        }
        private bool UtilisateurExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.UtilisateurId == id);
        }
    }
}
