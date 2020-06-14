using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using XdtExtensions.Microsoft.Web.XmlTransform;

using XdtPlayground.Helpers;

namespace XdtPlayground.Models
{
    public class XmlTransformView
    {
        private string _loadXdtExtensionsAssembly = XdtExtensions.DefaultNamespace.Namespace;

        public string SourceXML { get; set; }
        public string TransformXML { get; set; }
        public string DiffResultXML { get; set; }
        public string DiffSourceXML { get; set; }
        public string Log { get; set; }

        private static object _lock = new object();

        public XmlTransformView()
        {
        }

        public void ApplyTransform()
        {
            lock (_lock)
            {
                var logger = new TransformLogger();
                try
                {
                    Log = "";
                    using (var document = new XmlTransformableDocument() { PreserveWhitespace = true })
                    using (var transform = new XmlTransformation(TransformXML, false, logger))
                    {
                        document.LoadXml(SourceXML);

                        if (!transform.Apply(document))
                        {
                            Log = logger.GetMessages();
                            DiffResultXML = SourceXML;
                        }
                        else
                        {
                            // save to get the final formatting
                            document.Save(new MemoryStream());
                            DiffResultXML = document.OuterXml;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                    Log = ex.Message + "\n" + logger.GetMessages();
                    DiffResultXML = SourceXML;
                }
                finally
                {
                    DiffSourceXML = SourceXML;
                }
            }
        }
    }

    public class TransformLogger : IXmlTransformationLogger
    {
        StringBuilder _sb;
        public TransformLogger()
        {
            _sb = new StringBuilder();
        }

        public string GetMessages()
        {
            return _sb.ToString();
        }

        public void EndSection(string message, params object[] messageArgs)
        {
            _sb.AppendLine("EndSection");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            _sb.AppendLine("EndSection");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogError(string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogError");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogError(string file, string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogError");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogError");
            _sb.AppendLine($"lineNumber {lineNumber} linePosition {linePosition}");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogErrorFromException(Exception ex)
        {
            _sb.AppendLine("LogError");
            _sb.AppendLine(ex.ToString());
            _sb.AppendLine();
        }

        public void LogErrorFromException(Exception ex, string file)
        {
            _sb.AppendLine("LogError");
            _sb.AppendLine(ex.ToString());
            _sb.AppendLine();
        }

        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            _sb.AppendLine("LogError");
            _sb.AppendLine($"lineNumber {lineNumber} linePosition {linePosition}");
            _sb.AppendLine(ex.ToString());
            _sb.AppendLine();
        }

        public void LogMessage(string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogMessage");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogMessage");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogWarning");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogWarning");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            _sb.AppendLine("LogWarning");
            _sb.AppendLine($"lineNumber {lineNumber} linePosition {linePosition}");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void StartSection(string message, params object[] messageArgs)
        {
            _sb.AppendLine("StartSection");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }

        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            _sb.AppendLine("StartSection");
            _sb.AppendFormat(message, messageArgs);
            _sb.AppendLine();
        }
    }
}
