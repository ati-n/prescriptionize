using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescribeAPI.Models;

namespace PrescribeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionItemsController : ControllerBase
    {
        private readonly PrescriptionContext _context;

        public PrescriptionItemsController(PrescriptionContext context)
        {
            _context = context;
        }

        // GET: api/PrescriptionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionItemDTO>>> GetPrescriptionItems()
        {
          if (_context.PrescriptionItems == null)
          {
              return NotFound();
          }
            return await _context.PrescriptionItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/PrescriptionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionItemDTO>> GetPrescriptionItem(long id)
        {
          if (_context.PrescriptionItems == null)
          {
              return NotFound();
          }
            var prescriptionItem = await _context.PrescriptionItems.FindAsync(id);

            if (prescriptionItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(prescriptionItem);
        }

        // PUT: api/PrescriptionItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescriptionItem(long id, PrescriptionItemDTO prescriptionItemDTO)
        {
            if (id != prescriptionItemDTO.Id)
            {
                return BadRequest();
            }

            var prescriptionItem = await _context.PrescriptionItems.FindAsync(id);
            if (prescriptionItem == null)
            {
                return NotFound();
            }

            prescriptionItem.Name = prescriptionItemDTO.Name;
            prescriptionItem.IsPrescribed = prescriptionItemDTO.IsPrescribed;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PrescriptionItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionItemDTO>> PostPrescriptionItem(PrescriptionItemDTO prescriptionItemDTO)
        {
            var prescriptionItem = new PrescriptionItem
            {
                IsPrescribed = prescriptionItemDTO.IsPrescribed,
                Name = prescriptionItemDTO.Name
            };

            _context.PrescriptionItems.Add(prescriptionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrescriptionItem), new { id = prescriptionItem.Id }, ItemToDTO(prescriptionItem));
        }


        // DELETE: api/PrescriptionItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescriptionItem(long id)
        {
            if (_context.PrescriptionItems == null)
            {
                return NotFound();
            }

            var prescriptionItem = await _context.PrescriptionItems.FindAsync(id);
            if (prescriptionItem == null)
            {
                return NotFound();
            }

            _context.PrescriptionItems.Remove(prescriptionItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrescriptionItemExists(long id)
        {
            return (_context.PrescriptionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static PrescriptionItemDTO ItemToDTO(PrescriptionItem prescriptionItem) =>
            new PrescriptionItemDTO
            {
                Id = prescriptionItem.Id,
                Name = prescriptionItem.Name,
                IsPrescribed = prescriptionItem.IsPrescribed
            };
    }
}
