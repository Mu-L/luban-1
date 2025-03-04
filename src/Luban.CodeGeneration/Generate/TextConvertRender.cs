﻿using Luban.Core;
using Luban.Job.Common.Generate;
using Luban.Job.Common.Utils;

namespace Luban.Generate;

[Render("convert_json")]
[Render("convert_lua")]
class TextConvertRender : DataRenderBase
{
    public override void Render(GenerationContext ctx)
    {
        string genType = ctx.GenType;
        foreach (var table in ctx.ExportTables)
        {
            var records = ctx.Assembly.GetTableAllDataList(table);
            int index = 0;
            string dirName = table.FullName;
            foreach (var record in records)
            {
                var fileName = table.IsMapTable ?
                    record.Data.GetField(table.IndexField.Name).Apply(ToStringVisitor2.Ins).Replace("\"", "").Replace("'", "")
                    : (++index).ToString();
                var file = RenderFileUtil.GetOutputFileName(genType, $"{dirName}/{fileName}", ctx.GenArgs.OutputConvertFileExtension);
                ctx.Tasks.Add(Task.Run(() =>
                {
                    //if (!FileRecordCacheManager.Ins.TryGetRecordOutputData(table, records, genType, out string md5))
                    //{
                    var content = DataConvertUtil.ToConvertRecord(table, record, genType);
                    var md5 = CacheFileUtil.GenStringOrBytesMd5AndAddCache(file, content);
                    FileRecordCacheManager.Ins.AddCachedRecordOutputData(table, records, genType, md5);
                    //}
                    ctx.GenDataFilesInOutputDataDir.Add(new FileInfo() { FilePath = file, MD5 = md5 });
                }));
            }
        }
    }
}