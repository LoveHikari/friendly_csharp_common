﻿
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hikari.Common.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;

namespace Hikari.Common
{
    /// <summary>
    /// 文件压缩与解压缩帮助类
    /// </summary>
    public class ZipLibHelper
    {
        /// <summary>  
        /// 生成 ***.tar.gz 文件  
        /// </summary>  
        /// <param name="sourceFolderPath">待压缩的源文件夹或文件路径</param>
        /// <param name="tgzFileDir">生成的文件路径</param>
        /// <param name="tgzFileName">生成的文件名称，不带扩展名</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public bool CreateTarGzArchive(string sourceFolderPath, string tgzFileDir, string tgzFileName = "temp",  string encoding = "utf-8")
        {
            if (!System.IO.Directory.Exists(tgzFileDir))
            {
                System.IO.Directory.CreateDirectory(tgzFileDir);
            }

            //string fileName;
            //if (File.Exists(sourceFolderPath))  // 如果是文件
            //{
            //    fileName = Path.GetFileNameWithoutExtension(sourceFolderPath);
            //}
            //else
            //{
            //    string tempStr = sourceFolderPath.Replace('\\', '/');
            //    Regex regex = new Regex("/(.+?)$", RegexOptions.RightToLeft);
            //    fileName = regex.Match(tempStr).Groups[1].Value;
            //}

            tgzFileName = Path.Combine(tgzFileDir, tgzFileName + ".tar.gz");

            using Stream outTmpStream = new FileStream(tgzFileName, FileMode.OpenOrCreate);

            //注意此处源文件大小大于4096KB  
            using Stream outStream = new GZipOutputStream(outTmpStream);
            using TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor, Encoding.GetEncoding(encoding));
            archive.RootPath = Path.GetDirectoryName(sourceFolderPath);
            TarEntry entry = TarEntry.CreateEntryFromFile(sourceFolderPath);
            archive.WriteEntry(entry, true);

            return true;
        }
        /// <summary>
        /// 文件解压
        /// </summary>
        /// <param name="zipPath">压缩文件路径***.tar.gz</param>
        /// <param name="goalDir">解压到的目录</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public bool UnzipTgz(string zipPath, string goalDir, string encoding = "utf-8")
        {
            using Stream inStream = File.OpenRead(zipPath);
            using Stream gzipStream = new GZipInputStream(inStream);
            using TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream, Encoding.GetEncoding(encoding));
            tarArchive.ExtractContents(goalDir);

