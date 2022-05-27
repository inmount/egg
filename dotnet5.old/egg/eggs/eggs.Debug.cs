using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

    /// <summary>
    /// 调试输出
    /// </summary>
    /// <returns></returns>
    public static void Debug(string content, bool hasTime = false, bool endLine = false) { egg.Debug.Write(content, hasTime, endLine); }

    /// <summary>
    /// 调试输出行
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static void DebugLine(string content) { egg.Debug.Write(content, true, true); }

}
