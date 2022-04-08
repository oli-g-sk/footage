using System;

namespace Footage.Persistence.EFCore.Dao
{
    public class DbException : Exception
    {
        public DbException(Exception cause) : base($"Error interacting with the DB: {cause.Message}", cause)
        {
            
        }
    }
}