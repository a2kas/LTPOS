using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace POS_display.Helpers
{
    public static class StackTracePrinter
    {

        public static void PrintCurrentStackTrace(int skipFrames = 1, bool includeFileInfo = true, int maxFrames = 0)
        {
            var stackTrace = new StackTrace(skipFrames, includeFileInfo);
            PrintStackTrace(stackTrace, maxFrames);
        }

        public static string GetFormattedStackTrace(int skipFrames = 1, bool includeFileInfo = true, int maxFrames = 0)
        {
            var stackTrace = new StackTrace(skipFrames, includeFileInfo);
            return FormatStackTrace(stackTrace, maxFrames);
        }

        private static void PrintStackTrace(StackTrace stackTrace, int maxFrames)
        {
            Console.WriteLine(FormatStackTrace(stackTrace, maxFrames));
        }

        private static string FormatStackTrace(StackTrace stackTrace, int maxFrames)
        {
            var sb = new StringBuilder();
            var frames = stackTrace.GetFrames();

            if (frames == null || frames.Length == 0)
            {
                return "No stack trace available";
            }

            sb.AppendLine("Stack Trace:");
            sb.AppendLine(new string('=', 50));

            int framesToShow = maxFrames > 0 ? Math.Min(maxFrames, frames.Length) : frames.Length;

            for (int i = 0; i < framesToShow; i++)
            {
                var frame = frames[i];
                var method = frame.GetMethod();

                if (method == null) continue;

                var methodName = $"{method.DeclaringType?.Name}.{method.Name}";
                var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                var fullMethodSignature = $"{methodName}({parameters})";

                sb.AppendLine($"  {i + 1,2}. {fullMethodSignature}");

                var fileName = frame.GetFileName();
                var lineNumber = frame.GetFileLineNumber();
                var columnNumber = frame.GetFileColumnNumber();

                if (!string.IsNullOrEmpty(fileName))
                {
                    var shortFileName = Path.GetFileName(fileName);
                    sb.AppendLine($"      at {shortFileName}:line {lineNumber}:col {columnNumber}");
                }
                else
                {
                    sb.AppendLine($"      in {method.DeclaringType?.Assembly.GetName().Name}");
                }

                if (i < framesToShow - 1)
                    sb.AppendLine();
            }

            if (maxFrames > 0 && frames.Length > maxFrames)
            {
                sb.AppendLine($"      ... and {frames.Length - maxFrames} more frames");
            }

            return sb.ToString();
        }
    }
}
