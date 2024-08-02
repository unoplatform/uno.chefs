using System.Collections.Immutable;
using System.Text.Json;
using Chefs.Data;

namespace ChefsApi.Server.Apis;

/// <summary>
/// User Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly string _usersFilePath = "Data/AppData/Users.json";
    private Guid? _currentUserId = new Guid("3c896419-e280-40e7-8552-240635566fed");

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of users.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = LoadData<List<UserData>>(_usersFilePath);
        return Ok(users.ToImmutableList());
    }

    /// <summary>
    /// Authenticates a user with email and password.
    /// </summary>
    /// <param name="loginRequest">The login request containing email and password.</param>
    /// <returns>The user ID if authentication is successful, otherwise Unauthorized.</returns>
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] LoginRequest loginRequest)
    {
        var users = LoadData<List<UserData>>(_usersFilePath);
        var user = users.FirstOrDefault(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

        if (user is null)
        {
            return Unauthorized();
        }

        _currentUserId = user.Id;
        return Ok(user.Id);
    }

    /// <summary>
    /// Retrieves a list of popular creators, excluding the current user.
    /// </summary>
    /// <returns>A list of popular creators.</returns>
    [HttpGet("popular-creators")]
    public IActionResult GetPopularCreators()
    {
        var users = LoadData<List<UserData>>(_usersFilePath);
        var popularCreators = users.Where(x => x.Id != _currentUserId).ToImmutableList();
        return Ok(popularCreators);
    }

    /// <summary>
    /// Retrieves the current user.
    /// </summary>
    /// <returns>The current user.</returns>
    [HttpGet("current")]
    public IActionResult GetCurrent()
    {
        var users = LoadData<List<UserData>>(_usersFilePath);
        var currentUser = users.FirstOrDefault(u => u.Id == _currentUserId);

        if (currentUser is null)
        {
            return NotFound("Current user not found");
        }

        if (!currentUser.IsCurrent)
        {
            currentUser.IsCurrent = true;
        }

        return Ok(currentUser);
    }

    /// <summary>
    /// Updates the current user.
    /// </summary>
    /// <param name="user">The updated user data.</param>
    /// <returns>The updated user, or NotFound if the user does not exist.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] UserData user)
    {
        var users = LoadData<List<UserData>>(_usersFilePath);
        var userIndex = users.FindIndex(u => u.Id == _currentUserId);

        if (userIndex != -1)
        {
            users[userIndex] = new UserData
            {
                Id = user.Id,
                UrlProfileImage = user.UrlProfileImage,
                FullName = user.FullName,
                Description = user.Description,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                Followers = user.Followers,
                Following = user.Following,
                Recipes = user.Recipes
            };

            System.IO.File.WriteAllText(_usersFilePath, JsonSerializer.Serialize(users));
            return Ok(users[userIndex]);
        }

        return NotFound("User not found");
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The user, or NotFound if the user does not exist.</returns>
    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var users = LoadData<List<UserData>>(_usersFilePath);
        var user = users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User not found");
        }

        return Ok(user);
    }

    /// <summary>
    /// Loads data from a specified JSON file.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="filePath">The file path of the JSON file.</param>
    /// <returns>The loaded data.</returns>
    private T LoadData<T>(string filePath)
    {
        var json = System.IO.File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
