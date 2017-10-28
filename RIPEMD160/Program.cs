using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

class Program
{
    public class TestItem {
        public string Data { get; set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
    };

    [STAThreadAttribute]
    public static void Main(String[] args)
    {
        var items = new List<TestItem> {
            new TestItem { Data = "", Description = "\"\" (empty string)", ExpectedResult = "9c1185a5c5e9fc54612808977ee8f548b2258d31"},
            new TestItem { Data = "a", Description = "\"a\"", ExpectedResult = "0bdc9d2d256b3ee9daae347be6f4dc835a467ffe"},
            new TestItem { Data = "abc", Description = "\"abc\"", ExpectedResult = "8eb208f7e05d987a9b044a8e98c6b087f15a0bfc" },
            new TestItem { Data = "message digest", Description = "\"message digest\"", ExpectedResult = "5d0689ef49d2fae572b881b123a85ffa21595f36" },
            new TestItem { Data = "abcdefghijklmnopqrstuvwxyz", Description = "\"abcdefghijklmnopqrstuvwxyz\"", ExpectedResult = "f71c27109c692c1b56bbdceb5b9d2865b3708dbc" },
            new TestItem { Data = "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq", Description = "\"abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq\"", ExpectedResult = "12a053384a9c0c88e405a06c27dcf49ada62eb2b"},
            new TestItem { Data = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", Description = "\"A...Za...z0...9\"", ExpectedResult = "b0e20b6e3116640286ed3a87a5713079b21f5189" },
            new TestItem { Data = "12345678901234567890123456789012345678901234567890123456789012345678901234567890", Description = "8 times \"1234567890\"", ExpectedResult = "9b752e45573d4b39f4dbd3323cab82bf63326bfb" },
            new TestItem { Data = "The quick brown fox jumped over the lazy dog.", Description = "A lazy dog", ExpectedResult = "ec457d0a974c48d5685a7efa03d137dc8bbde7e3" },
            new TestItem { Data = "The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.", Description = "lots of lazy dogs", ExpectedResult = "c6c679e161a8b51a83329c74dc6f36680fccca1a" }
        };

        // Initialize a RIPE160 hash object.
        RIPEMD160 myRIPEMD160 = RIPEMD160Managed.Create();
        foreach(var item in items) 
        {
            var hashValue = myRIPEMD160.ComputeHash(Encoding.ASCII.GetBytes(item.Data));
            var hashText = string.Join("", hashValue.Select(x => x.ToString("x2")).ToList());
            Console.WriteLine(
                "* " + item.Description + 
                " - Expected " + item.ExpectedResult + 
                " Received " + hashText +
                (item.ExpectedResult == hashText ? " PASS" : " FAIL"));
        }
    }
}
