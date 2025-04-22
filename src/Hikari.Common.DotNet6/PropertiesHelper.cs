using System;
using System.Collections;
using System.IO;
using Hikari.Common.IO;

/******************************************************************************************************************
 * 
 * 
 * 标  题： Properties配置文件帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/11/23
 * 修  改：
 * 参  考： http://blog.sina.com.cn/s/blog_5e6b02590100cwxs.html http://www.cnblogs.com/xudong-bupt/p/3758136.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common;
/// <summary>
/// Properties配置文件帮助类
/// </summary>
public class PropertiesHelper : Hashtable
{
    private string _filePath;
    private ArrayList _keylist = new ArrayList();
    private ArrayList _valuelist = new ArrayList();

    /// <summary>
    /// 构造函数，之后需要调用Load 方法
    /// </summary>
    public PropertiesHelper()
    {

    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="filePath">要读写的properties文件名</param>
    public PropertiesHelper(string filePath)
    {
        Load(filePath);
    }
    /// <summary>
    /// 重写父类的方法
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public override void Add(object key, object value)
    {
        base.Add(key, value);
        _keylist.Add(key);
        _valuelist.Add(value);
    }
    /// <summary>
    /// 获取包含Properties中键的ICollection
    /// </summary>
    public override ICollection Keys
    {
        get
        {
            return _keylist;
        }
    }
    /// <summary>
    /// 获取包含Properties中值的ICollection
    /// </summary>
    public override ICollection Values
    {
        get
        {
            return _valuelist;
        }
    }
    /// <summary>
    /// 加载文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    public void Load(string filePath)
    {
        this._filePath = filePath;
        char[] convertBuf = new char[1024];
        int limit;
        int keyLen;
        int valueStart;
        char c;
        string bufLine = string.Empty;
        bool hasSep;
        bool precedingBackslash;
        using (StreamReader sr = new StreamReader(filePath))
        {
            while (sr.Peek() >= 0)
            {
                bufLine = sr.ReadLine();
                limit = bufLine.Length;
                keyLen = 0;
                valueStart = limit;
                hasSep = false;
                precedingBackslash = false;
                if (bufLine.StartsWith("#"))
                    keyLen = bufLine.Length;
                while (keyLen < limit)
                {
                    c = bufLine[keyLen];
                    if ((c == '=' || c == ':') & !precedingBackslash)
                    {
                        valueStart = keyLen + 1;
                        hasSep = true;
                        break;
                    }
                    else if ((c == ' ' || c == '\t' || c == '\f') & !precedingBackslash)
                    {
                        valueStart = keyLen + 1;
                        break;
                    }
                    if (c == '\\')
                    {
                        precedingBackslash = !precedingBackslash;
                    }
                    else
                    {
                        precedingBackslash = false;
                    }
                    keyLen++;
                }
                while (valueStart < limit)
                {
                    c = bufLine[valueStart];
                    if (c != ' ' && c != '\t' && c != '\f')
                    {
                        if (!hasSep && (c == '=' || c == ':'))
                        {
                            hasSep = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    valueStart++;
                }
                string key = bufLine.Substring(0, keyLen);
                string values = bufLine.Substring(valueStart, limit - valueStart);
                if (key == "")
                    key += "#";
                while (key.StartsWith("#") & this.Contains(key))
                {
                    key += "#";
                }
                this.Add(key, values);
            }
        }
    }
    /// <summary>
    /// 保存文件
    /// </summary>
    public void Save()
    {
        SaveAs(_filePath);
    }

    /// <summary>
    /// 另保存文件，文件存在则覆盖，注释的key为#
    /// </summary>
    /// <param name="filePath">要保存的文件的路径</param>
    public void SaveAs(string filePath)
    {
        filePath = FileHelper.PathProcess(filePath);
        if (File.Exists(filePath))  //文件存在则删除
        {
            System.IO.StreamWriter sw1 = new System.IO.StreamWriter(filePath, false);
            sw1.Write("");
            sw1.Close();
            sw1.Dispose();
        }
        //创建文件
        string dir = System.IO.Path.GetDirectoryName(filePath);
        if (!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }
        if (!System.IO.File.Exists(filePath))
        {
            System.IO.FileStream f = System.IO.File.Create(filePath);
            f.Close();
            f.Dispose();
        }

        System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, true);
        foreach (object item in _keylist)
        {
            String key = (String)item;
            String val = (String)this[key];
            if (key.StartsWith("#"))
            {
                sw.WriteLine(key + val);
            }
            else
            {
                sw.WriteLine(key + "=" + val);
            }
        }
        sw.Close();
        sw.Dispose();
    }
}
