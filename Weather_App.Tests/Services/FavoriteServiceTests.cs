using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using Weather_App.Data;
using Weather_App.Services;

public class FavoriteServiceTests
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly FavoriteService _favoriteService;

    public FavoriteServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHashers = new Mock<IPasswordHasher<IdentityUser>>();
        var userValidators = new List<IUserValidator<IdentityUser>>();
        var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
        var lookupNormalizer = new Mock<ILookupNormalizer>();
        var errors = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<IdentityUser>>>();

        _userManager = new UserManager<IdentityUser>(userStoreMock.Object, options.Object, passwordHashers.Object, userValidators, passwordValidators, lookupNormalizer.Object, errors.Object, services.Object, logger.Object);

        var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(dbOptions);

        _favoriteService = new FavoriteService(_userManager, _context);
    }

    [Fact]
    public async Task AddFavorite_AddsFavorite_WhenCalled()
    {
        // Arrange
        var location = "test_location";
        var latitude = 50.760002f;
        var longitude = 15.059999f;
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "test_user_id") }));

        // Act
        await _favoriteService.AddFavorite(location, latitude, longitude, user);

        // Assert
        Assert.Single(_context.Favorites);
        var favorite = _context.Favorites.Single();
        Assert.Equal(location, favorite.Location);
        Assert.Equal(latitude, favorite.Latitude);
        Assert.Equal(longitude, favorite.Longitude);
        Assert.Equal("test_user_id", favorite.UserId);
    }

    [Fact]
    public async Task RemoveFavorite_RemovesFavorite_WhenCalled()
    {
        // Arrange
        var location = "test_location";
        var latitude = 50.760002f;
        var longitude = 15.059999f;
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "test_user_id") }));

        await _favoriteService.AddFavorite(location, latitude, longitude, user);

        // Act
        await _favoriteService.RemoveFavorite(location, user);

        // Assert
        Assert.Empty(_context.Favorites);
    }

    [Fact]
    public async Task GetFavorites_ReturnsFavorites_WhenCalled()
    {
        // Arrange
        var location1 = "test_location1";
        var latitude1 = 50.760002f;
        var longitude1 = 15.059999f;
        var location2 = "test_location2";
        var latitude2 = 51.760002f;
        var longitude2 = 16.059999f;
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "test_user_id") }));

        await _favoriteService.AddFavorite(location1, latitude1, longitude1, user);
        await _favoriteService.AddFavorite(location2, latitude2, longitude2, user);

        // Act
        var favorites = await _favoriteService.GetFavorites(user);

        // Assert
        Assert.Equal(2, favorites.Count);
        Assert.Contains(favorites, f => f.Location == location1 && f.Latitude == latitude1 && f.Longitude == longitude1 && f.UserId == "test_user_id");
        Assert.Contains(favorites, f => f.Location == location2 && f.Latitude == latitude2 && f.Longitude == longitude2 && f.UserId == "test_user_id");
    }
}
