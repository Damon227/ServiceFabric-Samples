// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Data
// File             : DbContextBase.cs
// Created          : 2016-07-01  10:32 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Credit.Kolibre.Foundation.Data
{
    /// <summary>
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" /> 的扩展类，主要是添加重试机制。
    /// </summary>
    public static class DbContextExtensions
    {
        private static readonly RetryPolicy s_retryPolicy = new RetryPolicy(new SqlDatabaseTransientErrorDetectionStrategy(), RetryStrategy.DefaultExponential);


        /// <summary>
        ///     Deletes the specified entity form this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the deleting entity.</typeparam>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <param name="entity">The deleting entity.</param>
        /// <returns>
        ///     The number of entries deleted from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static int Delete<T>(this DbContext db, T entity) where T : class
        {
            db.Remove(entity);
            return db.ExecuteSaveChanges();
        }

        /// <summary>
        ///     Asynchronously deletes the specified entity form this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the deleting entity.</typeparam>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <param name="entity">The deleting entity.</param>
        /// <returns>
        ///     A task that represents the asynchronous delete operation.
        ///     The task result contains the number of entries deleted from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static Task<int> DeleteAsync<T>(this DbContext db, T entity) where T : class
        {
            db.Remove(entity);

            return db.ExecuteSaveChangesAsync();
        }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <returns>
        ///     The number of state entries written to the underlying database. This can include
        ///     state entries for entities and/or relationships. Relationship state entries are created for
        ///     many-to-many relationships and relationships where there is no foreign key property
        ///     included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static int ExecuteSaveChanges(this DbContext db)
        {
            return ExecuteAction(() => db.SaveChanges());
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <param name="cancellationToken">
        ///     A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of state entries written to the underlying database. This can include
        ///     state entries for entities and/or relationships. Relationship state entries are created for
        ///     many-to-many relationships and relationships where there is no foreign key property
        ///     included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static Task<int> ExecuteSaveChangesAsync(this DbContext db, CancellationToken cancellationToken)
        {
            return ExecuteAsync(() => db.SaveChangesAsync(cancellationToken), cancellationToken);
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of state entries written to the underlying database. This can include
        ///     state entries for entities and/or relationships. Relationship state entries are created for
        ///     many-to-many relationships and relationships where there is no foreign key property
        ///     included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static Task<int> ExecuteSaveChangesAsync(this DbContext db)
        {
            return ExecuteAsync(() => db.SaveChangesAsync());
        }

        /// <summary>
        ///     Returns a <see cref="T:System.Data.Entity.DbSet`1" /> instance for access to entities of the given type in the context and the underlying store.
        /// </summary>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <remarks>
        ///     Note that Entity Framework requires that this method return the same instance each time that it is called
        ///     for a given context instance and entity type. Also, the non-generic <see cref="T:System.Data.Entity.DbSet" /> returned by the
        ///     <see cref="M:System.Data.Entity.DbContext.Set(System.Type)" /> method must wrap the same underlying query and set of entities. These invariants must
        ///     be maintained if this method is overridden for anything other than creating test doubles for unit testing.
        ///     See the <see cref="T:System.Data.Entity.DbSet`1" /> class for more details.
        /// </remarks>
        /// <typeparam name="T">The type entity for which a set should be returned. </typeparam>
        /// <returns>
        ///     A set for the given entity type.
        /// </returns>
        public static DbSet<T> Query<T>(this DbContext db) where T : class
        {
            return db.Set<T>();
        }

        /// <summary>
        ///     Returns a new query where the entities returned will not be cached in the <see cref="T:System.Data.Entity.DbContext" />.
        /// </summary>
        /// <typeparam name="T">The type entity for which a query should be returned. </typeparam>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <returns>
        ///     A new query with NoTracking applied.
        /// </returns>
        public static IQueryable<T> ReadonlyQuery<T>(this DbContext db) where T : class
        {
            return db.Set<T>().AsNoTracking();
        }

        /// <summary>
        ///     Saves the specified entity to this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the saving entity.</typeparam>
        /// <param name="entity">The saving entity.</param>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <returns>
        ///     The number of entries saved to the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static int Save<T>(this DbContext db, T entity) where T : class
        {
            db.Add(entity);

            return db.ExecuteSaveChanges();
        }

        /// <summary>
        ///     Asynchronously saves the specified entity to this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the saving entity.</typeparam>
        /// <param name="entity">The saving entity.</param>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <returns>
        ///     A task that represents the asynchronous delete operation.
        ///     The task result contains the number of entries deleted from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static Task<int> SaveAsync<T>(this DbContext db, T entity) where T : class
        {
            db.Add(entity);

            return db.ExecuteSaveChangesAsync();
        }

        /// <summary>
        ///     Saves the specified entity to this context or updates the entity in this context, and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to save or update.</param>
        /// <param name="identifierExpression">The identifier expression for determining whether the entity is exsting in the underlying database.</param>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <returns>
        ///     The number of entries saved or updated to the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static int SaveOrUpdate<T>(this DbContext db, T entity, Expression<Func<T, bool>> identifierExpression) where T : class
        {
            EntityEntry<T> entry = db.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                db.Add(entry);
            }

            if (db.Set<T>().Any(identifierExpression))
            {
                db.Update(entry);
            }

            return db.ExecuteSaveChanges();
        }

        /// <summary>
        ///     Asynchronously saves the specified entity to this context or updates the entity in this context, and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to save or update.</param>
        /// <param name="identifierExpression">The identifier expression for determining whether the entity is exsting in the underlying database.</param>
        /// <param name="db">The instance of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        /// <returns>
        ///     A task that represents the asynchronous save or update operation.
        ///     The task result contains the number of entries saved or updated from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values unsuccessfully.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public static Task<int> SaveOrUpdateAsync<T>(this DbContext db, T entity, Expression<Func<T, bool>> identifierExpression) where T : class
        {
            EntityEntry<T> entry = db.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                db.Add(entry);
            }

            if (db.Set<T>().Any(identifierExpression))
            {
                db.Update(entry);
            }

            return db.ExecuteSaveChangesAsync();
        }

        private static TResult ExecuteAction<TResult>(Func<TResult> func)
        {
            return s_retryPolicy.ExecuteAction(func);
        }

        private static Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc)
        {
            return s_retryPolicy.ExecuteAsync(taskFunc);
        }

        private static Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc, CancellationToken cancellationToken)
        {
            return s_retryPolicy.ExecuteAsync(taskFunc, cancellationToken);
        }
    }
}