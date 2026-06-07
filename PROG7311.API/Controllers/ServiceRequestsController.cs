using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE.Data;
using PROG7311_POE.Models;
using PROG7311_POE.Service;


namespace PROG7311_POE.Controllers
{
    [ApiController]
    [Route("api/servicerequests")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/servicerequests
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .Select(s => new
                {
                    s.ServiceRequestId,
                    s.Description,
                    s.CostUSD,
                    s.CostZAR,
                    s.ContractId,
                    Contract = new
                    {
                        s.Contract.ContractId,
                        s.Contract.Status,
                        Client = new
                        {
                            s.Contract.Client.ClientId,
                            s.Contract.Client.Name
                        }
                    }
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/servicerequests/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (request == null)
                return NotFound();

            return Ok(request);
        }

        // POST: api/servicerequests
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.ContractId == request.ContractId);

            if (contract == null)
                return BadRequest("Contract not found.");

            // BUSINESS RULE 1: Only active contracts allowed
            if (contract.Status == ContractStatus.Expired ||
                contract.Status == ContractStatus.OnHold)
            {
                return BadRequest("Cannot create Service Request. Contract is not active.");
            }

            // BUSINESS RULE 2: Currency conversion
            decimal rate = 18.5m;
            request.CostZAR = request.CostUSD * rate;

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }

        // DELETE: api/servicerequests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}