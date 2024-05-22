using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Weather_App.Data;
using Weather_App.Models;

namespace Weather_App.Services
{
    public interface IFavoriteService
    {
        Task Add(string location, float latitude, float longitude, ClaimsPrincipal user);
        Task Remove(string location, ClaimsPrincipal user);
        Task<List<Favorite>> GetAll(ClaimsPrincipal user);
    }
    public class FavoriteService:IFavoriteService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public FavoriteService(UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task Add(string location, float latitude, float longitude, ClaimsPrincipal user)
        {
            var favorite = new Favorite
            {
                UserId = _userManager.GetUserId(user),
                Location = location,
                Latitude = latitude,
                Longitude = longitude
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            return;
        }
        public async Task Remove(string location, ClaimsPrincipal user)
        {
            var favorite = await _context.Favorites
                .Where(f => f.UserId == _userManager.GetUserId(user) && f.Location == location)
                .FirstOrDefaultAsync();
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            return;
        }
        public async Task<List<Favorite>> GetAll(ClaimsPrincipal user)
        {
            var favorite= await _context.Favorites
                .Where(f => f.UserId == _userManager.GetUserId(user))
                .ToListAsync();
            return favorite;
        }

    }
}
