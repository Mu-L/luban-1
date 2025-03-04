using Luban.Core.Types;

namespace Luban.Core.TypeVisitors;

public class CppDefineTypeName : DecoratorFuncVisitor<string>
{
    public static CppDefineTypeName Ins { get; } = new CppDefineTypeName();

    public override string DoAccept(TType type)
    {
        return type.IsNullable ? $"::bright::SharedPtr<{type.Apply(CppSharedPtrUnderingDefineTypeName.Ins)}>" : type.Apply(CppSharedPtrUnderingDefineTypeName.Ins);
    }

    public override string Accept(TBean type)
    {
        return type.Apply(CppSharedPtrUnderingDefineTypeName.Ins);
    }
}