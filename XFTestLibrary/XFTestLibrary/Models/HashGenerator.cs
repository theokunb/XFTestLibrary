using System;
using System.Security.Cryptography;
using System.Text;
using XFTestLibrary.Services;

namespace XFTestLibrary.Models
{
    public class HashGenerator : IHashGenerator
    {
        public HashGenerator()
        {
        }


        private byte[] sourceBytes;
        private byte[] hashbytes;

        public string ComputeHash(string input)
        {
            sourceBytes = Encoding.ASCII.GetBytes(input);
            hashbytes = new MD5CryptoServiceProvider().ComputeHash(sourceBytes);
            return ByteArrayToString(hashbytes);
        }

        private string ByteArrayToString(byte[] input)
        {
            StringBuilder output = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length - 1; i++)
            {
                output.Append(input[i].ToString("X2"));
            }
            return output.ToString();
        }
    }
}
