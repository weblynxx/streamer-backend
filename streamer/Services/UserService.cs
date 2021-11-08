using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.db.Database.Helpers;

namespace streamer.Services
{
    public interface IUserService
    {
        StreamerDm Authenticate(string username, string password);
        IEnumerable<StreamerDm> GetAll();
        StreamerDm Account(Guid id);

        StreamerDm AccountWithPassword(Guid id);
        
        void CreateAccount(StreamerDm user);
        StreamerDm FindByEmail(string email);
        StreamerDm FindByLogin(string loginProvider, string ProviderKey);
    }
    
    public class UserService : IUserService
    {
        private readonly StreamerDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, StreamerDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }

        public StreamerDm Authenticate(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public StreamerDm Account(Guid id)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Id == id);

            // return null if user not found
            if (user == null)
                return null;
            
            // remove password before returning
            user.Password = null;

            return user;
        }

        public StreamerDm AccountWithPassword(Guid id)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Id == id);

            // return null if user not found
            return user ?? null;
        }

        public void CreateAccount(StreamerDm newUser)
        {
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChangesAsync();
        }

        public StreamerDm FindByEmail(string email) //await userManager.FindByEmailAsync(email);
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Email == email);

            // return null if user not found
            if (user == null)
                return null;

            // remove password before returning
            user.Password = null;

            return user;
        }

        public StreamerDm FindByLogin(string loginProvider, string ProviderKey) //await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        {
            return null;
        }

        public IEnumerable<StreamerDm> GetAll()
        {
            var users = _dbContext.Users.ToList();
            // return users without passwords
            return users.Select(x => { x.Password = null; return x; } );
        }

    }
}
