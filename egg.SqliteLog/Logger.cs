using egg.db;
using Orm = egg.db.Orm;
using System;
using egg;
using System.Collections.Generic;
using egg.db.SqlUnits;

namespace egg.SqliteLog {

    /// <summary>
    /// 日志管理器
    /// </summary>
    public class Logger : egg.BasicObject {

        /// <summary>
        /// 获取关联数据库定义
        /// </summary>
        public egg.db.SqliteDatabase Database { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Logger(string path) {
            // 定义数据库
            this.Database = new db.SqliteDatabase() { Path = path };
            // 安装数据库
            LoggerSetup.Setup(this.Database);
        }

        #region [=====读取相关=====]

        /// <summary>
        /// 加载所有的对象
        /// </summary>
        /// <returns></returns>
        public Orm.Rows<Orms.Objects> GetAllObjects() {
            // 定义数据库
            var db = this.Database;
            // 定义表对象
            var Objects = Orm.Table.CreateTableDefine<Orms.Objects>();
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                // 读取所有数据
                return dbc.Select(Objects).GetRows<Orms.Objects>();
            }
        }

        /// <summary>
        /// 加载所有的事件
        /// </summary>
        /// <returns></returns>
        public Orm.Rows<Orms.Events> GetAllEvents(long objId) {
            // 定义数据库
            var db = this.Database;
            // 定义表对象
            var Events = Orm.Table.CreateTableDefine<Orms.Events>();
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                // 读取所有数据
                return dbc.Select(Events).Where(Events["ObjectID"] == objId).GetRows<Orms.Events>();
            }
        }

