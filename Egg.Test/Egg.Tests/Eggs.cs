using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Egg.Tests
{
    public class Eggs
    {
        private readonly ITestOutputHelper _output;

        public Eggs(ITestOutputHelper testOutput)
        {
            _output = testOutput;
        }

        [Fact]
        public void Terminal_Execute()
        {
            // ��������
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("GB2312");
            // ִ�з���
            string res = egg.Terminal.Execute("ping", "www.baidu.com", encoding);
            _output.WriteLine(res);
            // ���ؽ��
        }

        [Fact]
        public void ToString_Compare()
        {
            int t1 = 0;
            int t2 = 0;
            double sum = 0;
            string sz = string.Empty;
            StringBuilder sb = new StringBuilder();
            // ��һ�����
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = "sum:" + sum.ToString();
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"��һ�飺{sz} / {t2 - t1} ����");
            // �ڶ������
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = "sum:" + sum;
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"�ڶ��飺{sz} / {t2 - t1} ����");
            // ���������
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = $"sum:{sum}";
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"�����飺{sz} / {t2 - t1} ����");
            // ���������
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = "sum:" + Convert.ToString(sum);
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"�����飺{sz} / {t2 - t1} ����");
            // ���������
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = $"sum:{Convert.ToString(sum)}";
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"�����飺{sz} / {t2 - t1} ����");
            // ���������
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sb.Clear();
                sb.Append("sum:");
                sb.Append(Convert.ToString(sum));
                sz = sb.ToString();
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"�����飺{sz} / {t2 - t1} ����");
            // ���������
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sb.Clear();
                sb.Append("sum:");
                sb.Append(sum);
                sz = sb.ToString();
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"�����飺{sz} / {t2 - t1} ����");
        }
    }
}