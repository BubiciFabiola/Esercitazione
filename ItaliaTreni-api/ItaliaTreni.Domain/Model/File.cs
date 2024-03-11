using ItaliaTreni.Domain.Primitives;

namespace ItaliaTreni.Domain.Model;

public sealed class File : Entity<Guid>, IAggregate
{
    private readonly List<FileData> fileDatas = new();
    public string Name { get; private set; }
    public DateTime InsertDate { get; private set; }
    public IReadOnlyList<FileData> FileDatas => fileDatas.AsReadOnly();

    public File(string name) : base(Guid.NewGuid())
    {
        Name = name;
        InsertDate = DateTime.Now;
    }

    public FileData AddFileData(int mm, double p1, double p2, double p3, double p4)
    {
        var fileData = new FileData(mm, p1, p2, p3, p4);
        fileDatas.Add(fileData);

        return fileData;
    }
}

