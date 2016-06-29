using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace HttpServ.Http
{
    // *****************************
    //          NOT USED 
    // *****************************
    public static class ResponseEmitter
    {
        public static async Task Emit(
            this HttpResponse response,
            Stream stream)
        {
            await WriteStringAsync("HTTP/1.1 ", stream);
            await WriteStringAsync(response.code.ToString() + " ", stream);
            await WriteStringAsync(response.reasonPhrase + "\r\n", stream);

            foreach (var header in response.headers)
            {
                await WriteStringAsync(header.Key, stream);
                await WriteStringAsync(": ", stream);
                await WriteStringAsync(header.Value, stream);
                await WriteStringAsync("\r\n", stream);
            }

            await WriteStringAsync("\r\n", stream);
            if (response.content != null)
                await stream.WriteAsync(response.content, 0, response.content.Length);

            Console.WriteLine("WROTE");
        }

        private static Task WriteStringAsync(string msg, Stream stream)
        {
            Console.Write(msg);
            var buffer = Encoding.UTF8.GetBytes(msg);
            return stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
