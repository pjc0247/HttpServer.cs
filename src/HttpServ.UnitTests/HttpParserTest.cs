using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HttpServ;
using HttpServ.Http;

namespace UnitTests
{
    [TestClass]
    public class HttpParserTest
    {
        [TestMethod]
        public void ParseEnd()
        {
            var parser = new HttpParser();
            var input = 
@"GET / HTTP/1.1
Content-Length: 4

HiHi";
            var data = Encoding.UTF8.GetBytes(input);

            Assert.AreEqual(
                parser.Write(data),
                HttpParseResult.End);
        }

        [TestMethod]
        public void ParseEndWithTrailing()
        {
            var parser = new HttpParser();
            var input =
@"GET / HTTP/1.1
Content-Length: 4

HiHiSOME_TRAILING_DATA";
            var data = Encoding.UTF8.GetBytes(input);

            Assert.AreEqual(
                parser.Write(data),
                HttpParseResult.EndWithTrailing);
        }

        [TestMethod]
        public void ParseProcessing()
        {
            var parser = new HttpParser();
            var input =
@"GET / HTTP/1.1
Content-Length: 4

Hi";
            var data = Encoding.UTF8.GetBytes(input);

            Assert.AreEqual(
                parser.Write(data),
                HttpParseResult.Processing);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpServ.Http.Exceptions.HttpParseException))]
        public void InvalidRequest()
        {
            var parser = new HttpParser();
            var input =
@"1";
            var data = Encoding.UTF8.GetBytes(input);

            parser.Write(data);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpServ.Http.Exceptions.HttpParseException))]
        public void MethodTooLong()
        {
            var parser = new HttpParser();
            var input =
@"VERY_LONG_HTTP_METHOD_NAME_VV / HTTP/1.1";
            var data = Encoding.UTF8.GetBytes(input);

            parser.Write(data);
        }
    }
}
