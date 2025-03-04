using Luban.Core.RawDefs;
using Luban.Core.Types;
using Luban.Core.TypeVisitors;
using Luban.Core.Utils;

namespace Luban.Core.Defs;

public class DefField
{
    public DefAssembly Assembly => HostType.Assembly;

    public DefBean HostType { get; set; }

    public string Name { get; protected set; }

    // public string ConventionName
    // {
    //     get
    //     {
    //         string cn;
    //         ELanguage curLan = Assembly.CurrentLanguage;
    //         switch (Assembly.NamingConventionBeanMember)
    //         {
    //             case NamingConvention.None: cn = Name; break;
    //             case NamingConvention.CameraCase: cn = TypeUtil.ToCamelCase(Name); break;
    //             case NamingConvention.PascalCase: cn = TypeUtil.ToPascalCase(Name); break;
    //             case NamingConvention.UnderScores: cn = TypeUtil.ToUnderScores(Name); break;
    //             case NamingConvention.Invalid: throw new Exception($"invalid NamingConvention");
    //             case NamingConvention.LanguangeRecommend:
    //             {
    //                 switch (curLan)
    //                 {
    //                     case ELanguage.INVALID: throw new Exception($"not set current language. can't get recommend naming convention name");
    //                     case ELanguage.CS: cn = TypeUtil.ToPascalCase(Name); break;
    //                     case ELanguage.JAVA: cn = TypeUtil.ToCamelCase(Name); break;
    //                     case ELanguage.GO: cn = TypeUtil.ToPascalCase(Name); break;
    //                     case ELanguage.CPP: cn = TypeUtil.ToCamelCase(Name); break;
    //                     case ELanguage.LUA: cn = TypeUtil.ToUnderScores(Name); break;
    //                     case ELanguage.JAVASCRIPT: cn = TypeUtil.ToCamelCase(Name); break;
    //                     case ELanguage.TYPESCRIPT: cn = TypeUtil.ToCamelCase(Name); break;
    //                     case ELanguage.PYTHON: cn = TypeUtil.ToUnderScores(Name); break;
    //                     case ELanguage.GDSCRIPT: cn = TypeUtil.ToUnderScores(Name); break;
    //                     case ELanguage.RUST: cn = TypeUtil.ToUnderScores(Name); break;
    //                     case ELanguage.PROTOBUF: cn = Name; break;
    //                     default: throw new Exception($"unknown language:{curLan}");
    //                 }
    //                 break;
    //             }
    //             default: throw new Exception($"unknown NamingConvention:{Assembly.NamingConventionBeanMember}");
    //         }
    //         if (curLan == ELanguage.RUST)
    //         {
    //             if (cn == "type")
    //             {
    //                 cn = "r#type";
    //             }
    //         }
    //         return cn;
    //     }
    // }

    public string Type { get; }

    public TType CType { get; protected set; }

    public bool IsNullable => CType.IsNullable;

    public string UpperCaseName => Name.ToUpper();

    public string Comment { get; }

    // public string EscapeComment => DefUtil.EscapeCommentByCurrentLanguage(Comment);

    public Dictionary<string, string> Tags { get; }

    public bool IgnoreNameValidation { get; set; }

    public bool HasTag(string attrName)
    {
        return Tags != null && Tags.ContainsKey(attrName);
    }

    public string GetTag(string attrName)
    {
        return Tags != null && Tags.TryGetValue(attrName, out var value) ? value : null;
    }
    
    public string Index { get; private set; }

    public List<string> Groups { get; }

    public DefField IndexField { get; private set; }

    // public RefValidator Ref { get; private set; }

    // private TType _refType;
    //
    // public TType RefType => _refType ??= Assembly.GetCfgTable(Ref.FirstTable).ValueTType;
    //
    // public RefValidator ElementRef { get; private set; }

