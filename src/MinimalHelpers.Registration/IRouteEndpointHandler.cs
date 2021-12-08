using Microsoft.AspNetCore.Routing;

namespace MinimalRegistration;

public interface IRouteEndpointHandler
{
    public void Map(IEndpointRouteBuilder app);
}
