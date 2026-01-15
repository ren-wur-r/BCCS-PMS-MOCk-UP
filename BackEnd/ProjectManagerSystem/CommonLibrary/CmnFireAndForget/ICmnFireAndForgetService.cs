using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary.CmnFireAndForget;

/// <summary>通用-射後不理服務</summary>
public interface ICmnFireAndForgetService
{
    /// <summary>執行</summary>
    public void ExecuteAsync<T>(Func<T, Task> work);

    public Task WriterChannelAsync<T>(Func<T, Task> work);

    public Task<Task> ReadChannelAsync(CancellationToken cancellationToken);

}