[ApiController]
public class InfraController : ControllerBase
{
    private readonly IConfiguration _config;

    public InfraController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpGet("/auth-check")]
    public IActionResult AuthCheck()
    {
        return Ok(new {
            KeyLength = (_config["Jwt:Key"]?.Length ?? 0),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        });
    }
}
