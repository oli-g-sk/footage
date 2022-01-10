namespace Footage.UI
{
    using System;
    using System.Threading.Tasks;
    using Avalonia.Threading;
    using IDispatcher = Footage.IDispatcher;

    public class AvaloniaDispatcher : IDispatcher
    {
        public async Task InvokeAsync(Action action)
        {
            await Dispatcher.UIThread.InvokeAsync(action);
        }
        
        public async Task InvokeAsync<TResult>(Func<TResult> function)
        {
            await Dispatcher.UIThread.InvokeAsync(function);
        }
    }
}