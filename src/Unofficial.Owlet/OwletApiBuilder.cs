using Microsoft.Extensions.DependencyInjection;
using Unofficial.Owlet.Interfaces;

namespace Unofficial.Owlet
{
    internal class OwletApiBuilder : IOwletApiBuilder
    {
        private readonly IServiceCollection _services;
        public OwletApiBuilder(IServiceCollection services)
        {
            this._services = services;
        }

        public IOwletApiBuilder WithSettings(IOwletApiSettings settings)
        {
            this._services.AddScoped(provider =>
            {
                return settings;
            });
            return this;
        }
    }
}
