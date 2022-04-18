using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DotNetDBTools.IntegrationTests.Utilities;

internal static class ExclusiveExecutionScope
{
    private static readonly ConcurrentDictionary<string, SemaphoreScope> s_semaphoreScopes = new();

    public static IDisposable CreateScope(string scopeName)
    {
        if (scopeName is null)
            return new NoActionDisposable();
        SemaphoreScope semaphoreScope = s_semaphoreScopes.GetOrAdd(scopeName, new SemaphoreScope(new SemaphoreSlim(1, 1)));
        semaphoreScope.Enter();
        return semaphoreScope;
    }

    private class SemaphoreScope : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;

        public SemaphoreScope(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public void Enter()
        {
            _semaphore.Wait();
        }

        public void Dispose()
        {
            _semaphore.Release();
        }
    }

    private class NoActionDisposable : IDisposable
    {
        public void Dispose() { }
    }
}
