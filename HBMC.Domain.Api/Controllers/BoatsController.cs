using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBMC.Domain.Api.Helper;
using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HBMC.Domain.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoatsController : Controller
    {
        private readonly IBoatsService _boatsService;
        private readonly ILogger<Boats> _logger;

        public BoatsController(IBoatsService boatsService , ILogger<Boats> logger)
        {
            _boatsService = boatsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Boats>> Get()
        {
            return await _boatsService.GetAllBoats();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string Id)
        {
            return Ok(await _boatsService.GetById(Id));
        }
    }
}
