using System.Runtime.InteropServices;
namespace TCGame.Client.Core {
    public static class CoreApI
    {
        public const int SUCCESS_INIT = 1;
        public const int SUCCESS_BIND = 6;
        public const int SUCCESS_CLOSE = 8;

        /// <summary>
        /// ����������socket
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern byte CreateSocketServer();

        /// <summary>
        /// �󶨷�����socket
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern byte BindSocketServer();

        /// <summary>
        /// �����첽�̷߳�����
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern void StartServer();

        /// <summary>
        /// �ر��첽�̷߳�����
        /// </summary>
        /// <returns></returns>
        [DllImport("GameCore.dll")]
        public static extern byte CloseServer();
    }
}
