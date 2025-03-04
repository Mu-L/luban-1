using Luban.Job.Common.Types;

namespace Luban.DataSources;

abstract class AbstractDataSource
{
    public const string TAG_KEY = "__tag__";

    public string RawUrl { get; protected set; }

    public abstract Record ReadOne(TBean type);

    public abstract List<Record> ReadMulti(TBean type);

    public abstract void Load(string rawUrl, string sheetName, Stream stream);
}