# egg.db

egg开发套件数据库应用扩展(.Net 5)

## 致力于多数据库操作可移植性的类库

本类库目前支持SqlServer、MySql、Sqlite三个类型的数据库，使用本套件在这些数据库间切换只需重新指定数据类型即可。

## 数据库申明

Sql Server:

    var db = new egg.db.Databases.MicrosoftSqlServer() {
        Address = "127.0.0.1",
        Port = 1433,
        Name = "mater",
        User = "sa",
        Password = "123456"
    };

MySql:

    var db = new egg.db.Databases.MySql() {
        Address = "127.0.0.1",
        Port = 3306,
        Name = "mysql",
        User = "root",
        Password = "123456"
    };

Sqlite:

    var db = new egg.db.Databases.Sqlite() {
        Path="/my/temp.db"
    };

## 使用更接近原始SQL数据库操作语句，降低学习成本

建立数据查询，并判断后获取结果：

    // 建立数据库连接
    using (egg.db.Connection dbc = new egg.db.Connection(db)) {

        // 创建一个ORM对象
        using (SqlServerSystemTables orm = new SqlServerSystemTables()) {

            // 定义Select语句并读取一行数据
            var row = dbc.Select(orm.SysObjects, orm.SysColumns)
                .Columns(orm.SysColumns.Name)
                .Where((orm.SysObjects.Id == orm.SysColumns.Id) & (orm.SysObjects.Name == "SyatemObjects") & (orm.SysColumns.Name == "Name") & orm.SysObjects.Type.In("S", "U"))
                .GetRow();

            // 判断是否读取到数据
            if (!row.IsEmpty) {

                // 字段定义方式读取数据
                Console.WriteLine($"Name = {row[orm.SysColumns.Name]}");

                // 字符串定义方式读取数据
                Console.WriteLine($"Name = {row["Name"]}");

                // 动态方式读取数据
                dynamic dyc = row;
                Console.WriteLine($"Name = {dyc.Name}");
            }
        }
    }

插入数据：

    // 建立数据库连接
    using (egg.db.Connection dbc = new egg.db.Connection(db)) {

        // 创建一个ORM对象
        using (SqlServerSystemTables orm = new SqlServerSystemTables()) {

            // 新建一个行数据对象，定义需要插入的数据
            Row row = new Row();
            orm.SysColumns.Rower(row)
                .SetName("123");

            // 创建一个Insert语句并执行
            dbc.Insert(orm.SysColumns, row).Exec();
        }
    }

以Id为主键更新数据：

    // 建立数据库连接
    using (egg.db.Connection dbc = new egg.db.Connection(db)) {

        // 创建一个ORM对象
        using (SqlServerSystemTables orm = new SqlServerSystemTables()) {

            // 新建一个行数据对象，定义需要插入的数据
            Row row = new Row();
            orm.SysColumns.Rower(row)
                .SetId(1)
                .SetName("123");

            // 创建一个Update语句并执行
            dbc.Update(orm.SysColumns, row, orm.SysColumns.Id).Exec();
        }
    }

自定义条件更新数据：

    // 建立数据库连接
    using (egg.db.Connection dbc = new egg.db.Connection(db)) {

        // 创建一个ORM对象
        using (SqlServerSystemTables orm = new SqlServerSystemTables()) {

            // 新建一个行数据对象，定义需要插入的数据
            Row row = new Row();
            orm.SysColumns.Rower(row)
                .SetName("123");

            // 创建一个Update语句并执行
            dbc.Update(orm.SysColumns, row)
                .Where(orm.SysColumns.Name == "456")
                .Exec();
        }
    }

删除数据：

    // 建立数据库连接
    using (egg.db.Connection dbc = new egg.db.Connection(db)) {

        // 创建一个ORM对象
        using (SqlServerSystemTables orm = new SqlServerSystemTables()) {

            // 创建一个Insert语句并执行
            dbc.Delete(orm.SysColumns)
                .Where(orm.SysColumns.Name == "456")
                .Exec();
        }
    }

## 预映射和快速映射

dpz3支持两种ORM结构映射方式：预映射和快速映射

### 预映射

使用C#针对数据库进行提前ORM类编写

优点：集成化高，表结构清晰，对IDE智能提示友好，使用直观，支持Rower数据操作，可扩展性强，可维护性强

缺点：代码冗余，需要花多余时间编写表结构，开发周期拉长

预映射删除数据示例：

    // 建立数据库连接
    using (egg.db.Connection dbc = new egg.db.Connection(db)) {

        // 创建一个ORM对象
        using (SqlServerSystemTables orm = new SqlServerSystemTables()) {

            // 创建一个Delete语句并执行
            dbc.Delete(orm.SysColumns)
                .Where(orm.SysColumns.Name == "456")
                .Exec();

        }
    }

### 快速映射

使用OrmMapper类动态进行ORM操作对象生成

优点：开发迅捷，上手简单

缺点：功能单一，使用者必须提前了解表结构

快速映射删除数据示例：

    // 建立数据库连接
    using (egg.db.Connection dbc = new egg.db.Connection(db)) {

		// 快速映射表
        var SysColumns = egg.db.OrmMapper.Table("SysColumns");
        // 创建一个Delete语句并执行
        dbc.Delete(SysColumns)
            .Where(SysColumns["Name"] == "456")
            .Exec();

    }