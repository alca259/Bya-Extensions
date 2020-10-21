using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bya.BackgroundTasks
{
    /// <inheritdoc cref="FactoryHostedService"/>
    /// <summary>
    /// Implementación de un servicio en segundo plano con capacidad para ejecutar servicio en scoped
    /// </summary>
    public abstract class ScopedHostedService : FactoryHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected ScopedHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<ScopedHostedService> logger, TimeSpan delayBetweenProcess)
            : base(logger, delayBetweenProcess)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process(CancellationToken stoppingToken)
        {
            // Crea una petición scope y ejecuta la tarea, al finalizar, cierra la conexión
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProcessInScope(scope.ServiceProvider, stoppingToken);
            }
        }

        /// <summary>
        /// Tarea a implementar en las clases que hereden de esta
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="stoppingToken">The stopping token.</param>
        /// <returns></returns>
        public abstract Task ProcessInScope(IServiceProvider serviceProvider, CancellationToken stoppingToken);
    }
}
