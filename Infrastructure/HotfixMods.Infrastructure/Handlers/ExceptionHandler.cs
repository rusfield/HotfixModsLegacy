namespace HotfixMods.Infrastructure.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        Action<Exception>? action;
        public void Handle(Exception exception)
        {
            if (action != null)
                action(exception);
            else
                throw exception;

        }

        public void RegisterCallback(Action<Exception> callback)
        {
            action = callback;
        }
    }
}
