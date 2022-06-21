using egg.db.SqlUnits;
using System;
using System.Collections.Generic;
using System.Text;

namespace egg.SqliteLog {

    /// <summary>
    /// 安装器
    /// </summary>
    internal static class LoggerSetup {

        // 安装对象注册模块
        private static void SetupObjects(egg.db.Database db) {
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                string Objects = "Objects";
                if (!dbc.CheckTable(Objects)) dbc.CreateTable(Objects);
                if (!dbc.CheckTableFiled(Objects, "Name")) dbc.AddTableFiled(Objects, new FieldDefine() { Name = "Name", Type = "string", Size = 150, Float = 0 });
                if (!dbc.CheckTableFiled(Objects, "Description")) dbc.AddTableFiled(Objects, new FieldDefine() { Name = "Description", Type = "string", Size = 150, Float = 0 });
            }
        }

        // 安装事件注册模块
        private static void SetupEvents(egg.db.Database db) {
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                string Events = "Events";
                if (!dbc.CheckTable(Events)) dbc.CreateTable(Events);
                if (!dbc.CheckTableFiled(Events, "ObjectID")) dbc.AddTableFiled(Events, new FieldDefine() { Name = "ObjectID", Type = "numeric", Size = 18, Float = 0 });
                if (!dbc.CheckTableFiled(Events, "Name")) dbc.AddTableFiled(Events, new FieldDefine() { Name = "Name", Type = "string", Size = 150, Float = 0 });
                if (!dbc.CheckTableFiled(Events, "Description")) dbc.AddTableFiled(Events, new FieldDefine() { Name = "Description", Type = "string", Size = 150, Float = 0 });
            }
        }

        // 安装日志模块
        private static void SetupLogs(egg.db.Database db) {
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                string Logs = "Logs";
                if (!dbc.CheckTable(Logs)) dbc.CreateTable(Logs);
                if (!dbc.CheckTableFiled(Logs, "ObjectID")) dbc.AddTableFiled(Logs, new FieldDefine() { Name = "ObjectID", Type = "numeric", Size = 18, Float = 0 });
                if (!dbc.CheckTableFiled(Logs, "EventID")) dbc.AddTableFiled(Logs, new FieldDefine() { Name = "EventID", Type = "numeric", Size = 18, Float = 0 });
                if (!dbc.CheckTableFiled(Logs, "TypeID")) dbc.AddTableFiled(Logs, new FieldDefine() { Name = "TypeID", Type = "numeric", Size = 18, Float = 0 });
                if (!dbc.CheckTableFiled(Logs, "Detail")) dbc.AddTableFiled(Logs, new FieldDefine() { Name = "Detail", Type = "string", Size = 5000, Float = 0 });
                if (!dbc.CheckTableFiled(Logs, "LogTime")) dbc.AddTableFiled(Logs, new FieldDefine() { Name = "LogTime", Type = "numeric", Size = 18, Float = 0 });
            }
        }

        // 安装数据库
        internal static void Setup(egg.db.Database db) {
            // 安装对象注册模块
            SetupObjects(db);
            // 安装事件注册模块
            SetupEvents(db);
            // 安装日志模块
            SetupLogs(db);
        }
    }
}
