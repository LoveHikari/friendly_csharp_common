using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using FluentFTP;

namespace Hikari.Common
{
    /// <summary>
    /// ftp操作类
    /// </summary>
    public class FTPHelper
    {
        #region 字段

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
        private AsyncFtpClient _client;
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
            _client = new AsyncFtpClient(ftpServerIp, ftpUserId, ftpPassword);


        }
        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            // 连接到服务器，并设置自动重连
            await _client.AutoConnect();
        }
        /// <summary>
        /// 上传文件到当前目录
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        public async Task<bool> UploadFileAsync(string localFilePath)
        {
            var remoteFilePath = Path.Combine(_ftpRemotePath, Path.GetFileName(localFilePath));
            if (await _client.FileExists(remoteFilePath))
            {
                var status = await _client.UploadFile(localFilePath, remoteFilePath);
                return status == FtpStatus.Success;
            }

            return false;

        }
        /// <summary>
        /// 上传文件夹到当前目录
        /// </summary>
        /// <param name="localFolder">本地文件夹路径</param>
        public async Task<bool> UploadDirectoryAsync(string localFolder)
        {
            var remoteFolder = Path.Combine(_ftpRemotePath, Path.GetDirectoryName(localFolder) ?? "");
            var status = await _client.UploadDirectory(@"C:\website\videos\", remoteFolder, FtpFolderSyncMode.Update);
            foreach (var ftpResult in status)
            {
                if (!ftpResult.IsSuccess)
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 下载当前目录的文件
        /// </summary>
        /// <param name="localFileDir">要保存到的本地文件目录</param>
        /// <param name="fileName">要下载文件名</param>
        public async Task<bool> DownloadFileAsync(string localFileDir, string fileName)
        {
            var localFilePath = Path.Combine(localFileDir, fileName);
            var remoteFilePath = Path.Combine(_ftpRemotePath, fileName);
            var status =  await _client.DownloadFile(localFilePath, remoteFilePath);
            return status == FtpStatus.Success;
        }

        /// <summary>
        /// 删除当前目录的文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var remoteFilePath = Path.Combine(_ftpRemotePath, fileName);
            if (await _client.FileExists(remoteFilePath))
            {
                await _client.DeleteFile(remoteFilePath);
                return true;
            }

            return false;
        }
        /// <summary>
        /// 删除当前目录
        /// </summary>
        public async Task<bool> DeleteDirectoryAsync()
        {
            if (await _client.DirectoryExists(_ftpRemotePath))
            {
                await _client.DeleteDirectory(_ftpRemotePath);
                return true;
            }

            return false;
        }
        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        public async Task<FtpListItem[]> GetListingAsync()
        {
            FtpListItem[] items = await _client.GetListing(_ftpRemotePath);
            return items;
        }


        /// <summary>
        /// 判断当前目录下指定的文件是否存在  
        /// </summary>
        /// <param name="remoteFileName">远程文件名</param>  
        public async Task<bool> FileExist(string remoteFileName)
        {
            var remoteFilePath = Path.Combine(_ftpRemotePath, remoteFileName);
            return await _client.FileExists(remoteFilePath);
        }


        /// <summary>
        /// 更改当前目录的文件的文件名  
        /// </summary>
        /// <param name="currentFilename">原文件名</param>
        /// <param name="newFilename">新文件名</param>
        public async Task<bool> ReNameAsync(string currentFilename, string newFilename)
        {
            var currentFilePath = Path.Combine(_ftpRemotePath, currentFilename);
            var newFilePath = Path.Combine(_ftpRemotePath, newFilename);
            return await _client.MoveFile(currentFilePath, newFilePath);

            
        }

        /// <summary>
        /// 移动当前目录的文件到指定目录
        /// </summary>
        /// <param name="currentFilename">原文件名</param>
        /// <param name="newDirectory">新目录</param>
        public async Task<bool> MoveFileAsync(string currentFilename, string newDirectory)
        {
            var currentFilePath = Path.Combine(_ftpRemotePath, currentFilename);
            var newFilePath = Path.Combine(newDirectory, currentFilename);
            return await _client.MoveFile(currentFilePath, newFilePath);
        }

        /// <summary>
        /// 切换当前目录
        /// </summary>
        /// <param name="dir">远程目录</param>
        /// <param name="isRoot">true:绝对路径 false:相对路径</param>
        public void GotoDirectory(string dir, bool isRoot = true)
        {
            if (isRoot)
            {
                _ftpRemotePath = dir;
            }
            else
            {
                _ftpRemotePath += dir + "/";
            }
        }
    }
}