using System;
using System.Text;
using Xunit;

namespace NCRC.Tests
{
    public class Crc32Tests
    {
        [Fact]
        public void ComputeTest()
        {
            var me = Encoding.ASCII.GetBytes("moonpyk");
            Assert.Equal((uint)0x3E583D47, Crc32.Compute(me));

            var awaited = BitConverter.GetBytes(0x3E583D47);
            Array.Reverse(awaited);

            using (var fcrc = new FastCrc32())
            {
                fcrc.ComputeHash(me);

                Assert.Equal(awaited, fcrc.Hash);
            }

        }
    }
}
