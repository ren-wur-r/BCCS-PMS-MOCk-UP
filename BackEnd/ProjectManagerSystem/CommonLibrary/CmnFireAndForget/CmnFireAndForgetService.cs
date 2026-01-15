using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.CmnFireAndForget;

/// <summary>通用-射後不理服務</summary>
public class CmnFireAndForgetService : ICmnFireAndForgetService
{
    private readonly ILogger<CmnFireAndForgetService> _logger;

    /// <summary>服務範圍工廠</summary>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>通道</summary>
    private readonly Channel<Task> _channel;

    /// <summary>建構式</summary>
    public CmnFireAndForgetService(
        ILogger<CmnFireAndForgetService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        this._logger = logger;
        this._serviceScopeFactory = serviceScopeFactory;
        this._channel = Channel.CreateUnbounded<Task>();
    }

    /// <summary>執行</summary>
    public void ExecuteAsync<T>(Func<T, Task> work)
    {
        _ = Task.Run(async () =>
        {
            IServiceScope scope = null;

            try
            {
                scope = this._serviceScopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<T>();
                await work(service);
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    $"T: {typeof(T)?.FullName}" +
                    $", ex: {ex?.Message}" +
                    $", ex.InnerException: {ex?.InnerException?.Message}");
                return;
            }
            finally
            {
                scope?.Dispose();
            }
        });
    }

    /// <summary>寫入通道</summary>
    public async Task WriterChannelAsync<T>(Func<T, Task> work)
    {
        var newTask = new Task(async () =>
        {
            IServiceScope scope = null;

            try
            {
                scope = this._serviceScopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<T>();
                await work(service);
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    $"T: {typeof(T)?.FullName}" +
                    $", ex: {ex?.Message}" +
                    $", ex.InnerException: {ex?.InnerException?.Message}");
                return;
            }
            finally
            {
                scope?.Dispose();
            }
        });

        await this._channel.Writer.WriteAsync(newTask);
    }

    /// <summary>讀取通道</summary>
    public async Task<Task> ReadChannelAsync(CancellationToken cancellationToken)
    {
        // blocking if empty
        var workItem = await this._channel.Reader.ReadAsync(cancellationToken);
        return workItem;
    }

}
