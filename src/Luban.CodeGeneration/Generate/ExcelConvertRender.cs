﻿using Luban.Core;
using Luban.Core.Defs;
using Luban.Core.Types;
using Luban.Core.TypeVisitors;
using Luban.Utils;
using Luban.Job.Common.Generate;

namespace Luban.Generate;

[Render("convert_xlsx")]
[Render("convert_excel")]
class ExcelConvertRender : DataRenderBase
{
    public override void Render(GenerationContext ctx)
    {
        string genType = ctx.GenType;
        foreach (var table in ctx.ExportTables)
        {
            ctx.Tasks.Add(Task.Run(() =>
            {
                var records = DefAssembly.ToSortByKeyDataList(table, ctx.Assembly.GetTableAllDataList(table));

                string dirName = table.FullName;
                var fileName = table.FullName;
                var filePath = $"{dirName}/{fileName}.xlsx";


                var title = TitleCreator.Ins.CreateTitle(table);

                TBean valueType = table.ValueTType;

                var dataRangeArray = new List<object[]>();
                {
                    var titleRow = new object[title.ToIndex + 1];
                    titleRow[0] = "##";

                    foreach (var subTitle in title.SubTitleList)
                    {
                        string titleAndTags = subTitle.Tags.Count == 0 ? subTitle.Name : subTitle.Name + "#" + string.Join('#', subTitle.Tags.Select(e => $"{e.Key}={e.Value}"));
                        titleRow[subTitle.FromIndex] = titleAndTags;
                    }
                    dataRangeArray.Add(titleRow);
                }
                {

                    var typeRow = new object[title.ToIndex + 1];
                    typeRow[0] = "##type";

                    foreach (var subTitle in title.SubTitleList)
                    {
                        string typeAndTags = valueType.Bean.TryGetField(subTitle.Name, out var f, out _) ?
                            (f.CType.Tags.Count == 0 ? f.CType.Apply(CsDefineTypeName.Ins) : f.CType.Apply(CsDefineTypeName.Ins) + "#" + string.Join('#', f.CType.Tags.Select(e => $"({e.Key}={e.Value})")))
                            : "";
                        typeRow[subTitle.FromIndex] = typeAndTags;
                    }
                    dataRangeArray.Add(typeRow);
                }


                dataRangeArray.Add(new object[] { "##" });

                int totalRowCount = dataRangeArray.Count;
                foreach (var rec in records)
                {
                    var fillVisitor = new FillSheetVisitor(dataRangeArray, title.ToIndex + 1, totalRowCount);
                    totalRowCount += rec.Data.Apply(fillVisitor, TBean.Create(false, rec.Data.Type, null), title);
                }

                using var workbook = new XLWorkbook(XLEventTracking.Disabled);
                var sheet = workbook.AddWorksheet();
                sheet.Cell(1, 1).InsertData(dataRangeArray);
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                byte[] content = DataUtil.StreamToBytes(stream);
                var md5 = CacheFileUtil.GenStringOrBytesMd5AndAddCache(filePath, content);
                FileRecordCacheManager.Ins.AddCachedRecordOutputData(table, records, genType, md5);
                ctx.GenDataFilesInOutputDataDir.Add(new Luban.Protos.FileInfo() { FilePath = filePath, MD5 = md5 });
            }));

        }
    }
}