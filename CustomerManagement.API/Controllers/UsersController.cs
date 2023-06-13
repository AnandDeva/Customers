
using CustomerManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> Get()
    {
        var users = await _usersService.GetAllUsers();
        if (users.Any())
        {
            return Ok(users);
        }
        return NotFound();
    }
}
