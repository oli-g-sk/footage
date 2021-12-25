namespace Footage.Dao
{
    using System;

    public class DbException : Exception
    {
        public DbException(Exception cause) : base($"Error interacting with the DB: {cause.Message}", cause)
        {
            
        }
    }
}