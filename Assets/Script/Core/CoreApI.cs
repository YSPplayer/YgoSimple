using System.Runtime.InteropServices;

public static class CoreApI 
{
    /// <summary>
    /// ����������socket
    /// </summary>
    /// <returns></returns>
   [DllImport("GameCore.dll")]
    public static extern byte CreateSocketServer();
}
