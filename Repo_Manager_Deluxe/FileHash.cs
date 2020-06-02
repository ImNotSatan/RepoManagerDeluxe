namespace Repo_Manager_Deluxe
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    internal class FileHash
    {
        private static SHA256 Sha256 = SHA256.Create();
        private static SHA512 Sha512 = SHA512.Create();
        private static SHA1 Sha1 = SHA1.Create();

        public static string BytesToString(byte[] bytes)
        {
            string str = "";
            foreach (byte num2 in bytes)
            {
                str = str + num2.ToString("x2");
            }
            return str;
        }

        private static byte[] GetHashMD5(string filename)
        {
            byte[] buffer;
            using (MD5 md = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    buffer = md.ComputeHash(stream);
                }
            }
            return buffer;
        }

        private static byte[] GetHashSha1(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha1.ComputeHash(stream);
            }
        }

        private static byte[] GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        private static byte[] GetHashSha521(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha512.ComputeHash(stream);
            }
        }

        public static string getMD5(string bestand) => 
            BytesToString(GetHashMD5(bestand));

        public static string getSHA1(string bestand) => 
            BytesToString(GetHashSha1(bestand));

        public static string getSHA256(string bestand) => 
            BytesToString(GetHashSha256(bestand));

        public static string getSHA521(string bestand) => 
            BytesToString(GetHashSha521(bestand));
    }
}

