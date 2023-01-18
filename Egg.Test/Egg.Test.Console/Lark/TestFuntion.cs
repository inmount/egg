using Egg.Lark;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console.Lark
{
    public class TestFuntion
    {
        private void Print(string content)
        {
            System.Console.WriteLine(content);
        }

        [Func("print")]
        public void Println(string content)
        {
            System.Console.WriteLine(content);
        }

        [Func]
        public bool Large(double value1, double value2)
        {
            return value1 > value2;
        }

        [Func]
        public bool LargeEqual(double value1, double value2)
        {
            return value1 >= value2;
        }

        [Func]
        public bool Small(double value1, double value2)
        {
            return value1 < value2;
        }

        [Func]
        public bool SmallEqual(double value1, double value2)
        {
            return value1 <= value2;
        }

        [Func]
        public bool IsNull(object? val)
        {
            return val is null;
        }

        [Func]
        public LarkList CreateList()
        {
            return new LarkList();
        }

        [Func]
        public LarkObject CreateObject()
        {
            return new LarkObject();
        }

        [Func]
        public int GetTick()
        {
            return Environment.TickCount;
        }

        [Func]
        public double Repeat100()
        {
            double sum = 0;
            for (double i = 0.01; i <= 10000.0; i += 0.01) sum += i;
            return sum;
        }

        [Func]
        public double Repeat1000()
        {
            double sum = 0;
            for (double i = 0.01; i <= 100000.0; i += 0.01) sum += i;
            return sum;
        }
    }
}
