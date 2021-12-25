namespace Footage
{
    using Footage.Dao;

    public static class Locator
    {
        public static IEntityDao Dao() => new EntityDao();
        
        public static T Get<T>() where T : class, new() => new();
    }
}