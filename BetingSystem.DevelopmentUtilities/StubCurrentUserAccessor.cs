using BetingSystem.Services;

namespace BetingSystem.DevelopmentUtilities
{
    public class StubCurrentUserAccessor : ICurrentUserAccessor
    {
        public string Id() => "mladen";
    }
}
