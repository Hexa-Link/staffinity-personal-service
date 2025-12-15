using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal sealed class QueueHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<Func<HttpRequestMessage, HttpResponseMessage>> _queue = new();
    public int CallCount { get; private set; }

    public void Enqueue(Func<HttpRequestMessage, HttpResponseMessage> factory) =>
        _queue.Enqueue(factory);

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        CallCount++;
        if (_queue.Count == 0)
            throw new InvalidOperationException("No queued responses.");
        return Task.FromResult(_queue.Dequeue().Invoke(request));
    }
}
