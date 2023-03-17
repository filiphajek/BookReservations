using BookReservations.Api.Controllers;
using System.Reflection;

namespace BookReservations.Api;

public static class ApiInstaller
{
    private static readonly List<Type> usedControllers = new();

    public static RouteGroupBuilder MapControllers(this WebApplication webApplication, string root = "api")
    {
        var apiGroup = webApplication.MapGroup(root);

        var controllers = Assembly.GetCallingAssembly().ExportedTypes
            .Where(i => typeof(MiniController).IsAssignableFrom(i) && !i.IsAbstract)
            .Where(i => !usedControllers.Contains(i))
            .Select(Activator.CreateInstance)
            .Cast<MiniController>();

        foreach (var item in controllers)
        {
            item.AddGroup(apiGroup);
        }

        usedControllers.AddRange(controllers.Select(i => i.GetType()));

        return apiGroup;
    }

    public static RouteGroupBuilder MapController<TController>(this WebApplication webApplication, string root = "")
        where TController : MiniController, new()
    {
        var apiGroup = webApplication.MapGroup(root);

        if (usedControllers.Contains(typeof(TController)))
        {
            return apiGroup;
        }

        var controller = new TController();
        controller.AddGroup(apiGroup);

        usedControllers.Add(typeof(TController));

        return apiGroup;
    }
}
