using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

/******************************************************************************************************************
 * 
 * 
 * 标  题： INI配置文件帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/01/06
 * 修  改：
 * 参  考： https://msdn.microsoft.com/en-us/library/ms724353(v=vs.85).aspx http://bbs.csdn.net/topics/100087877
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// 读写ini文件，需要绝对路径
    /// </summary>
    public class IniHelper : Dictionary<object, Dictionary<object, object>>
    {

        #region DllImport
        /// <summary>
        /// 声明INI文件的写操作函数 
        /// </summary>
        /// <param name="section">配置节</param>
        /// <param name="key">键名</param>
        /// <param name="val">键值</param>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        /// <summary>
        /// 声明INI文件的读操作函数
        /// </summary>
        /// <param name="section">配置节</param>
        /// <param name="key">键名</param>
        /// <param name="def"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary>
        /// 读取一INI文件中所有的配置节名。
        /// </summary>
        /// <param name="retVal">指向缓冲区的指针，用于接收与指定文件相关联的段名称。 缓冲区填充有一个或多个空终止字符串; 最后一个字符串后跟第二个空字符。</param>
        /// <param name="size">retVal参数指向的缓冲区的大小（以字符为单位）</param>
        /// <param name="filePath">初始化文件的名称。 如果此参数为NULL，该函数将搜索Win.ini文件。 如果此参数不包含文件的完整路径，系统将在Windows目录中搜索该文件。</param>
        /// <returns>指定复制到指定缓冲区的字符数，不包括终止空字符。 如果缓冲区大小不足以包含与指定初始化文件关联的所有段名称，则返回值等于由size指定的大小减去2。</returns>
        [DllImport("kernel32", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "GetPrivateProfileSectionNames")]
        private static extern int GetPrivateProfileSectionNames(byte[] retVal, int size, string filePath);
        /// <summary>
        /// 检索INI文件的指定配置节的的所有键和值。
        /// </summary>
        /// <param name="section">配置节名</param>
        /// <param name="retVal">指向缓冲区的指针，用于接收与命名段相关联的键名称和值对。 缓冲区填充有一个或多个空终止字符串; 最后一个字符串后跟第二个空字符。</param>
        /// <param name="size">retVal参数指向的缓冲区的大小（以字符为单位）</param>
        /// <param name="filePath">初始化文件的名称。 如果此参数为NULL，该函数将搜索Win.ini文件。 如果此参数不包含文件的完整路径，系统将在Windows目录中搜索该文件。</param>
        /// <returns>返回值指定复制到缓冲区的字符数，不包括终止空字符。 如果缓冲区不足以包含与命名段相关联的所有键名和值对，则返回值等于size减2。</returns>
        [DllImport("kernel32", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "GetPrivateProfileSection")]
        private static extern int GetPrivateProfileSection(string section, byte[] retVal, int size, string filePath);
        #endregion

        private string _iniPath;
        private Collection<object> _keyList = new Collection<object>();
        private Collection<Dictionary<object, object>> _valueList = new Collection<Dictionary<object, object>>();
        /// <summary>
        /// 获取包含INI中配置节的ICollection
        /// </summary>
        public new ICollection<object> Keys
        {
            get { return _keyList; }

        }
        /// <summary>
        /// 获取包含INI中键值对的ICollection
        /// </summary>
        public new ICollection<Dictionary<object, object>> Values
        {
            get { return _valueList; }
        }

        /// <summary>
        /// 构造函数，之后需要调用Load 方法
        /// </summary>
        public IniHelper() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iniPath">INI文件路径</param>
        public IniHelper(string iniPath)
        {
            Load(iniPath);
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="iniPath">文件路径</param>
        public void Load(string iniPath)
        {
            this._iniPath = iniPath;
            ArrayList keysList = new ArrayList();
            //获得所有配置节
            byte[] buffer = new byte[1024];
            int bufLen = GetPrivateProfileSectionNames(buffer, buffer.GetUpperBound(0), iniPath);
            if (bufLen > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bufLen; i++)
                {
                    if (buffer[i] != 0)
                    {
                        sb.Append((char)buffer[i]);
                    }
                    else
                    {
                        if (sb.Length > 0)
                        {
                            keysList.Add(sb.ToString());
                            sb = new StringBuilder();
                        }
                    }
                }
            }

            foreach (var key in keysList)
            {
            	  buffer = new byte[1024];
                Dictionary<object, object> value = new Dictionary<object, object>();
                bufLen = GetPrivateProfileSection(key.ToString(), buffer, buffer.GetUpperBound(0), iniPath);
                if (bufLen > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bufLen; i++)
                    {
                        if (buffer[i] != 0)
                        {
                            sb.Append((char)buffer[i]);
                        }
                        else
                        {
                            if (sb.Length > 0)
                            {
                                string item = sb.ToString();
                                int index = item.IndexOf("=", StringComparison.CurrentCulture);
                                value.Add(item.Substring(0, index), item.Substring(index + 1, item.Length - index - 1));
                                sb = new StringBuilder();
                            }
                        }
                    }
                    this.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 重写父类的方法
        /// </summary>
        /// <param name="key">配置节</param>
        /// <param name="value">键值对</param>
        public new void Add(object key, Dictionary<object, object> value)
        {
            base.Add(key, value);
            _keyList.Add(key);
            _valueList.Add(value);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="section">配置节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(object section, object key, object value)
        {
            if (base.ContainsKey(section))
            {
                Dictionary<object, object> dic = base[section];
                dic.Add(key, value);
                base[section] = dic;
                int index = _keyList.IndexOf(section);
                _valueList[index] = dic;
            }
            else
            {
                Dictionary<object,object> dic = new Dictionary<object, object>();
                dic.Add(key,value);
                base.Add(section, dic);
                _keyList.Add(section);
                _valueList.Add(dic);
            }
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        public void Save()
        {
            SaveAs(_iniPath);
        }

        /// <summary>
        /// 另保存文件，文件存在则覆盖
        /// </summary>
        /// <param name="iniPath"></param>
        public void SaveAs(string iniPath)
        {
            //创建文件
            string dir = System.IO.Path.GetDirectoryName(iniPath);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            if (!System.IO.File.Exists(iniPath))
            {
                System.IO.FileStream f = System.IO.File.Create(iniPath);
                f.Close();
                f.Dispose();
            }

            System.IO.StreamWriter sw = new System.IO.StreamWriter(iniPath, false);
            sw.Write("#表格配置档案");
            sw.Close();
            sw.Dispose();
            for (int i = 0; i < _keyList.Count; i++)
            {
                String key = _keyList[i].ToString();
                Dictionary<object, object> val = _valueList[i];
                if (key.StartsWith("#"))
                {

                }
                else
                {
                    foreach (KeyValuePair<object, object> pair in val)
                    {
                        WritePrivateProfileString(key, pair.Key.ToString(), pair.Value.ToString(), iniPath);
                    }
                }
            }

        }

    }
}