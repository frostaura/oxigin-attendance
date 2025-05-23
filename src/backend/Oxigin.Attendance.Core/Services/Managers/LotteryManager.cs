using Oxigin.Attendance.Core.Contracts;
using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Core.Interfaces.Gateways;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Enums;
using Oxigin.Attendance.Shared.Models.Configs;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.SmartContract;
using Oxigin.Attendance.Shared.Models.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Oxigin.Attendance.Core.Services.Managers
{
  public class LotteryManager : ILotteryManager
  {
    private readonly IDatastoreContext _datastoreContext;
    private readonly ILogger<LotteryManager> _logger;
    private readonly ITransactionsManager _transactionsManager;
    private string _merchantWalletAccount;
    private readonly ITonApiGateway _tonApiGateway;
    private readonly IMemoryCacheManager _memoryCacheManager;

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="datastoreContext">The datastore context instance.</param>
    /// <param name="logger">The logger instance.</param>
    public LotteryManager(IDatastoreContext datastoreContext, 
      ILogger<LotteryManager> logger, 
      ITransactionsManager transactionsManager,
      IOptions<MerchantConfig> merchantConfig,
      ITonApiGateway tonApiGateway,
      IMemoryCacheManager memoryCacheManager)
    {
      _datastoreContext = datastoreContext.ThrowIfNull(nameof(datastoreContext));
      _logger = logger;
      _transactionsManager = transactionsManager;
      _merchantWalletAccount = merchantConfig.Value.WalletAddress;
      _tonApiGateway = tonApiGateway;
      _memoryCacheManager = memoryCacheManager;
    }

    /// <summary>
    /// Draw lottery numbers.
    /// </summary>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns></returns>
    public async Task DrawAsync(CancellationToken token)
    {
      await _tonApiGateway.InitiateDrawOnContractAsync(token);
    }

    /// <summary>
    /// Retrieve the current state of the lottery
    /// </summary>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns>The lottery state containing the config and state objects.</returns>
    public async Task<CompositeState> GetLotteryStateAsync(CancellationToken token)
    {
      var lotteryState = await _tonApiGateway.GetStateFromContractAsync(token);
      return lotteryState;
    }

    /// <summary>
    /// Retrieve all lottery entries within a specified time period.
    /// </summary>
    /// <param name="numberOfDrawRepeats">The maximum supported repeats per/entry.</param>
    /// <param name="lengthOfLottery">The count of days per/draw period.</param>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns>A list of lottery transactions.</returns>
    public async Task<List<LotteryTransaction>> GetAllLotteryEntriesAsync(int numberOfDrawRepeats, int lengthOfLottery, CancellationToken token)
    {
      // Calculate the start time for retrieving lottery entries
      var startTime = CalculateFurthestDateTime(lengthOfLottery, numberOfDrawRepeats);
      // Retrieve all incoming transactions for the merchant wallet account starting from the calculated start time
      var incomingTransactions = await _transactionsManager.GetAllIncomingTransactionsForWalletAccount(_merchantWalletAccount, startTime);

      // Iterate through the incoming transactions and filter out the ones related to the lottery
      var lotteryEntriesList = new List<LotteryTransaction>();
      foreach (var transaction in incomingTransactions.Where(x => x.TransactionComment?.MainApplication == MainApplication.Lottery))
      {
        // Skip transactions without a valid lottery transaction value
        if (transaction.TransactionComment.Value is not { } lotteryTransactionValue)
        {
          continue;
        }

        // Set the timestamp and amount for the lottery transaction value
        lotteryTransactionValue.Timestamp = transaction.Timestamp;
        lotteryTransactionValue.Amount = transaction.Amount;

        // Add the lottery transaction value to the list
        lotteryEntriesList.Add(lotteryTransactionValue);
      }

      return lotteryEntriesList;
    }

    /// <summary>
    /// Retrieve all valid lottery entries based on the provided list of lottery transactions and discount factor.
    /// </summary>
    /// <param name="allLotteryEntries">The list of all lottery transactions.</param>
    /// <param name="discountFactor">The discount factor to apply for calculating valid entries.</param>
    /// <param name="cancellationToken">A token to allow for cancelling downstream operations.</param>
    /// <returns>A list of valid lottery entries.</returns>
    public async Task<List<LotteryEntry>> GetAllValidLotteryEntriesAsync(List<LotteryTransaction> allLotteryEntries, DiscountFactor discountFactor, CancellationToken cancellationToken)
    {
      var validLotteryEntries = new List<LotteryEntry>();

      foreach (var lotteryEntry in allLotteryEntries)
      {
       
        // Check if discount is applied correctly
        var totalEntries = lotteryEntry.LotteryEntries.Sum(entry => entry.EntryRepeatCount + 1);
        var discount = (totalEntries / discountFactor.ForEvery) * discountFactor.Get;
        if (totalEntries != lotteryEntry.Amount)
        {
          if (totalEntries - discount != lotteryEntry.Amount)
          {
            continue;
          }
        }

        // Check if entry is valid for this week
        foreach (var entry in lotteryEntry.LotteryEntries)
        {
          var now = DateTime.UtcNow;
          var lastMondayMidnight = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday).Date;
          // Calculate the difference in weeks
          var difference = lastMondayMidnight - lotteryEntry.Timestamp;
          var weeksAgo = (int)(difference.TotalDays / 7);

          if (weeksAgo <= entry.EntryRepeatCount + 1)
          {
            validLotteryEntries.Add(entry);
          }
        }
      }

      return validLotteryEntries;
    }

    /// <summary>
    /// Retrieve all winning lottery entries.
    /// </summary>
    /// <param name="validLotteryEntries">A list of all valid lottery entries.</param>
    /// <param name="newState"></param>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns>A list of all winning lottery entries.</returns>
    public async Task<List<KeyValuePair<LotteryEntry, int>>> GetAllWinningLotteryEntriesAsync(List<LotteryEntry> validLotteryEntries, CompositeState newState, CancellationToken token)
    {
      var winners = new List<KeyValuePair<LotteryEntry, int>>();

      var latestDraw = newState.State.LatestDraw;
      var jackpotNumber = latestDraw.Last();

      foreach (var entry in validLotteryEntries)
      {
        int matchingNumbers = 0;
        bool jackpotMatch = false;
        foreach (var lotteryEntryNumber in entry.Numbers)
        {
          if (lotteryEntryNumber == jackpotNumber)
          {
            jackpotMatch = true;
          }
          if (latestDraw.Contains(lotteryEntryNumber))
          {
            matchingNumbers++;
          }
        }
        // TODO: Implement finalized winning calculation
        var winnings = CalculateWinnings(matchingNumbers, jackpotMatch, newState.State.JackpotAbsoluteBalance);

        winners.Add(new KeyValuePair<LotteryEntry, int>(entry, winnings));
      }

      return winners;
    }

    /// <summary>
    /// Publish the winners of the lottery.
    /// </summary>
    /// <param name="winningEntries">A list of all winning lottery entries.</param>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns></returns>
    public Task PublishWinnersAsync(List<KeyValuePair<LotteryEntry, int>> winningEntries, CancellationToken token)
    {
      return null;
    }

    /// <summary>
    /// Deduct the admin fee from the lottery state.
    /// </summary>
    /// <param name="lotteryState">The lottery state containing the config and state objects.</param>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns></returns>
    public Task DeductAdminFeeAsync(CompositeState lotteryState, CancellationToken token)
    {
      return null;
    }

    /// <summary>
    /// Update the absolute jackpot balance.
    /// </summary>
    /// <param name="state">The current lottery state containing the config and state objects.</param>
    /// <param name="updatedJackpotAmount">The updated jackpot amount</param>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns></returns>
    public async Task UpdateJackpotAmountAsync(CompositeState state, int updatedJackpotAmount, CancellationToken token)
    {
      var newState = state.State;
      newState.JackpotAbsoluteBalance = updatedJackpotAmount;
      await _tonApiGateway.SetStateOnContractAsync(newState, token);
    }

    /// <summary>
    /// Calculate the furthest date and time in UTC based on the number of days and the repetition factor.
    /// </summary>
    /// <param name="days">The number of days to subtract from the most recent Monday.</param>
    /// <param name="repeat">The repetition factor indicating how many times the subtraction should be repeated.</param>
    /// <returns>The furthest date and time in UTC.</returns>
    private static DateTime CalculateFurthestDateTime(int days, int repeat)
    {
      // Get the current date and time in UTC
      DateTime now = DateTime.UtcNow;

      // Find the most recent Monday at midnight UTC
      DateTime mostRecentMonday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
      DateTime mostRecentMondayMidnight = new DateTime(mostRecentMonday.Year, mostRecentMonday.Month, mostRecentMonday.Day, 0, 0, 0, DateTimeKind.Utc);

      // Calculate the furthest date back
      DateTime furthestDate = mostRecentMondayMidnight.AddDays(-(days * (repeat - 1)));

      return furthestDate;
    }

    /// <summary>
    /// Calculate the winnings based on the number of matching numbers and the jackpot match status.
    /// </summary>
    /// <param name="matchingNumbers">The number of matching numbers.</param>
    /// <param name="jackpotMatch">Indicates whether there is a jackpot match.</param>
    /// <param name="jackpotAmount">The total jackpot amount available to win.</param>
    /// <returns>The winnings amount.</returns>
    private static int CalculateWinnings(int matchingNumbers, bool jackpotMatch, int jackpotAmount)
    {
      var winningsTable = new Dictionary<int, int>()
      {
        { 3, jackpotMatch ? 100: 10 },
        { 4, jackpotMatch ? 1000: 100 },
        { 5, jackpotMatch ? 10000: 1000 },
        { 6, jackpotMatch ? 100000: 0 },
      };

      if (winningsTable.ContainsKey(matchingNumbers))
      {
        return winningsTable[matchingNumbers];
      }

      return 0;
    }
  }
}
