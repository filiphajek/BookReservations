namespace BookReservations.App.Services;

public class MessengerService : IMessengerService
{
    private readonly IDictionary<Type, IList<Delegate>> registeredActions;

    public MessengerService() => registeredActions = new Dictionary<Type, IList<Delegate>>();

    public void Register<TMessage>(Action<TMessage> action)
    {
        Type key = typeof(TMessage);
        if (registeredActions.TryGetValue(key, out var list))
        {
            list.Add(action);
            return;
        }
        registeredActions[key] = new List<Delegate> { action };
    }

    public void Register<TMessage>(Func<TMessage, Task> func)
    {
        Type key = typeof(TMessage);
        if (registeredActions.TryGetValue(key, out var list))
        {
            list.Add(func);
            return;
        }
        registeredActions[key] = new List<Delegate> { func };
    }

    public void UnRegister<TMessage>()
    {
        if (registeredActions.TryGetValue(typeof(TMessage), out var list))
        {
            list.Clear();
        }
    }

    public void Send<TMessage>(TMessage message)
    {
        if (registeredActions.TryGetValue(typeof(TMessage), out var actions))
        {
            foreach (var action in actions.Select(action => action as Action<TMessage>).Where(action => action != null))
            {
                action(message);
            }
        }
    }
}
