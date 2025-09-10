namespace Autorovers.Application.Abstractions.Messaging
{
    public interface ICommand<TResponse> { }
    public interface ICommand : ICommand<Unit> { }
}
