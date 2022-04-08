using System;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Footage.Application.UI
{
    public class AvaloniaDispatcher : Footage.Application.IDispatcher
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