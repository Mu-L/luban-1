﻿using Luban.Core;
using Luban.Job.Common.Generate;

namespace Luban.Generate;

[Render("data_json_monolithic")]
class JsonDataMonolithicRender : DataRenderBase
{
    public override void Render(GenerationContext ctx)
    {
        ctx.Tasks.Add(this.GenJsonDataMonolithic(ctx));
    }

    private async Task GenJsonDataMonolithic(GenerationContext ctx)
    {
        var exportTables = ctx.ExportTables;
        var allJsonTask = new List<Task<string>>();
        foreach (var c in exportTables)
        {
            allJsonTask.Add(Task.Run(() =>
            {
                return System.Text.Encoding.UTF8.GetString((byte[])DataExporterUtil.ToOutputData(c, ctx.Assembly.GetTableExportDataList(c), "data_json2"));
            }));
        }

        var lines = new List<string>();

        lines.Add("{");
        for (int i = 0; i < exportTables.Count; i++)
        {
            if (i != 0)
            {
                lines.Add(",");
            }
            lines.Add($"\"{exportTables[i].FullName}\":");
            lines.Add(await allJsonTask[i]);
        }
        lines.Add("}");

        var content = string.Join('\n', lines);
        var outputFile = ctx.GenArgs.OutputDataJsonMonolithicFile;
        var md5 = CacheFileUtil.GenMd5AndAddCache(outputFile, content);
        ctx.GenScatteredFiles.Add(new FileInfo() { FilePath = outputFile, MD5 = md5 });
    }
}