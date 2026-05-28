#if !NET5_0_OR_GREATER
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

namespace System
{
    internal readonly struct Index
    {
        private readonly int _value;
        public Index(int value, bool fromEnd = false) => _value = fromEnd ? ~value : value;
        public static Index FromEnd(int value) => new Index(value, true);
        public int GetOffset(int length) => _value < 0 ? length + ~_value + 1 : _value;
        // Needed so the compiler can lower `^x`:
        public static implicit operator Index(int value) => new Index(value);
    }

    internal readonly struct Range
    {
        public Index Start { get; }
        public Index End { get; }
        public Range(Index start, Index end) { Start = start; End = end; }
        public static Range StartAt(Index start) => new Range(start, Index.FromEnd(0));
    }
}

namespace InstallerClean.Polyfills
{
    internal static class Net48Compat
    {
        // Dictionary.TryAdd polyfill
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
            where TKey : notnull
        {
            if (dict.ContainsKey(key)) return false;
            dict.Add(key, value);
            return true;
        }

        // File.Move with overwrite
        public static void FileMove(string sourceFileName, string destFileName, bool overwrite)
        {
            if (overwrite && System.IO.File.Exists(destFileName))
                System.IO.File.Delete(destFileName);
            System.IO.File.Move(sourceFileName, destFileName);
        }

        // Array.Clear single-param
        public static void ArrayClear(Array array)
        {
            Array.Clear(array, 0, array.Length);
        }

        // Path.IsPathFullyQualified polyfill
        public static bool IsPathFullyQualified(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (path.Length >= 2 && path[1] == ':' && (path[0] >= 'A' && path[0] <= 'Z' || path[0] >= 'a' && path[0] <= 'z'))
                return path.Length >= 3 && (path[2] == '\\' || path[2] == '/');
            return path.StartsWith("\\\\", StringComparison.Ordinal) || path.StartsWith("//", StringComparison.Ordinal);
        }

        // String range indexer polyfill
        public static string Substring(this string s, Range range)
        {
            var start = range.Start.GetOffset(s.Length);
            var end = range.End.GetOffset(s.Length);
            return s.Substring(start, end - start);
        }
    }
}
#endif