    // private TType _eleRefType;
    //
    // public TType ElementRefType
    // {
    //     get
    //     {
    //         if (_eleRefType == null)
    //         {
    //             TType refValueType = Assembly.GetCfgTable(ElementRef.FirstTable).ValueTType;
    //             _eleRefType = CType switch
    //             {
    //                 TArray ta => TArray.Create(false, null, refValueType),
    //                 TList tl => TList.Create(false, null, refValueType, true),
    //                 TSet ts => TSet.Create(false, null, refValueType, false),
    //                 TMap tm => TMap.Create(false, null, tm.KeyType, refValueType, false),
    //                 _ => throw new Exception($"not support ref type:'{CType.TypeName}'"),
    //             };
    //         }
    //         return _eleRefType;
    //     }
    // }


    // 如果ref了多个表，不再生成 xxx_ref之类的字段，也不会resolve
    // public bool GenRef
    // {
    //     get
    //     {
    //         if(Ref != null)
    //         {
    //             return Ref.GenRef;
    //         }
    //         // 特殊处理, 目前只有c#和java支持.而这个属性已经被多种语言模板引用了，故单独处理一下
    //         if (DefAssembly.LocalAssebmly.CurrentLanguage != Common.ELanguage.CS
    //             && DefAssembly.LocalAssebmly.CurrentLanguage != Common.ELanguage.JAVA)
    //         {
    //             return false;
    //         }
    //         return ElementRef?.GenRef == true;
    //     }
    // }
    //
    // public bool HasRecursiveRef => (CType is TBean tb && HostType.Assembly.GetExternalTypeMapper(tb) == null)
    //                                || (CType.ElementType is TBean eb && HostType.Assembly.GetExternalTypeMapper(eb) == null);
    //
    // public string CsRefTypeName => RefType.Apply(CsDefineTypeName.Ins);
    //
    // public string CsRefValidatorDefine
    // {
    //     get
    //     {
    //         if (Ref != null)
    //         {
    //             return $"{RefType.Apply(CsDefineTypeName.Ins)} {RefVarName} {{ get; private set; }}";
    //         }
    //         else if (ElementRef != null)
    //         {
    //             return $"{ElementRefType.Apply(CsDefineTypeName.Ins)} {RefVarName} {{ get; private set; }}";
    //         }
    //         else
    //         {
    //             throw new NotSupportedException();
    //         }
    //     }
    // }
    //
    // public string JavaRefTypeName
    // {
    //     get
    //     {
    //         var table = Assembly.GetCfgTable(Ref.FirstTable);
    //         return table.ValueTType.Apply(JavaDefineTypeName.Ins);
    //     }
    // }
    //
    // public string JavaRefValidatorDefine
    // {
    //     get
    //     {
    //         if (Ref != null)
    //         {
    //             return $"{RefType.Apply(JavaDefineTypeName.Ins)} {RefVarName};";
    //         }
    //         else if (ElementRef != null)
    //         {
    //             return $"{ElementRefType.Apply(JavaDefineTypeName.Ins)} {RefVarName};";
    //         }
    //         else
    //         {
    //             throw new NotSupportedException();
    //         }
    //     }
    // }
    //
    // public string CppRefValidatorDefine
    // {
    //     get
    //     {
    //         var table = Assembly.GetCfgTable(Ref.FirstTable);
    //         return $"{table.ValueTType.Apply(CppDefineTypeName.Ins)} {RefVarName};";
    //     }
    // }
    //
    // public string TsRefValidatorDefine
    // {
    //     get
    //     {
    //         var table = Assembly.GetCfgTable(Ref.FirstTable);
    //         return $"{RefVarName} : {table.ValueTType.Apply(TypescriptDefineTypeNameVisitor.Ins)}{(IsNullable ? "" : " = undefined!")}";
    //     }
    // }
    //
    // public string RefVarName => $"{ConventionName}_Ref";


    public string ConventionGetterName => TypeUtil.ToJavaGetterName(Name);

    //public string JavaGetterName => TypeUtil.ToJavaGetterName(Name);

    //public string CppGetterName => JavaGetterName;

    // public bool NeedExport => Assembly.NeedExport(this.Groups);

    public TEnum Remapper { get; private set; }

    public RawField RawDefine { get; }

