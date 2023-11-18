using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Foodzilla.Kernel.Commons.Api;

[ApiController]
[Produces("application/json")]
[Route("/api/OFood/[controller]/[action]/")]
//[Route("/api/OFood/v{version:apiVersion}/[controller]/[action]")]

public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected int UserId => !string.IsNullOrEmpty(HttpContext.Request.Headers?["sub"].ToString()) ||
                         !string.IsNullOrWhiteSpace(HttpContext.Request.Headers?["sub"].ToString())
    ? int.Parse(HttpContext.Request.Headers?["sub"].ToString()!)
    : 0;

    protected string? UserName => !string.IsNullOrEmpty(HttpContext.Request.Headers?["username"].ToString()) ||
                                  !string.IsNullOrWhiteSpace(HttpContext.Request.Headers?["username"].ToString())
        ? HttpContext.Request.Headers?["username"].ToString()
        : null;

    protected string[]? Roles => !string.IsNullOrEmpty(HttpContext.Request.Headers?["role"].ToString()) ||
                                 !string.IsNullOrWhiteSpace(HttpContext.Request.Headers?["role"].ToString())
        ? HttpContext.Request.Headers?["role"].ToString().ToLower().Split(",")
        : null;

    //protected int UserId
    //{
    //    get
    //    {
    //        var value = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
    //        return value != null ? int.Parse(value) : 0;
    //    }
    //}
}
