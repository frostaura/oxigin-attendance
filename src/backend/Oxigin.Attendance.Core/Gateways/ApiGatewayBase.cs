using System.Globalization;
using Oxigin.Attendance.Shared.Models.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Oxigin.Attendance.Core.Gateways;

/// <summary>
/// An abstract base gateway for all APIs.
/// </summary>
public abstract class ApiGatewayBase
{
  private readonly ILogger<ApiGatewayBase> _logger;
  private readonly ResiliencePolicyConfig _resiliencePolicy;

  /// <summary>
  /// Creates a new instance of the <see cref="ApiGatewayBase"/> class.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="resiliencePolicy">Polling policy configuration.</param>
  protected ApiGatewayBase(
    ILogger<ApiGatewayBase> logger,
    IOptions<ResiliencePolicyConfig> resiliencePolicy)
  {
    _logger = logger;
    _resiliencePolicy = resiliencePolicy.Value;
  }

  /// <summary>
  /// Executes the provided call asynchronously with the Polly policy configuration with retry and circuit breaker functions.
  /// </summary>
  /// <param name="func">The reference to the function to be invoked.</param>
  /// <returns>The response of type HttpResponseMessage.</returns>
  protected virtual async Task<HttpResponseMessage> ExecuteResilientRequestAsync(Func<Task<HttpResponseMessage>> func)
  {
    var policy = Policy
      .HandleResult<HttpResponseMessage>(y => !y.IsSuccessStatusCode)
      .WaitAndRetryAsync(
        Backoff.DecorrelatedJitterBackoffV2(
          TimeSpan.FromSeconds(_resiliencePolicy.MedianFirstRetryDelayInSeconds),
          _resiliencePolicy.NumberOfRetries))
      .WrapAsync(Policy
        .HandleResult<HttpResponseMessage>(y => !y.IsSuccessStatusCode)
        .CircuitBreakerAsync(
          _resiliencePolicy.AllowedBeforeBreak,
          TimeSpan.FromSeconds(_resiliencePolicy.CircuitBreakDurationInSeconds),
          OnBreak,
          OnReset,
          OnHalfOpen));

    return await policy.ExecuteAsync(async () => await func());
  }

  /// <summary>
  /// Logs breaking of circuit breaker and logs responses.
  /// </summary>
  /// <param name="result">Http response model.</param>
  /// <param name="timestamp">Time of break event.</param>
  protected virtual async void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan timestamp)
  {
    var response = await result.Result.Content.ReadAsStringAsync();

    _logger.LogDebug(
      "The circuit is now open and is not allowing calls. Response: {Response}, TimeOnBreak: {Timestamp}",
      response,
      timestamp.ToString(@"hh\:mm", CultureInfo.InvariantCulture));
  }

  /// <summary>
  /// Logs reset of circuit breaker.
  /// </summary>
  public virtual void OnReset()
  {
    _logger.LogDebug("Circuit closed, requests flow normally");
  }

  /// <summary>
  /// Circuit breaker half open state. Allows one request to pass through then resets.
  /// </summary>
  protected virtual void OnHalfOpen()
  {
    _logger.LogDebug("Circuit in test mode, one request will be allowed");
  }
}
