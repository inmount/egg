using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

    /// <summary>
    /// 判断对象是否为空
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNull(System.Object obj) {
        return Equals(obj, null);
    }

    /// <summary>
    /// 判断字符串是否为数字
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNumber(string str) {
        double temp = 0;
        if (double.TryParse(str, out temp)) {
            return true;
        }
        return false;
    }

}
