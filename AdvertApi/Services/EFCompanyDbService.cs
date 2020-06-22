using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Exceptions;
using AdvertApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AdvertApi.Services
{
    public class EFCompanyDbService : ICompanyDbService
    {
        private readonly s18943Context _context;
        public IConfiguration Configuration { get; set; }
        private ICalculationsService _service;
        public EFCompanyDbService(s18943Context context, IConfiguration configuration, ICalculationsService service)
        {
            _context = context;
            _service = service;
            Configuration = configuration;
        }
        public AddClientResponse AddClient(AddClientRequest request)
        {
            if (_context.Client.Where(c => c.Login.Equals(request.Login)).FirstOrDefault() != null)
                throw new LoginAlreadyExistsException("You can't take this login");

            Client client = new Client { 
                FirstName = request.FirstName,
                LastName = request.LastName, 
                Email = request.Email, 
                Phone = request.Phone, 
                Login = request.Login, 
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.SaltRevision.Revision2) 
            };

            AddClientResponse response = new AddClientResponse { FirstName = client.FirstName,
                LastName = client.LastName, 
                Email = client.Email,
                Phone = client.Phone,
                Login = client.Login
            };

            _context.Client.Add(client);
            _context.SaveChanges();

            return response;
        }

        public ICollection<CampaignsResponse> GetCampaigns()
        {
            List<CampaignsResponse> resp = new List<CampaignsResponse>();

            var campaigns = _context.Campaign
                .Include(cam => cam.IdClientNavigation)
                .Select(c => new CampaignsResponse
                {
                    Campaign = c,
                    Client = c.IdClientNavigation
                })
                .OrderByDescending(c => c.Campaign.StartDate).ToList();
            return campaigns;
        }

        public LoginClientResponse LoginClient(LoginClientRequest request)
        {
            var user = _context.Client.Where(c => c.Login.Equals(request.Login)).FirstOrDefault();
            if (user == null) throw new AuthenticationException("Incorrect login");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))throw new AuthenticationException("Incorrect password");

            string Token = Guid.NewGuid().ToString();
            user.RefreshToken = Token;
            _context.Update(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Name, user.FirstName+" "+user.LastName),
                new Claim(ClaimTypes.Role, "client")
            };

            var JwtToken = new JwtSecurityToken
            (
                issuer: "s18943",
                audience: "Clients",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["S2ecretK1ey"])),
                SecurityAlgorithms.HmacSha256)
            );
            _context.SaveChanges();
            return new LoginClientResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                RefreshToken = Token
            };
        }

        public NewCampaignResponse NewCampaign(NewCampaignRequest request)
        {
            if (_context.Building.Count() < 2)
                throw new NotEnoughBuildingsException("Not enough buildings");

            var building1 = _context.Building
                .Where(b => b.IdBuilding.Equals(request.FromIdBuilding))
                .FirstOrDefault();
            var building2 = _context.Building
                .Where(b => b.IdBuilding.Equals(request.ToIdBuilding))
                .FirstOrDefault();

            if (!building1.Street.Equals(building2.Street))
                throw new BuildingsTooFarException("Buildings are too far");

            Campaign campaign = new Campaign 
            { 
                IdClient = request.IdClient, 
                StartDate = request.StartDate, 
                EndDate = request.EndDate, 
                PricePerSquareMeter = request.PricePerSquareMeter, 
                FromIdBuilding = request.FromIdBuilding, 
                ToIdBuilding = request.ToIdBuilding 
            };
            var buildings = _context.Building
                .Where(b => b.StreetNumber >= building1.StreetNumber && b.StreetNumber <= building2.StreetNumber)
                .OrderBy(b => b.StreetNumber).ToList();

            List<Banner> banners = _service.calculateArea(buildings, campaign, request.PricePerSquareMeter);

            _context.Campaign.Add(campaign);
            _context.Banner.AddRange(banners.First(), banners.Last());
            _context.SaveChanges();

            return new NewCampaignResponse { Campaign = campaign, Banner1 = banners.First(), Banner2 = banners.Last() };
        }

        public TokenUpdateResponse UpdateToken(TokenUpdateRequest request)
        {
            var client = _context.Client
                .Where(c => c.RefreshToken.Equals(request.RefreshToken))
                .FirstOrDefault();
            if (client == null)
                throw new RefreshTokenIncorrectException("Refresh token incorrect");

            Guid refreshToken = Guid.NewGuid();
            client.RefreshToken = refreshToken.ToString();
            _context.Update(client);

            var claims = new[]
             {
                new Claim(ClaimTypes.NameIdentifier, client.Login),
                new Claim(ClaimTypes.Name, client.FirstName+" "+client.LastName),
                new Claim(ClaimTypes.Role, "client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["S2ecretK1ey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "s18943",
                audience: "Clients",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );
            _context.SaveChanges();
            return new TokenUpdateResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.ToString()
            };
        }
    }
}
