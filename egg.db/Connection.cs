using egg.db.Orm;
using egg.db.SqlStatements;
using System;
using System.Reflection;

namespace egg.db {

    /// <summary>
    /// 通用数据库连接管理器
    /// </summary>
    public class Connection : egg.BasicObject, IConnectionable {

        private IConnectionable dbc; //数据库连接管理器
        private egg.db.Database db;    //数据库定义对象

        // 内置索引器
        private int _index;

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public egg.db.DatabaseTypes DatabaseType { get; private set; }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public string ConnectionString { get; private set; }
        bool IConnectionable.IsOpen { get => throw new NotImplementedException(); }

        void IConnectionable.Open(string connectionString) {
            throw new NotImplementedException();
        }

        void IConnectionable.Close() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="database"></param>
        public Connection(egg.db.Database database) {
            db = database;
            this.DatabaseType = db.Type;
            this.ConnectionString = db.ToString();
            dbc = db.CreateConnection();
            if (dbc == null) throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
        }

        /// <summary>
        /// 获取一个索引值
        /// </summary>
        /// <returns></returns>
        public int GetNewIndex() {
            _index++;
            return _index;
        }

        #region [=====语句操作=====]

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="sql">Generic Database Manipulation Language</param>
        /// <returns></returns>
        public Rows GetRows(string sql) {
            if (!dbc.IsOpen) dbc.Open(this.ConnectionString);

            try {
                return dbc.GetRows(sql);
            } catch (Exception ex) {
                throw new Exception(ex.Message + " => SQL:" + sql, ex);
            }
        }


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Rows<T> GetRows<T>(string sql) where T : Orm.Row, new() {
            //throw new NotImplementedException();
            if (!dbc.IsOpen) dbc.Open(this.ConnectionString);
            try {
                return dbc.GetRows<T>(sql);
            } catch (Exception ex) {
                throw new Exception(ex.Message + " => SQL:" + sql, ex);
            }
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="sql">Generic Database Manipulation Language</param>
        /// <returns></returns>
        public Row GetRow(string sql) {
            if (!dbc.IsOpen) dbc.Open(this.ConnectionString);
            try {
                return dbc.GetRow(sql);
            } catch (Exception ex) {
                throw new Exception(ex.Message + " => SQL:" + sql, ex);
            }
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T GetRow<T>(string sql) where T : Orm.Row, new() {
            if (!dbc.IsOpen) dbc.Open(this.ConnectionString);
            try {
                return dbc.GetRow<T>(sql);
            } catch (Exception ex) {
                throw new Exception(ex.Message + " => SQL:" + sql, ex);
            }
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql">Generic Database Manipulation Language</param>
        public int Exec(string sql) {
            if (!dbc.IsOpen) dbc.Open(this.ConnectionString);
            try {
                return dbc.Exec(sql);
            } catch (Exception ex) {
                throw new Exception(ex.Message + " => SQL:" + sql, ex);
            }
        }

        /// <summary>
        /// 获取标准的SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetStandardSqlString(ISqlStringable sql) {
            return sql.ToSqlString(this.DatabaseType);
        }

        /// <summary>
        /// 生成一个查询语句
        /// </summary>
        /// <param name="sqlTables"></param>
        /// <returns></returns>
        public Select Select(params ISqlTableStringable[] sqlTables) {
            return new Select(this, sqlTables);
        }

        /// <summary>
        /// 生成一个插入语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public Insert Insert(SqlUnits.Table table, Row row) {
            return new Insert(this, table, row);
        }

        /// <summary>
        /// 生成一个插入语句
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Insert Insert(Orm.Row row) {
            return new Insert(this, row);
        }

        /// <summary>
        /// 生成一个更新语句
        /// </summary>
        /// <param name="table">表对象</param>
        /// <param name="row">数据行对象</param>
        /// <param name="keyField">更新键字段</param>
        /// <returns></returns>
        public Update Update(SqlUnits.Table table, Row row, SqlUnits.TableField keyField = null) {
            return new Update(this, table, row, keyField);
        }

        /// <summary>
        /// 生成一个删除语句
        /// </summary>
        /// <param name="table">表对象</param>
        /// <returns></returns>
        public Delete Delete(SqlUnits.Table table) {
            return new Delete(this, table);
        }

        #endregion

        #region [=====数据库操作=====]

        /// <summary>
        /// 检测数据库是否存在
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public bool CheckDatabase(string dbName) {
            bool res = false;

            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    var row = GetRow($"select * from sys.databases where [name] = '{dbName}'");
                    res = !row.IsEmpty;
                    break;
                case DatabaseTypes.MySQL:
                    row = GetRow($"show databases like '{dbName}';");
                    res = !row.IsEmpty;
                    break;
                case DatabaseTypes.PostgreSQL:
                    row = GetRow($"SELECT u.datname FROM pg_catalog.pg_database u where u.datname='{dbName}';");
                    res = !row.IsEmpty;
                    break;
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    throw new Exception($"文件型数据库不支持库检测");
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }

            return res;
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public void CreateDatabase(string dbName, string path = "") {
            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    if (path != "") {
                        string sql = $"create database [{dbName}] " +
                            $"on primary (" +
                            $"name='{dbName}'," +
                            $"filename='{path}/{dbName}.mdf'," +
                            $"size=5MB," +
                            $"maxsize=2048MB," +
                            $"filegrowth=20%" +
                            $")" +
                            $"log on (" +
                            $"name='{dbName}_log'," +
                            $"filename='{path}/{dbName}_log.ldf'," +
                            $"size=5mb," +
                            $"filegrowth=5mb" +
                            $")";
                        Exec(sql);
                    } else {
                        Exec($"CREATE DATABASE [{dbName}]");
                    }
                    break;
                case DatabaseTypes.MySQL:
                    Exec($"CREATE DATABASE `{dbName}`;");
                    break;
                case DatabaseTypes.PostgreSQL:
                    Exec($"CREATE DATABASE \"{dbName}\";");
                    break;
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    throw new Exception($"文件型数据库不支持库检测");
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public void DropDatabase(string dbName) {
            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    Exec($"drop database [{dbName}];");
                    break;
                case DatabaseTypes.MySQL:
                    Exec($"drop database if exists `{dbName}`;");
                    break;
                case DatabaseTypes.PostgreSQL:
                    Exec($"drop database if exists \"{dbName}\";");
                    break;
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    throw new Exception($"文件型数据库不支持库操作");
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        #endregion

        #region [=====表操作=====]

        /// <summary>
        /// 检测数据表是否存在
        /// </summary>
        /// <param name="row"></param>
        public void CheckTable(Orm.Row row) {
            CheckTable(row.GetType());
        }

        /// <summary>
        /// 检测数据表是否存在
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CheckTable(Type tp) {
            TableAttribute[] tables = (TableAttribute[])tp.GetCustomAttributes(typeof(TableAttribute), false);
            if (tables.Length <= 0) throw new Exception("该对象没有包含表格定义");
            var table = tables[tables.Length - 1];
            string tabName = table.Name;
            if (tabName.IsEmpty()) tabName = tp.Name;
            return CheckTable(tabName);
        }

        /// <summary>
        /// 检测数据表是否存在
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public bool CheckTable(string tabName) {
            bool res = false;

            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    var row = GetRow($"SELECT * FROM [sysObjects] Where Name = '{tabName}' AND [Type] In ('S', 'U')");
                    res = !row.IsEmpty;
                    break;
                case DatabaseTypes.MySQL:
                    row = GetRow($"show tables like '{tabName}';");
                    res = !row.IsEmpty;
                    break;
                //case DatabaseTypes.Microsoft_Office_Access:
                //case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    row = GetRow($"select * from [sqlite_master] where type='table' and name ='{tabName}';");
                    res = !row.IsEmpty;
                    break;
                case DatabaseTypes.PostgreSQL:
                    row = GetRow($"select tablename from pg_tables where schemaname = 'public' and tablename = '{tabName}';");
                    res = !row.IsEmpty;
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }

            return res;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="row"></param>
        public void CreateTable(Orm.Row row) {
            CreateTable(row.GetType());
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public void CreateTable(Type tp) {
            string sql = "";
            string szFields = "";
            // 获取表相关的设定
            TableAttribute[] tables = (TableAttribute[])tp.GetCustomAttributes(typeof(TableAttribute), false);
            if (tables.Length <= 0) throw new Exception("该对象没有包含表格定义");
            var table = tables[tables.Length - 1];
            string tabName = table.Name;
            if (tabName.IsEmpty()) tabName = tp.Name;
            // 获取所有字段设定
            PropertyInfo[] pros = tp.GetProperties();
            for (int i = 0; i < pros.Length; i++) {
                // 获取字段相关设定
                var pro = pros[i];
                var fields = (FieldAttribute[])pro.GetCustomAttributes(typeof(FieldAttribute), false);
                // 拼接Sql
                if (fields.Length > 0) {
                    var field = fields[fields.Length - 1];
                    if (field.IsRealField) {
                        if (field.Name.IsEmpty()) field.Name = pro.Name;
                        szFields += $",{field.ToSqlString(this.DatabaseType)}";
                    }
                }
            }
            // 生成完整Sql
            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    sql = $"CREATE TABLE [{tabName}](";
                    sql += "[ID] numeric(18,0) identity(1,1) primary key";
                    sql += szFields;
                    sql += ")";
                    break;
                case DatabaseTypes.MySQL:
                    sql = $"CREATE TABLE `{tabName}`(";
                    sql += "`ID` int not null primary key Auto_increment";
                    sql += szFields;
                    sql += ")";
                    break;
                case DatabaseTypes.PostgreSQL:
                    sql = $"CREATE TABLE \"{tabName}\"(";
                    sql += "\"ID\" serial PRIMARY KEY";
                    sql += szFields;
                    sql += ")";
                    break;
                //case DatabaseTypes.Microsoft_Office_Access:
                //case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    sql = $"CREATE TABLE [{tabName}](";
                    sql += "[ID] INTEGER PRIMARY KEY AUTOINCREMENT";
                    sql += szFields;
                    sql += ")";
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
            // 执行Sql
            Exec(sql);
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public void CreateTable(string tabName, SqlUnits.FieldDefine[] fields = null) {
            string sql = "";
            string szFields = "";
            if (fields != null) {
                foreach (var fid in fields) {
                    szFields += $",{fid.ToSqlString(this.DatabaseType)}";
                }
            }
            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    sql = $"CREATE TABLE [{tabName}](";
                    sql += "[ID] numeric(18,0) identity(1,1) primary key";
                    sql += szFields;
                    sql += ")";
                    Exec(sql);
                    break;
                case DatabaseTypes.MySQL:
                    sql = $"CREATE TABLE `{tabName}`(";
                    sql += "`ID` int not null primary key Auto_increment";
                    sql += szFields;
                    sql += ")";
                    Exec(sql);
                    break;
                case DatabaseTypes.PostgreSQL:
                    sql = $"CREATE TABLE \"{tabName}\"(";
                    sql += "\"ID\" serial PRIMARY KEY";
                    sql += szFields;
                    sql += ")";
                    Exec(sql);
                    break;
                //case DatabaseTypes.Microsoft_Office_Access:
                //case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    sql = $"CREATE TABLE [{tabName}](";
                    sql += "[ID] INTEGER PRIMARY KEY AUTOINCREMENT";
                    sql += szFields;
                    sql += ")";
                    Exec(sql);
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        /// <summary>
        /// 重命名数据库
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public void RenameTable(string tabName, string newName) {
            switch (db.Type) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                case DatabaseTypes.Microsoft_SQL_Server:
                    Exec($"ALTER TABLE [{tabName}] RENAME [{newName}];");
                    break;
                case DatabaseTypes.MySQL:
                    Exec($"ALTER TABLE `{tabName}` RENAME `{newName}`;");
                    break;
                case DatabaseTypes.PostgreSQL:
                    Exec($"ALTER TABLE `{tabName}` RENAME TO `{newName}`;");
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public void DropTable(string tabName) {
            switch (db.Type) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                case DatabaseTypes.Microsoft_SQL_Server:
                    Exec($"DROP TABLE [{tabName}];");
                    break;
                case DatabaseTypes.MySQL:
                    Exec($"DROP TABLE `{tabName}`;");
                    break;
                case DatabaseTypes.PostgreSQL:
                    Exec($"DROP TABLE \"{tabName}\";");
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        #endregion

        #region [=====字段操作=====]

        /// <summary>
        /// 检测数据表是否存在
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fidName"></param>
        /// <returns></returns>
        public bool CheckTableField(string tabName, string fidName) {
            bool res = false;

            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    var row = GetRow($"select [syscolumns].* from [sysobjects],[syscolumns] where [sysobjects].id=[syscolumns].id and  [syscolumns].name='{fidName}' and [sysobjects].type In ('S','U') and [sysobjects].name='{tabName}'");
                    res = !row.IsEmpty;
                    break;
                case DatabaseTypes.MySQL:
                    row = GetRow($"SELECT * FROM information_schema.columns WHERE table_schema = DATABASE()  AND table_name = '{tabName}' AND column_name = '{fidName}';");
                    res = !row.IsEmpty;
                    break;
                case DatabaseTypes.PostgreSQL:
                    row = GetRow($"select column_name from information_schema.columns WHERE table_schema = 'public' and table_name = '{tabName}' and column_name = '{fidName}';");
                    res = !row.IsEmpty;
                    break;
                //case DatabaseTypes.Microsoft_Office_Access:
                //case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    row = GetRow($"select * from [sqlite_master] where type='table' and name ='{tabName}' and (sql like '%[{fidName}]%' or sql like '%,{fidName},%'or sql like '%({fidName},%'or sql like '%,{fidName})%' or sql like '% {fidName} %');");
                    res = !row.IsEmpty;
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }

            return res;
        }

        /// <summary>
        /// 更新数据表
        /// </summary>
        /// <param name="row"></param>
        public void UpdateTableFields(Orm.Row row) {
            UpdateTableFields(row.GetType());
        }

        /// <summary>
        /// 更新数据表
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public void UpdateTableFields(Type tp) {
            // 获取表相关的设定
            TableAttribute[] tables = (TableAttribute[])tp.GetCustomAttributes(typeof(TableAttribute), false);
            if (tables.Length <= 0) throw new Exception("该对象没有包含表格定义");
            var table = tables[tables.Length - 1];
            string tabName = table.Name;
            if (tabName.IsEmpty()) tabName = tp.Name;
            // 获取所有字段设定
            PropertyInfo[] pros = tp.GetProperties();
            for (int i = 0; i < pros.Length; i++) {
                // 获取字段相关设定
                var pro = pros[i];
                var fields = (FieldAttribute[])pro.GetCustomAttributes(typeof(FieldAttribute), false);
                // 拼接Sql
                if (fields.Length > 0) {
                    var field = fields[fields.Length - 1];
                    if (field.IsRealField) {
                        if (field.Name.IsEmpty()) field.Name = pro.Name;
                        // 判断字段是否存在
                        if (!CheckTableField(tabName, field.Name)) {
                            AddTableField(tabName, field);
                            SetTableFieldDefault(tabName, field);
                        } else {
                            try { UpdateTableField(tabName, field.Name, field); } catch { }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public void AddTableField(string tabName, FieldAttribute fid) {
            string sql = "";
            switch (db.Type) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    sql = $"ALTER TABLE [{tabName}] ADD {fid.ToSqlString(this.DatabaseType)}";
                    Exec(sql);
                    break;
                case DatabaseTypes.MySQL:
                    sql = $"ALTER TABLE `{tabName}` ADD {fid.ToSqlString(this.DatabaseType)}";
                    Exec(sql);
                    break;
                case DatabaseTypes.PostgreSQL:
                    sql = $"ALTER TABLE \"{tabName}\" ADD COLUMN {fid.ToSqlString(this.DatabaseType)}";
                    Exec(sql);
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public void SetTableFieldDefault(string tabName, FieldAttribute fid) {
            string sql = "";
            // 获取默认字符串
            string defaultValue = fid.DefaultValue.ToString();
            switch (db.Type) {
                case DatabaseTypes.MySQL:
                    defaultValue = defaultValue.Replace("'", "\\'");
                    break;
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                case DatabaseTypes.PostgreSQL:
                    defaultValue = defaultValue.Replace("'", "''");
                    break;
                default:
                    throw new Exception($"尚未支持数据库 {db.Type.ToString()} 中的字符串转义。");
            }
            // 更新数据
            switch (db.Type) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    sql = $"Update [{tabName}] set [{fid.Name}]='{defaultValue}' where [{fid.Name}] is null";
                    Exec(sql);
                    break;
                case DatabaseTypes.MySQL:
                    sql = $"Update `{tabName}` set `{fid.Name}`='{defaultValue}' where `{fid.Name}` is null";
                    Exec(sql);
                    break;
                case DatabaseTypes.PostgreSQL:
                    sql = $"Update \"{tabName}\" set \"{fid.Name}\"='{defaultValue}' where \"{fid.Name}\" is null";
                    sql = $"ALTER TABLE \"{tabName}\" ADD COLUMN {fid.ToSqlString(this.DatabaseType)}";
                    Exec(sql);
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }


        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public void AddTableField(string tabName, SqlUnits.FieldDefine fid) {
            string sql = "";
            switch (db.Type) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    sql = $"ALTER TABLE [{tabName}] ADD {fid.ToSqlString(this.DatabaseType)}";
                    Exec(sql);
                    break;
                case DatabaseTypes.MySQL:
                    sql = $"ALTER TABLE `{tabName}` ADD {fid.ToSqlString(this.DatabaseType)}";
                    Exec(sql);
                    break;
                case DatabaseTypes.PostgreSQL:
                    sql = $"ALTER TABLE \"{tabName}\" ADD COLUMN {fid.ToSqlString(this.DatabaseType)}";
                    Exec(sql);
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        /// <summary>
        /// 修改字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fidName"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public void UpdateTableField(string tabName, string fidName, FieldAttribute fid) {
            //string sql = "";
            string stp = fid.GetTypeString(this.DatabaseType);
            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    Exec($"ALTER TABLE [{tabName}] ALTER COLUMN [{fidName}] {stp}");
                    //重命名
                    if (fid.Name != "" && fid.Name != fidName) {
                        Exec($"ALTER TABLE [{tabName}] RENAME COLUMN [{fidName}] TO [{fid.Name}]");
                    }
                    break;
                case DatabaseTypes.MySQL:
                    //带重命名
                    if (fid.Name != "" && fid.Name != fidName) {
                        Exec($"ALTER TABLE `{tabName}` CHANGE `{fidName}` `{fid.Name}` {stp}");
                    } else {
                        Exec($"ALTER TABLE `{tabName}` MODIFY `{fidName}` {stp}");
                    }
                    break;
                case DatabaseTypes.PostgreSQL:
                    //带重命名
                    if (fid.Name != "" && fid.Name != fidName) {
                        Exec($"ALTER TABLE \"{tabName}\" RENAME \"{fidName}\" TO \"{fid.Name}\"");
                    } else {
                        Exec($"ALTER TABLE \"{tabName}\" ALTER COLUMN \"{fidName}\" TYPE {stp}");
                    }
                    break;
                //case DatabaseTypes.Microsoft_Office_Access:
                //case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    throw new Exception($"Sqlite数据库不支持修改字段");
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        /// <summary>
        /// 修改字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fidName"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public void UpdateTableField(string tabName, string fidName, SqlUnits.FieldDefine fid) {
            //string sql = "";
            string stp = fid.GetTypeString(this.DatabaseType);
            switch (db.Type) {
                case DatabaseTypes.Microsoft_SQL_Server:
                    Exec($"ALTER TABLE [{tabName}] ALTER COLUMN [{fidName}] {stp}");
                    //重命名
                    if (fid.Name != "" && fid.Name != fidName) {
                        Exec($"ALTER TABLE [{tabName}] RENAME COLUMN [{fidName}] TO [{fid.Name}]");
                    }
                    break;
                case DatabaseTypes.MySQL:
                    //带重命名
                    if (fid.Name != "" && fid.Name != fidName) {
                        Exec($"ALTER TABLE `{tabName}` CHANGE `{fidName}` `{fid.Name}` {stp}");
                    } else {
                        Exec($"ALTER TABLE `{tabName}` MODIFY `{fidName}` {stp}");
                    }
                    break;
                case DatabaseTypes.PostgreSQL:
                    //带重命名
                    if (fid.Name != "" && fid.Name != fidName) {
                        Exec($"ALTER TABLE \"{tabName}\" RENAME \"{fidName}\" TO \"{fid.Name}\"");
                    } else {
                        Exec($"ALTER TABLE \"{tabName}\" ALTER COLUMN \"{fidName}\" TYPE {stp}");
                    }
                    break;
                //case DatabaseTypes.Microsoft_Office_Access:
                //case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    throw new Exception($"Sqlite数据库不支持修改字段");
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="fidName"></param>
        /// <returns></returns>
        public void DropTableField(string tabName, string fidName) {
            switch (db.Type) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                case DatabaseTypes.Microsoft_SQL_Server:
                    Exec($"ALTER TABLE [{tabName}] DROP [{fidName}];");
                    break;
                case DatabaseTypes.MySQL:
                    Exec($"ALTER TABLE `{tabName}` DROP `{fidName}`;");
                    break;
                case DatabaseTypes.PostgreSQL:
                    Exec($"ALTER TABLE \"{tabName}\" DROP COLUMN \"{fidName}\";");
                    break;
                default: throw new Exception($"连接管理器尚未支持\"{db.Type.ToString()}\"数据库");
            }
        }

        #endregion

        #region [=====函数操作=====]

        /// <summary>
        /// 创建一个Count函数对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SqlUnits.Functions.Count Count(ISqlStringable obj) {
            return new SqlUnits.Functions.Count(obj);
        }

        /// <summary>
        /// 创建一个Sum函数对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SqlUnits.Functions.Sum Sum(ISqlStringable obj) {
            return new SqlUnits.Functions.Sum(obj);
        }

        /// <summary>
        /// 创建一个Avg函数对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SqlUnits.Functions.Avg Avg(ISqlStringable obj) {
            return new SqlUnits.Functions.Avg(obj);
        }

        /// <summary>
        /// 创建一个Max函数对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SqlUnits.Functions.Max Max(ISqlStringable obj) {
            return new SqlUnits.Functions.Max(obj);
        }

        /// <summary>
        /// 创建一个Min函数对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SqlUnits.Functions.Min Min(ISqlStringable obj) {
            return new SqlUnits.Functions.Min(obj);
        }

        /// <summary>
        /// 创建一个随机排序对象
        /// </summary>
        /// <returns></returns>
        public SqlUnits.Rand Rand() {
            return new SqlUnits.Rand();
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();

            dbc.Close();
            dbc = null;
        }
    }
}
