using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.Features.interfaces.services;

namespace streamer.Features.User.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResult>
    {
        private readonly UserManager<StreamerDm> _userManager;
        private readonly StreamerDbContext _appDbContext;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public LoginCommandHandler(UserManager<StreamerDm> userManager, StreamerDbContext appDbContext, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
            _appDbContext = appDbContext ?? throw new System.ArgumentNullException(nameof(appDbContext));
            
            _jwtFactory = jwtFactory ?? throw new System.ArgumentNullException(nameof(jwtFactory));
            _tokenFactory = tokenFactory ?? throw new System.ArgumentNullException(nameof(tokenFactory));
        }

        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Password))
            {
                // ensure we have a user with the given user name
                var user =await _userManager.FindByNameAsync(request.UserName);
                if (user != null)
                {
                    // validate password
                    if (await _userManager.CheckPasswordAsync(user, request.Password))
                    {
                        // generate refresh token
                        var refreshToken =request.RememberMe? _tokenFactory.GenerateToken():"";
                        if(request.RememberMe)
                            addRefreshToken(refreshToken, user.Id, request.RemoteIpAddress);

                        // generate access token
                        var identity = _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id);
                        var auth_token = await _jwtFactory.GenerateEncodedToken(user.UserName, identity);

                        user.LastLoginDate = DateTime.UtcNow;
                        await _userManager.UpdateAsync(user);
                        return new LoginCommandResult(auth_token, refreshToken);
                    }
                }
            }
            return new LoginCommandResult(new[] { new Error("login_failure", "invalid_login") });

        }

        private async void addRefreshToken(string refreshToken, Guid userId,string remoteIpAddress, double daysToExpire = 5)
        {
            var rt = new RefreshTokenDm(refreshToken, DateTime.UtcNow.AddDays(daysToExpire), userId, remoteIpAddress);
            _appDbContext.RefreshTokens.Add(rt);
            _appDbContext.SaveChanges();
        }
    }
}
