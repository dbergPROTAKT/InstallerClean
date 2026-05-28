using System.Runtime.InteropServices;

namespace InstallerClean.Interop.Native;

internal static partial class User32
{
    private const string Library = "user32.dll";

#if NET7_0_OR_GREATER
    [LibraryImport(Library, EntryPoint = "GetShellWindow")]
    public static partial IntPtr GetShellWindow();

    [LibraryImport(Library, EntryPoint = "GetForegroundWindow")]
    public static partial IntPtr GetForegroundWindow();

    [LibraryImport(Library, EntryPoint = "GetWindowThreadProcessId")]
    public static partial uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
#else
    [DllImport(Library, EntryPoint = "GetShellWindow")]
    public static extern IntPtr GetShellWindow();

    [DllImport(Library, EntryPoint = "GetForegroundWindow")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport(Library, EntryPoint = "GetWindowThreadProcessId")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
#endif
}
