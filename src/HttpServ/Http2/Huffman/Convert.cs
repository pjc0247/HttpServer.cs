using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2.Huffman
{
    public static class Convert
    {
        private static HuffmanDecoder decoder { get; set; }

        static Convert()
        {
            decoder = new HuffmanDecoder(HuffStaticTable.HUFFMAN_CODES, HuffStaticTable.HUFFMAN_CODE_LENGTHS);
        }

        public static byte[] Decode(byte[] input)
        {
            return decoder.Decode(input);
        }
    }
}
