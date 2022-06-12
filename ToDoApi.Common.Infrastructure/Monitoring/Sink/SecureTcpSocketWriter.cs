using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.Network.Sinks.TCP;

namespace ToDoApi.Common.Infrastructure.Monitoring.Sink;

internal class SecureTcpSocketWriter : IDisposable
{
    private readonly X509Certificate2 _clientCertificate;
    private readonly TaskCompletionSource<bool> _disposed;

    private readonly Action<Exception> _loggingFailureHandler = exception =>
    {
        Log.Error(exception, "Logstash secure TCP sink failure: {message}", exception.Message);
    };

    private readonly Queue<string> _queue;
    private readonly ExponentialBackoffTcpReconnectionPolicy _reconnectionPolicy;
    private readonly X509Certificate2 _rootCertificate;

    private Stream _stream;
    private CancellationTokenSource _tokenSource;

    internal SecureTcpSocketWriter(
        string host,
        int port,
        X509Certificate2 clientCertificate,
        X509Certificate2 rootCertificate
    )
    {
        _clientCertificate = clientCertificate;
        _rootCertificate = rootCertificate;
        _queue = new Queue<string>();
        _reconnectionPolicy = new ExponentialBackoffTcpReconnectionPolicy();
        _disposed = new TaskCompletionSource<bool>();

        InitializeWriter(new Uri($"tls://{host}:{port}"));
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        Task.Run(async () => await _disposed.Task).Wait();
    }

    private void InitializeWriter(Uri uri)
    {
        var completion = new TaskCompletionSource<bool>();
        _tokenSource = new CancellationTokenSource();

        Task.Factory.StartNew(async () =>
        {
            try
            {
                _stream = await _reconnectionPolicy.ConnectAsync(OpenSocket(), uri, _tokenSource.Token);
                completion.SetResult(true);

                while (_stream != null)
                {
                    if (_tokenSource.IsCancellationRequested)
                    {
                        while (_queue.Count > 0) await SendAsync(_stream, _queue.Dequeue());

                        break;
                    }

                    if (_queue.Count == 0) continue;

                    await SendAsync(_stream, _queue.Dequeue());
                }
            }
            catch (Exception exception)
            {
                _loggingFailureHandler(exception);
                throw;
            }
            finally
            {
                if (_stream != null) await _stream.DisposeAsync();
                _disposed.SetResult(true);
            }
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        completion.Task.Wait(TimeSpan.FromSeconds(5));
    }

    private async Task SendAsync(Stream stream, string message)
    {
        try
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(bytes.AsMemory(0, bytes.Length));
            await stream.FlushAsync();
        }
        catch (SocketException exception)
        {
            _loggingFailureHandler(exception);
            throw;
        }
    }

    private Func<Uri, Task<Stream>> OpenSocket()
    {
        return async uri =>
        {
            var client = new TcpClient();
            await client.ConnectAsync(uri.Host, uri.Port);

            var clientCertificates = new X509CertificateCollection {_clientCertificate};

            var validationCallback = (RemoteCertificateValidationCallback) ((_, _, chain, _) =>
            {
                if (chain is null) return false;

                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreWrongUsage;

                return chain.Build(_rootCertificate);
            });

            var sslStream = new SslStream(client.GetStream(), false, validationCallback, null);

            await sslStream.AuthenticateAsClientAsync(uri.Host, clientCertificates, false);

            return sslStream;
        };
    }

    internal void Enqueue(string message)
    {
        _queue.Enqueue(message);
    }
}