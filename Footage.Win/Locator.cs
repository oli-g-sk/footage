namespace Footage.Win
{
    using Footage.UI;
    using GalaSoft.MvvmLight.Ioc;

    public class Locator
    {
        public static void Initialize()
        {
            Footage.Locator.RegisterDefaultDatabase();
            Footage.Locator.RegisterDefaultEngine();
            Footage.Locator.RegisterDefaultRepositories();
            Footage.Locator.RegisterDefaultServices();

            SimpleIoc.Default.Register<IDispatcher, AvaloniaDispatcher>();
        }
    }
}