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
    }
}