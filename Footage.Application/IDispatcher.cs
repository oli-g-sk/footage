namespace Footage.Application
{
    using System;
    using System.Threading.Tasks;

    public interface IDispatcher
    {
        Task InvokeAsync(Action action);
        
        Task InvokeAsync<TResult>(Func<TResult> function);
    }
}