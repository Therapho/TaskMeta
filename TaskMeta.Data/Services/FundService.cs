﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services;

public class FundService : EntityService<Fund>, IFundService
{
    private readonly ITransactionLogService _transactionLogService;

    public FundService(ApplicationDbContext applicationDbContext, IUserService userService, 
        ITransactionLogService transactionLogService, ILogger<EntityService<Fund>> logger)
        : base(applicationDbContext, userService, logger) { 
        _transactionLogService = transactionLogService;
    }

    /// <summary>
    /// Retrieves a list of funds associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of funds.</returns>
    public Task<List<Fund>> GetFundsByUser(string userId)
    {
        try
        {
            return Task.FromResult(Context.Funds.Where(f => f.UserId == userId).ToList());
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting funds by user {userId}.");
            throw;
        }
    }
    
    /// <summary>
    /// Processes a transaction by updating the balances of the associated funds.
    /// </summary>
    /// <param name="transaction">The transaction to be processed.</param>
    public async Task Process(Transaction transaction)
    {
        switch (transaction.CategoryId)
        {
            case Constants.Category.Deposit:
                {
                    var targetFund = transaction.TargetFund!;
                    targetFund.Balance += transaction.Amount;
                    break;
                }
            case Constants.Category.Withdrawal:
                {
                    var sourceFund = transaction.SourceFund!;
                    if (sourceFund.Balance < transaction.Amount)
                    {
                        throw new InvalidOperationException("Insufficient funds.");
                    }
                    sourceFund.Balance -= transaction.Amount;
                    break;
                }
            case Constants.Category.Transfer:
                {
                    var sourceFund = transaction.TargetFund!;
                    var targetFund = transaction.TargetFund!;
                    if (sourceFund.Balance < transaction.Amount)
                    {
                        throw new InvalidOperationException("Insufficient funds.");
                    }
                    sourceFund.Balance -= transaction.Amount;
                    targetFund.Balance += transaction.Amount;
                    break;
                }
        }
        await _transactionLogService!.LogTransaction(transaction, false);

        await Commit();
    }


}
