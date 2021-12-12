using Microsoft.AspNetCore.Routing;

namespace MinimalHelpers.Registration;

public interface IEndpointRouteHandler
{
    public void Map(IEndpointRouteBuilder app);
}
