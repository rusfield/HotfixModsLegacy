﻿namespace HotfixMods.Infrastructure.Handlers
{
    public interface IExceptionHandler
    {
        public void Handle(Exception exception);
        public void RegisterCallback(Action<Exception> callback);
    }
}
