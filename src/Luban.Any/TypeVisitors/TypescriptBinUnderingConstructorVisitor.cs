using Luban.Core.Types;

namespace Luban.Core.TypeVisitors;

class TypescriptBinUnderingConstructorVisitor : TypescriptBinUnderingDeserializeVisitorBase
{
    public static TypescriptBinUnderingConstructorVisitor Ins { get; } = new TypescriptBinUnderingConstructorVisitor();

    public override string Accept(TBean type, string bufVarName, string fieldName)
    {
        if (type.Bean.IsAbstractType)
        {
            return $"{fieldName} = {type.Bean.FullName}.constructorFrom({bufVarName})";
        }
        else
        {
            return $"{fieldName} = new {type.Bean.FullName}({bufVarName})";
        }
    }
}