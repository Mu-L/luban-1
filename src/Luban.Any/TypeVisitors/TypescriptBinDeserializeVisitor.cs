using Luban.Core.Types;

namespace Luban.Core.TypeVisitors;

class TypescriptBinDeserializeVisitor : DecoratorFuncVisitor<string, string, string>
{
    public static TypescriptBinDeserializeVisitor Ins { get; } = new TypescriptBinDeserializeVisitor();

    public override string DoAccept(TType type, string byteBufName, string fieldName)
    {
        if (type.IsNullable)
        {
            return $"if({byteBufName}.ReadBool()) {{ {type.Apply(TypescriptBinUnderingDeserializeVisitor.Ins, byteBufName, fieldName)} }} else {{ {fieldName} = null; }}";
        }
        else
        {
            return type.Apply(TypescriptBinUnderingDeserializeVisitor.Ins, byteBufName, fieldName);
        }
    }
}