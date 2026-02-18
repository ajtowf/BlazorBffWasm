using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.ExceptionServices;

namespace IntegrationTests.e2e.Setup;

public abstract class WebHostServerFixture : IDisposable
{
    private readonly Lazy<Uri> _rootUriInitializer;

    public Uri RootUri => _rootUriInitializer.Value;
    public IHost Host { get; set; } = default!;

    protected WebHostServerFixture()
    {
        _rootUriInitializer = new Lazy<Uri>(() => new Uri(StartAndGetRootUri()));
    }

    protected static void RunInBackgroundThread(Action action)
    {
        using var isDone = new ManualResetEvent(false);

        ExceptionDispatchInfo? edi = null;
        new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            // ReSharper disable once AccessToDisposedClosure
            isDone.Set();
        }).Start();

        if (!isDone.WaitOne(TimeSpan.FromSeconds(10)))
        {
            throw new TimeoutException("Timed out waiting for: " + action);
        }

        if (edi != null)
        {
            throw edi.SourceException;
        }
    }

    protected virtual string StartAndGetRootUri()
    {
        // As the port is generated automatically, we can use IServerAddressesFeature to get the actual server URL
        Host = CreateWebHost();
        RunInBackgroundThread(Host.Start);
        var val = Host.Services.GetRequiredService<IServer>().Features
            .Get<IServerAddressesFeature>()
            ?.Addresses.Single();
        return val ?? string.Empty;
    }

    public virtual void Dispose()
    {
        Host?.Dispose();
        Host?.StopAsync();

        GC.SuppressFinalize(this);
    }

    protected abstract IHost CreateWebHost();
}
