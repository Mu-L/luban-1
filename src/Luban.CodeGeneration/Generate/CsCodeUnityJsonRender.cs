using Luban.Job.Common.Generate;

namespace Luban.Generate;

[Render("code_cs_unity_json")]
class CsCodeUnityJsonRender : TemplateCodeRenderBase
{
    protected override string RenderTemplateDir => "cs_unity_json";
}