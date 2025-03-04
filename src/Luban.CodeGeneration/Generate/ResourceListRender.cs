﻿using Luban.Core;
using Luban.Job.Common.Generate;

namespace Luban.Generate;

[Render("data_resources")]
class ResourceListRender : DataRenderBase
{
    public override void Render(GenerationContext ctx)
    {
        var genDataTasks = new List<Task<List<ResourceInfo>>>();
        foreach (var c in ctx.ExportTables)
        {
            genDataTasks.Add(Task.Run(() =>
            {
                return DataExporterUtil.ExportResourceList(ctx.Assembly.GetTableExportDataList(c));
            }));
        }

        ctx.Tasks.Add(Task.Run(async () =>
        {
            var ress = new HashSet<(string, string)>(10000);
            var resourceLines = new List<string>(10000);
            foreach (var task in genDataTasks)
            {
                foreach (var ri in await task)
                {
                    if (ress.Add((ri.Resource, ri.Tag)))
                    {
                        resourceLines.Add($"{ri.Tag},{ri.Resource}");
                    }
                }
            }
            var file = ctx.GenArgs.OutputDataResourceListFile;
            var content = string.Join("\n", resourceLines);
            var md5 = CacheFileUtil.GenMd5AndAddCache(file, content);

            ctx.GenScatteredFiles.Add(new FileInfo() { FilePath = file, MD5 = md5 });
        }));
    }
}