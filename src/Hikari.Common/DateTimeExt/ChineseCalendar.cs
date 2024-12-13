﻿namespace Hikari.Common.DateTimeExt
{
    /// <summary>
    /// 中国农历类 支持 1900.1.31日起至 2069.12.31日止的数据
    /// </summary>
    /// <remarks>
    /// 本程序使用数据来源于网上的万年历查询，并综合了一些其它数据
    /// </remarks>
    public class ChineseCalendar
    {
        private class ChineseCalendarException(string msg) : Exception(msg);

        private readonly DateTime _datetime;

        #region 基础数据

        private const int MinYear = 1900;
        private const int MaxYear = 2100;
        private static readonly DateOnly MinDay = new DateOnly(1900, 1, 30);
        private const int GanZhiStartYear = 1864; //干支计算起始年
        private static readonly DateOnly GanZhiStartDay = new DateOnly(1899, 12, 22); //起始日
        private const string HzNum = "零一二三四五六七八九";
        private const int AnimalStartYear = 1900; //1900年为鼠年
        private static readonly DateOnly ChineseConstellationReferDay = new DateOnly(2007, 9, 13); //28星宿参考值,本日为角

        /// <summary>
        /// 来源于网上的农历数据
        /// </summary>
        /// <remarks>
        /// 数据结构如下，共使用17位数据<br/>
        /// 说明如下：<br/>
        ///1-4：判断当年是否为闰年，若为闰年，则为闰年的月份，反之为0；<br/>
        ///5-16：为除了闰月外的正常月份是大月还是小月，1为30天，0为29天。（注意：1月对应第16位，2月对应第15位……12月对应第5位)<br/>
        ///17-20： 表示闰月是大月还是小月，若为1，则为大月，若为0，则为小月。（注意：仅当存在闰月的情况下有意义）<br/>
        /// -<br/>
        ///举例说明：<br/>
        ///例一：0x04bd8<br/>
        ///对应二进制：0000    0100     1011    1101    1000<br/>
        ///则表示当年有闰月8月，且闰月为小月29天<br/>
        ///该年1-12月的天数为：29    30   29   29   30   29   30    29（闰月）  30     30   29    30<br/>
        ///例二：0x04ae0<br/>
        ///对应二进制：0000    0100     1010    1110    0000<br/>
        ///则表示当年没有闰月<br/>
        ///该年1-12月的天数为：29    30    29     29    30    29    30    29    30   30    30    29<br/>
        ///补充闰月介绍：<br/>
        ///闰月是阴阳历中为使历年平均长度接近会归年而增设的月。阴阳历以朔望月的长度（29.5306日）为一个月的平均值，全年12个月，同回归年（365.2422日）相差约10日21时，故需要置闰，三年闰一个月，五年闰二个月，十九年闰七个月。阴阳历以朔望月的长度（29.5306日）为一个月的平均值，全年12个月，同回归年（365.2422日）相差约10日21时，故需要置闰，三年闰一个月，五年闰二个月，十九年闰七个月。
        ///</remarks>
        private static readonly int[] LunarDateArray =
        {
            0x04BD8, 0x04AE0, 0x0A570, 0x054D5, 0x0D260, 0x0D950, 0x16554, 0x056A0, 0x09AD0, 0x055D2, 0x04AE0,
            0x0A5B6, 0x0A4D0, 0x0D250, 0x1D255, 0x0B540, 0x0D6A0, 0x0ADA2, 0x095B0, 0x14977, 0x04970, 0x0A4B0,
            0x0B4B5, 0x06A50, 0x06D40, 0x1AB54, 0x02B60, 0x09570, 0x052F2, 0x04970, 0x06566, 0x0D4A0, 0x0EA50,
            0x06E95, 0x05AD0, 0x02B60, 0x186E3, 0x092E0, 0x1C8D7, 0x0C950, 0x0D4A0, 0x1D8A6, 0x0B550, 0x056A0,
            0x1A5B4, 0x025D0, 0x092D0, 0x0D2B2, 0x0A950, 0x0B557, 0x06CA0, 0x0B550, 0x15355, 0x04DA0, 0x0A5B0,
            0x14573, 0x052B0, 0x0A9A8, 0x0E950, 0x06AA0, 0x0AEA6, 0x0AB50, 0x04B60, 0x0AAE4, 0x0A570, 0x05260,
            0x0F263, 0x0D950, 0x05B57, 0x056A0, 0x096D0, 0x04DD5, 0x04AD0, 0x0A4D0, 0x0D4D4, 0x0D250, 0x0D558,
            0x0B540, 0x0B6A0, 0x195A6, 0x095B0, 0x049B0, 0x0A974, 0x0A4B0, 0x0B27A, 0x06A50, 0x06D40, 0x0AF46,
            0x0AB60, 0x09570, 0x04AF5, 0x04970, 0x064B0, 0x074A3, 0x0EA50, 0x06B58, 0x055C0, 0x0AB60, 0x096D5,
            0x092E0, 0x0C960, 0x0D954, 0x0D4A0, 0x0DA50, 0x07552, 0x056A0, 0x0ABB7, 0x025D0, 0x092D0, 0x0CAB5,
            0x0A950, 0x0B4A0, 0x0BAA4, 0x0AD50, 0x055D9, 0x04BA0, 0x0A5B0, 0x15176, 0x052B0, 0x0A930, 0x07954,
            0x06AA0, 0x0AD50, 0x05B52, 0x04B60, 0x0A6E6, 0x0A4E0, 0x0D260, 0x0EA65, 0x0D530, 0x05AA0, 0x076A3,
            0x096D0, 0x04BD7, 0x04AD0, 0x0A4D0, 0x1D0B6, 0x0D250, 0x0D520, 0x0DD45, 0x0B5A0, 0x056D0, 0x055B2,
            0x049B0, 0x0A577, 0x0A4B0, 0x0AA50, 0x1B255, 0x06D20, 0x0ADA0, 0x14B63, 0x9370, 0x49f8, 0x4970,
            0x64b0, 0x168a6, 0xea50, 0x6aa0, 0x1a6c4, 0xaae0, 0x92e0, 0xd2e3, 0xc960, 0xd557, 0xd4a0, 0xda50,
            0x5d55, 0x56a0, 0xa6d0, 0x55d4, 0x52d0, 0xa9b8, 0xa950, 0xb4a0, 0xb6a6, 0xad50, 0x55a0, 0xaba4,
            0xa5b0, 0x52b0, 0xb273, 0x6930, 0x7337, 0x6aa0, 0xad50, 0x14b55, 0x4b60, 0xa570, 0x54e4, 0xd160,
            0xe968, 0xd520, 0xdaa0, 0x16aa6, 0x56d0, 0x4ae0, 0xa9d4, 0xa2d0, 0xd150, 0xf252, 0xd520
        };

        /// <summary>
        /// 星座
        /// </summary>
        private static readonly string[] ConstellationName =
        {
            "白羊座", "金牛座", "双子座", "巨蟹座",
            "狮子座", "处女座", "天秤座", "天蝎座",
            "射手座", "摩羯座", "水瓶座", "双鱼座"
        };

        /// <summary>
        /// 二十八星宿
        /// </summary>
        private static readonly string[] ChineseConstellationName =
        {
            //四           五          六         日          一         二         三
            "角木蛟", "亢金龙","氐土貉" , "房日兔", "心月狐", "尾火虎", "箕水豹",
            "斗木獬", "牛金牛", "女土蝠", "虚日鼠", "危月燕", "室火猪", "壁水獝",
            "奎木狼", "娄金狗", "胃土彘", "昴日鸡", "毕月乌", "觜火猴", "参水猿",
            "井木犴", "鬼金羊", "柳土獐", "星日马", "张月鹿", "翼火蛇", "轸水蚓"
        };

        #region 节气数据

        /// <summary>
        /// 节气数据
        /// </summary>
        private static readonly string[] SolarTerm =
        {
            "小寒", "大寒", "立春", "雨水", "惊蛰", "春分",
            "清明", "谷雨", "立夏", "小满", "芒种", "夏至",
            "小暑", "大暑", "立秋", "处暑", "白露", "秋分",
            "寒露", "霜降", "立冬", "小雪", "大雪", "冬至"
        };

        private static readonly int[] STermInfo =
        {
            0,
            21208,
            42467,
            63836,
            85337,
            107014,
            128867,
            150921,
            173149,
            195551,
            218072,
            240693,
            263343,
            285989,
            308563,
            331033,
            353350,
            375494,
            397447,
            419210,
            440795,
            462224,
            483532,
            504758
        };

        #endregion 节气数据

        #region 农历相关数据

        private const string TianGan = "甲乙丙丁戊己庚辛壬癸";
        private const string DiZhi = "子丑寅卯辰巳午未申酉戌亥";
        private const string AnimalStr = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
        private const string NStr1 = "日一二三四五六七八九";
        private const string NStr2 = "初十廿卅";

        private static readonly string[] MonthString =
        {
            "出错", "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "冬月", "腊月"
        };

        #endregion 农历相关数据

        #endregion 基础数据

        /// <summary>
        /// 用一个标准的公历日期来初使化
        /// </summary>
        /// <param name="dt"></param>
        public ChineseCalendar(in DateTime dt)
        {
            if (dt.Year > MaxYear)
            {
                throw new ChineseCalendarException("最大年份支持2100年");
            }

            int i;
            Date = DateOnly.FromDateTime(dt.Date);
            DateTime = dt;
            _datetime = dt;

            //农历日期计算部分
            var temp = 0;
            var ts = Date.ToDateTime(TimeOnly.MinValue) - MinDay.ToDateTime(TimeOnly.MinValue); //计算两天的基本差距
            var offset = ts.Days;
            for (i = MinYear; i <= MaxYear; i++)
            {
                temp = GetChineseYearDays(i); //求当年农历年天数
                if (offset - temp < 1)
                {
                    break;
                }

                offset = offset - temp;
            }

            ChineseYear = i;
            var leap = GetChineseLeapMonth(ChineseYear);

            //设定当年是否有闰月
            IsChineseLeapYear = leap > 0;
            IsChineseLeapMonth = false;
            for (i = 1; i <= 12; i++)
            {
                //闰月
                if (leap > 0 && i == leap + 1 && IsChineseLeapMonth == false)
                {
                    IsChineseLeapMonth = true;
                    i = i - 1;
                    temp = GetChineseLeapMonthDays(ChineseYear); //计算闰月天数
                }
                else
                {
                    IsChineseLeapMonth = false;
                    temp = GetChineseMonthDays(ChineseYear, i); //计算非闰月天数
                }

                offset = offset - temp;
                if (offset <= 0)
                {
                    break;
                }
            }

            offset = offset + temp;
            ChineseMonth = i;
            ChineseDay = offset;
        }

        /// <summary>
        /// 用农历的日期来初使化
        /// </summary>
        /// <param name="cy">农历年</param>
        /// <param name="cm">农历月</param>
        /// <param name="cd">农历日</param>
        public ChineseCalendar(int cy, int cm, int cd)
        {
            int i, temp;
            CheckChineseDateLimit(cy, cm, cd);
            ChineseYear = cy;
            ChineseMonth = cm;
            ChineseDay = cd;
            var offset = 0;
            for (i = MinYear; i < cy; i++)
            {
                temp = GetChineseYearDays(i); //求当年农历年天数
                offset = offset + temp;
            }

            var leap = GetChineseLeapMonth(cy);
            IsChineseLeapYear = leap != 0;
            IsChineseLeapMonth = cm == leap;
            if (IsChineseLeapYear == false || cm < leap) //当年没有闰月||计算月份小于闰月
            {
                for (i = 1; i < cm; i++)
                {
                    temp = GetChineseMonthDays(cy, i); //计算非闰月天数
                    offset = offset + temp;
                }

                //检查日期是否大于最大天
                if (cd > GetChineseMonthDays(cy, cm))
                {
                    throw new ChineseCalendarException("不合法的农历日期");
                }

                offset = offset + cd; //加上当月的天数
            }
            else //是闰年，且计算月份大于或等于闰月
            {
                for (i = 1; i < cm; i++)
                {
                    temp = GetChineseMonthDays(cy, i); //计算非闰月天数
                    offset = offset + temp;
                }

                if (cm > leap) //计算月大于闰月
                {
                    temp = GetChineseLeapMonthDays(cy); //计算闰月天数
                    offset = offset + temp; //加上闰月天数
                    if (cd > GetChineseMonthDays(cy, cm))
                    {
                        throw new ChineseCalendarException("不合法的农历日期");
                    }

                    offset = offset + cd;
                }
                else //计算月等于闰月
                {
                    //如果需要计算的是闰月，则应首先加上与闰月对应的普通月的天数
                    if (IsChineseLeapMonth) //计算月为闰月
                    {
                        temp = GetChineseMonthDays(cy, cm); //计算非闰月天数
                        offset = offset + temp;
                    }

                    if (cd > GetChineseLeapMonthDays(cy))
                    {
                        throw new ChineseCalendarException("不合法的农历日期");
                    }

                    offset = offset + cd;
                }
            }

            Date = MinDay.AddDays(offset);
            DateTime = Date.ToDateTime(TimeOnly.MinValue);
            _datetime = DateTime;
        }

        #region 私有函数

        /// <summary>
        /// 传回农历 y年m月的总天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int GetChineseMonthDays(int year, int month)
        {
            return BitTest32(LunarDateArray[year - MinYear] & 0x0000FFFF, 16 - month) ? 30 : 29;
        }

        /// <summary>
        /// 传回农历 y年闰哪个月 1-12 , 没闰传回 0
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private int GetChineseLeapMonth(int year)
        {
            return LunarDateArray[year - MinYear] & 0xF;
        }

        /// <summary>
        /// 传回农历 y年闰月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private int GetChineseLeapMonthDays(int year)
        {
            return GetChineseLeapMonth(year) != 0 ? (LunarDateArray[year - MinYear] & 0x10000) != 0 ? 30 : 29 : 0;
        }

        /// <summary>
        /// 取农历年一年的天数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private int GetChineseYearDays(int year)
        {
            var sumDay = 348;
            var i = 0x8000;
            var info = LunarDateArray[year - MinYear] & 0x0FFFF;

            //计算12个月中有多少天为30天
            for (int m = 0; m < 12; m++)
            {
                var f = info & i;
                if (f != 0)
                {
                    sumDay++;
                }

                i >>= 1;
            }

            return sumDay + GetChineseLeapMonthDays(year);
        }

        /// <summary>
        /// 获得当前时间的干支时辰
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        ///
        private string GetGanZhiHour(DateTime dt)
        {
            //计算时辰的地支
            var hour = dt.Hour;
            int offset;
            if (hour is 23 or 0)
            {
                offset = 0;
            }
            else
            {
                offset = (hour - 1) / 2 + 1;
                if (offset >= 12)
                {
                    offset = 0;
                }
            }

            //计算天干
            var ts = Date.ToDateTime(TimeOnly.MinValue) - GanZhiStartDay.ToDateTime(TimeOnly.MinValue);
            var i = ts.Days % 60;
            var indexGan = ((i % 10 + 1) * 2 - 1) % 10 - 1;
            var tmpGan = TianGan.Substring(indexGan) + TianGan.Substring(0, indexGan + 2);
            return tmpGan[offset].ToString() + DiZhi[offset];
        }
        /// <summary>
        /// 获得当前时间的时辰
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        ///
        private string GetChineseHour(DateTime dt)
        {
            //计算时辰的地支
            var hour = dt.Hour;
            int offset;
            // 子时需要特殊处理，因为23:00 - 00:59 属于子时
            if (hour is 23 or 0)
            {
                offset = 0;
            }
            else
            {
                offset = (hour - 1) / 2 + 1;
                if (offset >= 12)
                {
                    offset = 0;
                }
            }



            return DiZhi[offset] + "";
        }
        /// <summary>
        /// 检查农历日期是否合理
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        private void CheckChineseDateLimit(int year, int month, int day)
        {
            if (year < MinYear || year > MaxYear)
            {
                throw new ChineseCalendarException("非法农历日期");
            }

            if (month < 1 || month > 12)
            {
                throw new ChineseCalendarException("非法农历日期");
            }

            if (day is < 1 or > 30) //中国的月最多30天
            {
                throw new ChineseCalendarException("非法农历日期");
            }
        }

        /// <summary>
        /// 将0-9转成汉字形式
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private string ConvertNumToChineseNum(char n)
        {
            if (n < '0' || n > '9')
            {
                return "";
            }

            return n switch
            {
                '0' => HzNum[0].ToString(),
                '1' => HzNum[1].ToString(),
                '2' => HzNum[2].ToString(),
                '3' => HzNum[3].ToString(),
                '4' => HzNum[4].ToString(),
                '5' => HzNum[5].ToString(),
                '6' => HzNum[6].ToString(),
                '7' => HzNum[7].ToString(),
                '8' => HzNum[8].ToString(),
                '9' => HzNum[9].ToString(),
                _ => ""
            };
        }

        /// <summary>
        /// 测试某位是否为真
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bitpostion"></param>
        /// <returns></returns>
        private bool BitTest32(int num, int bitpostion)
        {
            if (bitpostion > 31 || bitpostion < 0)
            {
                throw new ArgumentException("参数错误: 取值范围0-31", nameof(bitpostion));
            }

            int bit = 1 << bitpostion;
            return (num & bit) != 0;
        }

        #endregion 私有函数



        #region 公历日期

        /// <summary>
        /// 取对应的公历日期
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// 取对应的公历日期
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 取星期几
        /// </summary>
        public DayOfWeek WeekDay => Date.DayOfWeek;

        /// <summary>
        /// 周几的字符
        /// </summary>
        public string WeekDayStr => Date.DayOfWeek switch
        {
            DayOfWeek.Sunday => "星期日",
            DayOfWeek.Monday => "星期一",
            DayOfWeek.Tuesday => "星期二",
            DayOfWeek.Wednesday => "星期三",
            DayOfWeek.Thursday => "星期四",
            DayOfWeek.Friday => "星期五",
            _ => "星期六"
        };

        /// <summary>
        /// 公历日期中文表示法 如一九九七年七月一日
        /// </summary>
        public string DateString => "公元" + Date.ToLongDateString();

        /// <summary>
        /// 当前是否公历闰年
        /// </summary>
        public bool IsLeapYear => DateTime.IsLeapYear(Date.Year);

        /// <summary>
        /// 28星宿计算
        /// </summary>
        public string ChineseConstellation
        {
            get
            {
                var ts = Date.ToDateTime(TimeOnly.MinValue) - ChineseConstellationReferDay.ToDateTime(TimeOnly.MinValue);
                var offset = ts.Days;
                var modStarDay = offset % 28;
                return (modStarDay >= 0 ? ChineseConstellationName[modStarDay] : ChineseConstellationName[27 + modStarDay]);
            }
        }

        /// <summary>
        /// 时辰
        /// </summary>
        public string ChineseHour => GetChineseHour(_datetime);

        #endregion 公历日期

        #region 农历日期

        /// <summary>
        /// 是否闰月
        /// </summary>
        public bool IsChineseLeapMonth { get; }

        /// <summary>
        /// 当年是否有闰月
        /// </summary>
        public bool IsChineseLeapYear { get; }

        /// <summary>
        /// 农历日
        /// </summary>
        public int ChineseDay { get; }

        /// <summary>
        /// 农历日中文表示
        /// </summary>
        public string ChineseDayString => ChineseDay switch
        {
            0 => "",
            10 => "初十",
            20 => "二十",
            30 => "三十",
            _ => (NStr2[ChineseDay / 10] + NStr1[ChineseDay % 10].ToString())
        };

        /// <summary>
        /// 农历的月份
        /// </summary>
        public int ChineseMonth { get; }

        /// <summary>
        /// 农历月份字符串
        /// </summary>
        public string ChineseMonthString => MonthString[ChineseMonth];

        /// <summary>
        /// 取农历年份
        /// </summary>
        public int ChineseYear { get; }

        /// <summary>
        /// 取农历年字符串如，一九九七年
        /// </summary>
        public string ChineseYearString
        {
            get
            {
                string tempStr = "";
                string num = ChineseYear.ToString();
                for (int i = 0; i < 4; i++)
                {
                    tempStr += ConvertNumToChineseNum(num[i]);
                }

                return tempStr + "年";
            }
        }

        /// <summary>
        /// 取农历日期表示法：农历一九九七年正月初五
        /// </summary>
        public string ChineseDateString
        {
            get
            {
                if (IsChineseLeapMonth)
                {
                    return ChineseYearString + "闰" + ChineseMonthString + ChineseDayString;
                }

                return ChineseYearString + ChineseMonthString + ChineseDayString;
            }
        }


        /// <summary>
        /// 定气法计算二十四节气,二十四节气是按地球公转来计算的，并非是阴历计算的
        /// </summary>
        /// <remarks>
        /// 节气的定法有两种。古代历法采用的称为"恒气"，即按时间把一年等分为24份，
        /// 每一节气平均得15天有余，所以又称"平气"。现代农历采用的称为"定气"，即
        /// 按地球在轨道上的位置为标准，一周360°，两节气之间相隔15°。由于冬至时地
        /// 球位于近日点附近，运动速度较快，因而太阳在黄道上移动15°的时间不到15天。
        /// 夏至前后的情况正好相反，太阳在黄道上移动较慢，一个节气达16天之多。采用
        /// 定气时可以保证春、秋两分必然在昼夜平分的那两天。
        /// </remarks>
        public string ChineseTwentyFourDay
        {
            get
            {
                var baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0, DateTimeKind.Local); //#1/6/1900 2:05:00 AM#
                var y = Date.Year;
                for (int i = 1; i <= 24; i++)
                {
                    var num = 525948.76 * (y - 1900) + STermInfo[i - 1];
                    var newDate = baseDateAndTime.AddMinutes(num);
                    if (newDate.DayOfYear != Date.DayOfYear)
                    {
                        continue;
                    }

                    return SolarTerm[i - 1];
                }

                return "";
            }
        }

        /// <summary>
        /// 当前日期前一个最近节气
        /// </summary>
        public ChineseCalendar ChineseTwentyFourPrevDay
        {
            get
            {
                var baseTime = new DateTime(1900, 1, 6, 2, 5, 0, DateTimeKind.Local); //#1/6/1900 2:05:00 AM#
                var y = Date.Year;
                for (int i = 24; i >= 1; i--)
                {
                    var num = 525948.76 * (y - 1900) + STermInfo[i - 1];
                    var newDate = baseTime.AddMinutes(num);
                    if (newDate.DayOfYear < Date.DayOfYear)
                    {
                        return new ChineseCalendar(newDate);
                    }
                }

                return this;
            }
        }

        /// <summary>
        /// 当前日期后一个最近节气
        /// </summary>
        public ChineseCalendar ChineseTwentyFourNextDay
        {
            get
            {
                var baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0, DateTimeKind.Local); //#1/6/1900 2:05:00 AM#
                var y = Date.Year;
                for (int i = 1; i <= 24; i++)
                {
                    var num = 525948.76 * (y - 1900) + STermInfo[i - 1];
                    var newDate = baseDateAndTime.AddMinutes(num);
                    if (newDate.DayOfYear > Date.DayOfYear)
                    {
                        return new ChineseCalendar(newDate);
                    }
                }

                return this;
            }
        }

        #endregion 农历日期

        #region 星座

        /// <summary>
        /// 计算指定日期的星座序号
        /// </summary>
        /// <returns></returns>
        public string Constellation
        {
            get
            {
                int index;
                var m = Date.Month;
                var d = Date.Day;
                var y = m * 100 + d;
                if (y is >= 321 and <= 419)
                {
                    index = 0;
                }
                else if (y is >= 420 and <= 520)
                {
                    index = 1;
                }
                else if (y is >= 521 and <= 620)
                {
                    index = 2;
                }
                else if (y is >= 621 and <= 722)
                {
                    index = 3;
                }
                else if (y is >= 723 and <= 822)
                {
                    index = 4;
                }
                else if (y is >= 823 and <= 922)
                {
                    index = 5;
                }
                else if (y is >= 923 and <= 1022)
                {
                    index = 6;
                }
                else if (y >= 1023 && y <= 1121)
                {
                    index = 7;
                }
                else if (y is >= 1122 and <= 1221)
                {
                    index = 8;
                }
                else if (y is >= 1222 or <= 119)
                {
                    index = 9;
                }
                else if (y is >= 120 and <= 218)
                {
                    index = 10;
                }
                else if (y is >= 219 and <= 320)
                {
                    index = 11;
                }
                else
                {
                    index = 0;
                }

                return ConstellationName[index];
            }
        }

        #endregion 星座

        #region 生肖

        /// <summary>
        /// 计算属相的索引，注意虽然属相是以农历年来区别的，但是目前在实际使用中是按公历来计算的
        /// 鼠年为1,其它类推
        /// </summary>
        public int Animal
        {
            get
            {
                int offset = Date.Year - AnimalStartYear;
                return offset % 12 + 1;
            }
        }

        /// <summary>
        /// 取属相字符串
        /// </summary>
        public string AnimalString
        {
            get
            {
                int offset = Date.Year - AnimalStartYear; //阳历计算
                return AnimalStr[offset % 12].ToString();
            }
        }

        #endregion 生肖

        #region 天干地支

        /// <summary>
        /// 取农历年的干支表示法如 乙丑年
        /// </summary>
        public string GanZhiYearString
        {
            get
            {
                int i = (ChineseYear - GanZhiStartYear) % 60; //计算干支
                var tempStr = TianGan[i % 10] + DiZhi[i % 12].ToString() + "年";
                return tempStr;
            }
        }

        /// <summary>
        /// 取干支的月表示字符串，注意农历的闰月不记干支
        /// </summary>
        public string GanZhiMonthString
        {
            get
            {
                //每个月的地支总是固定的,而且总是从寅月开始
                int zhiIndex;
                if (ChineseMonth > 10)
                {
                    zhiIndex = ChineseMonth - 10;
                }
                else
                {
                    zhiIndex = ChineseMonth + 2;
                }

                var zhi = DiZhi[zhiIndex - 1].ToString();

                //根据当年的干支年的干来计算月干的第一个
                int ganIndex = 1;
                int i = (ChineseYear - GanZhiStartYear) % 60; //计算干支
                ganIndex = (i % 10) switch
                {
                    0 => 3, //甲
                    1 => 5, //乙
                    2 => 7, //丙
                    3 => 9, //丁
                    4 => 1, //戊
                    5 => 3, //己
                    6 => 5, //庚
                    7 => 7, //辛
                    8 => 9, //壬
                    9 => 1, //癸
                    _ => ganIndex
                };

                var gan = TianGan[(ganIndex + ChineseMonth - 2) % 10].ToString();
                return gan + zhi + "月";
            }
        }

        /// <summary>
        /// 取干支日表示法
        /// </summary>
        public string GanZhiDayString
        {
            get
            {
                var ts = Date.ToDateTime(TimeOnly.MinValue) - GanZhiStartDay.ToDateTime(TimeOnly.MinValue);
                var offset = ts.Days;
                var i = offset % 60;
                return TianGan[i % 10].ToString() + DiZhi[i % 12] + "日";
            }
        }
        /// <summary>
        /// 时辰
        /// </summary>
        public string GanZhiHour => GetGanZhiHour(_datetime);
        /// <summary>
        /// 取当前日期的干支表示法如 甲子年乙丑月丙庚日
        /// </summary>
        public string GanZhiDateString => GanZhiYearString + GanZhiMonthString + GanZhiDayString;

        #endregion 天干地支
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ChineseDateString + " " + ChineseHour + "时";
        }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public string ToGanZhiString()
        {
            return GanZhiDateString + GanZhiHour + "时";
        }
    }
}