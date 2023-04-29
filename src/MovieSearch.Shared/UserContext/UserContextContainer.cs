namespace MovieSearch.Shared.UserContext
{
    public static class UserContextContainer
    {
        private static AsyncLocal<UserContext?>? _asyncLocalUserContext;

        public static UserContext? AsyncLocalUserContext
        {
            get => _asyncLocalUserContext?.Value;
            set
            {
                if (_asyncLocalUserContext == null)
                {
                    if (value == null)
                    {
                        return;
                    }
                    // Interlocked.CompareExchanges() method insures that assign operation will be atomic.
                    // The code was taken from Thread.CurrentPrincipal implementation of dotnet core.
                    // https://github.com/dotnet/runtime/blob/00813dfd305778ea1cfadc2ef0028de05630e535/src/libraries/System.Private.CoreLib/src/System/Threading/Thread.cs#L150
                    Interlocked.CompareExchange(ref _asyncLocalUserContext, new AsyncLocal<UserContext>(), null);
                }

                _asyncLocalUserContext!.Value = value!;
            }
        }
    }
}