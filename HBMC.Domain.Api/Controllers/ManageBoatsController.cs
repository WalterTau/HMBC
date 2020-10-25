using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HBMC.Domain.Api.Controllers
{
    [Route("api/manageboats")]
    [ApiController]
    public class ManageBoatsController : ControllerBase
    {
        private readonly IBoatsService _boatsService;

        public ManageBoatsController(IBoatsService boatsService)
        {
            _boatsService = boatsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Boats>), 200)]
        [MapToApiVersion(RouteHelper.V1.ApiNumber)]
        public async Task<IActionResult> Get([FromHeader] string orgId, [FromHeader] string userId)
        {
            return Ok(await applicationService.GetAllApplications(orgId, userId));
        }
    }
}
