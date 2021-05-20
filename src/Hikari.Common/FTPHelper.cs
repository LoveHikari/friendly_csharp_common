using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Hikari.Common
{
    /// <summary>
    /// ftp操作类
    /// </summary>
    public class FTPHelper
    {
        #region 字段
        /// <summary>
        /// ftp当前目录地址
        /// </summary>
        private string _ftpUri;
        /// <summary>
        /// 用户名
        /// </summary>
        private readonly string _ftpUserId;
        /// <summary>
        /// FTP连接地址
        /// </summary>
        private readonly string _ftpServerIp;
        /// <summary>
        /// 密码
        /// </summary>
        private readonly string _ftpPassword;
        /// <summary>
        /// FTP连接成功后的当前目录
        /// </summary>
        private string _ftpRemotePath;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ftpServerIp">FTP连接地址，可以带端口</param>
        /// <param name="ftpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录，前后不需要/</param>
        /// <param name="ftpUserId">用户名</param>
        /// <param name="ftpPassword">密码</param>
        public FTPHelper(string ftpServerIp, string ftpRemotePath, string ftpUserId, string ftpPassword)
        {
            _ftpServerIp = ftpServerIp;
            _ftpRemotePath = ftpRemotePath;
            _ftpUserId = ftpUserId;
            _ftpPassword = ftpPassword;
            _ftpUri = "ftp://" + _ftpServerIp + "/" + _ftpRemotePath + "/";
        }

        /// <summary>
        /// 上传文件到当前目录
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        public void Upload(string localFilePath)
        {
            FileInfo fileInfo = new FileInfo(localFilePath);
            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpUri + fileInfo.Name));
            reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            reqFtp.KeepAlive = false;
            reqFtp.UseBinary = true;
            reqFtp.ContentLength = fileInfo.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            try
            {
                using (FileStream fs = fileInfo.OpenRead())
                {

                    using (Stream strm = reqFtp.GetRequestStream())
                    {
                        int contentLen = fs.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        /// <summary>
        /// 下载当前目录的文件
        /// </summary>
        /// <param name="localFileDir">要保存到的本地文件目录</param>
        /// <param name="fileName">要下载文件名</param>
        public void Download(string localFileDir, string fileName)
        {
            try
            {
                using (FileStream outputStream = new FileStream(System.IO.Path.Combine(localFileDir, fileName), FileMode.Create))
                {
                    FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpUri + fileName));
                    reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                    reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFtp.UseBinary = true;
                    using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
                    {
                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            long cl = response.ContentLength;
                            int bufferSize = 2048;
                            byte[] buffer = new byte[bufferSize];
                            int readCount = ftpStream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                outputStream.Write(buffer, 0, readCount);
                                readCount = ftpStream.Read(buffer, 0, bufferSize);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除当前目录的文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void Delete(string fileName)
        {
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpUri + fileName));
                reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFtp.KeepAlive = false;
                using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
                {
                    long size = response.ContentLength;
                    using (Stream datastream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(datastream))
                        {
                            string result = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        public string[] GetFilesDetailList()
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpUri));
                ftp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return result.ToString().Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取FTP文件列表(包括文件夹)
        /// </summary>
        /// <param name="requedstPath">服务器下的相对路径，不需要首尾的/</param>
        /// <returns></returns>
        public string[] GetAllList(string requedstPath)
        {
            string url = "ftp://" + _ftpServerIp + "/" + requedstPath + "/";  //目标路径
            List<string> list = new List<string>();
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(url));
            req.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            req.UseBinary = true;
            req.UsePassive = true;
            try
            {
                using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            list.Add(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取当前目录下文件列表(不包括文件夹)  
        /// </summary>
        public string[] GetFileList()
        {
            StringBuilder result = new StringBuilder();
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpUri));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {

                    if (line.IndexOf("<DIR>",StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        string[] ss = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        result.Append(ss[ss.GetUpperBound(0)]);
                        //result.Append(Regex.Match(line, @"[\S]+ [\S]+", RegexOptions.IgnoreCase).Value.Split(' ')[1]);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result.ToString().Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 判断当前目录下指定的文件是否存在  
        /// </summary>
        /// <param name="remoteFileName">远程文件名</param>  
        public bool FileExist(string remoteFileName)
        {
            string[] fileList = GetFileList();
            foreach (string str in fileList)
            {
                if (str.Trim() == remoteFileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 在当前目录创建文件夹
        /// </summary>
        /// <param name="dirName">文件夹名</param>
        public void MakeDir(string dirName)
        {
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前目录指定文件大小
        /// </summary>
        /// <param name="filename">文件名</param>
        public long GetFileSize(string filename)
        {
            long fileSize;
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri + filename));
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return fileSize;
        }

        /// <summary>
        /// 更改当前目录的文件的文件名  
        /// </summary>
        /// <param name="currentFilename">原文件名</param>
        /// <param name="newFilename">新文件名</param>
        public void ReName(string currentFilename, string newFilename)
        {
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri + currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 移动当前目录的文件到指定目录
        /// </summary>
        /// <param name="currentFilename">原文件名</param>
        /// <param name="newDirectory">新目录</param>
        public void MovieFile(string currentFilename, string newDirectory)
        {
            ReName(currentFilename, newDirectory);
        }

        /// <summary>
        /// 切换当前目录
        /// </summary>
        /// <param name="dir">远程目录</param>
        /// <param name="isRoot">true:绝对路径 false:相对路径</param>
        public void GotoDirectory(string dir, bool isRoot)
        {
            if (isRoot)
            {
                _ftpRemotePath = dir;
            }
            else
            {
                _ftpRemotePath += dir + "/";
            }
            _ftpUri = "ftp://" + _ftpServerIp + "/" + _ftpRemotePath + "/";
        }
    }
}