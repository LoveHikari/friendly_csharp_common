using System.Text;
using System.Text.RegularExpressions;

/***
 * title:汉字转拼音静态类
 * date:2016-5-31
 * author:YUXiaoWei
 ***/
namespace System
{
    /// <summary>
    /// 汉字转拼音静态类,包括功能全拼和缩写，方法全部是静态的
    /// </summary>
    public class Chinese2Spell
    {
        #region 属性数据定义
        /// <summary>
        /// 汉字的机内码数组
        /// </summary>
        private static int[] pyValue = new int[]
        {
            -20319,
            -20317,
            -20304,
            -20295,
            -20292,
            -20283,
            -20265,
            -20257,
            -20242,
            -20230,
            -20051,
            -20036,
            -20032,
            -20026,
            -20002,
            -19990,
            -19986,
            -19982,
            -19976,
            -19805,
            -19784,
            -19775,
            -19774,
            -19763,
            -19756,
            -19751,
            -19746,
            -19741,
            -19739,
            -19728,
            -19725,
            -19715,
            -19540,
            -19531,
            -19525,
            -19515,
            -19500,
            -19484,
            -19479,
            -19467,
            -19289,
            -19288,
            -19281,
            -19275,
            -19270,
            -19263,
            -19261,
            -19249,
            -19243,
            -19242,
            -19238,
            -19235,
            -19227,
            -19224,
            -19218,
            -19212,
            -19038,
            -19023,
            -19018,
            -19006,
            -19003,
            -18996,
            -18977,
            -18961,
            -18952,
            -18783,
            -18774,
            -18773,
            -18763,
            -18756,
            -18741,
            -18735,
            -18731,
            -18722,
            -18710,
            -18697,
            -18696,
            -18526,
            -18518,
            -18501,
            -18490,
            -18478,
            -18463,
            -18448,
            -18447,
            -18446,
            -18239,
            -18237,
            -18231,
            -18220,
            -18211,
            -18201,
            -18184,
            -18183,
            -18181,
            -18012,
            -17997,
            -17988,
            -17970,
            -17964,
            -17961,
            -17950,
            -17947,
            -17931,
            -17928,
            -17922,
            -17759,
            -17752,
            -17733,
            -17730,
            -17721,
            -17703,
            -17701,
            -17697,
            -17692,
            -17683,
            -17676,
            -17496,
            -17487,
            -17482,
            -17468,
            -17454,
            -17433,
            -17427,
            -17417,
            -17202,
            -17185,
            -16983,
            -16970,
            -16942,
            -16915,
            -16733,
            -16708,
            -16706,
            -16689,
            -16664,
            -16657,
            -16647,
            -16474,
            -16470,
            -16465,
            -16459,
            -16452,
            -16448,
            -16433,
            -16429,
            -16427,
            -16423,
            -16419,
            -16412,
            -16407,
            -16403,
            -16401,
            -16393,
            -16220,
            -16216,
            -16212,
            -16205,
            -16202,
            -16187,
            -16180,
            -16171,
            -16169,
            -16158,
            -16155,
            -15959,
            -15958,
            -15944,
            -15933,
            -15920,
            -15915,
            -15903,
            -15889,
            -15878,
            -15707,
            -15701,
            -15681,
            -15667,
            -15661,
            -15659,
            -15652,
            -15640,
            -15631,
            -15625,
            -15454,
            -15448,
            -15436,
            -15435,
            -15419,
            -15416,
            -15408,
            -15394,
            -15385,
            -15377,
            -15375,
            -15369,
            -15363,
            -15362,
            -15183,
            -15180,
            -15165,
            -15158,
            -15153,
            -15150,
            -15149,
            -15144,
            -15143,
            -15141,
            -15140,
            -15139,
            -15128,
            -15121,
            -15119,
            -15117,
            -15110,
            -15109,
            -14941,
            -14937,
            -14933,
            -14930,
            -14929,
            -14928,
            -14926,
            -14922,
            -14921,
            -14914,
            -14908,
            -14902,
            -14894,
            -14889,
            -14882,
            -14873,
            -14871,
            -14857,
            -14678,
            -14674,
            -14670,
            -14668,
            -14663,
            -14654,
            -14645,
            -14630,
            -14594,
            -14429,
            -14407,
            -14399,
            -14384,
            -14379,
            -14368,
            -14355,
            -14353,
            -14345,
            -14170,
            -14159,
            -14151,
            -14149,
            -14145,
            -14140,
            -14137,
            -14135,
            -14125,
            -14123,
            -14122,
            -14112,
            -14109,
            -14099,
            -14097,
            -14094,
            -14092,
            -14090,
            -14087,
            -14083,
            -13917,
            -13914,
            -13910,
            -13907,
            -13906,
            -13905,
            -13896,
            -13894,
            -13878,
            -13870,
            -13859,
            -13847,
            -13831,
            -13658,
            -13611,
            -13601,
            -13406,
            -13404,
            -13400,
            -13398,
            -13395,
            -13391,
            -13387,
            -13383,
            -13367,
            -13359,
            -13356,
            -13343,
            -13340,
            -13329,
            -13326,
            -13318,
            -13147,
            -13138,
            -13120,
            -13107,
            -13096,
            -13095,
            -13091,
            -13076,
            -13068,
            -13063,
            -13060,
            -12888,
            -12875,
            -12871,
            -12860,
            -12858,
            -12852,
            -12849,
            -12838,
            -12831,
            -12829,
            -12812,
            -12802,
            -12607,
            -12597,
            -12594,
            -12585,
            -12556,
            -12359,
            -12346,
            -12320,
            -12300,
            -12120,
            -12099,
            -12089,
            -12074,
            -12067,
            -12058,
            -12039,
            -11867,
            -11861,
            -11847,
            -11831,
            -11798,
            -11781,
            -11604,
            -11589,
            -11536,
            -11358,
            -11340,
            -11339,
            -11324,
            -11303,
            -11097,
            -11077,
            -11067,
            -11055,
            -11052,
            -11045,
            -11041,
            -11038,
            -11024,
            -11020,
            -11019,
            -11018,
            -11014,
            -10838,
            -10832,
            -10815,
            -10800,
            -10790,
            -10780,
            -10764,
            -10587,
            -10544,
            -10533,
            -10519,
            -10331,
            -10329,
            -10328,
            -10322,
            -10315,
            -10309,
            -10307,
            -10296,
            -10281,
            -10274,
            -10270,
            -10262,
            -10260,
            -10256,
            -10254
        };
        /// <summary>
        /// 机内码对应的拼音数组
        /// </summary>
        private static string[] pyName = new string[]
        {
            "A",
            "Ai",
            "An",
            "Ang",
            "Ao",
            "Ba",
            "Bai",
            "Ban",
            "Bang",
            "Bao",
            "Bei",
            "Ben",
            "Beng",
            "Bi",
            "Bian",
            "Biao",
            "Bie",
            "Bin",
            "Bing",
            "Bo",
            "Bu",
            "Ba",
            "Cai",
            "Can",
            "Cang",
            "Cao",
            "Ce",
            "Ceng",
            "Cha",
            "Chai",
            "Chan",
            "Chang",
            "Chao",
            "Che",
            "Chen",
            "Cheng",
            "Chi",
            "Chong",
            "Chou",
            "Chu",
            "Chuai",
            "Chuan",
            "Chuang",
            "Chui",
            "Chun",
            "Chuo",
            "Ci",
            "Cong",
            "Cou",
            "Cu",
            "Cuan",
            "Cui",
            "Cun",
            "Cuo",
            "Da",
            "Dai",
            "Dan",
            "Dang",
            "Dao",
            "De",
            "Deng",
            "Di",
            "Dian",
            "Diao",
            "Die",
            "Ding",
            "Diu",
            "Dong",
            "Dou",
            "Du",
            "Duan",
            "Dui",
            "Dun",
            "Duo",
            "E",
            "En",
            "Er",
            "Fa",
            "Fan",
            "Fang",
            "Fei",
            "Fen",
            "Feng",
            "Fo",
            "Fou",
            "Fu",
            "Ga",
            "Gai",
            "Gan",
            "Gang",
            "Gao",
            "Ge",
            "Gei",
            "Gen",
            "Geng",
            "Gong",
            "Gou",
            "Gu",
            "Gua",
            "Guai",
            "Guan",
            "Guang",
            "Gui",
            "Gun",
            "Guo",
            "Ha",
            "Hai",
            "Han",
            "Hang",
            "Hao",
            "He",
            "Hei",
            "Hen",
            "Heng",
            "Hong",
            "Hou",
            "Hu",
            "Hua",
            "Huai",
            "Huan",
            "Huang",
            "Hui",
            "Hun",
            "Huo",
            "Ji",
            "Jia",
            "Jian",
            "Jiang",
            "Jiao",
            "Jie",
            "Jin",
            "Jing",
            "Jiong",
            "Jiu",
            "Ju",
            "Juan",
            "Jue",
            "Jun",
            "Ka",
            "Kai",
            "Kan",
            "Kang",
            "Kao",
            "Ke",
            "Ken",
            "Keng",
            "Kong",
            "Kou",
            "Ku",
            "Kua",
            "Kuai",
            "Kuan",
            "Kuang",
            "Kui",
            "Kun",
            "Kuo",
            "La",
            "Lai",
            "Lan",
            "Lang",
            "Lao",
            "Le",
            "Lei",
            "Leng",
            "Li",
            "Lia",
            "Lian",
            "Liang",
            "Liao",
            "Lie",
            "Lin",
            "Ling",
            "Liu",
            "Long",
            "Lou",
            "Lu",
            "Lv",
            "Luan",
            "Lue",
            "Lun",
            "Luo",
            "Ma",
            "Mai",
            "Man",
            "Mang",
            "Mao",
            "Me",
            "Mei",
            "Men",
            "Meng",
            "Mi",
            "Mian",
            "Miao",
            "Mie",
            "Min",
            "Ming",
            "Miu",
            "Mo",
            "Mou",
            "Mu",
            "Na",
            "Nai",
            "Nan",
            "Nang",
            "Nao",
            "Ne",
            "Nei",
            "Nen",
            "Neng",
            "Ni",
            "Nian",
            "Niang",
            "Niao",
            "Nie",
            "Nin",
            "Ning",
            "Niu",
            "Nong",
            "Nu",
            "Nv",
            "Nuan",
            "Nue",
            "Nuo",
            "O",
            "Ou",
            "Pa",
            "Pai",
            "Pan",
            "Pang",
            "Pao",
            "Pei",
            "Pen",
            "Peng",
            "Pi",
            "Pian",
            "Piao",
            "Pie",
            "Pin",
            "Ping",
            "Po",
            "Pu",
            "Qi",
            "Qia",
            "Qian",
            "Qiang",
            "Qiao",
            "Qie",
            "Qin",
            "Qing",
            "Qiong",
            "Qiu",
            "Qu",
            "Quan",
            "Que",
            "Qun",
            "Ran",
            "Rang",
            "Rao",
            "Re",
            "Ren",
            "Reng",
            "Ri",
            "Rong",
            "Rou",
            "Ru",
            "Ruan",
            "Rui",
            "Run",
            "Ruo",
            "Sa",
            "Sai",
            "San",
            "Sang",
            "Sao",
            "Se",
            "Sen",
            "Seng",
            "Sha",
            "Shai",
            "Shan",
            "Shang",
            "Shao",
            "She",
            "Shen",
            "Sheng",
            "Shi",
            "Shou",
            "Shu",
            "Shua",
            "Shuai",
            "Shuan",
            "Shuang",
            "Shui",
            "Shun",
            "Shuo",
            "Si",
            "Song",
            "Sou",
            "Su",
            "Suan",
            "Sui",
            "Sun",
            "Suo",
            "Ta",
            "Tai",
            "Tan",
            "Tang",
            "Tao",
            "Te",
            "Teng",
            "Ti",
            "Tian",
            "Tiao",
            "Tie",
            "Ting",
            "Tong",
            "Tou",
            "Tu",
            "Tuan",
            "Tui",
            "Tun",
            "Tuo",
            "Wa",
            "Wai",
            "Wan",
            "Wang",
            "Wei",
            "Wen",
            "Weng",
            "Wo",
            "Wu",
            "Xi",
            "Xia",
            "Xian",
            "Xiang",
            "Xiao",
            "Xie",
            "Xin",
            "Xing",
            "Xiong",
            "Xiu",
            "Xu",
            "Xuan",
            "Xue",
            "Xun",
            "Ya",
            "Yan",
            "Yang",
            "Yao",
            "Ye",
            "Yi",
            "Yin",
            "Ying",
            "Yo",
            "Yong",
            "You",
            "Yu",
            "Yuan",
            "Yue",
            "Yun",
            "Za",
            "Zai",
            "Zan",
            "Zang",
            "Zao",
            "Ze",
            "Zei",
            "Zen",
            "Zeng",
            "Zha",
            "Zhai",
            "Zhan",
            "Zhang",
            "Zhao",
            "Zhe",
            "Zhen",
            "Zheng",
            "Zhi",
            "Zhong",
            "Zhou",
            "Zhu",
            "Zhua",
            "Zhuai",
            "Zhuan",
            "Zhuang",
            "Zhui",
            "Zhun",
            "Zhuo",
            "Zi",
            "Zong",
            "Zou",
            "Zu",
            "Zuan",
            "Zui",
            "Zun",
            "Zuo"
        };
        #endregion

