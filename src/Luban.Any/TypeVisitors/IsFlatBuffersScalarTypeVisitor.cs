﻿using Luban.Core.Types;

namespace Luban.Core.TypeVisitors;

public class IsFlatBuffersScalarTypeVisitor : AllTrueVisitor
{
    public static IsFlatBuffersScalarTypeVisitor Ins { get; } = new();

    public override bool Accept(TBytes type)
    {
        return false;
    }

    public override bool Accept(TBean type)
    {
        return false;
    }

    public override bool Accept(TArray type)
    {
        return false;
    }

    public override bool Accept(TList type)
    {
        return false;
    }

    public override bool Accept(TSet type)
    {
        return false;
    }

    public override bool Accept(TMap type)
    {
        return false;
    }

    public override bool Accept(TVector2 type)
    {
        return false;
    }

    public override bool Accept(TVector3 type)
    {
        return false;
    }

    public override bool Accept(TVector4 type)
    {
        return false;
    }
}