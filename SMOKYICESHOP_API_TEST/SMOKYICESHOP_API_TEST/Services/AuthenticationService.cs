using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using SMOKYICESHOP_API_TEST.DTO.Authentication;
using SMOKYICESHOP_API_TEST.Entities;
using System.Composition;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace SMOKYICESHOP_API_TEST.Services
{
    public class AuthenticationService
    {
        private readonly string _telegramKey;
        private readonly byte[] _salt;
        private readonly IConfiguration _config;
        private readonly SmokyIceDbContext _dbcontext;
        private readonly TokenService _tokenService;

        public AuthenticationService(IConfiguration config, TokenService tokenService, SmokyIceDbContext dbcontext)
        {
            _config = config;
            _telegramKey = _config["TelegramKey"];
            _salt = _config.GetSection("PassKey").Get<byte[]>(); ;
            _tokenService = tokenService;
            _dbcontext = dbcontext;
        }

        public bool IsAdmin(Guid clientId)
        {
            Client? client = _dbcontext.Clients
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == clientId);

            if (client != null)
                if (client.Role.Name == "Admin")
                    return true;

            return false;
        }

        public Guid GetClientsIdFromToken(ClaimsIdentity claimsIdentity)
        {
            ArgumentNullException.ThrowIfNull(claimsIdentity);

            var userClaims = claimsIdentity.Claims;
            return _tokenService.GetClientIdFromClaims(userClaims);
        }

        public AuthenticatedDTO AuthenticateAndGenrateToken(TelegramLoginDTO login)
        {
            if (login.TelegramKey == _telegramKey)
            {
                Client client = _dbcontext.Clients.First(x => x.ChatId == login.UserID);
                return GenerateToken(client);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public AuthenticatedDTO AuthenticateAndGenrateToken(LoginDTO login)
        {
            string encodedPassword = EncodePassword(login.Password);
            string phone = login.PhoneNumber;

            Client client = _dbcontext.Clients.First(x => x.PhoneNumber == phone && x.Password == encodedPassword);

            return GenerateToken(client);
        }

        private AuthenticatedDTO GenerateToken(Client client)
        {
            var roleName = _dbcontext.Roles.First(x => x.Id == client.RoleId).Name;

            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, client.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, client.PhoneNumber),
                new Claim(ClaimTypes.Role, roleName)
            };

            AuthenticatedDTO authenticatedCE = new AuthenticatedDTO
            {
                RefreshToken = _tokenService.GenerateRefreshToken(),
                AccessToken = _tokenService.GenerateAccessToken(claims)
            };

            SetRefreshToken(client.Id, authenticatedCE.RefreshToken);
            return authenticatedCE;
        }

        public bool RegisterClient(LoginDTO login)
        {
            try
            {
                Guid roleId = _dbcontext.Roles.First(x => x.Name == "Customer").Id;
                Client client = RegisterClient(roleId, login.PhoneNumber, EncodePassword(login.Password));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool RegisterClient(TelegramRegistrationDTO registrationCE)
        {
            if (registrationCE.TelegramKey == _telegramKey)
            {
                try
                {
                    Guid roleId = _dbcontext.Roles.First(x => x.Name == "Customer").Id;
                    Client client = RegisterClient(roleId, registrationCE.Phone, registrationCE.UserID);
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Client RegisterClient(Guid roleId, string phoneNumber, string encodedPassword)
        {
            Client? client = _dbcontext.Clients.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

            if (client != null)
            {
                if (client.Password == null)
                {
                    client.Password = encodedPassword;
                    _dbcontext.Clients.Update(client);
                    _dbcontext.SaveChanges();
                    return client;
                }

                throw new InvalidOperationException();
            }
            else
            {
                client = new Client();
                client.PhoneNumber = phoneNumber;
                client.Password = encodedPassword;
                client.RoleId = roleId;
                _dbcontext.Clients.Add(client);
                _dbcontext.SaveChanges();
                return client;
            }
        }

        public Client RegisterClient(Guid roleId, string phoneNumber, long chatId)
        {
            Client? client = _dbcontext.Clients.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

            if (client != null)
            {
                if (client.ChatId == null)
                {
                    client.ChatId = chatId;
                    _dbcontext.Clients.Update(client);
                    _dbcontext.SaveChanges();
                    return client;
                }

                throw new InvalidOperationException();
            }
            else
            {
                client = new Client();
                client.PhoneNumber = phoneNumber;
                client.ChatId = chatId;
                client.RoleId = roleId;
                _dbcontext.Clients.Add(client);
                _dbcontext.SaveChanges();
                return client;
            }
        }

        private string EncodePassword(string password)
        {
            byte[] salt = _salt;

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public AuthenticatedDTO RefreshToken(AuthenticatedDTO authenticatedCE)
        {
            AuthenticatedDTO export = new AuthenticatedDTO();
            var principal = _tokenService.GetPrincipalFromExpiredToken(authenticatedCE.AccessToken);
            Guid clientId = _tokenService.GetClientIdFromClaims(principal.Claims);

            if(IsRefreshTokenValid(clientId, authenticatedCE.RefreshToken))
            {
                export.RefreshToken = _tokenService.GenerateRefreshToken();
                export.AccessToken = _tokenService.GenerateAccessToken(principal.Claims);

                SetRefreshToken(clientId, export.RefreshToken);
            }
            else
            {
                throw new InvalidOperationException();
            }

            return export;
        }

        public void SetRefreshToken(Guid clientId, string token)
        {
            DateTime expiration = DateTime.Now.AddDays(14);
            RefreshToken entity;
            try
            {
                entity = _dbcontext.RefreshTokens.First(x => x.ClientId == clientId);
                entity.Token = token;
                entity.Expiration = expiration;
            }
            catch (InvalidOperationException)
            {
                entity = new RefreshToken();
                entity.ClientId = clientId;
                entity.Token = token;
                entity.Expiration = expiration;
                _dbcontext.RefreshTokens.Add(entity);
            }
            _dbcontext.SaveChanges();
        }

        public bool IsRefreshTokenValid(Guid clientId, string token)
        {
            try
            {
                RefreshToken refreshToken = _dbcontext.RefreshTokens.First(x => x.ClientId == clientId);

                if (refreshToken.Token == token && refreshToken.Expiration >= DateTime.Now)
                    return true;

                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool RevokeToken(Guid clientId)
        {
            try
            {
                RefreshToken refreshToken = _dbcontext.RefreshTokens.First(x => x.ClientId == clientId);
                _dbcontext.RefreshTokens.Remove(refreshToken);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
