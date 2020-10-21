using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bya.BackgroundTasks
{
    /// <inheritdoc cref="IHostedService" />
    /// <inheritdoc cref="IDisposable"/>
    /// <summary>
    /// Clase básica de implementación de un servicio en background en Net Core
    /// </summary>
    public abstract class FactoryHostedService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly TimeSpan _delayBetweenProcess;

        protected ILogger<FactoryHostedService> Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryHostedService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="delayBetweenProcess">El tiempo de espera entre ejecuciones para volver a lanzar el proceso.</param>
        protected FactoryHostedService(ILogger<FactoryHostedService> logger, TimeSpan delayBetweenProcess)
        {
            Logger = logger;
            _delayBetweenProcess = delayBetweenProcess;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Factory hosted service is starting.");

            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Factory hosted service is stopping.");

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="stoppingToken">The stopping token.</param>
        /// <returns></returns>
        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Logger.LogInformation("Cancellation token triggered. Programmed shutdown is starting..."));

            do
            {
                await Process(stoppingToken);

                await Task.Delay(_delayBetweenProcess, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        /// <summary>
        /// Función a implementar y que se ejecuta cada cierto tiempo especificado en el constructor
        /// </summary>
        /// <param name="stoppingToken">The stopping token.</param>
        /// <returns></returns>
        protected abstract Task Process(CancellationToken stoppingToken);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
