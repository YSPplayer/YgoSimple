using System.Runtime.InteropServices;
namespace TCGame.Client.Core {
    public static class CoreApI
    {
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
    }
}
