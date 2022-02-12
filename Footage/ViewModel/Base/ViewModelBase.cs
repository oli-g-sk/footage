namespace Footage.ViewModel.Base
{
    using GalaSoft.MvvmLight.Ioc;

    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected static IDispatcher Dispatcher => Locator.Dispatcher;
    }
}