using Bright.Serialization;
using Luban.Datas;
using Luban.DataSources;
using Luban.Defs;

namespace Luban.DataExporters;

class BinaryIndexExportor
{
    public static BinaryIndexExportor Ins { get; } = new BinaryIndexExportor();

    public void WriteList(DefTable table, List<Record> datas, ByteBuf x)
    {
        x.WriteSize(datas.Count);
        var tableDataBuf = new ByteBuf(10 * 1024);
        tableDataBuf.WriteSize(datas.Count);

        foreach (var d in datas)
        {
            int offset = tableDataBuf.Size;
            d.Data.Apply(BinaryExportor.Ins, tableDataBuf);

            string keyStr = "";
            foreach (IndexInfo index in table.IndexList)
            {
                DType key = d.Data.Fields[index.IndexFieldIdIndex];
                key.Apply(BinaryExportor.Ins, x);
                keyStr += key.ToString() + ",";
            }
            x.WriteSize(offset);
            Console.WriteLine($"table:{table.Name} key:{keyStr} offset:{offset}");
        }
          
    }
}