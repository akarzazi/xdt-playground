using System;
using System.Linq;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

using XdtPlayground.Helpers;
using XdtPlayground.Samples;

namespace XdtPlayground
{
    public static class Navigation
    {
        public const string APP_ROOT = "/XdtPlayground/";

        public const string QUERY = "query";
        public const string SAMPLE = "sample";

        public static string CreateSampleUri(string sampleTitle)
        {
            return QueryHelpers.AddQueryString("", SAMPLE, sampleTitle);
        }

        public static string CreateQueryUri(string xml, string xdt)
        {
            var encoded = Encode(xml, xdt);
            var compressed = DeflateUrlCompression.Compress(encoded);
            return QueryHelpers.AddQueryString("", QUERY, compressed);
        }

        public static bool TryParseSample(string rawSampleTitle, out string xml, out string xdt)
        {
            xml = "";
            xdt = "";

            var sample = SampleRepository.FindSample(rawSampleTitle);
            if (sample == null)
            {
                return false;
            }

            xml = sample.XML;
            xdt = sample.XDT;
            return true;
        }


        public static bool TryParseQuery(string rawCompressedQuery, out string xml, out string xdt)
        {
            xml = "";
            xdt = "";

            try
            {
                var decompressed = DeflateUrlCompression.Decompress(rawCompressedQuery);
                (xml, xdt) = Decode(decompressed);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public static string Encode(string xml, string xdt)
        {
            return xml.Length + "|" + xml + xdt;
        }

        public static (string xml, string xdt) Decode(string encoded)
        {
            var xmlLengthStr = "";
            var i = 0;
            while (i < encoded.Length)
            {
                var cha = encoded[i];
                if (cha == '|')
                {
                    break;
                }

                xmlLengthStr += cha;
                i++;
            }

            var xmlLength = int.Parse(xmlLengthStr);
            var xmlStart = i + 1;

            var xml = encoded.Substring(xmlStart, xmlLength);
            var xdt = encoded.Substring(xmlStart + xmlLength);

            return (xml, xdt);
        }
    }
}
