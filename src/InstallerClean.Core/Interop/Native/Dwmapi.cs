using System.Runtime.InteropServices;

namespace InstallerClean.Interop.Native;

internal static partial class Dwmapi
{
    private const string Library = "dwmapi.dll";

#if NET7_0_OR_GREATER
    [LibraryImport(Library)]
    public static partial int DwmSetWindowAttribute(
        IntPtr hwnd,
        int attr,
        ref int value,
        int size);
#else
    [DllImport(Library)]
    public static extern int DwmSetWindowAttribute(
        IntPtr hwnd,
        int attr,
        ref int value,
        int size);
#endif

    public const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
}
