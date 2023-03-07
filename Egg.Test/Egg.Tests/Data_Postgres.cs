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
            // ��������
            string connectionString = "server=10.21.38.151;port=1921;database=test_imes_modules;username=mesadmin;password=Mesadmin#2O19;";
            // ִ�з���
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.PostgreSQL, connectionString))
            {
                conn.Open();
                //conn.TableCreated<People>().Wait();
            }
            // ���ؽ��
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Insert()
        {
            // ��������
            string connectionString = "data source=D:\\Temp\\test.db";
            // ִ�з���
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                People people = new People()
                {
                    Age = 10,
                    Name = "����",
                };
                IRepository<People, long> repository = new Repository<People, long>(conn);
                repository.Insert(people);
            }
            // ���ؽ��
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Delete()
        {
            // ��������
            string connectionString = "data source=D:\\Temp\\test.db";
            // ִ�з���
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                IRepository<People, long> repository = new Repository<People, long>(conn);
                repository.Delete(d => d.Age < 20);
            }
            // ���ؽ��
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Get()
        {
            // ��������
            string connectionString = "data source=D:\\Temp\\test.db";
            // ִ�з���
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
                            Name = "����",
                        };
                        repository.Insert(people);
                    }
                }
            }
            // ���ؽ��
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_Get_List()
        {
            // ��������
            string connectionString = "data source=D:\\Temp\\test.db";
            // ִ�з���
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                using (IRepository<People, long> repository = new Repository<People, long>(conn))
                {
                    var peoples = repository.GetRows(d => d.Age > 10);
                    _output.WriteLine(peoples.Count.ToString());
                }
            }
            // ���ؽ��
            _output.WriteLine("OK");
        }

        [Fact]
        public void Sqlite_UnitOfWork()
        {
            // ��������
            string connectionString = "data source=D:\\Temp\\test.db";
            // ִ�з���
            using (DatabaseConnection conn = new DatabaseConnection(DatabaseTypes.Sqlite, connectionString))
            {
                conn.Open();
                using (var uow = conn.BeginUnitOfWork())
                {
                    using (IRepository<People, long> repository = new Repository<People, long>(conn))
                    {
                        People? people5 = repository.GetRow(d => d.Name == "����");
                        if (people5 is null)
                        {
                            people5 = new People()
                            {
                                Age = 22,
                                Name = "����",
                            };
                            repository.Insert(people5);
                        }
                        People? people6 = repository.GetRow(d => d.Name == "����");
                        if (people6 is null)
                        {
                            people6 = new People()
                            {
                                Age = 28,
                                Name = "����",
                            };
                            repository.Insert(people6);
                        }
                    }
                    uow.Complete();
                }
            }
            // ���ؽ��
            _output.WriteLine("OK");
        }
    }
}