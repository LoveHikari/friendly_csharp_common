# Hikari.Common
[![LICENSE](https://img.shields.io/badge/license-Anti%20996-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)
<img alt="dotnet-version" src="https://img.shields.io/badge/.net-%3E%3D8.0-blue.svg"></img>
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

全龄段友好的C#.NET万能工具库，不管你是菜鸟新手还是骨灰级玩家都能轻松上手，这个库包含一些常用的操作类，大都是静态类，加密解密，反射操作，树结构，文件探测，权重随机筛选算法，分布式短id，表达式树，linq扩展，文件压缩，多线程下载，硬件信息，字符串扩展方法，日期时间扩展操作，中国农历，大文件拷贝，图像裁剪，验证码，断点续传，集合扩展、Excel导出等常用封装。

**诸多功能集一身，代码量不到2MB！**

项目开发模式：日常代码积累+网络搜集

⭐⭐⭐喜欢这个项目的话就Star、Fork、Follow素质三连关♂注一下吧⭐⭐⭐

关于本项目，如果你有任何不懂的地方或使用过程中遇到任何问题，可以直接提issue或私信联系我，我会为你提供完全免费的技术指导，当然，如果你觉得不好意思接受免费的指导，想适当打赏我也是不会拒绝的！🤣🤣🤣

## 请注意：
一旦使用本开源项目以及引用了本项目或包含本项目代码的公司因为违反劳动法（包括但不限定非法裁员、超时用工、雇佣童工等）在任何法律诉讼中败诉的，一经发现，本项目作者有权利追讨本项目的使用费（**公司工商注册信息认缴金额的2-5倍作为本项目的授权费**），或者直接不允许使用任何包含本项目的源代码！ `人力外包公司`或 `007公司`需要使用本类库，请联系作者进行商业授权！其他企业或个人可随意使用不受限。007那叫用人，也是废人。8小时工作制才可以让你有时间自我提升，将来有竞争力。反对007，人人有责！

## 建议开发环境
操作系统：Windows 11 23H2及以上版本

开发工具：VisualStudio2022 v17.8及以上版本

SDK：.Net 8.0及以上**所有版本**

## 安装程序包
.NET 8.0以上
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

### 44. 真实文件类型探测/文本编码检测

```csharp
var encoding=new FileInfo(filepath).GetEncoding(); // 获取文件编码(扩展调用)
var encoding=stream.GetEncoding(); // 获取流的编码(扩展调用)
var encoding=TextEncodingDetector.GetEncoding(filepath); // 获取文件编码(类调用)

// 多种方式，任君调用
var detector=new FileInfo(filepath).DetectFiletype(); // 扩展调用
//var detector=File.OpenRead(filepath).DetectFiletype(); // 流扩展调用
//var detector=FileSignatureDetector.DetectFiletype(filepath); // 类调用

detector.Precondition;//基础文件类型
detector.Extension;//真实扩展名
detector.MimeType;//MimeType
detector.FormatCategories;//格式类别
```

#### 默认支持的文件类型

|   扩展名   |                              说明                              |
| :--------: | :-------------------------------------------------------------: |
|    3GP    |                          3GPP, 3GPP 2                          |
|     7Z     |                              7-Zip                              |
|    APK    |                    ZIP based Android Package                    |
|    AVI    |                     Audio-Video Interleave                     |
|     SH     |                          Shell Script                          |
|   BPLIST   |                      Binary Property List                      |
|  BMP, DIB  |                             Bitmap                             |
|    BZ2    |                       Bunzip2 Compressed                       |
|    CAB    |                        Microsoft Cabinet                        |
|   CLASS   |                          Java Bytecode                          |
|   CONFIG   |                     .NET Configuration File                     |
| CRT, CERT |                           Certificate                           |
|    CUR    |                             Cursor                             |
|     DB     |              Windows Thumbs.db Thumbnail Database              |
|    DDS    |                       DirectDraw Surface                       |
|    DLL    |                 Windows Dynamic Linkage Library                 |
|    DMG    |                     Apple Disk Mount Image                     |
|    DMP    |                    Windows Memory Dump File                    |
|    DOC    |             Microsoft Office Word 97-2003 Document             |
|    DOCX    |             Microsoft Office Word OpenXML Document             |
|    EPUB    |                         e-Pub Document                         |
|    EXE    |                        Windows Executive                        |
|    FLAC    |                         Loseless Audio                         |
|    FLV    |                           Flash Video                           |
|    GIF    |                   Graphics Interchage Format                   |
|     GZ     |                          GZ Compressed                          |
|    HDP    |                     HD Photo(JPEG XR) Image                     |
|    HWP    |                   Legacy HWP, HWPML, CFBF HWP                   |
|    ICO    |                              Icon                              |
|    INI    |                       Initialization File                       |
|    ISO    |                       ISO-9660 Disc Image                       |
|    LNK    |                      Windows Shortcut Link                      |
|    JP2    |                         JPEG 2000 Image                         |
| JPG, JPEG |             Joint Photographic Experts Group Image             |
|    LZH    |                         LZH Compressed                         |
|    M4A    |               MP4 Container Contained Audio Only               |
|    M4V    |                  MP4 Container Contained Video                  |
|    MID    |                           Midi Sound                           |
|    MKA    |             Matroska Container Contained Audio Only             |
|    MKV    |               Matroska Container Contained Video               |
|    MOV    |                      QuickTime Movie Video                      |
|    MP4    |                MP4 Container Contained Contents                |
|    MSI    |                       Microsoft Installer                       |
|    OGG    |                       OGG Video or Audio                       |
|    ODF    |                      OpenDocument Formula                      |
|    ODG    |                      OpenDocument Graphics                      |
|    ODP    |                    OpenDocument Presentation                    |
|    ODS    |                    OpenDocument Spreadsheet                    |
|    ODT    |                        OpenDocument Text                        |
|    PAK    |                  PAK Archive or Quake Archive                  |
|    PDB    |                   Microsoft Program Database                   |
|    PDF    |                    Portable Document Format                    |
|    PFX    |       Microsoft Personal Information Exchange Certificate       |
|    PNG    |                 Portable Network Graphics Image                 |
|    PPT    |          Microsoft Office PowerPoint 97-2003 Document          |
|    PPTX    |          Microsoft Office PowerPoint OpenXML Document          |
|    PPSX    | Microsoft Office PowerPoint OpenXML Document for Slideshow only |
|    PSD    |                       Photoshop Document                       |
|    RAR    |                        WinRAR Compressed                        |
|    REG    |                        Windows Registry                        |
|    RPM    |                 RedHat Package Manager Package                 |
|    RTF    |                    Rich Text Format Document                    |
|    SLN    |                Microsoft Visual Studio Solution                |
|    SRT    |                         SubRip Subtitle                         |
|    SWF    |                         Shockwave Flash                         |
| SQLITE, DB |                         SQLite Database                         |
|    TAR    |             pre-ISO Type and UStar Type TAR Package             |
|    TIFF    |                 Tagged Image File Format Image                 |
|    TXT    |                           Plain Text                           |
|    WAV    |                           Wave Audio                           |
|    WASM    |                       Binary WebAssembly                       |
|    WEBM    |                           WebM Video                           |
|    WEBP    |                           WebP Image                           |
|    XAR    |                           XAR Package                           |
|    XLS    |             Microsoft Office Excel 97-2003 Document             |
|    XLSX    |             Microsoft Office Excep OpenXML Document             |
|    XML    |               Extensible Markup Language Document               |
|     Z     |                          Z Compressed                          |
|    ZIP    |                           ZIP Package                           |

## 代码包含
DateTime扩展类
DateTime帮助类

## License
[MIT](https://github.com/LoveHikari/friendly_csharp_common/blob/master/LICENSE)
