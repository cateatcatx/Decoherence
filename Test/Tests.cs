using System;
using System.Linq;
using Decoherence;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestPriorityList1()
        {
            PriorityList<int> list = new PriorityList<int>((a, b) => a - b);

            Random random = new Random();

            for (var i = 0; i < 1000; ++i)
            {
                list.Add(random.Next(10));
            }

            for (int i = 1; i < 1000; ++i)
            {
                Assert.True(list[i] <= list[i-1]);
            }
        }
        
        [Test]
        public void TestPriorityList2()
        {
            PriorityList<int> list = new PriorityList<int>((a, b) => b - a);

            Random random = new Random();

            for (var i = 0; i < 1000; ++i)
            {
                list.Add(random.Next(10));
            }

            for (int i = 1; i < 1000; ++i)
            {
                Assert.True(list[i] >= list[i-1]);
            }
        }
        
        [Test]
        public void TestPriorityList3()
        {
            PriorityList<Tuple<int>> list = new PriorityList<Tuple<int>>((a, b) => a.Item1 - b.Item1);

            list.Add(new Tuple<int>(1));
            list.Add(new Tuple<int>(1));
            list.Add(new Tuple<int>(1));

            var item = new Tuple<int>(1);
            list.Add(item);
            
            Assert.True(ReferenceEquals(item, list[^1]));
        }
        
        [Test]
        public void TestPriorityList4()
        {
            PriorityList<Tuple<int>> list = new PriorityList<Tuple<int>>((a, b) => b.Item1 - a.Item1);

            list.Add(new Tuple<int>(1));
            list.Add(new Tuple<int>(1));
            list.Add(new Tuple<int>(1));

            var item = new Tuple<int>(1);
            list.Add(item);
            
            Assert.True(ReferenceEquals(item, list[^1]));

        }
    }
}