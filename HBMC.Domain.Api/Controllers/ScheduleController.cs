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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<Schedule> _logger;

        public ScheduleController(IScheduleService scheduleService, ILogger<Schedule> logger)
        {
            _scheduleService = scheduleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Schedule>> Get()
        {
            return await _scheduleService.GetAllSchedules();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string Id)
        {
            return Ok(await _scheduleService.GetById(Id));
        }
    }
}
