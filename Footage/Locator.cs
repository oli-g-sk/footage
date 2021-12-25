namespace Footage
{
    public static class Locator
    {
        public static T Get<T>() where T : class, new() => new();
    }
}