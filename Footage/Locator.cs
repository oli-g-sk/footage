namespace Footage
{
    using LibVLCSharp.Shared;

    public static class Locator
    {
        public static LibVLC LibVlc { get; }
        
        public static T Get<T>() where T : class, new() => new();

        static Locator()
        {
            Core.Initialize();
            LibVlc = new LibVLC();
        }
    }
}