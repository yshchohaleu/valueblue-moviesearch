namespace MovieSearch.Shared.Hosting;

internal class RootInitializer
{
    private readonly IEnumerable<IAsyncInitializer> _initializers;

    public RootInitializer(IEnumerable<IAsyncInitializer> initializers)
    {
        _initializers = initializers;
    }

    public async Task InitializeAsync()
    {
        foreach (var initializer in _initializers)
        {
            await initializer.InitializeAsync();
        }
    }
}