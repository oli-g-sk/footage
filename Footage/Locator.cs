namespace Footage
{
    using Footage.Dao;
    using Footage.Model;

    public static class Locator
    {
        /// <summary>
        /// Provides entity DAOs for the business logic layer.
        /// This assigns the EFCore specific implementation to the interfaces required by business logic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEntityDao<T> Dao<T>() where T : Entity, new() => new EntityDao<T>();
        
        public static T Get<T>() where T : class, new() => new();
    }
}