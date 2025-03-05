# Hikari.Common
[![LICENSE](https://img.shields.io/badge/license-Anti%20996-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)
<img alt="dotnet-version" src="https://img.shields.io/badge/.net-%3E%3D6.0-blue.svg"></img>
<img alt="csharp-version" src="https://img.shields.io/badge/C%23-latest-blue.svg"></img>
<img alt="IDE-version" src="https://img.shields.io/badge/IDE-vs2022-blue.svg"></img>
[![MIT Licence](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/mit-license.php)
<a href="https://github.com/LoveHikari/friendly_csharp_common"><img src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/95/Font_Awesome_5_brands_github.svg/54px-Font_Awesome_5_brands_github.svg.png" height="24"><img src="https://upload.wikimedia.org/wikipedia/commons/thumb/2/29/GitHub_logo_2013.svg/128px-GitHub_logo_2013.svg.png" height="24"></a>

<p align="center">
    <a href="https://github.com/LoveHikari/friendly_csharp_common/blob/master/README.md">中文</a>
    ❤
    <a href="https://github.com/LoveHikari/friendly_csharp_common/blob/master/README.en.md">English</a>
	❤
    <a href="https://github.com/LoveHikari/friendly_csharp_common/blob/master/README.jp.md">日本語</a>
</p>

A universal C#.NET utility library friendly for all ages and skill levels—whether you're a beginner or an advanced user, you can easily get started. This library includes several commonly used utility classes, most of which are static, covering tasks such as encryption and decryption, reflection operations, tree structures, file detection, weighted random selection algorithms, distributed short IDs, expression trees, LINQ extensions, file compression, multithreaded downloads, hardware information, string extension methods, date-time extensions, Chinese lunar calendar, large file copying, image cropping, CAPTCHA, breakpoint resume, collection extensions, and Excel export, among other commonly used encapsulations.

**With numerous features all in one, the total code size is under 2MB!**

Project development model: daily code accumulation + online research.

⭐⭐⭐If you like this project, don't forget to give it a Star, Fork, and Follow!⭐⭐⭐

If you have any questions about this project or encounter any issues during use, feel free to open an issue or contact me privately. I will provide you with completely free technical support. Of course, if you feel uncomfortable accepting free guidance, any kind of tip or donation will be graciously accepted!🤣🤣🤣

## Please Note：
If any company that uses this open-source project or incorporates its code into their products is found guilty in any legal lawsuit due to violations of labor laws (including but not limited to illegal layoffs, excessive working hours, child labor, etc.), the author of this project reserves the right to claim a usage fee for the project (**2-5 times the company's registered capital as the licensing fee**), or to prohibit the use of any source code containing this project! `Labor outsourcing companies` or `007 companies` that wish to use this library must contact the author for commercial licensing! Other enterprises or individuals may freely use it without restriction. The term "007" refers to employing people as tools, which is unproductive. The 8-hour workday is essential for self-improvement and future competitiveness. Opposing the "007" mentality is everyone's responsibility!

## Recommended Development Environment
Operating System: Windows 11 23H2 or above

Development Tool: Visual Studio 2022 v17.8 or above

SDK: .NET 6.0 or above (all versions)

## Installation Package
.NET 6.0 or above
```shell
PM> Install-Package Hikari.Common
```

## 特色功能示例代码
### 1.检验字符串是否是Email、手机号、URL、IP地址、身份证号等
```csharp
bool isEmail="3444764617@qq.com".MatchEmail(); // 可在appsetting.json中添加EmailDomainWhiteList和EmailDomainBlockList配置邮箱域名黑白名单，逗号分隔，如"EmailDomainBlockList": "^\\w{1,5}@qq.com,^\\w{1,5}@163.com,^\\w{1,5}@gmail.com,^\\w{1,5}@outlook.com",
bool isInetAddress = "114.114.114.114".MatchInetAddress();
bool isUrl = "http://masuit.com".MatchUrl();
bool isPhoneNumber = "15205201520".MatchPhoneNumber();
bool isIdentifyCard = "312000199502230660".MatchIdentifyCard();// 校验中国大陆身份证号
bool isCNPatentNumber = "200410018477.9".MatchCNPatentNumber(); // 校验中国专利申请号或专利号，是否带校验位，校验位前是否带“.”，都可以校验，待校验的号码前不要带CN、ZL字样的前缀
```

### 2.硬件监测(仅支持Windows)
```csharp
float load = SystemInfo.CpuLoad;// 获取CPU占用率
long physicalMemory = SystemInfo.PhysicalMemory;// 获取物理内存总数
long memoryAvailable = SystemInfo.MemoryAvailable;// 获取物理内存可用率
double freePhysicalMemory = SystemInfo.GetFreePhysicalMemory();// 获取可用物理内存
Dictionary<string, string> diskFree = SystemInfo.DiskFree();// 获取磁盘每个分区可用空间
Dictionary<string, string> diskTotalSpace = SystemInfo.DiskTotalSpace();// 获取磁盘每个分区总大小
Dictionary<string, double> diskUsage = SystemInfo.DiskUsage();// 获取磁盘每个分区使用率
double temperature = SystemInfo.GetCPUTemperature();// 获取CPU温度
int cpuCount = SystemInfo.GetCpuCount();// 获取CPU核心数
IList<string> ipAddress = SystemInfo.GetIPAddress();// 获取本机所有IP地址
string localUsedIp = SystemInfo.GetLocalUsedIP();// 获取本机当前正在使用的IP地址
IList<string> macAddress = SystemInfo.GetMacAddress();// 获取本机所有网卡mac地址
string osVersion = SystemInfo.GetOsVersion();// 获取操作系统版本
RamInfo ramInfo = SystemInfo.GetRamInfo();// 获取内存信息
var cpuSN=SystemInfo.GetCpuInfo()[0].SerialNumber; // CPU序列号
var driveSN=SystemInfo.GetDiskInfo()[0].SerialNumber; // 硬盘序列号
```

## 代码包含
DateTime扩展类
DateTime帮助类

## License
[MIT](https://github.com/LoveHikari/friendly_csharp_common/blob/master/LICENSE)
