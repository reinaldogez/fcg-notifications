using Testcontainers.Redis;

namespace Fcg.Notifications.Tests.Integration.Fixtures;

// Um Redis real para os casos de /health/ready saudável.
public sealed class HealthFixture : IAsyncLifetime
{
    private readonly RedisContainer _redis = new RedisBuilder("redis:7.4-alpine").Build();

    public string RedisConnection => _redis.GetConnectionString();

    public Task InitializeAsync() => _redis.StartAsync();

    public async Task DisposeAsync() => await _redis.DisposeAsync();
}
