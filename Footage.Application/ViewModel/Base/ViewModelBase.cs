namespace Footage.Application.ViewModel.Base
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected static IDispatcher Dispatcher => Locator.Dispatcher;
    }
}