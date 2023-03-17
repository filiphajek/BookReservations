using System.Text.RegularExpressions;

namespace BookReservations.Api.Controllers;

public abstract class MiniController
{
    private static readonly Regex controllerRegex = new(nameof(MiniController), RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public void AddGroup(RouteGroupBuilder apiBuilder)
    {
        var name = UseApiName();
        var endpoints = apiBuilder.MapGroup(name.ToLower()).WithTags(name);
        AddEnpoints(endpoints);
    }

    protected virtual string UseApiName()
    {
        return controllerRegex.Replace(GetType().Name, "");
    }

    protected abstract void AddEnpoints(RouteGroupBuilder endpoints);
}
