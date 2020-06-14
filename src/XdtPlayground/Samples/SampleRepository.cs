using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using XdtPlayground.Assets;

namespace XdtPlayground.Samples
{
    public class SampleRepository
    {
        public const int CategoryIndex = 1;
        public const int SampleNameIndex = 3;
        public const int SampleTypeIndex = 4;

        public static Sample DefaultSample => GetSamples().First();

        public static Sample FindSample(string title)
        {
            return GetSamples().FirstOrDefault(p => string.Compare(p.Title, title, ignoreCase: true) == 0);
        }
        public static IReadOnlyCollection<IGrouping<string, Sample>> GetSamplesByDisplayCategory()
        {
            return GetSamples().GroupBy(p => p.DisplayCategory).ToList();
        }

        private static IReadOnlyCollection<Sample> _samples = null;
        public static IReadOnlyCollection<Sample> GetSamples()
        {
            if (_samples != null)
            {

                return _samples;
            }

            var samples = new List<Sample>();

            var assembly = typeof(AssetMarker).Assembly;
            var samplesPrefix = typeof(AssetMarker).Namespace + ".Samples.";
            var sampleResources = assembly.GetManifestResourceNames()
                .Where(p => p.StartsWith(samplesPrefix))
                .OrderBy(p => p).ToArray();

            //Console.WriteLine(string.Join("\n", sampleResources));
            Sample currentSample = null;
            foreach (var sampleRes in sampleResources)
            {
                var relativePath = sampleRes.Substring(samplesPrefix.Length);
                var parts = relativePath.Split(".");
                var category = parts[CategoryIndex];
                var name = parts[SampleNameIndex];
                var sampleType = parts[SampleTypeIndex];
                var id = category + "/" + name;

                if (currentSample == null || (currentSample.Category + "/" + currentSample.Title) != id)
                {
                    var sample = new Sample();
                    sample.Category = category;
                    sample.Title = name;

                    samples.Add(sample);
                    currentSample = sample;
                }

                var content = GetEmbeddedResourceContent(assembly, sampleRes);
                if (sampleType == "xdt")
                {
                    currentSample.XDT = content;
                }
                else if (sampleType == "xml")
                {
                    currentSample.XML = content;
                }
                else
                {
                    throw new Exception($"Unknown file {sampleRes}");
                }
            }

            return _samples = samples;
        }

        public static string GetEmbeddedResourceContent(Assembly asm, string resourceName)
        {
            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            {
                using (StreamReader source = new StreamReader(stream, Encoding.UTF8))
                {
                    return source.ReadToEnd();
                }
            }
        }
    }
}
