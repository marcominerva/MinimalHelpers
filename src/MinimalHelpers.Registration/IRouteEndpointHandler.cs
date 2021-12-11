using Microsoft.AspNetCore.Routing;

namespace MinimalHelpers.Registration;

public interface IRouteEndpointHandler
{
    public void Map(IEndpointRouteBuilder app);
}
