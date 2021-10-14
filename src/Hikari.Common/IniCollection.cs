// Copyright (c) the Hikari. Foundation. All rights reserved.
// The Hikari. Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/******************************************************************************************************************
 * 
 * 
 * 标  题： INI配置文件帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2021/10/14
 * 修  改：
 * 参  考： 
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
    public class IniCollection : Dictionary<string, Dictionary<string, string>>
    {

        /// <summary>
        /// 构造函数，之后需要调用Load 方法
        /// </summary>
        public IniCollection() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iniPath">INI文件路径</param>
        public IniCollection(string iniPath) : this()
        {
            Load(iniPath);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reader"></param>
        public IniCollection(TextReader reader) : this()
        {
            Load(reader);
        }
        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="iniPath">文件路径</param>
        private void Load(string iniPath)
        {
            using var file = new StreamReader(iniPath);
            Load(file);
        }
        private void Load(TextReader reader)
        {
            IDictionary<string, string>? section = null;
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();

                // 跳过空行
                if (line == string.Empty)
                    continue;

                // 跳过注释
                if (line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    var sectionName = line.Substring(1, line.Length - 2);  // 配置节名称
                    if (!this.ContainsKey(sectionName))
                    {
                        this.Add(sectionName, new Dictionary<string, string>());
                    }
                    section = this[sectionName];
                    continue;
                }


                if (section != null)
                {
                    var keyValue = line.Split(new[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (keyValue.Length != 2)
                        continue;

                    section.Add(keyValue[0].Trim(), keyValue[1].Trim());
                }
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="section">配置节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(string section, string key, string value)
        {
            if (this.ContainsKey(section))
            {
                IDictionary<string, string> dic = this[section];
                dic.Add(key, value);
            }
            else
            {
                Dictionary<string, string> dic = new Dictionary<string, string> { { key, value } };
                this.Add(section, dic);
            }
        }

        /// <summary>
        /// 创建一个新的 INI 文件
        /// </summary>
        /// <param name="path">要创建的 INI 文件的路径</param>
        public void Save(string path)
        {
            using var file = new StreamWriter(path);
            Save(file);
        }

        /// <summary>
        /// 创建一个新的 INI 文件
        /// </summary>
        /// <param name="writer"></param>
        public void Save(TextWriter writer)
        {
            foreach (var sectionName in this.Keys)
            {
                if (this[sectionName].Count == 0)
                    continue;

                writer.WriteLine($"[{sectionName}]");

                foreach (var property in this[sectionName])
                {
                    writer.WriteLine($"{property.Key}={property.Value}");
                }

                writer.WriteLine();
            }
        }

    }
}