    public string GetTextKeyName(string name) => name + TText.L10N_FIELD_SUFFIX;

    public bool GenTextKey => this.CType is TText;


    public DefField(DefBean host, RawField f, int idOffset)
    {
        HostType = host;
        Name = f.Name;
        Type = f.Type;
        Comment = f.Comment;
        Tags = DefUtil.ParseAttrs(f.Tags);
        IgnoreNameValidation = f.IgnoreNameValidation;
        this.Groups = f.Groups;
        this.RawDefine = f;
    }

    public void Compile()
    {
        if (!IgnoreNameValidation && !TypeUtil.IsValidName(Name))
        {
            throw new Exception($"type:'{HostType.FullName}' field name:'{Name}' is reserved");
        }

        try
        {
            CType = Assembly.CreateType(HostType.Namespace, Type, false);
        }
        catch (Exception e)
        {
            throw new Exception($"type:'{HostType.FullName}' field:'{Name}' type:'{Type}' is invalid", e);
        }

        //if (IsNullable && (CType.IsCollection || (CType is TBean)))
        //{
        //    throw new Exception($"type:{HostType.FullName} field:{Name} type:{Type} is collection or bean. not support nullable");
        //}

        switch (CType)
        {
            case TArray t:
            {
                if (t.ElementType is TBean e && !e.IsDynamic && e.Bean.HierarchyFields.Count == 0)
                {
                    throw new Exception($"container element type:'{e.Bean.FullName}' can't be empty bean");
                }
                break;
            }
            case TList t:
            {
                if (t.ElementType is TBean e && !e.IsDynamic && e.Bean.HierarchyFields.Count == 0)
                {
                    throw new Exception($"container element type:'{e.Bean.FullName}' can't be empty bean");
                }
                break;
            }
        }

        // ValidatorUtil.CreateValidators(CType);
        // var selfRef = this.CType.Processors.Find(v => v is RefValidator);
        // if (selfRef != null)
        // {
        //     this.Ref = (RefValidator)selfRef;
        // }
        //
        // var eleType = CType.ElementType;
        // if (eleType != null)
        // {
        //     ElementRef = (RefValidator)eleType.Processors.Find(p => p is RefValidator);
        // }
    }

    private void ValidateIndex()
    {
        Index = CType.GetTag("index");

        if (!string.IsNullOrEmpty(Index))
        {
            if ((CType is TArray tarray) && (tarray.ElementType is TBean b))
            {
                if ((IndexField = b.GetBeanAs<DefBean>().GetField(Index)) == null)
                {
                    throw new Exception($"type:'{HostType.FullName}' field:'{Name}' index:'{Index}'. index not exist");
                }
            }
            else if ((CType is TList tlist) && (tlist.ElementType is TBean tb))
            {
                if ((IndexField = tb.GetBeanAs<DefBean>().GetField(Index)) == null)
                {
                    throw new Exception($"type:'{HostType.FullName}' field:'{Name}' index:'{Index}'. index not exist");
                }
            }
            else
            {
                throw new Exception($"type:'{HostType.FullName}' field:'{Name}' index:'{Index}'. only array:bean or list:bean support index");
            }
        }
    }

    public void PostCompile()
    {
        CType.PostCompile(this);
        ValidateIndex();
    }

    public static void CompileFields<T>(DefTypeBase hostType, List<T> fields) where T : DefField
    {
        var names = new HashSet<string>();
        foreach (var f in fields)
        {
            var fname = f.Name;
            if (fname.Length == 0)
            {
                throw new Exception($"type:'{hostType.FullName}' field name can't be empty");
            }
            if (!names.Add(fname))
            {
                throw new Exception($"type:'{hostType.FullName}' 'field:{fname}' duplicate");
            }
            if (TypeUtil.ToCsStyleName(fname) == hostType.Name)
            {
                throw new Exception($"type:'{hostType.FullName}' field:'{fname}' 生成的c#字段名与类型名相同，会引起编译错误");
            }
        }

        foreach (var f in fields)
        {
            f.Compile();
        }
    }
}