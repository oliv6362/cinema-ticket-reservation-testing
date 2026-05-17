using Microsoft.AspNetCore.Mvc;
using Cinema.Core.Interfaces;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/test-data")]
public class TestDataController : ControllerBase
{
    private readonly ICinemaStore _store;

    public TestDataController(ICinemaStore store)
    {
        _store = store;
    }

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _store.Reset();

        return NoContent();
    }
}