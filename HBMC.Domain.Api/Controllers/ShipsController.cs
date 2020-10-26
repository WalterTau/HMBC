using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HBMC.Domain.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipsController : ControllerBase
    {
        private readonly IShipsService _shipsService;
        private readonly ILogger<Ships> _logger;

        public ShipsController(IShipsService shipsService, ILogger<Ships> logger)
        {
            _shipsService = shipsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Ships>> Get()
        {
            return await _shipsService.GetAllBoats();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string Id)
        {
            return Ok(await _shipsService.GetById(Id));
        }
    }
}
