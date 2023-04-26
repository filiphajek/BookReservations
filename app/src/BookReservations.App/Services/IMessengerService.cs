namespace BookReservations.App.Services;

public interface IMessengerService
{
    void Register<TMessage>(Action<TMessage> action);
    void Register<TMessage>(Func<TMessage, Task> func);
    void Send<TMessage>(TMessage message);
    void UnRegister<TMessage>();
}
