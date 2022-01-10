namespace Footage.Win
{
    using Footage.UI;
    using GalaSoft.MvvmLight.Ioc;

    public class Locator
    {
        public static void Initialize()
        {
            SimpleIoc.Default.Register<IDispatcher, AvaloniaDispatcher>();
        }
    }
}