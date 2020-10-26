using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using HBMC.Domain.Api.Services.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HBMC.Domain.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarbourController : ControllerBase
    {
        private readonly IHarbourService _harbourService;
        private readonly ILogger<Harbor> _logger;

        public HarbourController(IHarbourService harbourService, ILogger<Harbor> logger)
        {
            _harbourService = harbourService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Harbor>> Get()
        {
            return await _harbourService.GetAllHarbour();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string Id)
        {
            return Ok(await _harbourService.GetById(Id));
        }
    }
}
