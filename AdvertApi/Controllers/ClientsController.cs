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
using AdvertApi.Exceptions;

namespace AdvertApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ICompanyDbService _dbService;

        public ClientsController(ICompanyDbService dbService)
        {
            _dbService = (EFCompanyDbService)dbService;
        }

        // POST: api/Clients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("login")]
        public  IActionResult LoginClient(LoginClientRequest request)
        {
            try
            {
                 return Ok(_dbService.LoginClient(request));
            }
            catch (AuthenticationException exc)
            {
                return BadRequest(exc);
            }
        }

        [HttpPost]
        public IActionResult AddClient(AddClientRequest request)
        {
            try
            {
                var response = _dbService.AddClient(request);
                return Created("", response);
            }
            catch (LoginAlreadyExistsException exc)
            {
                return BadRequest(exc);
            }
        }
        [HttpPost("update")]
        public IActionResult UpdateTokenClient(TokenUpdateRequest request)
        {
            try
            {
                return Ok(_dbService.UpdateToken(request));
            }
            catch (RefreshTokenIncorrectException exc)
            {
                return NotFound(exc);
            }
        }
    }
}
