using Cinema.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/test-data")]
public class TestDataController : ControllerBase
{
    private readonly InMemoryCinemaStore _store;

    public TestDataController(InMemoryCinemaStore store)
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