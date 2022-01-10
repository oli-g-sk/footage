namespace Footage
{
    using LibVLCSharp.Shared;

    public static class Locator
    {
        public static LibVLC LibVlc { get; }

        static Locator()
        {
            Core.Initialize();
            LibVlc = new LibVLC();
        }
    }
}