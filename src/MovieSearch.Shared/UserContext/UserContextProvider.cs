namespace MovieSearch.Shared.UserContext
{
    public interface IUserContextProvider
    {
        IUserContext UserContext { get; }
    }
    
    public class UserContextProvider : IUserContextProvider
    {
        public UserContext UserContext => UserContextContainer.AsyncLocalUserContext ??
                                           (UserContextContainer.AsyncLocalUserContext = UserContext.Empty())!;

        IUserContext IUserContextProvider.UserContext => UserContext;

        public void Initialize(UserContext userContext)
        {
            UserContext?.Properties.Clear();
            foreach (var userContextProperty in userContext.Properties)
            {
                UserContext?.Properties.Add(userContextProperty);
            }
        }
    }
}