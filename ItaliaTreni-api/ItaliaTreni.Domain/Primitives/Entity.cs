using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace ItaliaTreni.Domain.Primitives;

public abstract class Entity<TKey> : IEntity
{
    [ExcludeFromCodeCoverage(Justification = "Not needed")]
    protected Entity()
    {
    }

    protected Entity(TKey id) => Id = id;

    [JsonProperty(nameof(Id))]
    public TKey Id { get; protected set; } = default!;

    [JsonIgnore]
    object IEntity.Id => Id!;

    public override bool Equals(object? obj)
    {
        var other = obj as Entity<TKey>;
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id);
    }

    [ExcludeFromCodeCoverage(Justification = "Not needed")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }
}