using Arduino.Common.Application.Extensions;
using Arduino.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Common.Application.Transaction;

public class TransactionFilter : IAsyncActionFilter 
{
    private readonly ArduinoContext dbContext;

    public TransactionFilter(ArduinoContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executionStrategy = dbContext.Database.CreateExecutionStrategy();

        await executionStrategy.ExecuteAsync(async () =>
        {
            var transactionAttribute = context
                .ActionDescriptor
                .GetCustomAttributes<TransactionAttribute>()
                .SingleOrDefault();

            // IF THE METHOD IS NOT GOING TO SAVE IN THE DATABASE THEN WE WON'T TRACK THE CHANGES
            if (transactionAttribute == null)
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }
            else
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                await dbContext.Database.BeginTransactionAsync();
            }

            var executedContext = await next();

            if (transactionAttribute != null)
            {
                try
                {

                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        if (executedContext.Exception == null)
                        {
                            await dbContext.SaveChangesAsync();

                            await dbContext.Database.CurrentTransaction.CommitAsync();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await dbContext.Database.CurrentTransaction?.RollbackAsync();
                    throw;
                }
                catch (Exception ex)
                {
                    await dbContext.Database.CurrentTransaction?.RollbackAsync();
                    throw;
                }
                finally
                {
                    dbContext.Database.CurrentTransaction?.Dispose();
                }
            }
        });
    }

}