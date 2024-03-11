using ItaliaTreni.Domain.Primitives;

namespace ItaliaTreni.Domain.Model;

public sealed class FileData : Entity<Guid>
{
    public int MM { get; private set; }
    public double P1 { get; private set; }
    public double P2 { get; private set; }
    public double P3 { get; private set; }
    public double P4 { get; private set; }

    internal FileData() { }
    internal FileData(int mm, double p1, double p2, double p3, double p4) : base(Guid.NewGuid())
    {
        MM = mm;
        P1 = p1;
        P2 = p2;
        P3 = p3;
        P4 = p4;
    }
}
