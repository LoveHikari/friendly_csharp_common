using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;

namespace Hikari.Common;

/// <summary>
/// 脚本引擎
/// </summary>
/// <see>
///     <cref>https://www.cnblogs.com/FFFirer/p/csharpjs.html</cref>
/// </see>
public class ScriptEngine
{
    /// <summary>
    /// 运行Run方法
    /// </summary>
    /// <param name="code">函数体 例如：fucniton add(int a,int b){return a+b;}</param>
    /// <param name="functionName">函数名称 例如:add</param>
    /// <param name="args">参数</param>
    /// <returns>返回值</returns>
    public string? Run(string code, string functionName, params object[] args)
    {
        var switcher = JsEngineSwitcher.Current;
        switcher.EngineFactories.Add(new ChakraCoreJsEngineFactory());
        switcher.DefaultEngineName = ChakraCoreJsEngine.EngineName;
        IJsEngine engine = switcher.CreateDefaultEngine();
        engine.Execute(code);
        string result = engine.CallFunction<string>(functionName, args);
        return result == "undefined" ? null : result;
    }
}