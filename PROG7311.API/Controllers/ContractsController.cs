using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311.API.DTOs;
using PROG7311_POE.API.DTOs;
using PROG7311_POE.Data;
using PROG7311_POE.Factories;
using PROG7311_POE.Models;
using PROG7311_POE.Service;

namespace PROG7311_POE.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ContractNotificationService _notificationService;

        public ContractsController(AppDbContext context)
        {
            _context = context;
            _notificationService = new ContractNotificationService();
        }

        // GET: api/contracts
        [HttpGet]
        public async Task<IActionResult> GetAll(DateTime? startDate, DateTime? endDate, ContractStatus? status)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate);

            if (status.HasValue)
                query = query.Where(c => c.Status == status);

            var result = await query.Select(c => new ContractReadDto
            {
                ContractId = c.ContractId,
                ClientId = c.ClientId,
                ClientName = c.Client.Name,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Status = c.Status,
                ServiceLevel = c.ServiceLevel,
                SignedAgreementPath = c.SignedAgreementPath
            }).ToListAsync();

            return Ok(result);
        }

        // GET: api/contracts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

        // POST: api/contracts
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ContractFormRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var factory = ContractFactory.GetContract(request.ServiceLevel);

            var contract = factory.Create(
                request.ClientId,
                request.StartDate,
                request.EndDate,
                request.Status
            );

            contract.ServiceLevel = request.ServiceLevel;

            // FILE UPLOAD
            if (request.File != null && request.File.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + "_" + request.File.FileName;
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await request.File.CopyToAsync(stream);

                contract.SignedAgreementPath = "/files/" + fileName;
            }

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            _notificationService.NotifyContractChange(contract);


            return Ok(contract);
        }

        // PATCH: api/contracts/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ContractStatus status)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            contract.Status = status;

            await _context.SaveChangesAsync();

            return Ok(contract);
        }

        // DELETE: api/contracts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DOWNLOAD FILE
        [HttpGet("download")]
        public IActionResult Download(string path)
        {
            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                path.TrimStart('/'));

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, "application/pdf", Path.GetFileName(fullPath));
        }
    }
}