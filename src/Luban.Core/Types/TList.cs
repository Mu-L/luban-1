using Luban.Core.Defs;
using Luban.Core.TypeVisitors;

namespace Luban.Core.Types;

public class TList : TType
{
    public static TList Create(bool isNullable, Dictionary<string, string> tags, TType elementType, bool isArrayList)
    {
        return new TList(isNullable, tags, elementType, isArrayList);
    }

    public override string TypeName => "list";

    public override TType ElementType { get; }

    public bool IsArrayList { get; }

    private TList(bool isNullable, Dictionary<string, string> tags, TType elementType, bool isArrayList) : base(isNullable, tags)
    {
        ElementType = elementType;
        IsArrayList = isArrayList;
    }

    public override bool TryParseFrom(string s)
    {
        throw new NotSupportedException();
    }

    public override bool IsCollection => true;

    public override void PostCompile(DefField field)
    {
        base.PostCompile(field);

        foreach (var p in ElementType.Processors)
        {
            p.Compile(field);
        }

        if (ElementType is TBean e && !e.IsDynamic && e.Bean.HierarchyFields.Count == 0)
        {
            throw new Exception($"container element type:'{e.Bean.FullName}' can't be empty bean");
        }
        if (ElementType is TText)
        {
            throw new Exception($"bean:{field.HostType.FullName} field:{field.Name} container element type can't be text");
        }
    }

    public override void Apply<T>(ITypeActionVisitor<T> visitor, T x)
    {
        visitor.Accept(this, x);
    }

    public override void Apply<T1, T2>(ITypeActionVisitor<T1, T2> visitor, T1 x, T2 y)
    {
        visitor.Accept(this, x, y);
    }

    public override TR Apply<TR>(ITypeFuncVisitor<TR> visitor)
    {
        return visitor.Accept(this);
    }

    public override TR Apply<T, TR>(ITypeFuncVisitor<T, TR> visitor, T x)
    {
        return visitor.Accept(this, x);
    }

    public override TR Apply<T1, T2, TR>(ITypeFuncVisitor<T1, T2, TR> visitor, T1 x, T2 y)
    {
        return visitor.Accept(this, x, y);
    }

    public override TR Apply<T1, T2, T3, TR>(ITypeFuncVisitor<T1, T2, T3, TR> visitor, T1 x, T2 y, T3 z)
    {
        return visitor.Accept(this, x, y, z);
    }
}