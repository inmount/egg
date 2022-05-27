using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

    /// <summary>
    /// 解析Json字符串
    /// </summary>
    /// <param name="json"></param>
    /// <param name="tp"></param>
    /// <returns></returns>
    public static egg.JsonBean.IUnit ParseJson(string json, Type tp = null) {
        return egg.JsonBean.Parser.Parse(json, tp);
    }

}
