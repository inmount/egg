using Egg.Data;
using Egg.Tests.Entity;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Egg.Tests
{
    public class Data_Postgres
    {
        private readonly ITestOutputHelper _output;

        public Data_Postgres(ITestOutputHelper testOutput)
        {
            _output = testOutput;
        }

        [Fact]
        public void Postgres_Create()
        {
            // 定义数据
            string connectionString = "server=10.21.38.151;port=1921;database=test_imes_modules;username=mesadmin;password=Mesadmin#2O19;";
            // 执行方法
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.PostgreSQL, connectionString))
            {
                conn.Open();
                //conn.TableCreated<People>().Wait();
            }
            // 返回结果
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Insert()
        {
            // 定义数据
            string connectionString = "data source=D:\\Temp\\test.db";
            // 执行方法
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                People people = new People()
                {
                    Age = 10,
                    Name = "张三",
                };
                IRepository<People, long> repository = new Repository<People, long>(conn);
                repository.Insert(people);
            }
            // 返回结果
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Delete()
        {
            // 定义数据
            string connectionString = "data source=D:\\Temp\\test.db";
            // 执行方法
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                IRepository<People, long> repository = new Repository<People, long>(conn);
                repository.Delete(d => d.Age < 20);
            }
            // 返回结果
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Get()
        {
            // 定义数据
            string connectionString = "data source=D:\\Temp\\test.db";
            // 执行方法
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                using (IRepository<People, long> repository = new Repository<People, long>(conn))
                {
                    People? people = repository.Get(100);
                    if (people is null)
                    {
                        people = new People()
                        {
                            Id = 100,
                            Age = 18,
                            Name = "李四",
                        };
                        repository.Insert(people);
                    }
                }
            }
            // 返回结果
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Get_List()
        {
            // 定义数据
            string connectionString = "data source=D:\\Temp\\test.db";
            // 执行方法
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                using (IRepository<People, long> repository = new Repository<People, long>(conn))
                {
                    var peoples = repository.GetRows(d => d.Age > 10);
                    _output.WriteLine(peoples.Count.ToString());
                }
            }
            // 返回结果
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_UnitOfWork()
        {
            // 定义数据
            string connectionString = "data source=D:\\Temp\\test.db";
            // 执行方法
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                using (var uow = conn.BeginUnitOfWork())
                {
                    using (IRepository<People, long> repository = new Repository<People, long>(conn))
                    {
                        People? people5 = repository.GetRow(d => d.Name == "王五");
                        if (people5 is null)
                        {
                            people5 = new People()
                            {
                                Age = 22,
                                Name = "王五",
                            };
                            repository.Insert(people5);
                        }
                        People? people6 = repository.GetRow(d => d.Name == "马六");
                        if (people6 is null)
                        {
                            people6 = new People()
                            {
                                Age = 28,
                                Name = "马六",
                            };
                            repository.Insert(people6);
                        }
                    }
                    uow.Complete();
                }
            }
            // 返回结果
            _output.WriteLine("OK");
        }
    }
}