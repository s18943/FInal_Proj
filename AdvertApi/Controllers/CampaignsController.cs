using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvertApi.Models;
using AdvertApi.Services;
using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;

namespace AdvertApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private ICompanyDbService _dbService;

        public CampaignsController(ICompanyDbService dbService)
        {
            _dbService = (EFCompanyDbService)dbService;
        }

        // GET: api/Campaigns
        [HttpGet]
        public IActionResult GetCampaign()
        {
            return  Ok(_dbService.GetCampaigns());
        }

        // POST: api/Campaigns
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("create")]
        public IActionResult PostCampaign(NewCampaignRequest request)
        {
            try
            {
                var response = _dbService.NewCampaign(request);

                return Created("GetCampaign", request);
            }
            catch (NoBuildingsException exc)
            {
                return NotFound(exc);
            }
            catch (BuildingsTooFarException exc)
            {
                return BadRequest(exc);
            }

        }
    }
}
