using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Transactions;
using Oxigin.Attendance.Shared.Models.SmartContract;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Oxigin.Attendance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LotteryController : BaseController
{
  /// <summary>
  /// A manager to facilitate lottery-related use-cases.
  /// </summary>
  private readonly ILotteryManager _lotteryManager;

  /// <summary>
  /// A manager to facilitate payout-related use-cases.
  /// </summary>
  private readonly IPayoutManager _payoutManager;

  /// <summary>
  /// The overall smart contract state containing the config and state objects.
  /// </summary>
  private CompositeState lotteryState;

  /// <summary>
  /// The list of all lottery entries.
  /// </summary>
  private List<LotteryTransaction> lotteryEntries;

  /// <summary>
  /// The list of all valid lottery entries.
  /// </summary>
  private List<LotteryEntry> validEntries;

  /// <summary>
  /// The list of all winning lottery entries.
  /// </summary>
  private List<KeyValuePair<LotteryEntry, int>> winningEntries;

  /// <summary>
  /// Overloaded constructor to allow for injecting dependencies.
  /// </summary>
  /// <param name="lotteryManager">A manager to facilitate lottery-related use cases.</param>
  /// <param name="payoutManager">A manager to facilitate payout-related use cases.</param>
  /// <param name="logger">The controller logger instance.</param>
  public LotteryController(ILotteryManager lotteryManager, IPayoutManager payoutManager, ILogger<LotteryController> logger)
    :base(logger)
  {
    _lotteryManager = lotteryManager.ThrowIfNull(nameof(lotteryManager));
    _payoutManager = payoutManager.ThrowIfNull(nameof(payoutManager));
  }

  /// <summary>
  /// Draw and publish lottery winners.
  /// </summary>
  /// <param name="token">Token to cancel downstream operations.</param>
  [HttpPost("Draw", Name = "DrawAsync")]
  public async Task DrawAsync(CancellationToken token)
  {
    await _lotteryManager.DrawAsync(token);
    //TODO: Run refresh transactions cache
    lotteryState = await _lotteryManager.GetLotteryStateAsync(token);
    lotteryEntries = await _lotteryManager.GetAllLotteryEntriesAsync(lotteryState.Config.MaxSupportedRepeatsPerDraw, lotteryState.Config.DaysPerDraw, token);
    validEntries = await _lotteryManager.GetAllValidLotteryEntriesAsync(lotteryEntries, lotteryState.Config.DiscountFactor, token);
    winningEntries = await _lotteryManager.GetAllWinningLotteryEntriesAsync(validEntries, lotteryState, token);
    //await _lotteryManager.PublishWinnersAsync(winningEntries, token);
  }

  /// <summary>
  /// Payout lottery winners.
  /// </summary>
  /// <param name="paymentAccountId">The ID of the source account for the payout.</param>
  /// <param name="paymentAccountType">The type of the source account for the payout.</param>
  /// <param name="payees">A dictionary containing the blockchain addresses and payout amounts for each winner destination account.</param>
  /// <param name="assetId">The ID of the asset used in the payout.</param>
  /// <param name="token">Token to cancel downstream operations.</param>
  /// <returns>An identifier associated with the payout transaction</returns>
  [HttpPost("Payout", Name = "PayoutAsync")]
  public async Task<string> PayoutAsync(
    [FromQuery][DefaultValue("1")] string paymentAccountId,
    [FromQuery][DefaultValue("VAULT_ACCOUNT")] string paymentAccountType,
    [FromBody][DefaultValue("{'0QB2esajbom8S3tfdN90ORI5XCfSTUmPGOkd_1SF8I3dVqIw':0.001}")] string payeesJson,
    [FromQuery][DefaultValue("TON_TEST")] string assetId,
    CancellationToken token)
  {
    try
    {
      var payees = JsonConvert.DeserializeObject<Dictionary<string, double>>(payeesJson);
      if (payees != null)
      {
        return await _payoutManager.PayoutWinnersAsync(paymentAccountId, paymentAccountType, payees, assetId, token);
      }
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex.Message);
    }
    return "Failure";
  }

  /// <summary>
  /// Payout lottery affiliates.
  /// </summary>
  /// <param name="paymentAccountId">The ID of the source account for the payout.</param>
  /// <param name="paymentAccountType">The type of the source account for the payout.</param>
  /// <param name="payees">A dictionary containing the blockchain addresses and payout amounts for each affiliate destination account.</param>
  /// <param name="assetId">The ID of the asset used in the payout.</param>
  /// <param name="token">Token to cancel downstream operations.</param>
  /// <returns>An identifier associated with the payout transaction.</returns>
  [HttpPost("PayoutAffiliates", Name = "PayoutAffiliatesAsync")]
  public async Task<string> PayoutAffiliatesAsync(
    [FromQuery][DefaultValue("1")] string paymentAccountId,
    [FromQuery][DefaultValue("VAULT_ACCOUNT")] string paymentAccountType,
    [FromBody] [DefaultValue("{'0QB2esajbom8S3tfdN90ORI5XCfSTUmPGOkd_1SF8I3dVqIw':0.001}")] string payeesJson,
    [FromQuery] [DefaultValue("TON_TEST")] string assetId, 
    CancellationToken token)
  {
    try
    {
      var payees = JsonConvert.DeserializeObject<Dictionary<string, double>>(payeesJson);
      if (payees != null)
      {
        return await _payoutManager.PayoutAffiliatesAsync(paymentAccountId, paymentAccountType, payees, assetId, token);
      }
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex.Message);
    }
    return "Failure";
  }

  /// <summary>
  /// Update lottery jackpot amount.
  /// </summary>
  /// <param name="token">Token to cancel downstream operations.</param>
  [HttpPost("UpdateJackpotAmount", Name = "UpdateJackpotAmountAsync")]
  public async Task UpdateJackpotAmountAsync(CancellationToken token)
  {
    lotteryState = await _lotteryManager.GetLotteryStateAsync(token);
    lotteryEntries = await _lotteryManager.GetAllLotteryEntriesAsync(lotteryState.Config.MaxSupportedRepeatsPerDraw, lotteryState.Config.DaysPerDraw, token);
    validEntries = await _lotteryManager.GetAllValidLotteryEntriesAsync(lotteryEntries, lotteryState.Config.DiscountFactor, token);

    var lotteryTicketPrice = 1;
    var totalEntries = validEntries.Count() * lotteryTicketPrice;

    // TODO: Calculate total lottery payments
    var totalLotteryPayments = 0;

    var jackpotAbsoluteBalance = totalEntries - totalLotteryPayments + lotteryState.State.JackpotRolloverBalance;

    await _lotteryManager.UpdateJackpotAmountAsync(lotteryState, jackpotAbsoluteBalance, token);
  }


  /// <summary>
  /// Get lottery entries.
  /// </summary>
  /// <param name="lengthOfLottery"></param>
  /// <param name="token">Token to cancel downstream operations.</param>
  /// <param name="numberOfDrawRepeats"></param>
  /// <returns>A collection of active affiliates.</returns>
  [HttpGet(Name = "GetLotteryEntriesAsync")]
  public async Task<IEnumerable<LotteryTransaction>> GetAsync(int numberOfDrawRepeats, int lengthOfLottery, CancellationToken token)
  {
    return await _lotteryManager.GetAllLotteryEntriesAsync(numberOfDrawRepeats, lengthOfLottery, CancellationToken.None);
  }
}