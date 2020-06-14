using System.IO;
using System.IO.Compression;
using System.Text;

using Microsoft.AspNetCore.WebUtilities;

namespace XdtPlayground.Helpers
{
    public class DeflateUrlCompression
    {
        public static string Compress(string uncompressed)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressed)))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }

                    compressedBytes = compressedStream.ToArray();
                }
            }

            return Base64UrlTextEncoder.Encode(compressedBytes);

        }

        public static string Decompress(string compressed)
        {
            byte[] decompressedBytes;

            var compressedStream = new MemoryStream(Base64UrlTextEncoder.Decode(compressed));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);

                    decompressedBytes = decompressedStream.ToArray();
                }
            }

            return Encoding.UTF8.GetString(decompressedBytes);
        }
    }
}
