using Luban.Core.Defs;
using Luban.Core.TypeVisitors;

namespace Luban.Core.Types;

public class TMap : TType
{
    public static TMap Create(bool isNullable, Dictionary<string, string> tags, TType keyType, TType valueType, bool isOrderedMap)
    {
        return new TMap(isNullable, tags, keyType, valueType, isOrderedMap);
    }

    public override string TypeName => "map";

    public bool IsMap => true;

    public TType KeyType { get; }

    public TType ValueType { get; }

    public override TType ElementType => ValueType;

    public bool IsOrderedMap { get; }

    private TMap(bool isNullable, Dictionary<string, string> tags, TType keyType, TType valueType, bool isOrderedMap) : base(isNullable, tags)
    {
        KeyType = keyType;
        ValueType = valueType;
        IsOrderedMap = isOrderedMap;
    }

    public override bool TryParseFrom(string s)
    {
        throw new NotSupportedException();
    }

    public override bool IsCollection => true;

    public override void PostCompile(DefField field)
    {
        base.PostCompile(field);

        foreach (var p in KeyType.Processors)
        {
            p.Compile(field);
        }

        foreach (var p in ValueType.Processors)
        {
            p.Compile(field);
        }

        if (KeyType is TText)
        {
            throw new Exception($"bean:{field.HostType.FullName} field:{field.Name} container key type can't be text");
        }
        if (ValueType is TText)
        {
            throw new Exception($"bean:{field.HostType.FullName} field:{field.Name} container value type can't be text");
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