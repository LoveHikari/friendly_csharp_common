using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

/******************************************************************************************************************
 * 
 * 
 * 标  题：文件帮助类(版本：Version1.0.0)
 * 作  者：YuXiaoWei
 * 日  期：2016/10/20
 * 修  改：2016/10/31
 * 参  考：
 * 说  明：暂无...
 * 备  注：暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {
        #region 获得文件
        /// <summary>
        /// 获得目录下所有文件，不包括子目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns>完整路径集合</returns>
        public static List<string> FindFile(string dirPath)
        {
            return FindFile(dirPath, "*.*");
        }
        /// <summary>
        /// 获得目录下指定后缀的文件，不包括子目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="extension">指定要访问的文件的类型的扩展名，例：*.txt</param>
        public static List<string> FindFile(string dirPath, string extension)
        {
            List<string> filePathList = new List<string>();  //文件路径集合
            if (System.IO.Directory.Exists(dirPath))
            {
                //在指定目录查找文件(不包括子目录)
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirPath);
                try
                {
                    foreach (System.IO.FileInfo f in dir.GetFiles(extension)) //查找文件
                    {
                        filePathList.Add(f.FullName);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return filePathList;
        }
        /// <summary>
        /// 获得目录下所有文件，包括全部目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns></returns>
        public static List<string> FindAllFile(string dirPath)
        {
            List<string> filePathList = new List<string>();  //文件路径集合
            return GetAll(dirPath, "*.*", filePathList);
        }
        /// <summary>
        /// 获得目录下指定后缀的文件，包括全部目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="extension">指定要访问的文件的类型的扩展名，例：*.txt</param>
        /// <returns></returns>
        public static List<string> FindAllFile(string dirPath, string extension)
        {
            List<string> filePathList = new List<string>();  //文件路径集合
            return GetAll(dirPath, extension, filePathList);
        }
        /// <summary>
        /// 获得目录下指定后缀的文件
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="extension">指定要访问的文件的类型的扩展名，例：*.txt</param>
        /// <param name="filePathList">文件路径集合</param>
        /// <returns></returns>
        private static List<string> GetAll(string dirPath, string extension, List<string> filePathList)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirPath);
            try
            {
                System.IO.FileInfo[] allFile = dir.GetFiles(extension);
                foreach (System.IO.FileInfo fi in allFile)
                {
                    filePathList.Add(fi.FullName);
                }

                System.IO.DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (System.IO.DirectoryInfo d in allDir)
                {
                    GetAll(d.FullName, extension, filePathList);
                }
                return filePathList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 获得目录下所有文件，只包括一级目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns></returns>
        public static List<string> FindOneFile(string dirPath)
        {
            return FindOneFile(dirPath, "*.*");
        }
        /// <summary>
        /// 获得目录下指定后缀的文件，只包括一级目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="extension">指定要访问的文件的类型的扩展名，例：*.txt</param>
        /// <returns></returns>
        public static List<string> FindOneFile(string dirPath, string extension)
        {
            List<string> filePathList = new List<string>();  //文件路径集合
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirPath);
            try
            {
                foreach (System.IO.DirectoryInfo d in dir.GetDirectories()) //查找子目录
                {
                    filePathList.AddRange(FindFile(d.FullName, extension));
                }
                foreach (System.IO.FileInfo f in dir.GetFiles(extension)) //查找文件
                {
                    filePathList.Add(f.FullName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return filePathList;
        }

        #endregion

        #region 获得文件夹
        /// <summary>
        /// 获得目录下所有一级子目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns>目录集合</returns>
        public static List<string> FindDirectories(string dirPath)
        {
            List<string> filePathList = new List<string>();  //文件路径集合
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirPath);
            try
            {
                foreach (System.IO.DirectoryInfo d in dir.GetDirectories()) //查找子目录
                {
                    filePathList.Add(d.FullName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return filePathList;
        }
        /// <summary>
        /// 获得目录下所有子目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns>目录集合</returns>
        public static List<string> FindAllDirectories(string dirPath)
        {
            List<string> filePathList = new List<string>();
            return FindAllDirectories(dirPath, filePathList);
        }
        /// <summary>
        /// 获得目录下所有子目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="filePathList">文件路径集合</param>
        /// <returns></returns>
        private static List<string> FindAllDirectories(string dirPath, List<string> filePathList)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirPath);
            try
            {
                foreach (System.IO.DirectoryInfo d in dir.GetDirectories()) //查找子目录
                {
                    filePathList.Add(d.FullName);
                    FindAllDirectories(d.FullName, filePathList);

                }
                return filePathList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        /// <summary>
        /// 删除目录及其内容，默认不删除根目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="recursive">是否删除根目录</param>
        public static void DeleteDirectory(string dirPath, bool recursive = false)
        {
            foreach (string d in System.IO.Directory.GetFileSystemEntries(dirPath))
            {
                if (System.IO.File.Exists(d))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = System.IO.FileAttributes.Normal;
                    System.IO.File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    System.IO.DirectoryInfo d1 = new System.IO.DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0 || d1.GetDirectories().Length > 0)
                    {
                        DeleteDirectory(d1.FullName);////递归删除子文件夹
                    }
                    System.IO.Directory.Delete(d);
                }
            }

            if (recursive)
            {
                System.IO.Directory.Delete(dirPath);
            }
        }
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="srcPath">原始文件夹</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void RenameDirectory(string srcPath, string aimPath)
        {
            CopyDir(srcPath, aimPath);
            DeleteDirectory(srcPath, true);

        }
        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面，目标文件夹为只读属性就会报错。适合重命名复制
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        private static void CopyDir(string srcPath, string aimPath)
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[^1] != System.IO.Path.DirectorySeparatorChar)
                aimPath += System.IO.Path.DirectorySeparatorChar;
            // 判断目标目录是否存在如果不存在则新建之
            if (!System.IO.Directory.Exists(aimPath))
                System.IO.Directory.CreateDirectory(aimPath);
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            //string[] fileList = Directory.GetFiles(srcPath);
            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
            //遍历所有的文件和目录
            foreach (string file in fileList)
            {
                //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                if (System.IO.Directory.Exists(file))
                    CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                //否则直接Copy文件
                else
                    System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
            }
        }
        /// <summary>
        /// 复制包括了原文件的根目录名称，适合直接复制
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <remarks>https://www.cnblogs.com/wangjianhui008/p/3234519.html?ivk_sa=1024320u</remarks>
        public static void CopyDirectory(string sourceFolder, string destFolder)
        {
            string folderName = System.IO.Path.GetFileName(sourceFolder);
            string destfolderdir = System.IO.Path.Combine(destFolder, folderName);
            string[] filenames = System.IO.Directory.GetFileSystemEntries(sourceFolder);
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (System.IO.Directory.Exists(file))
                {
                    string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                    if (!System.IO.Directory.Exists(currentdir))
                    {
                        System.IO.Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, destfolderdir);
                }
                else
                {
                    string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                    if (!System.IO.Directory.Exists(destfolderdir))
                    {
                        System.IO.Directory.CreateDirectory(destfolderdir);
                    }
                    System.IO.File.Copy(file, srcfileName);
                }
            }

        }

        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void Create(string path)
        {
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream f = System.IO.File.Create(path);
                f.Close();
                f.Dispose();
            }
        }
        #endregion

        #region 写文件
        /// <summary>
        /// 写文件，如果文件不存在则创建，存在则覆盖
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="strings">文件内容</param>
        /// <param name="encod">编码，默认utf-8</param>
        public static void WriteFile(string path, string strings, string encod = "utf-8")
        {
            path = FilePathProcess(path);
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream f = System.IO.File.Create(path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding(encod));
            f2.Write(strings);
            f2.Close();
            f2.Dispose();
        }

        /// <summary>
        /// 写文件，如果文件不存在则创建，存在则追加
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="strings">内容</param>
        /// <param name="encod">编码，默认utf-8</param>
        public static void WriteFile2(string path, string strings, string encod = "utf-8")
        {
            path = FilePathProcess(path);
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream f = System.IO.File.Create(path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(path, true, System.Text.Encoding.GetEncoding(encod));
            f2.Write(strings);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encod">编码，默认utf-8</param>
        /// <returns></returns>
        public static string ReadFile(string path, string encod = "utf-8")
        {
            string s;
            if (!System.IO.File.Exists(path))
                throw new System.NullReferenceException("文件不存在");
            else
            {
                System.IO.StreamReader f2 = new System.IO.StreamReader(path, System.Text.Encoding.GetEncoding(encod));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir">目录</param>
        public static void DirectoryCreate(string dir)
        {
            // 判断目标目录是否存在如果不存在则新建之
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">路径</param>
        public static void FileCreate(string path)
        {
            System.IO.FileInfo createFile = new System.IO.FileInfo(path); //创建文件 
            if (!createFile.Exists)
            {
                System.IO.FileStream fs = createFile.Create();
                fs.Close();
            }
        }
        /// <summary>
        /// 拷贝文件，如果目标文件存在则覆盖
        /// </summary>
        /// <param name="orignFile">原始文件</param>
        /// <param name="newFile">新文件路径</param>
        public static void FileCopy(string orignFile, string newFile)
        {
            string dir = System.IO.Path.GetDirectoryName(newFile);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            System.IO.File.Copy(orignFile, newFile, true);
        }
        /// <summary>
        /// 取文件后缀名
        /// </summary>
        /// <param name="filename">文件名（或路径）</param>
        /// <returns>后缀，如：.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)
        {
            int start = filename.LastIndexOf(".", StringComparison.Ordinal);
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }
        /// <summary>
        /// 判断目录是否合法
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns>true合法</returns>
        public static bool DirectoryLegality(string dir)
        {
            Regex reg = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
            return reg.IsMatch(dir);
        }
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!System.IO.Directory.Exists(dirPath))
                return 0;
            long len = 0;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dirPath);
            foreach (System.IO.FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            System.IO.DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
        /// <summary>
        /// 获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
        /// <returns></returns>
        public static string GetFileAttibe(string filePath)
        {
            string str = "";
            System.IO.FileInfo objFi = new System.IO.FileInfo(filePath);
            str += "详细路径:" + objFi.FullName + "\r\n文件名称:" + objFi.Name + "\r\n文件长度:" + objFi.Length + "字节\r\n创建时间" + objFi.CreationTime + "\r\n最后访问时间:" + objFi.LastAccessTime + "\r\n修改时间:" + objFi.LastWriteTime + "\r\n所在目录:" + objFi.DirectoryName + "\r\n扩展名:" + objFi.Extension;
            return str;
        }
        /// <summary>
        /// 判断文件编码
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>编码</returns>
        public static System.Text.Encoding GetFileEncodeType(string path)
        {
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    return System.Text.Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return System.Text.Encoding.Unicode;
                }
                else
                {
                    return System.Text.Encoding.Default;  //System.Text.Encoding.Default是指操作系统的当前 ANSI 代码页的编码
                }
            }
            else
            {
                return System.Text.Encoding.Default;
            }
        }

        /// <summary>
        /// 模糊搜索文件
        /// </summary>
        /// <param name="dirPath">文件夹目录</param>
        /// <param name="sea">搜索的关键词</param>
        /// <returns>所有文件路径</returns>
        public static List<string> SearchFile(string dirPath, string sea)
        {
            sea = "*" + string.Join("*", sea.ToCharArray()) + "*";
            List<string> fileList = new List<string>(System.IO.Directory.GetFiles(dirPath, sea));
            List<string> m_dir = FileHelper.FindAllDirectories(dirPath);
            foreach (string dir in m_dir)
            {
                List<string> l = new List<string>(System.IO.Directory.GetFiles(dir, sea));
                fileList = new List<string>(fileList.Union(l));
            }
            return fileList;
        }

        /// <summary>
        /// 文件路径处理
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>去除了特殊字符的文件名的文件路径</returns>
        public static string FilePathProcess(string filePath)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            fileName = FileNameProcess(fileName);
            string extension = System.IO.Path.GetExtension(filePath);
            string fileDir = System.IO.Path.GetDirectoryName(filePath);
            return System.IO.Path.Combine(fileDir, fileName + extension);
        }
        /// <summary>
        /// 文件名处理
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>去除了特殊字符的文件名</returns>
        public static string FileNameProcess(string fileName)
        {
            return fileName.Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
        }

        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 将 byte[] 转成 Stream 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        /// <summary>
        ///  将 Stream 写入文件 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filePath">文件路径</param>
        public static void StreamToFile(Stream stream, string filePath)
        {
            string fileDir = System.IO.Path.GetDirectoryName(filePath);
            DirectoryCreate(fileDir);

            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
        /// <summary>
        /// 从文件读取 Stream 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}