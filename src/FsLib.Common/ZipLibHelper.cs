
/******************************************************************************************************************
 * 
 * 
 * 标  题：文件压缩与解压缩帮助类(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2021/03/30
 * 修　改：
 * 参　考：https://www.cnblogs.com/vichin/p/9071239.html
 * 说　明：https://github.com/icsharpcode/SharpZipLib
 * 备　注：暂无...
 * 
 * 
 * ***************************************************************************************************************/

using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;

namespace System
{
    /// <summary>
    /// 文件压缩与解压缩帮助类
    /// </summary>
    public class ZipLibHelper
    {
        ///// <summary>  
        ///// 生成 ***.tar.gz 文件  
        ///// </summary>  
        ///// <param name="strBasePath">文件基目录（源文件、生成文件所在目录）</param>  
        ///// <param name="strSourceFolderName">待压缩的源文件夹名</param>  
        //public bool CreatTarGzArchive(string strBasePath, string strSourceFolderName)
        //{
        //    if (string.IsNullOrEmpty(strBasePath)
        //        || string.IsNullOrEmpty(strSourceFolderName)
        //        || !System.IO.Directory.Exists(strBasePath)
        //        || !System.IO.Directory.Exists(Path.Combine(strBasePath, strSourceFolderName)))
        //    {
        //        return false;
        //    }

        //    Environment.CurrentDirectory = strBasePath;
        //    string strSourceFolderAllPath = Path.Combine(strBasePath, strSourceFolderName);
        //    string strOupFileAllPath = Path.Combine(strBasePath, strSourceFolderName + ".tar.gz");

        //    Stream outTmpStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);

        //    //注意此处源文件大小大于4096KB  
        //    Stream outStream = new GZipOutputStream(outTmpStream);
        //    TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
        //    TarEntry entry = TarEntry.CreateEntryFromFile(strSourceFolderAllPath);
        //    archive.WriteEntry(entry, true);

        //    archive.Close();

        //    outTmpStream.Close();
        //    outStream.Close();

        //    return true;
        //}
        ///// <summary>
        ///// 文件解压
        ///// </summary>
        ///// <param name="zipPath">压缩文件路径</param>
        ///// <param name="goalFolder">解压到的目录</param>
        ///// <returns></returns>
        //public static bool UnzipTgz(string zipPath, string goalFolder)
        //{
        //    Stream inStream = null;
        //    Stream gzipStream = null;
        //    TarArchive tarArchive = null;
        //    try
        //    {
        //        using (inStream = File.OpenRead(zipPath))
        //        {
        //            using (gzipStream = new GZipInputStream(inStream))
        //            {
        //                tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
        //                tarArchive.ExtractContents(goalFolder);
        //                tarArchive.Close();
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("压缩出错！");
        //        return false;
        //    }
        //    finally
        //    {
        //        if (null != tarArchive) tarArchive.Close();
        //        if (null != gzipStream) gzipStream.Close();
        //        if (null != inStream) inStream.Close();
        //    }
        //}
        ///// <summary>  
        ///// 生成 ***.tar 文件  
        ///// </summary>  
        ///// <param name="strBasePath">文件基目录（源文件、生成文件所在目录）</param>  
        ///// <param name="strSourceFolderName">待压缩的源文件夹名</param>  
        //public bool CreatTarArchive(string strBasePath, string strSourceFolderName)
        //{
        //    if (string.IsNullOrEmpty(strBasePath)
        //        || string.IsNullOrEmpty(strSourceFolderName)
        //        || !System.IO.Directory.Exists(strBasePath)
        //        || !System.IO.Directory.Exists(Path.Combine(strBasePath, strSourceFolderName)))
        //    {
        //        return false;
        //    }

        //    Environment.CurrentDirectory = strBasePath;
        //    string strSourceFolderAllPath = Path.Combine(strBasePath, strSourceFolderName);
        //    string strOupFileAllPath = Path.Combine(strBasePath, strSourceFolderName + ".tar");

        //    Stream outStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);

        //    TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
        //    TarEntry entry = TarEntry.CreateEntryFromFile(strSourceFolderAllPath);
        //    archive.WriteEntry(entry, true);

        //    if (archive != null)
        //    {
        //        archive.Close();
        //    }

