namespace Footage.ViewModel.Base
{
    using GalaSoft.MvvmLight.Ioc;

    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected IDispatcher Dispatcher => SimpleIoc.Default.GetInstance<IDispatcher>();
    }
}