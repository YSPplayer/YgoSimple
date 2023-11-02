using System.Runtime.InteropServices;
namespace TCGame.Client.Core {
    public static class CoreApI
    {
        /// <summary>
        /// 创建服务器socket
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern byte CreateSocketServer();

        /// <summary>
        /// 绑定服务器socket
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern byte BindSocketServer();

        /// <summary>
        /// 开启异步线程服务器
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern void StartServer();
    }
}
