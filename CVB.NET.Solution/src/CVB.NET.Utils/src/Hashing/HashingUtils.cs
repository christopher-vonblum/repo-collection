namespace CVB.NET.Utils.Hashing
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class HashingUtils
    {
        private static string HashFile<THashAlgorithm>(string path) where THashAlgorithm : HashAlgorithm, new()
        {
            using (FileStream stream = File.OpenRead(path))
            {
                return HashStream<THashAlgorithm>(stream);
            }
        }

        private static string HashStream<THashAlgorithm>(Stream stream) where THashAlgorithm : HashAlgorithm, new()
        {
            using (BufferedStream bufferedStream = new BufferedStream(stream, 1024 * 32))
            {
                THashAlgorithm algorithm = new THashAlgorithm();
                return ChecksumToString(algorithm.ComputeHash(bufferedStream));
            }
        }

        private static string HashString<THashAlgorithm>(string value) where THashAlgorithm : HashAlgorithm, new()
        {
            THashAlgorithm algorithm = new THashAlgorithm();
            return ChecksumToString(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }

        private static string HashBytes<THashAlgorithm>(byte[] value) where THashAlgorithm : HashAlgorithm, new()
        {
            THashAlgorithm algorithm = new THashAlgorithm();
            return ChecksumToString(algorithm.ComputeHash(value));
        }

        private static string ChecksumToString(byte[] checksum)
        {
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }
    }
}
