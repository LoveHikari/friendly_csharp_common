using Spire.Xls;

/******************************************************************************************************************
 * 
 * 
 * 说　明： Excel操作(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2016/10/08
 * 修　改：
 * 参　考：http://www.knowsky.com/604102.html ， http://www.cnblogs.com/asxinyu/p/4374015.html
 * 备　注：利用Spire.XLS控件
 * 
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public class SpireExcelHelper
    {
        private string _mFilename;
        private Workbook _wb;
        /// <summary>
        /// 坐标从1，1开始
        /// </summary>
        public SpireExcelHelper()
        {

        }
        /// <summary>
        /// 创建一个Excel对象
        /// </summary>
        public void Create()
        {
            _wb = new Workbook();
        }
        /// <summary>
        /// 打开一个Excel文件
        /// </summary>
        /// <param name="fileName">文件地址</param>
        public void Open(string fileName)
        {
            _wb = new Workbook();
            _wb.LoadFromFile(fileName);
            _mFilename = fileName;
        }

        /// <summary>
        /// //获取一个工作表
        /// </summary>
        /// <param name="sheetName">工作表名称</param>
        /// <returns></returns>
        public Worksheet GetSheet(string sheetName)

        {
            Worksheet s = _wb.Worksheets[sheetName];
            return s;
        }
        /// <summary>
        /// 添加一个工作表
        /// </summary>
        /// <param name="sheetName">工作簿名称</param>
        /// <returns></returns>
        public Worksheet AddSheet(string sheetName)
        {
            Worksheet s = _wb.Worksheets.Add(sheetName);
            s.Name = sheetName;
            return s;
        }
        /// <summary>
        /// 删除一个工作表
        /// </summary>
        /// <param name="sheetName"></param>
        public void DelSheet(string sheetName)
        {
            _wb.Worksheets.Remove(sheetName);
        }

        /// <summary>
        /// 重命名一个工作表一
        /// </summary>
        /// <param name="oldSheetName"></param>
        /// <param name="newSheetName"></param>
        /// <returns></returns>
        public Worksheet ReNameSheet(string oldSheetName, string newSheetName)
        {
            Worksheet s = _wb.Worksheets[oldSheetName];
            s.Name = newSheetName;
            return s;
        }
        /// <summary>
        /// 重命名一个工作表二
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="newSheetName"></param>
        /// <returns></returns>
        public Worksheet ReNameSheet(Worksheet sheet, string newSheetName)
        {
            sheet.Name = newSheetName;
            return sheet;
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="ws">工作表</param>
        /// <param name="x">X行Y列</param>
        /// <param name="y">X行Y列</param>
        /// <param name="value">value 值</param>
        public void SetCellValue(Worksheet ws, int x, int y, string value)
        {
            if (value == null)
            {
                value = "";
            }
            ws.SetCellValue(x, y, value);
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="ws">工作表</param>
        /// <param name="range">单元格位置：A1</param>
        /// <param name="value">value 值</param>
        public void SetCellValue(Worksheet ws, string range, string value)
        {
            if (value == null)
            {
                value = "";
            }
            ws.Range[range].Text = value;
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="x">X行Y列</param>
        /// <param name="y">X行Y列</param>
        /// <param name="value">value 值</param>
        public void SetCellValue(string wsn, int x, int y, string value)
        {
            if (value == null)
            {
                value = "";
            }
            GetSheet(wsn).SetCellValue(x, y, value);
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="range">单元格位置：A1</param>
        /// <param name="value">value 值</param>
        public void SetCellValue(string wsn, string range, string value)
        {
            if (value == null)
            {
                value = "";
            }
            GetSheet(wsn).Range[range].Text = value;
        }

        /// <summary>
        /// 设置一个单元格的属性(字体，大小，颜色，对齐方式)
        /// </summary>
        /// <param name="ws">工作表的名称</param>
        /// <param name="startRange">开始单元格</param>
        /// <param name="endRange">结束单元格</param>
        /// <param name="size">字体大小：12</param>
        /// <param name="name">字体名称：宋体</param>
        /// <param name="color">字体颜色：System.Drawing.Color.White</param>
        /// <param name="horizontalAlignment">文本水平对齐方式：HorizontalAlignType.Center</param>
        /// <param name="verticalAlignment">文本垂直对齐方式：VerticalAlignType.Center</param>
        public void SetCellProperty(Worksheet ws, string startRange, string endRange, int size, string name, System.Drawing.Color color, HorizontalAlignType horizontalAlignment, VerticalAlignType verticalAlignment)
        {
            string range = startRange + ":" + endRange;
            CellStyle style = ws.Range[range].Style;

            style.Font.FontName = name;  //设置字体
            style.Font.Color = color;   //设置字体颜色
            style.Font.Size = size;   //设置字体大小
            style.HorizontalAlignment = horizontalAlignment; //设置文本水平对齐方式
            style.VerticalAlignment = verticalAlignment;   //设置文本垂直对齐方式
        }
        /// <summary>
        /// 设置一个单元格的属性：大小，字体，颜色，对齐方式
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="startRange">开始单元格</param>
        /// <param name="endRange">结束单元格</param>
        /// <param name="size">字体大小：12</param>
        /// <param name="name">字体名称：宋体</param>
        /// <param name="color">字体颜色：System.Drawing.Color.White</param>
        /// <param name="horizontalAlignment">文本水平对齐方式：HorizontalAlignType.Center</param>
        /// <param name="verticalAlignment">文本垂直对齐方式：VerticalAlignType.Center</param>
        /// <param name="bold">是否加粗</param>
        public void SetCellProperty(string wsn, string startRange, string endRange, int size, string name, System.Drawing.Color color, HorizontalAlignType horizontalAlignment, VerticalAlignType verticalAlignment, bool bold)
        {
            string range = startRange + ":" + endRange;
            Worksheet ws = GetSheet(wsn);
            CellStyle style = ws.Range[range].Style;

            style.Font.FontName = name;  //设置字体
            style.Font.Color = color;   //设置字体颜色
            style.Font.Size = size;   //设置字体大小
            style.HorizontalAlignment = horizontalAlignment; //设置文本水平对齐方式
            style.VerticalAlignment = verticalAlignment;   //设置文本垂直对齐方式
            style.Font.IsBold = bold;
        }
        /// <summary>
        /// 设置单元格背景色及填充方式
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="startRange">开始单元格</param>
        /// <param name="endRange">结束单元格</param>
        /// <param name="color">颜色索引：ExcelColors.Green</param>
        public void SetCellsBackColor(string wsn, string startRange, string endRange, ExcelColors color)
        {
            Worksheet ws = GetSheet(wsn);
            string range = startRange + ":" + endRange;
            CellStyle style = ws.Range[range].Style;

            style.KnownColor = color;
        }
        /// <summary>
        /// 设置单元格边框
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="startRange">开始单元格</param>
        /// <param name="endRange">结束单元格</param>
        /// <param name="lineStyle">边框</param>
        public void SetCellsBorders(string wsn, string startRange, string endRange, int lineStyle)
        {
            Worksheet ws = GetSheet(wsn);
            string range = startRange + ":" + endRange;
            CellStyle style = ws.Range[range].Style;

            style.Borders[BordersLineType.EdgeLeft].LineStyle = (LineStyleType)lineStyle;//设置左边的
            style.Borders[BordersLineType.EdgeRight].LineStyle = (LineStyleType)lineStyle;//设置右边的
            style.Borders[BordersLineType.EdgeTop].LineStyle = (LineStyleType)lineStyle;//设置上边的
            style.Borders[BordersLineType.EdgeBottom].LineStyle = (LineStyleType)lineStyle;//设置下边的
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="ws">工作表</param>
        /// <param name="startRange">开始单元格</param>
        /// <param name="endRange">结束单元格</param>
        public void UniteCells(Worksheet ws, string startRange, string endRange)
        {
            string range = startRange + ":" + endRange;
            ws.Range[range].Merge();
        }
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="startRange">开始单元格</param>
        /// <param name="endRange">结束单元格</param>
        public void UniteCells(string wsn, string startRange, string endRange)
        {
            string range = startRange + ":" + endRange;
            GetSheet(wsn).Range[range].Merge();
        }

        /// <summary>
        /// 将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时使用一
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="startX">开始x坐标</param>
        /// <param name="startY">开始y坐标</param>
        public void InsertTable(System.Data.DataTable dt, string wsn, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(wsn).SetCellValue(startX + i, j + startY, dt.Rows[i][j].ToString());
                }
            }
        }

        /// <summary>
        /// 将内存中数据表格插入到Excel指定工作表的指定位置二
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="ws">工作表</param>
        /// <param name="startX">开始x坐标</param>
        /// <param name="startY">开始y坐标</param>
        public void InsertTable(System.Data.DataTable dt, Worksheet ws, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    ws.SetCellValue(startX + i, j + startY, dt.Rows[i][j].ToString());
                }
            }
        }
#if !NETSTANDARD2_0
/// <summary>
/// 插入图片操作一(在A1处插入图片)
/// </summary>
/// <param name="wsn">工作表的名称</param>
/// <param name="filename">图片地址</param>
/// <param name="pictureName">图片名称</param>
        public void InsertPictures(string wsn, string filename, string pictureName)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(filename);
            GetSheet(wsn).Pictures.Add(image, pictureName);
        }
        /// <summary>
        /// 插入图片操作二
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="filename">图片地址</param>
        /// <param name="topRow">顶部x坐标</param>
        /// <param name="leftColumn">左部y坐标</param>
        public void InsertPictures(string wsn, string filename, int topRow, int leftColumn)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(filename);
            GetSheet(wsn).Pictures.Add(topRow, leftColumn, image);
        }
        /// <summary>
        /// 插入图片操作三
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="filename">图片地址</param>
        /// <param name="topRow">顶部x坐标</param>
        /// <param name="leftColumn">左部y坐标</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public void InsertPictures(string wsn, string filename, int topRow, int leftColumn, int width, int height)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(filename);
            GetSheet(wsn).Pictures.Add(topRow, leftColumn, image, width, height);
        }
#endif

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if (_mFilename == "")
            {
                return false;
            }
            else
            {
                try
                {
                    _wb.Save();
                    return true;
                }

                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 文档另存为
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="excelVersion">Excel版本</param>
        /// <returns>是否保存成功</returns>
        public bool SaveAs(string fileName, ExcelVersion excelVersion = ExcelVersion.Version97to2003)
        {
            try
            {
                string m_dir = System.IO.Path.GetDirectoryName(fileName);
                if (m_dir != null && !System.IO.Directory.Exists(m_dir))
                {
                    System.IO.Directory.CreateDirectory(m_dir);
                }
                _wb.SaveToFile(fileName, excelVersion);
                _wb.Save();
                return true;

            }

            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return false;

            }
        }

        /// <summary>
        /// 关闭一个Excel对象，销毁对象
        /// </summary>
        public void Close()
        {
            _wb = null;
            _mFilename = null;
            GC.Collect();
        }

#if NET35
        /// <summary>
        /// 获得最大行列数
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <returns>最大行数,最大列数</returns>
        public object[] GetMaxCount(string wsn)
        {
            Worksheet ws = GetSheet(wsn);
            int rowsCount = ws.Rows.Length;
            int colsCount = ws.Columns.Length;
            return new object[] { rowsCount, colsCount };
        }
#else
        /// <summary>
        /// 获得最大行列数
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <returns>最大行数,最大列数</returns>
        public (int rowsCount, int colsCount) GetMaxCount(string wsn)
        {
            Worksheet ws = GetSheet(wsn);
            int rowsCount = ws.Rows.Length;
            int colsCount = ws.Columns.Length;
            return (rowsCount, colsCount);
        }
#endif

        /// <summary>
        /// 获得最大行数
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        public int GetMaxRowsCount(string wsn)
        {
            Worksheet ws = GetSheet(wsn);
            return ws.Rows.Length;
        }
        /// <summary>
        /// 获得最大列数
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        public int GetMaxColsCount(string wsn)
        {
            Worksheet ws = GetSheet(wsn);
            return ws.Columns.Length;

        }
        /// <summary>
        /// 获得单元格文本
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public string GetCellsValue(string wsn, int x, int y)
        {
            Worksheet ws = GetSheet(wsn);
            return Convert.ToString(ws.Range[x, y].Value2);
        }
        /// <summary>
        /// 获得单元格文本
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="range">单元格坐标</param>
        public string GetCellsValue(string wsn, string range)
        {
            Worksheet ws = GetSheet(wsn);
            return Convert.ToString(ws.Range[range].Value2);
        }

        /// <summary>
        /// HTML，CSV，TEXT，EXCEL，XML
        /// </summary>
        public enum SaveAsFileFormat
        {
            HTML,
            CSV,
            TEXT,
            EXCEL,
            XML
        }
    }
}