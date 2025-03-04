using Luban.Core.Types;

namespace Luban.Core.TypeVisitors;

class JavaDeserializeVisitor : DecoratorFuncVisitor<string, string, string>
{
    public static JavaDeserializeVisitor Ins { get; } = new JavaDeserializeVisitor();

    public override string DoAccept(TType type, string bufName, string fieldName)
    {
        if (type.IsNullable)
        {
            return $"if({bufName}.readBool()){{ {type.Apply(JavaUnderingDeserializeVisitor.Ins, bufName, fieldName)} }} else {{ {fieldName} = null; }}";
        }
        else
        {
            return type.Apply(JavaUnderingDeserializeVisitor.Ins, bufName, fieldName);
        }
    }
}