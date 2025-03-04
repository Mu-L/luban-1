using Luban.Utils;
using Luban.Server.Common;

namespace Luban.Job.Utils;

public static class CacheFileUtil
{
    private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

    public static string GenStringOrBytesMd5AndAddCache(string fileName, object content)
    {
        switch (content)
        {
            case string s: return GenMd5AndAddCache(fileName, s);
            case byte[] bs: return GenMd5AndAddCache(fileName, bs);
            default: throw new System.NotSupportedException();
        }
    }

    public static string GenMd5AndAddCache(string fileName, string content, bool withUtf8Bom = false)
    {
        content = content.Replace("\r\n", "\n");
        byte[] bytes;
        if (!withUtf8Bom)
        {
            bytes = System.Text.Encoding.UTF8.GetBytes(content);
        }
        else
        {
            bytes = new byte[System.Text.Encoding.UTF8.GetByteCount(content) + 3 /* bom header */];
            bytes[0] = 0xef;
            bytes[1] = 0xbb;
            bytes[2] = 0xbf;
            System.Text.Encoding.UTF8.GetBytes(content, bytes.AsSpan(3));
        }

        var md5 = FileUtil.CalcMD5(bytes);
        CacheManager.Ins.AddCache(fileName, md5, bytes);
        return md5;
    }

    public static string GenMd5AndAddCache(string fileName, byte[] bytes)
    {
        var md5 = FileUtil.CalcMD5(bytes);
        CacheManager.Ins.AddCache(fileName, md5, bytes);
        return md5;
    }
}