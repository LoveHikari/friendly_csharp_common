using System.Text.RegularExpressions;
/******************************************************************************************************************
 * 
 * 
 * 标  题：目录帮助类(版本：Version1.0.0)
 * 作  者：YuXiaoWei
 * 日  期：2021/12/15
 * 修  改：
 * 参  考：
 * 说  明：暂无...
 * 备  注：暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common.IO;

/// <summary>
/// 目录帮助类
/// </summary>
public class DirectoryHelper
{
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
        foreach (System.IO.DirectoryInfo d in dir.GetDirectories()) //查找子目录
        {
            filePathList.Add(d.FullName);
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
        foreach (System.IO.DirectoryInfo d in dir.GetDirectories()) //查找子目录
        {
            filePathList.Add(d.FullName);
            FindAllDirectories(d.FullName, filePathList);

        }
        return filePathList;
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
    /// 移动并重命名文件夹
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
    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="dir">目录</param>
    public static void Create(string dir)
    {
        // 判断目标目录是否存在如果不存在则新建之
        if (!System.IO.Directory.Exists(dir))
            System.IO.Directory.CreateDirectory(dir);
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
            filePathList.AddRange(dir.GetFiles(extension).Select(f => f.FullName));
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

        System.IO.FileInfo[] allFile = dir.GetFiles(extension);
        filePathList.AddRange(allFile.Select(fi => fi.FullName));

        System.IO.DirectoryInfo[] allDir = dir.GetDirectories();
        foreach (System.IO.DirectoryInfo d in allDir)
        {
            GetAll(d.FullName, extension, filePathList);
        }
        return filePathList;
    }
    /// <summary>
    /// 获得目录下所有子目录和子文件
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static List<string> GetCompressFiles(string dirPath)
    {
        var compressFiles = new List<string>();
        compressFiles.AddRange(Directory.EnumerateDirectories(
            dirPath, "*.*", SearchOption.AllDirectories));
        compressFiles.AddRange(Directory.EnumerateFiles(
            dirPath, "*.*", SearchOption.AllDirectories)
        );

        return compressFiles;
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
        foreach (System.IO.DirectoryInfo d in dir.GetDirectories()) //查找子目录
        {
            filePathList.AddRange(FindFile(d.FullName, extension));
        }
        foreach (System.IO.FileInfo f in dir.GetFiles(extension)) //查找文件
        {
            filePathList.Add(f.FullName);
        }
        return filePathList;
    }

    #endregion

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
        List<string> m_dir = FindAllDirectories(dirPath);
        foreach (string dir in m_dir)
        {
            List<string> l = new List<string>(System.IO.Directory.GetFiles(dir, sea));
            fileList = new List<string>(fileList.Union(l));
        }
        return fileList;
    }
}