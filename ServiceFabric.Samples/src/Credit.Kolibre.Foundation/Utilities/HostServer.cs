using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Credit.Kolibre.Foundation.Utilities
{
    /// <summary>
    /// 用户获取当前应用宿主机的环境值。
    /// </summary>
    public static class HostServer
    {
        /// <summary>
        ///     获取当前托管线程的唯一标识符。
        /// </summary>
        /// <example>6</example>
        public static int CurrentManagedThreadId
        {
            get { return Environment.CurrentManagedThreadId; }
        }

        /// <summary>
        ///     获取当前宿主机的IP v4 地址。
        /// </summary>
        /// <example>192.168.1.36</example>
        public static string IP
        {
            get { return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString(); }
        }

        /// <summary>
        ///     确定当前操作系统是否为 64 位操作系统。
        /// </summary>
        public static bool Is64BitOperatingSystem
        {
            get { return Environment.Is64BitOperatingSystem; }
        }

        /// <summary>
        ///     确定当前进程是否为 64 位进程。
        /// </summary>
        public static bool Is64BitProcess
        {
            get { return Environment.Is64BitProcess; }
        }

        /// <summary>
        ///     获取此本地计算机的 NetBIOS 名称。
        /// </summary>
        /// <example>SIQILU-SURFACE</example>
        public static string MachineName
        {
            get { return Environment.MachineName; }
        }

        /// <summary>
        ///     获取包含当前平台标识符和版本号的字符串。
        /// </summary>
        /// <example>Microsoft Windows NT 6.2.9200.0</example>
        public static string OSVersion
        {
            get { return Environment.OSVersion.VersionString; }
        }

        /// <summary>
        ///     获取当前计算机上的处理器数。
        /// </summary>
        public static int ProcessorCount
        {
            get { return Environment.ProcessorCount; }
        }

        /// <summary>
        ///     获取描述公共语言运行时的主版本、次版本、内部版本和修订号的字符串。
        /// </summary>
        /// <value>4.0.30319.42000</value>
        public static string RuntimeVersion
        {
            get { return Environment.Version.ToString(); }
        }
    }
}