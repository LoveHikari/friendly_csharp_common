#if !NETSTANDARD2_0
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

/******************************************************************************************************************
 * 
 * 
 * 标  题： 注册表 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/11/18
 * 修  改：
 * 参  考： http://www.cnblogs.com/langu/archive/2012/02/26/2368877.html ， http://www.cnblogs.com/mingmingruyuedlut/archive/2011/01/21/1941225.html ， http://www.cnblogs.com/onedime/archive/2012/08/23/2652398.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// 注册表操作类
    /// </summary>
    public class RegistryHelper
    {
        /// <summary>
        /// 读取指定名称的注册表的值的数据
        /// </summary>
        /// <param name="hive">顶级注册表节点</param>
        /// <param name="subkey">子项路径</param>
        /// <param name="name">要寻找的值</param>
        /// <param name="view">注册表视图</param>
        /// <returns></returns>
        public static string GetRegistryData(RegistryHive hive, string subkey, string name, RegistryView view = RegistryView.Default)
        {
            SafeRegistryHandle handle = new SafeRegistryHandle(GetHiveHandle(hive), true);//获得根节点的安全句柄
            RegistryKey myKey = RegistryKey.FromHandle(handle, view).OpenSubKey(subkey, true);//获得要访问的键

            string registData = "";
            if (myKey != null)
            {
                registData = myKey.GetValue(name).ToString();
            }

            return registData;
        }

        ///// <summary>
        ///// 向注册表中写数据
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="tovalue"></param> 
        //public void SetRegistryData(RegistryKey root, string subkey, string name, string value)
        //{
        //    RegistryKey aimdir = root.CreateSubKey(subkey);
        //    aimdir.SetValue(name, value);
        //}

        ///// <summary>
        ///// 删除注册表中指定的注册表项
        ///// </summary>
        ///// <param name="name"></param>
        //public void DeleteRegist(RegistryKey root, string subkey, string name)
        //{
        //    string[] subkeyNames;
        //    RegistryKey myKey = root.OpenSubKey(subkey, true);
        //    subkeyNames = myKey.GetSubKeyNames();
        //    foreach (string aimKey in subkeyNames)
        //    {
        //        if (aimKey == name)
        //            myKey.DeleteSubKeyTree(name);
        //    }
        //}

        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <param name="hive">顶级注册节点</param>
        /// <param name="subkey">子项路径</param>
        /// <param name="name">要寻找的项</param>
        /// <param name="view">注册表视图</param>
        /// <returns>存在返回true</returns>
        public static bool IsRegistryExist(RegistryHive hive, string subkey, string name, RegistryView view = RegistryView.Default)
        {
            SafeRegistryHandle handle = new SafeRegistryHandle(GetHiveHandle(hive), true);//获得根节点的安全句柄
            RegistryKey myKey = RegistryKey.FromHandle(handle, view).OpenSubKey(subkey);//获得要访问的键

            if (myKey != null)
            {
                string[] subkeyNames = myKey.GetSubKeyNames();
                foreach (string keyName in subkeyNames)
                {
                    if (keyName == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <param name="hive">顶级注册节点</param>
        /// <param name="subkey">子项路径</param>
        /// <param name="view">注册表视图</param>
        /// <returns>存在返回true</returns>
        public static bool IsRegistryExist(RegistryHive hive, string subkey, RegistryView view = RegistryView.Default)
        {
            SafeRegistryHandle handle = new SafeRegistryHandle(GetHiveHandle(hive), true);//获得根节点的安全句柄
            RegistryKey myKey = RegistryKey.FromHandle(handle, view).OpenSubKey(subkey);//获得要访问的键

            return myKey != null;
        }
        /// <summary>
        /// 判断指定注册表值是否存在
        /// </summary>
        /// <param name="hive">顶级注册节点</param>
        /// <param name="subkey">子项路径</param>
        /// <param name="name">要寻找的值</param>
        /// <param name="view">注册表视图</param>
        /// <returns>存在返回true</returns>
        public static bool IsRegistryValueExist(RegistryHive hive, string subkey, string name, RegistryView view = RegistryView.Default)
        {
            SafeRegistryHandle handle = new SafeRegistryHandle(GetHiveHandle(hive), true);//获得根节点的安全句柄
            RegistryKey myKey = RegistryKey.FromHandle(handle, view).OpenSubKey(subkey);//获得要访问的键

            if (myKey != null)
            {
                string[] subkeyNames = myKey.GetValueNames();
                foreach (string keyName in subkeyNames)
                {
                    if (keyName == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 获得所有子项的名称
        /// </summary>
        /// <param name="hive">顶级注册节点</param>
        /// <param name="subkey">子项路径</param>
        /// <param name="view">注册表视图</param>
        /// <returns>所有子项的名称的数组</returns>
        public static string[] GetSubKeyNames(RegistryHive hive, string subkey, RegistryView view = RegistryView.Default)
        {
            SafeRegistryHandle handle = new SafeRegistryHandle(GetHiveHandle(hive), true);//获得根节点的安全句柄
            RegistryKey myKey = RegistryKey.FromHandle(handle, view).OpenSubKey(subkey);//获得要访问的键

            return myKey?.GetSubKeyNames();
        }

        /// <summary>
        /// 获得注册表句柄
        /// </summary>
        /// <param name="hive">注册表顶级节点</param>
        /// <returns></returns>
        private static IntPtr GetHiveHandle(RegistryHive hive)
        {
            IntPtr preexistingHandle = IntPtr.Zero;

            IntPtr HKEY_CLASSES_ROOT = new IntPtr(-2147483648);
            IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);
            IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);
            IntPtr HKEY_USERS = new IntPtr(-2147483645);
            IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);
            IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);
            IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);
            switch (hive)
            {
                case RegistryHive.ClassesRoot: preexistingHandle = HKEY_CLASSES_ROOT; break;
                case RegistryHive.CurrentUser: preexistingHandle = HKEY_CURRENT_USER; break;
                case RegistryHive.LocalMachine: preexistingHandle = HKEY_LOCAL_MACHINE; break;
                case RegistryHive.Users: preexistingHandle = HKEY_USERS; break;
                case RegistryHive.PerformanceData: preexistingHandle = HKEY_PERFORMANCE_DATA; break;
                case RegistryHive.CurrentConfig: preexistingHandle = HKEY_CURRENT_CONFIG; break;
                case RegistryHive.DynData: preexistingHandle = HKEY_DYN_DATA; break;
            }
            return preexistingHandle;
        }
    }
}
#endif
