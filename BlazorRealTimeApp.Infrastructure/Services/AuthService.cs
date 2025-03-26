using BCrypt.Net;
using BlazorRealTimeApp.Application.Common.Interfaces;
using BlazorRealTimeApp.Application.Models;
using BlazorRealTimeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlazorRealTimeApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<AuthService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(LoginRequest request)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user != null)
            {
                return false;
            }
            // Jelsz� hashel�se
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = hash
                };
                context.Users.Add(newUser);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Hiba t�rt�nt a jelsz� hashel�se k�zben");
                return false;
            }
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return false;
            }

            // Ellen�rizd a jelsz�t (hash �sszehasonl�t�s)
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword("tipo01");


                var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                return isPasswordValid;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Hiba t�rt�nt a jelsz� ellen�rz�se k�zben");
                return false;
            }
        }
    }
}
