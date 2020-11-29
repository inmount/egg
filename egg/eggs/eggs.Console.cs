using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

    /// <summary>
    /// 根据URL获取超文本内容
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string GetHttpText(string url) {
        return egg.Net.HttpClient.Get(url);
    }

    /// <summary>
    /// 根据URL获取Json对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static egg.Json.JsonObject GetHttpJson(string url) {
        string json = egg.Net.HttpClient.Get(url);
        return (egg.Json.JsonObject)egg.Json.Parser.ParseJson(json);
    }

    /// <summary>
    /// 根据URL获取HTML对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static egg.Html.HtmlDocument GetHttpHtmlDocument(string url) {
        string html = egg.Net.HttpClient.Get(url);
        return egg.Html.Parser.GetDocument(html);
    }

}