        /// <summary>
        /// 汉字转拼音，每个汉字首字母大写
        /// 例：一二三  ====》YiErSan
        /// </summary>
        /// <param name="hzString"></param>
        /// <returns></returns>
        public static string Convert(string hzString)
        {
            Regex regex = new Regex("^[一-龥]$");
            byte[] array = new byte[2];
            string text = "";
            char[] array2 = hzString.ToCharArray();
            for (int i = 0; i < array2.Length; i++)
            {
                if (regex.IsMatch(array2[i].ToString()))
                {
                    array = Encoding.Default.GetBytes(array2[i].ToString());
                    int num = (int)array[0];
                    int num2 = (int)array[1];
                    int num3 = num * 256 + num2 - 65536;
                    if (num3 > 0 && num3 < 160)
                    {
                        text += array2[i];
                    }
                    else if (num3 == -9254)
                    {
                        text += "Zhen";
                    }
                    else
                    {
                        for (int j = Chinese2Spell.pyValue.Length - 1; j >= 0; j--)
                        {
                            if (Chinese2Spell.pyValue[j] <= num3)
                            {
                                text += Chinese2Spell.pyName[j];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    text += array2[i].ToString();
                }
            }
            return text;
        }
        /// <summary>
        /// 汉字转拼音，每个汉字首字母大写，汉字拼音之间相隔一个空格
        /// 例：一二三  ====》Yi Er San
        /// </summary>
        /// <param name="hzString"></param>
        /// <returns></returns>
        public static string ConvertWithBlank(string hzString)
        {
            Regex regex = new Regex("^[一-龥]$");
            byte[] array = new byte[2];
            string text = "";
            char[] array2 = hzString.ToCharArray();
            for (int i = 0; i < array2.Length; i++)
            {
                if (regex.IsMatch(array2[i].ToString()))
                {
                    array = Encoding.Default.GetBytes(array2[i].ToString());
                    int num = (int)array[0];
                    int num2 = (int)array[1];
                    int num3 = num * 256 + num2 - 65536;
                    if (num3 > 0 && num3 < 160)
                    {
                        text = text + " " + array2[i];
                    }
                    else if (num3 == -9254)
                    {
                        text += " Zhen";
                    }
                    else
                    {
                        for (int j = Chinese2Spell.pyValue.Length - 1; j >= 0; j--)
                        {
                            if (Chinese2Spell.pyValue[j] <= num3)
                            {
                                text = text + " " + Chinese2Spell.pyName[j];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    text = text + " " + array2[i].ToString();
                }
            }
            return text.Trim();
        }
        /// <summary>
        /// 在汉字之间插入字符串
        /// 例：一二三，11  ====》一11二11三
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <param name="splitChar">需要在汉字之间插入的字符串</param>
        /// <returns></returns>
        public static string ConvertWithSplitChar(string hzString, string splitChar)
        {
            // 匹配中文字符
            Regex regex = new Regex("^[u4e00-u9fa5]$");
            byte[] array = new byte[2];
            string text = "";
            char[] array2 = hzString.ToCharArray();
            for (int i = 0; i < array2.Length; i++)
            {
                // 中文字符
                if (regex.IsMatch(array2[i].ToString()))
                {
                    array = Encoding.Default.GetBytes(array2[i].ToString());
                    int num = (int)array[0];
                    int num2 = (int)array[1];
                    int num3 = num * 256 + num2 - 65536;
                    if (num3 > 0 && num3 < 160)
                    {
                        text = text + splitChar + array2[i];
                    }
                    else if (num3 == -9254) // 修正部分文字 // 修正“圳”字
                    {
                        text = text + splitChar + "Zhen";
                    }
                    else
                    {
                        for (int j = Chinese2Spell.pyValue.Length - 1; j >= 0; j--)
                        {
                            if (Chinese2Spell.pyValue[j] <= num3)
                            {
                                text = text + splitChar + Chinese2Spell.pyName[j];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    text = text + splitChar + array2[i].ToString();
                }
            }
            char[] trimChars = splitChar.ToCharArray();
            return text.TrimStart(trimChars);
        }
        /// <summary>
        /// 汉字转拼音缩写 (字符串) (小写)
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns> 
        public static string GetSpellStringLower(string str)
        {
            string text = "";
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c >= '!' && c <= '~')
                {
                    text += c.ToString();
                }
                else
                {
                    text += Chinese2Spell.GetSpellCharLower(c.ToString());
                }
            }
            return text;
        }
        /// <summary>
        /// 汉字转拼音缩写 (字符串) (小写) (空格间隔)
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns> 
        public static string GetSpellStringLowerSplitWithBlank(string str)
        {
            string text = "";
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c >= '!' && c <= '~')
                {
                    text = text + " " + c.ToString();
                }
                else
                {
                    text = text + " " + Chinese2Spell.GetSpellCharLower(c.ToString());
                }
            }
            return text.Trim();
        }
        /// <summary>
        /// 汉字转拼音缩写 (大写)
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetSpellStringSupper(string str)
        {
            string text = "";
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c >= '!' && c <= '~')
                {
                    //字母和符号原样保留
                    text += c.ToString();
                }
                else
                {
                    //累加拼音声母
                    text += Chinese2Spell.GetSpellCharSupper(c.ToString());
                }
            }
            return text;
        }
        /// <summary>
        /// 汉字转拼音缩写  (字符串)(大写)(空格间隔)
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetSpellStringSupperSplitWithBlank(string str)
        {
            string text = "";
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c >= '!' && c <= '~')
                {
                    //字母和符号原样保留
                    text = text + " " + c.ToString();
                }
                else
                {
                    //累加拼音声母
                    text = text + " " + Chinese2Spell.GetSpellCharSupper(c.ToString());
                }
            }
            return text.Trim();
        }
        /// <summary>
        /// 取单个字符的拼音声母(大写)
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母(大写)</returns>
        public static string GetSpellCharSupper(string c)
        {
            byte[] array = new byte[2];
            array = Encoding.Default.GetBytes(c);
            int num = (int)((short)(array[0] - '\0') * 256 + (short)(array[1] - '\0'));
            if (num < 45217)
            {
                return c;
            }
            if (num < 45253)
            {
                return "A";
            }
            if (num < 45761)
            {
                return "B";
            }
            if (num < 46318)
            {
                return "C";
            }
            if (num < 46826)
            {
                return "D";
            }
            if (num < 47010)
            {
                return "E";
            }
            if (num < 47297)
            {
                return "F";
            }
            if (num < 47614)
            {
                return "G";
            }
            if (num < 48119)
            {
                return "H";
            }
            if (num < 49062)
            {
                return "J";
            }
            if (num < 49324)
            {
                return "K";
            }
            if (num < 49896)
            {
                return "L";
            }
            if (num < 50371)
            {
                return "M";
            }
            if (num < 50614)
            {
                return "N";
            }
            if (num < 50622)
            {
                return "O";
            }
            if (num < 50906)
            {
                return "P";
            }
            if (num < 51387)
            {
                return "Q";
            }
            if (num < 51446)
            {
                return "R";
            }
            if (num < 52218)
            {
                return "S";
            }
            if (num < 52698)
            {
                return "T";
            }
            if (num < 52980)
            {
                return "W";
            }
            if (num < 53689)
            {
                return "X";
            }
            if (num < 54481)
            {
                return "Y";
            }
            if (num < 55290)
            {
                return "Z";
            }
            return c;
        }

        /// <summary>
        /// 取单个字符的拼音声母(小写)
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母(小写)</returns>
        public static string GetSpellCharLower(string c)
        {
            byte[] array = new byte[2];
            array = Encoding.Default.GetBytes(c);
            int num = (int)((short)(array[0] - '\0') * 256 + (short)(array[1] - '\0'));
            if (num < 45217)
            {
                return c;
            }
            if (num < 45253)
            {
                return "a";
            }
            if (num < 45761)
            {
                return "b";
            }
            if (num < 46318)
            {
                return "c";
            }
            if (num < 46826)
            {
                return "d";
            }
            if (num < 47010)
            {
                return "e";
            }
            if (num < 47297)
            {
                return "f";
            }
            if (num < 47614)
            {
                return "g";
            }
            if (num < 48119)
            {
                return "h";
            }
            if (num < 49062)
            {
                return "j";
            }
            if (num < 49324)
            {
                return "k";
            }
            if (num < 49896)
            {
                return "l";
            }
            if (num < 50371)
            {
                return "m";
            }
            if (num < 50614)
            {
                return "n";
            }
            if (num < 50622)
            {
                return "o";
            }
            if (num < 50906)
            {
                return "p";
            }
            if (num < 51387)
            {
                return "q";
            }
            if (num < 51446)
            {
                return "r";
            }
            if (num < 52218)
            {
                return "s";
            }
            if (num < 52698)
            {
                return "t";
            }
            if (num < 52980)
            {
                return "w";
            }
            if (num < 53689)
            {
                return "x";
            }
            if (num < 54481)
            {
                return "y";
            }
            if (num < 55290)
            {
                return "z";
            }
            return c;
        }
    }
}
