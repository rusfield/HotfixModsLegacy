namespace HotfixMods.Infrastructure.Handlers
{
    internal class DefaultExceptionHandler : IExceptionHandler
    {
        public void Handle(Exception exception)
        {
            throw exception;
        }
    }
}
