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
            // 定义数据
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("GB2312");
            // 执行方法
            string res = egg.Terminal.Execute("ping", "www.baidu.com", encoding);
            _output.WriteLine(res);
            // 返回结果
        }

        [Fact]
        public void ToString_Compare()
        {
            int t1 = 0;
            int t2 = 0;
            double sum = 0;
            string sz = string.Empty;
            StringBuilder sb = new StringBuilder();
            // 第一组测试
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = "sum:" + sum.ToString();
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"第一组：{sz} / {t2 - t1} 毫秒");
            // 第二组测试
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = "sum:" + sum;
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"第二组：{sz} / {t2 - t1} 毫秒");
            // 第三组测试
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = $"sum:{sum}";
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"第三组：{sz} / {t2 - t1} 毫秒");
            // 第四组测试
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = "sum:" + Convert.ToString(sum);
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"第四组：{sz} / {t2 - t1} 毫秒");
            // 第五组测试
            t1 = Environment.TickCount;
            sum = 0;
            for (double i = 0.00; i < 1000000; i += 0.01)
            {
                sum += i;
                sz = $"sum:{Convert.ToString(sum)}";
            }
            t2 = Environment.TickCount;
            _output.WriteLine($"第五组：{sz} / {t2 - t1} 毫秒");
            // 第六组测试
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
            _output.WriteLine($"第六组：{sz} / {t2 - t1} 毫秒");
            // 第七组测试
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
            _output.WriteLine($"第七组：{sz} / {t2 - t1} 毫秒");
        }
    }
}