using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Exceptions;
using AdvertApi.Models;
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
                throw new LoginAlreadyExistsException("You can't login take");
            Client client = new Client { FirstName = request.FirstName, LastName = request.LastName, Email = request.Email, Phone = request.Phone, Login = request.Login, Password = BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.SaltRevision.Revision2) };
            _context.Client.Add(client);
            _context.SaveChanges();
            AddClientResponse resp = new AddClientResponse { FirstName = client.FirstName, LastName = client.LastName, Email = client.Email, Phone = client.Phone, Login = client.Login };
            return resp;
        }

        public ICollection<GetCampaignsResponse> GetCampaigns()
        {
            List<GetCampaignsResponse> resp = new List<GetCampaignsResponse>();

            var campaigns = _context.Campaign
                .Join(_context.Client, ca => ca.IdClient, cl => cl.IdClient, (ca, cl) =>
                      new GetCampaignsResponse { Campaign = ca, Client = cl })
                .OrderByDescending(c => c.Campaign.StartDate).ToList();
            return campaigns;
        }

        public LoginClientResponse LoginClient(LoginClientRequest request)
        {
            var userExists = _context.Client.Where(c => c.Login.Equals(request.Login)).FirstOrDefault();
            if (userExists == null)
                throw new AuthLoginException("Incorrect login");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, userExists.Password))
                throw new AuthPasswordException("Incorrect password");

            Guid refreshToken = Guid.NewGuid();
            userExists.RefreshToken = refreshToken.ToString();
            _context.Update(userExists);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userExists.Login),
                new Claim(ClaimTypes.Name, userExists.FirstName+" "+userExists.LastName),
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
            return new LoginClientResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.ToString()
            };
        }

        public NewCampaignResponse NewCampaign(NewCampaignRequest request)
        {
            if (_context.Building.Count() < 2)
                throw new NoBuildingsException("No buildings in the database");

            var B1 = _context.Building.Where(b => b.IdBuilding.Equals(request.FromIdBuilding)).FirstOrDefault();
            var B2 = _context.Building.Where(b => b.IdBuilding.Equals(request.ToIdBuilding)).FirstOrDefault();

            if (!B1.Street.Equals(B2.Street))
                throw new BuildingsTooFarException("The buildings are not next to each other");

            Campaign campaign = new Campaign { IdClient = request.IdClient, StartDate = request.StartDate, EndDate = request.EndDate, PricePerSquareMeter = request.PricePerSquareMeter, FromIdBuilding = request.FromIdBuilding, ToIdBuilding = request.ToIdBuilding };
            _context.Campaign.Add(campaign);
            _context.SaveChanges();
            var buildings = _context.Building.Where(b => b.StreetNumber >= B1.StreetNumber && b.StreetNumber <= B2.StreetNumber).OrderBy(b => b.StreetNumber).ToList();

            List<Banner> banners = _service.calculateArea(buildings, campaign, request.PricePerSquareMeter);

            _context.Banner.AddRange(banners.First(), banners.Last());
            _context.SaveChanges();

            return new NewCampaignResponse { Campaign = campaign, Banner1 = banners.First(), Banner2 = banners.Last() };
        }

        public TokenUpdateResponse UpdateToken(TokenUpdateRequest request)
        {
            var client = _context.Client.Where(c => c.RefreshToken.Equals(request.RefreshToken)).FirstOrDefault();
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
