using AutoMapper;
using ItaliaTreni.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaTreni.Api.Tests;

public abstract class ItaliaTreniTestBase : IAsyncLifetime
{
    protected ItaliaTreniDbContext italiaTreniDbContext { get; private set; } = null!;
    protected IMapper mapper { get; private set; } = null!;

    private IServiceScope serviceScope = null!;
    protected IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

    public virtual async Task InitializeAsync()
    {
        var services = new ServiceCollection();

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddDbContext<ItaliaTreniDbContext>(
            options => options.UseInMemoryDatabase($"italia-treni-tests-{Guid.NewGuid():N}")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

        var serviceProvider = services.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        serviceScope = serviceScopeFactory.CreateScope();

        italiaTreniDbContext = ServiceProvider.GetRequiredService<ItaliaTreniDbContext>();
        await italiaTreniDbContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
    }
    public virtual Task DisposeAsync()
    {
        serviceScope.Dispose();
        return Task.CompletedTask;
    }

}
