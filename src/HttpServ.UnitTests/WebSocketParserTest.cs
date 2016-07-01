using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HttpServ;
using HttpServ.WebSocket;

namespace UnitTests
{
    [TestClass]
    public class WebSocketParserTest
    {
        [TestMethod]
        public void ParsePayload7()
        {
            var data = new byte[] {
                0x81, 0x82, 0x00, 0x00, 0x00, 0x00
            };

            var header = WebSocketParser.Parse(
                new ArraySegment<byte>(data));

            Assert.AreEqual(header.fin, true);
            Assert.AreEqual(header.opcode, (int)OpCode.Text);
            Assert.AreEqual(header.mask, true);
            Assert.AreEqual(header.payloadLength, 2);
        }

        [TestMethod]
        public void ParsePayload16()
        {
            var data = new byte[] {
                0x81, 0xFE, 0xC8, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            var header = WebSocketParser.Parse(
                new ArraySegment<byte>(data));

            Assert.AreEqual(header.fin, true);
            Assert.AreEqual(header.opcode, (int)OpCode.Text);
            Assert.AreEqual(header.mask, true);
            Assert.AreEqual(header.payloadLength, 200);
        }
    }
}
