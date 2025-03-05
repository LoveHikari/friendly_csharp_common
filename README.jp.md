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

すべての年齢層に優しい C#.NET ユニバーサル ツール ライブラリ。初心者でも熱心なプレーヤーでも、このライブラリには、主に静的クラス、暗号化と復号化、リフレクション操作など、一般的に使用される操作クラスが含まれています。ツリー構造とファイルの検出、加重ランダム スクリーニング アルゴリズム、分散型。短縮ID、式ツリー、linq展開、ファイル圧縮、マルチスレッドダウンロード、ハードウェア情報、文字列展開方法、日時展開操作、旧暦、大容量ファイルコピー、画像トリミング、検証コード、ブレークポイント再開、コレクション展開、 Excel エクスポートおよびその他の一般的なパッケージ。

**多くの機能が 1 つに統合されており、コード サイズは 2MB 未満です！**

プロジェクト開発モデル: 日々のコード蓄積 + ネットワーク収集

⭐⭐⭐この企画が気に入っていただけましたら、スター、フォーク、フォローの3つの良さに注目してください♂⭐⭐⭐

このプロジェクトに関して、何かわからないことや使用中に問題が発生した場合は、直接問題を提起するか、プライベートメッセージで私に連絡してください。もちろん、受け入れるのが恥ずかしい場合は、完全に無料の技術指導を提供します。 the free ご指導、相応の報酬をいただけるなら断りません！🤣🤣🤣

## ご注意ください：
このオープンソース プロジェクトを使用している企業、このプロジェクトを引用している企業、またはこのプロジェクトのコードを組み込んでいる企業が、労働法違反 (不法解雇、時間外労働、児童労働などを含むがこれらに限定されない) により法的措置に負けた場合、発見された場合、このプロジェクトの作成者は、このプロジェクトの使用料（**このプロジェクトの認可料として企業の産業および商業登録情報を登録した額の2～5倍**）を回収する権利を有します。または、このプロジェクトを含むソース コードを直接使用することは許可されていません。 `人間のアウトソーシング会社`または`007 会社`がこのライブラリを使用する必要があります。商用許可については著者に連絡してください。他の企業または個人は制限なく使用できます。 007 それは人を雇うということですが、これも無駄です。 8時間勤務制度により、自分自身を向上させ、将来の競争力を高める時間を確保できます。 007に対抗するのはみんなの責任です！

## 推奨開発環境
オペレーティング システム：Windows 11 23H2以降

開発ツール：VisualStudio2022 v17.8以降

SDK：.Net 6.0以降 **すべてのバージョン**

## インストールパッケージ
.NET 6.0以降
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
