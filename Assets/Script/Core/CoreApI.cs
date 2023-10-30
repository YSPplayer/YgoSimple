using System.Runtime.InteropServices;

public static class CoreApI 
{
    /// <summary>
    /// 创建服务器socket
    /// </summary>
    /// <returns></returns>
   [DllImport("GameCore.dll")]
    public static extern byte CreateSocketServer();
}
