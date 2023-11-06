using System.Runtime.InteropServices;
namespace TCGame.Client.Core {
    public static class CoreApI
    {
        public const int SUCCESS_INIT = 1;
        public const int SUCCESS_BIND = 6;
        public const int SUCCESS_CLOSE = 8;

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

        /// <summary>
        /// 关闭异步线程服务器
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern byte CloseServer();
    }
}
