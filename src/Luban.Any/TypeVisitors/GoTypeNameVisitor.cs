using Luban.Core.Types;

namespace Luban.Core.TypeVisitors;

public class GoTypeNameVisitor : DecoratorFuncVisitor<string>
{
    public static GoTypeNameVisitor Ins { get; } = new GoTypeNameVisitor();

    public override string DoAccept(TType type)
    {
        var s = type.Apply(GoTypeUnderingNameVisitor.Ins);
        return type.Apply(GoIsPointerTypeVisitor.Ins) ? "*" + s : s;
    }
}