        /// <summary>
        /// 获取对象编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetObjectId(string name) {
            // 定义数据库
            var db = this.Database;
            // 定义表对象
            var Objects = Orm.Table.CreateTableDefine<Orms.Objects>();
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                // 读取数据
                var row = dbc.Select(Objects).Where(Objects["Name"] == name).GetRow<Orms.Objects>();
                if (!row.IsEmpty) return row["ID"].ToLong();
                // 插入一条新数据
                var rowInsert = new Orms.Objects();
                rowInsert.Name = name;
                rowInsert.Description = $"";
                dbc.Insert(rowInsert).Exec();
                // 重新读取数据
                row = dbc.Select(Objects).Where(Objects["Name"] == name).GetRow<Orms.Objects>();
                return row.ID;
            }
        }

        /// <summary>
        /// 获取对象编号
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetEventId(long objId, string name) {
            // 定义数据库
            var db = this.Database;
            // 定义表对象
            var Events = Orm.Table.CreateTableDefine<Orms.Events>();
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                // 读取数据
                var row = dbc.Select(Events).Where(Events["Name"] == name & Events["ObjectID"] == objId).GetRow<Orms.Events>();
                if (!row.IsEmpty) return row["ID"].ToLong();
                // 插入一条新数据
                var rowInsert = new Orms.Events();
                rowInsert.ObjectID = objId;
                rowInsert["Name"] = name;
                rowInsert["Description"] = $"";
                dbc.Insert(rowInsert).Exec();
                // 重新读取数据
                row = dbc.Select(Events).Where(Events["Name"] == name & Events["ObjectID"] == objId).GetRow<Orms.Events>();
                return row.ID;
            }
        }

        /// <summary>
        /// 按条件获取记录
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(Formula formula) {
            // 定义数据库
            var db = this.Database;
            // 定义表对象
            //var Objects = OrmMapper.Table("Objects");
            //var Events = OrmMapper.Table("Events");
            //var Logs = OrmMapper.Table("Logs");
            var Objects = Orm.Table.CreateTableDefine<Orms.Objects>();
            var Events = Orm.Table.CreateTableDefine<Orms.Events>();
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                // 读取数据
                var rows = dbc.Select(Logs, Objects, Events)
                    .Columns(Logs["*"], Objects["Name"].As("ObjectName"), Events["Name"].As("EventName"))
                    .Where(formula & Logs["ObjectID"] == Objects["ID"] & Logs["EventID"] == Events["ID"])
                    .GetRows<LoggerRecord>();
                // 遍历数据
                foreach (var row in rows) {
                    //obj.Id = row["ID"].ToLong();
                    //obj.ObjectName = row["ObjectName"];
                    //obj.EventName = row["EventName"];
                    //obj.TypeName = ((LoggerTypes)(row["TypeID"].ToInteger())).ToString();
                    //obj.Detail = row["Detail"];
                    row.Time = eggs.Time.GetTime(row.LogTime);
                    //records.Add(obj);
                }
                return rows;
            }
            
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(long startTime) {
            // 定义表对象
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["LogTime"] >= startTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(long startTime, long endTime) {
            // 定义表对象
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(long objId, long startTime, long endTime) {
            // 定义表对象
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["ObjectID"] == objId & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(string objName, long startTime, long endTime) {
            // 定义表对象
            var Objects = Orm.Table.CreateTableDefine<Orms.Objects>();
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Objects["Name"] == objName & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="evtId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(long objId, long evtId, long startTime, long endTime) {
            // 定义表对象
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["ObjectID"] == objId & Logs["EventID"] == evtId & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="evtName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(long objId, string evtName, long startTime, long endTime) {
            // 定义表对象
            var Objects = Orm.Table.CreateTableDefine<Orms.Objects>();
            var Events = Orm.Table.CreateTableDefine<Orms.Events>();
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["ObjectID"] == objId & Events["Name"] == evtName & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="evtName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(string objName, string evtName, long startTime, long endTime) {
            // 定义表对象
            var Objects = Orm.Table.CreateTableDefine<Orms.Objects>();
            var Events = Orm.Table.CreateTableDefine<Orms.Events>();
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Objects["Name"] == objName & Events["Name"] == evtName & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(LoggerTypes tp, long startTime, long endTime) {
            // 定义表对象
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["TypeID"] == (int)tp & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="evtId"></param>
        /// <param name="tp"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(long evtId, LoggerTypes tp, long startTime, long endTime) {
            // 定义表对象
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["EventID"] == evtId & Logs["TypeID"] == (int)tp & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="evtName"></param>
        /// <param name="tp"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(long objId, string evtName, LoggerTypes tp, long startTime, long endTime) {
            // 定义表对象
            var Events = Orm.Table.CreateTableDefine<Orms.Events>();
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Logs["ObjectID"] == objId & Events["Name"] == evtName & Logs["TypeID"] == (int)tp & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="evtName"></param>
        /// <param name="tp"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Orm.Rows<LoggerRecord> GetRecords(string objName, string evtName, LoggerTypes tp, long startTime, long endTime) {
            // 定义表对象
            var Objects = Orm.Table.CreateTableDefine<Orms.Objects>();
            var Events = Orm.Table.CreateTableDefine<Orms.Events>();
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            return GetRecords(Objects["Name"] == objName & Events["Name"] == evtName & Logs["TypeID"] == (int)tp & Logs["LogTime"] >= startTime & Logs["LogTime"] <= endTime);
        }

        #endregion

        #region [=====记录相关=====]

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="evtId"></param>
        /// <param name="tp"></param>
        /// <param name="message"></param>
        public void Log(long objId, long evtId, LoggerTypes tp, string message) {
            // 定义数据库
            var db = this.Database;
            // 定义表对象
            var Logs = Orm.Table.CreateTableDefine<Orms.Logs>();
            using (egg.db.Connection dbc = new egg.db.Connection(db)) {
                var rowInsert = new Row();
                rowInsert["ObjectID"] = $"{objId}";
                rowInsert["EventID"] = $"{evtId}";
                rowInsert["TypeID"] = $"{(int)tp}";
                rowInsert["Detail"] = message;
                rowInsert["LogTime"] = $"{eggs.Time.GetNow().ToTimeStamp()}";
                dbc.Insert(Logs, rowInsert).Exec();
            }
        }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="evtId"></param>
        /// <param name="message"></param>
        public void LogInfo(long objId, long evtId, string message) {
            Log(objId, evtId, LoggerTypes.Info, message);
        }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="evtName"></param>
        /// <param name="message"></param>
        public void LogInfo(string objName, string evtName, string message) {
            long objId = GetObjectId(objName);
            long evtId = GetEventId(objId, evtName);
            LogInfo(objId, evtId, message);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="evtId"></param>
        /// <param name="message"></param>
        public void LogWarnning(long objId, long evtId, string message) {
            Log(objId, evtId, LoggerTypes.Warn, message);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="evtName"></param>
        /// <param name="message"></param>
        public void LogWarnning(string objName, string evtName, string message) {
            long objId = GetObjectId(objName);
            long evtId = GetEventId(objId, evtName);
            LogWarnning(objId, evtId, message);
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="evtId"></param>
        /// <param name="message"></param>
        public void LogError(long objId, long evtId, string message) {
            Log(objId, evtId, LoggerTypes.Error, message);
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="evtName"></param>
        /// <param name="message"></param>
        public void LogError(string objName, string evtName, string message) {
            long objId = GetObjectId(objName);
            long evtId = GetEventId(objId, evtName);
            LogError(objId, evtId, message);
        }

        #endregion
    }
}