        //    outStream.Close();

        //    return true;
        //}
        ///// <summary>
        ///// 生成tar文件
        ///// </summary>
        ///// <param name="strBasePath">文件夹路径——将被压缩的文件所在的地方</param>        
        ///// <param name="listFilesPath">文件的路径：H:\Demo\xxx.txt</param>
        ///// <param name="tarFileName">压缩后tar文件名称</param>
        ///// <returns></returns>
        //public static bool CreatTarArchive(string strBasePath, List<string> listFilesPath, string tarFileName)//"20180524" + ".tar"
        //{
        //    if (string.IsNullOrEmpty(strBasePath) || string.IsNullOrEmpty(tarFileName) || !System.IO.Directory.Exists(strBasePath))
        //        return false;

        //    Environment.CurrentDirectory = strBasePath;
        //    string strOupFileAllPath = strBasePath + tarFileName;//一个完整的文件路径 .tar
        //    Stream outStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);//打开.tar文件
        //    TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);

        //    for (int i = 0; i < listFilesPath.Count; i++)
        //    {
        //        string fileName = listFilesPath[i];
        //        TarEntry entry = TarEntry.CreateEntryFromFile(fileName);//将文件写到.tar文件中去
        //        archive.WriteEntry(entry, true);
        //    }

        //    if (archive != null)
        //    {
        //        archive.Close();
        //    }

        //    outStream.Close();

        //    return true;
        //}
        ///// <summary>  
        ///// tar包解压  
        ///// </summary>  
        ///// <param name="strFilePath">tar包路径</param>  
        ///// <param name="strUnpackDir">解压到的目录</param>  
        ///// <returns></returns>  
        //public static bool UnpackTarFiles(string strFilePath, string strUnpackDir)
        //{
        //    try
        //    {
        //        if (!File.Exists(strFilePath))
        //        {
        //            return false;
        //        }

        //        strUnpackDir = strUnpackDir.Replace("/", "\\");
        //        if (!strUnpackDir.EndsWith("\\"))
        //        {
        //            strUnpackDir += "\\";
        //        }

        //        if (!Directory.Exists(strUnpackDir))
        //        {
        //            Directory.CreateDirectory(strUnpackDir);
        //        }

        //        FileStream fr = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //        DhcEc.SharpZipLib.Tar.TarInputStream s = new DhcEc.SharpZipLib.Tar.TarInputStream(fr);
        //        DhcEc.SharpZipLib.Tar.TarEntry theEntry;
        //        while ((theEntry = s.GetNextEntry()) != null)
        //        {
        //            string directoryName = Path.GetDirectoryName(theEntry.Name);
        //            string fileName = Path.GetFileName(theEntry.Name);

        //            if (directoryName != String.Empty)
        //                Directory.CreateDirectory(strUnpackDir + directoryName);

        //            if (fileName != String.Empty)
        //            {
        //                FileStream streamWriter = File.Create(strUnpackDir + theEntry.Name);

        //                int size = 2048;
        //                byte[] data = new byte[2048];
        //                while (true)
        //                {
        //                    size = s.Read(data, 0, data.Length);
        //                    if (size > 0)
        //                    {
        //                        streamWriter.Write(data, 0, size);
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }

        //                streamWriter.Close();
        //            }
        //        }
        //        s.Close();
        //        fr.Close();

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        ///// <summary>  
        ///// zip压缩文件  
        ///// </summary>  
        ///// <param name="filename">filename生成的文件的名称，如：C\123\123.zip</param>  
        ///// <param name="directory">directory要压缩的文件夹路径</param>  
        ///// <returns></returns>  
        //public static bool PackFiles(string filename, string directory)
        //{
        //    try
        //    {
        //        directory = directory.Replace("/", "\\");

        //        if (!directory.EndsWith("\\"))
        //            directory += "\\";
        //        if (!Directory.Exists(directory))
        //        {
        //            Directory.CreateDirectory(directory);
        //        }
        //        if (File.Exists(filename))
        //        {
        //            File.Delete(filename);
        //        }