            return true;
        }
        /// <summary>  
        /// 生成 ***.tar 文件  
        /// </summary>  
        /// <param name="sourceFolderPath">待压缩的源文件夹或文件路径</param>
        /// <param name="tarFileDir">生成的文件路径</param>
        /// <param name="tarFileName">生成的文件名称，不带扩展名</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public bool CreateTarArchive(string sourceFolderPath, string tarFileDir, string tarFileName = "temp", string encoding = "utf-8")
        {
            if (!System.IO.Directory.Exists(tarFileDir))
            {
                System.IO.Directory.CreateDirectory(tarFileDir);
            }

            tarFileName = Path.Combine(tarFileDir, tarFileName + ".tar");

            using Stream outStream = new FileStream(tarFileName, FileMode.OpenOrCreate);

            using TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor, Encoding.GetEncoding(encoding));
            archive.RootPath = Path.GetDirectoryName(sourceFolderPath);
            TarEntry entry = TarEntry.CreateEntryFromFile(sourceFolderPath);
            archive.WriteEntry(entry, true);

            return true;
        }
        /// <summary>
        /// 生成tar文件
        /// </summary>
        /// <param name="listFilesPath">文件的路径：H:\Demo\xxx.txt</param>
        /// <param name="tarFileDir">生成的文件路径</param>
        /// <param name="tarFileName">生成的文件名称，不带扩展名</param>
        /// <param name="encoding">编码</param>
        /// <returns>压缩后的文件路径//"20180524" + ".tar"</returns>
        public string CreatTarArchive(List<string> listFilesPath, string tarFileDir,  string tarFileName = "temp", string encoding = "utf-8")
        {
            if (!System.IO.Directory.Exists(tarFileDir))
            {
                System.IO.Directory.CreateDirectory(tarFileDir);
            }

            tarFileName = Path.Combine(tarFileDir, tarFileName + ".tar");

            using Stream outStream = new FileStream(tarFileName, FileMode.OpenOrCreate);  //打开.tar文件


            using TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor, Encoding.GetEncoding(encoding));
            archive.RootPath = Path.GetDirectoryName(listFilesPath[0]);
            foreach (var fileName in listFilesPath)
            {
                TarEntry entry = TarEntry.CreateEntryFromFile(fileName);//将文件写到.tar文件中去
                archive.WriteEntry(entry, true);
            }

            return tarFileName;
        }
        /// <summary>
        /// tar文件解压
        /// </summary>
        /// <param name="zipPath">压缩文件路径***.tar</param>
        /// <param name="goalDir">解压到的目录</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public bool UnzipTar(string zipPath, string goalDir, string encoding = "utf-8")
        {
            using Stream inStream = File.OpenRead(zipPath);
            using TarArchive tarArchive = TarArchive.CreateInputTarArchive(inStream, Encoding.GetEncoding(encoding));
            tarArchive.ExtractContents(goalDir);

            return true;
        }

        /// <summary>  
        /// zip压缩文件  
        /// </summary>  
        /// <param name="directory">要压缩的文件夹路径</param>
        /// <param name="zipFileDir">生成的文件路径</param>
        /// <param name="zipFileName">生成的文件名称，不带扩展名</param>
        /// <param name="compressionLevel">压缩级别</param>
        /// <param name="password">压缩密码</param>
        /// <param name="comment">压缩包注释</param>
        /// <returns></returns>  
        public bool CreateZipArchive(string directory, string zipFileDir, string zipFileName = "", int compressionLevel = 9, string password = "", string comment = "")
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var dirInfo = new DirectoryInfo(directory);

            if (string.IsNullOrEmpty(zipFileName))
            {
                zipFileName = dirInfo.Name;
            }

            zipFileName = Path.Combine(zipFileDir, zipFileName + ".zip");
            
            if (File.Exists(zipFileName))
            {
                File.Delete(zipFileName);
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("gbk");
            StringCodec.FromCodePage(encode.CodePage);

            using var zipOutput = new ZipOutputStream(File.Create(zipFileName));
            zipOutput.SetLevel(compressionLevel);
            zipOutput.Password = password;
            zipOutput.SetComment(comment);

            var buffer = new byte[4096];
            var compressFiles = DirectoryHelper.GetCompressFiles(directory);
            compressFiles.Add(directory);
            foreach (var item in compressFiles)
            {
                var isFilePath = File.Exists(item);
                var zipName = item.Replace(dirInfo.Parent.FullName, "");
                zipName = isFilePath ? zipName : string.Format($"{zipName}/");
                var fileEntry = new ZipEntry(zipName)
                {
                    DateTime = DateTime.Now
                };
                zipOutput.PutNextEntry(fileEntry);
                if (!isFilePath) continue;
                using var fileStream = File.OpenRead(item);
                int sourceBytes;

                do
                {
                    sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                    zipOutput.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }


            //FastZip fz = new FastZip();
            //fz.CreateEmptyDirectories = true;
            //if (password != "")
            //{
            //    fz.Password = password;
            //}
            //fz.CreateZip(zipFileName, directory, true, "");
            //if (comment != "")
            //{
            //    using ZipFile zip = new ZipFile(zipFileName);
            //    zip.BeginUpdate();
            //    zip.SetComment(comment);
            //    zip.CommitUpdate();

            // }

            return true;
        }

        /// <summary>  
        /// zip解压文件  
        /// </summary>  
        /// <param name="strFilePath">压缩文件的名称，如：*.zip</param>
        /// <param name="dir">dir要解压的文件夹路径, 会自动带上文件名</param>
        /// <returns></returns>  
        public bool UnzipZip(string strFilePath, string dir)
        {
            if (!File.Exists(strFilePath))
                return false;

            dir = Path.Combine(dir, Path.GetFileNameWithoutExtension(strFilePath));

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using FileStream fr = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using ICSharpCode.SharpZipLib.Zip.ZipInputStream s = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(fr);
            ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);

                if (directoryName != String.Empty)
                    Directory.CreateDirectory(Path.Combine(dir, directoryName));

                if (fileName != String.Empty)
                {
                    using FileStream streamWriter = File.Create(Path.Combine(dir, theEntry.Name));

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }

            return true;
        }
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