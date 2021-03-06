using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Footage.Model;
using Footage.Persistence.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NLog;

namespace Footage.Persistence.EFCore.Dao
{
    public class EntityDao : IEntityDao
    {
        private static ILogger Log => LogManager.GetCurrentClassLogger();
        
        private static ILogger LogEF => LogManager.GetLogger("EntityFrameworkCore");
        
        private readonly VideoContext dbContext;
        
        public EntityDao()
        {
            dbContext = new VideoContext();
            dbContext.ChangeTracker.StateChanged += ChangeTracker_StateChanged;
            dbContext.ChangeTracker.Tracked += ChangeTracker_Tracked;
            dbContext.SavingChanges += DbContext_SavingChanges;
            dbContext.SavedChanges += DbContext_SavedChanges;
            dbContext.SaveChangesFailed += DbContext_SaveChangesFailed;
        }

        public async Task Commit()
        {
            try
            {
                Log.Trace($"Commiting DB changes.");
                // Log.Trace(dbContext.ChangeTracker.DebugView);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        public async Task<bool> Contains<T>(Expression<Func<T, bool>> predicate) where T : Entity 
        {
            var entities = dbContext.Set<T>();
            return await entities.AnyAsync(predicate);
        }

        public async Task<T> Get<T>(int id) where T : Entity
        {
            return await dbContext.FindAsync<T>(id);
        }

        public Task Insert<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            try
            {
                Log.Debug($"CRUD insert: {item}");
                dbContext.Add(item);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public Task InsertRange<T>(IEnumerable<T> items) where T : Entity
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            try
            {
                Log.Debug($"CRUD insertRange: {typeof(T)}[], count: {items.Count()}");
                dbContext.AddRange(items);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }

            return Task.CompletedTask;
        }

        public Task Remove<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            try
            {
                Log.Debug($"CRUD remove: {item}");
                dbContext.Remove(item);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public Task Update<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            try
            {
                Log.Debug($"CRUD update: {item}");
                dbContext.Update(item);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public Task UpdateRange<T>(IEnumerable<T> items) where T : Entity
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            
            try
            {
                Log.Debug($"CRUD updateRange: {typeof(T)}[], count: {items.Count()}");
                dbContext.UpdateRange(items);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>>? predicate = null) where T : Entity
        {
            Log.Debug($"CRUD query: {typeof(T)}");
            var entities = dbContext.Set<T>().AsQueryable();

            if (predicate != null)
            {
                entities = entities.Where(predicate);
            }
            
            return entities;
        }
        
        public void Dispose()
        {
            dbContext.Dispose();
        }

        private void ProcessException(Exception ex)
        {
            Log.Error($"DB exception occurred: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log.Error(ex.InnerException.Message);
            }

            Debugger.Break();
            // TODO crash the app!!!
            throw new DbException(ex);
        }

        private void LogContextMessage(string message)
        {
            LogEF.Trace($"{message} [ContextID: {dbContext.ContextId}]");
        }
        
        #region Event handlers
        
        private void DbContext_SavingChanges(object? sender, SavingChangesEventArgs e)
        {
            LogContextMessage("Saving changes.");
        }

        private void DbContext_SaveChangesFailed(object? sender, SaveChangesFailedEventArgs e)
        {
            LogContextMessage($"Save changes failed: {e.Exception.Message}.");
        }

        private void DbContext_SavedChanges(object? sender, SavedChangesEventArgs e)
        {
            LogContextMessage($"Saved changes; entity count: {e.EntitiesSavedCount}.");
        }

        private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
        {
            LogContextMessage($"New entity tracked: {e.Entry.DebugView.ShortView}");
        }

        private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            LogContextMessage($"Entity state changed from {e.OldState} to {e.NewState} in {e.Entry.DebugView.ShortView}");
        }
        
        #endregion
    }
}