        //        FastZip fz = new FastZip();
        //        fz.CreateEmptyDirectories = true;
        //        fz.CreateZip(filename, directory, true, "");

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        ///// <summary>  
        ///// zip解压文件  
        ///// </summary>  
        ///// <param name="file">压缩文件的名称，如：C:\123\123.zip</param>  
        ///// <param name="dir">dir要解压的文件夹路径</param>  
        ///// <returns></returns>  
        //public static bool UnpackFiles(string file, string dir)
        //{
        //    try
        //    {
        //        if (!File.Exists(file))
        //            return false;

        //        dir = dir.Replace("/", "\\");
        //        if (!dir.EndsWith("\\"))
        //            dir += "\\";

        //        if (!Directory.Exists(dir))
        //            Directory.CreateDirectory(dir);

        //        FileStream fr = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //        ICSharpCode.SharpZipLib.Zip.ZipInputStream s = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(fr);
        //        ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry;
        //        while ((theEntry = s.GetNextEntry()) != null)
        //        {
        //            string directoryName = Path.GetDirectoryName(theEntry.Name);
        //            string fileName = Path.GetFileName(theEntry.Name);

        //            if (directoryName != String.Empty)
        //                Directory.CreateDirectory(dir + directoryName);

        //            if (fileName != String.Empty)
        //            {
        //                FileStream streamWriter = File.Create(dir + theEntry.Name);

        //                int size = 2048;
        //                byte[] data = new byte[2048];
        //                while (true)
        //                {
        //                    size = s.Read(data, 0, data.Length);
        //                    if (size > 0)
        //                    {
        //                        streamWriter.Write(data, 0, size);
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }

        //                streamWriter.Close();
        //            }
        //        }
        //        s.Close();
        //        fr.Close();

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        ///// <summary>
        ///// 压缩文件夹
        ///// </summary>
        ///// <param name="DirectoryToZip">需要压缩的文件夹（绝对路径)</param>
        ///// <param name="ZipedPath">压缩后的文件路径（绝对路径）</param>
        ///// <param name="ZipedFileName">>压缩后的文件名称（文件名，默认 同源文件夹同名）</param>
        //public static void ZipDirectory(string DirectoryToZip, string ZipedPath, string ZipedFileName = "")
        //{
        //    //如果目录不存在，则报错
        //    if (!System.IO.Directory.Exists(DirectoryToZip))
        //    {
        //        throw new System.IO.FileNotFoundException("指定的目录: " + DirectoryToZip + " 不存在!");
        //    }
        //    //文件名称（默认同源文件名称相同）
        //    string ZipFileName = string.IsNullOrEmpty(ZipedFileName) ? ZipedPath + "\\" + new DirectoryInfo(DirectoryToZip).Name + ".zip" : ZipedPath + "\\" + ZipedFileName + ".zip";
        //    using (System.IO.FileStream ZipFile = System.IO.File.Create(ZipFileName))
        //    {
        //        using (ZipOutputStream s = new ZipOutputStream(ZipFile))
        //        {
        //            ZipSetp(DirectoryToZip, s, "");
        //        }
        //    }
        //}
        ///// <summary>
        ///// 递归遍历目录        
        ///// </summary>
        //private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        //{
        //    if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
        //    {
        //        strDirectory += Path.DirectorySeparatorChar;
        //    }
        //    Crc32 crc = new Crc32();
        //    string[] filenames = Directory.GetFileSystemEntries(strDirectory);
        //    foreach (string file in filenames)// 遍历所有的文件和目录
        //    {
        //        if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
        //        {
        //            string pPath = parentPath;
        //            pPath += file.Substring(file.LastIndexOf("\\") + 1);
        //            pPath += "\\";
        //            ZipSetp(file, s, pPath);
        //        }
        //        else // 否则直接压缩文件
        //        {
        //            //打开压缩文件
        //            using (FileStream fs = File.OpenRead(file))
        //            {
        //                byte[] buffer = new byte[fs.Length];
        //                fs.Read(buffer, 0, buffer.Length);
        //                string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
        //                ZipEntry entry = new ZipEntry(fileName);
        //                entry.DateTime = DateTime.Now;
        //                entry.Size = fs.Length;
        //                fs.Close();
        //                crc.Reset();
        //                crc.Update(buffer);
        //                entry.Crc = crc.Value;
        //                s.PutNextEntry(entry);
        //                s.Write(buffer, 0, buffer.Length);
        //            }
        //        }
        //    }
        //}
    }
}