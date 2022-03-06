namespace Footage.Extensions
{
    using System;
    using System.Linq;

    public static class QueryableExtensions
    {
        public static IQueryable<TSource> If<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Func<IQueryable<TSource>, IQueryable<TSource>> branch)
        {
            return condition ? branch(source) : source;
        }
    }
}