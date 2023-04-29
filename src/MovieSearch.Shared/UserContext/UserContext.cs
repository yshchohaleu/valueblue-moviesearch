using System.Collections.ObjectModel;

namespace MovieSearch.Shared.UserContext
{
    public interface IUserContext
    {
        ReadOnlyDictionary<string, object> Properties { get; }
    }
    
    public class UserContext : IUserContext
    {
        private UserContext()
        {
        }

        public IDictionary<string, object> Properties { get; private init; } = new Dictionary<string, object>();
        ReadOnlyDictionary<string, object> IUserContext.Properties => new (Properties);

        public static UserContext? Empty() => new()
        {
            Properties = new Dictionary<string, object>()
        };

        public static UserContext Create(Dictionary<string, object> properties)
        {
            return new UserContext
            {
                Properties = properties
            };
        }
    }
}