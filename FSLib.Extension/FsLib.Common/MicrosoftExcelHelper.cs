#if !NETSTANDARD2_0
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
/******************************************************************************************************************
 * 
 * 
 * 说　明： Excel操作-微软版(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2016/05/31
 * 修　改：
 * 参　考：http://www.knowsky.com/604102.html ， http://www.cnblogs.com/asxinyu/p/4374015.html
 * 备　注：需要引入Microsoft.Office.Interop.Excel.dll和COM组件：Microsoft Office 16.0 Object Library
 *        office版本不同，引用的版本也不同
 * 
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// MicrosoftExcelHelper 的摘要说明：一个C#操作Excel类，功能比较全。
    /// 需要引入Microsoft.Office.Interop.Excel.dll和COM组件：Microsoft Office 16.0 Object Library
    /// office版本不同，引用的版本也不同
    /// </summary>
    public class MicrosoftExcelHelper
    {
        private string _mFilename;
        private Excel.Application _app;
        private Excel.Workbooks _wbs;
        private Excel.Workbook _wb;
        //public Excel.Worksheets wss;
        //public Excel.Worksheet ws;

        /// <summary>
        /// 坐标从1，1开始
        /// </summary>
        public MicrosoftExcelHelper()
        {

        }
        /// <summary>
        /// 创建一个Excel对象
        /// </summary>
        public void Create()
        {
            _app = new Excel.Application();
            _wbs = _app.Workbooks;
            _wb = _wbs.Add(true);
        }
        /// <summary>
        /// 打开一个Excel文件
        /// </summary>
        /// <param name="fileName">文件地址</param>
        public void Open(string fileName)
        {
            _app = new Excel.Application();
            _wbs = _app.Workbooks;
            _wb = _wbs.Add(fileName);
            //wb = wbs.Open(FileName, 0, true, 5,"", "", true, Excel.XlPlatform.xlWindows, "t", false, false, 0, true,Type.Missing,Type.Missing);
            //wb = wbs.Open(FileName,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Excel.XlPlatform.xlWindows,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
            _mFilename = fileName;
        }
        /// <summary>
        /// //获取一个工作表
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <returns></returns>
        public Excel.Worksheet GetSheet(string wsn)
        {
            Excel.Worksheet s = (Excel.Worksheet)_wb.Worksheets[wsn];
            return s;
        }
        /// <summary>
        /// 添加一个工作表
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <returns></returns>
        public Excel.Worksheet AddSheet(string wsn)
        {
            Excel.Worksheet s = (Excel.Worksheet)_wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = wsn;
            return s;
        }
        /// <summary>
        /// 删除一个工作表
        /// </summary>
        /// <param name="wsn"></param>
        public void DelSheet(string wsn)
        {
            ((Excel.Worksheet)_wb.Worksheets[wsn]).Delete();
        }
        /// <summary>
        /// 重命名一个工作表
        /// </summary>
        /// <param name="oldSheetName">原来工作表名称</param>
        /// <param name="newSheetName">新工作表名称</param>
        /// <returns></returns>
        public Excel.Worksheet ReNameSheet(string oldSheetName, string newSheetName)
        {
            Excel.Worksheet s = (Excel.Worksheet)_wb.Worksheets[oldSheetName];
            s.Name = newSheetName;
            return s;
        }
        /// <summary>
        /// 重命名一个工作表
        /// </summary>
        /// <param name="sheet">原工作表对象</param>
        /// <param name="newSheetName">新工作表名称</param>
        /// <returns></returns>
        public Excel.Worksheet ReNameSheet(Excel.Worksheet sheet, string newSheetName)
        {

            sheet.Name = newSheetName;

            return sheet;
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="ws">工作表对象</param>
        /// <param name="x">X行</param>
        /// <param name="y">Y列</param>
        /// <param name="value">value 值</param>
        public void SetCellValue(Excel.Worksheet ws, int x, int y, object value)
        {
            if (value == null)
            {
                value = "";
            }
            ws.Cells[x, y] = value;
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="wsn">工作表的名称</param>
        /// <param name="x">X行</param>
        /// <param name="y">Y列</param>
        /// <param name="value">value 值</param>
        public void SetCellValue(string wsn, int x, int y, object value)
        {
            if (value == null)
            {
                value = "";
            }
            GetSheet(wsn).Cells[x, y] = value;
        }

        /// <summary>
        /// 设置一个单元格的属性：字体，大小，颜色，对齐方式
        /// </summary>
        /// <param name="ws">工作表对象</param>
        /// <param name="startx">开始x行</param>
        /// <param name="starty">开始y列</param>
        /// <param name="endx">结束x行</param>
        /// <param name="endy">结束y列</param>
        /// <param name="size">字体大小</param>
        /// <param name="name">字体型号</param>
        /// <param name="color">颜色</param>
        /// <param name="horizontalAlignment">对齐方式</param>
        /// <param name="bold">是否加粗</param>
        public void SetCellProperty(Excel.Worksheet ws, int startx, int starty, int endx, int endy, int size, string name, Excel.Constants color, Excel.Constants horizontalAlignment, bool bold)

        {
            //name = "宋体";
            //size = 12;
            //color = Excel.Constants.xlAutomatic;
            //horizontalAlignment = Excel.Constants.xlRight;
            Excel.Range range = ws.Range[ws.Cells[startx, starty], ws.Cells[endx, endy]];
            range.Font.Name = name;
            range.Font.Size = size;
            range.Font.Color = color;
            range.HorizontalAlignment = horizontalAlignment;
            range.Font.Bold = bold;
        }
        /// <summary>
        /// 设置一个单元格的属性：大小，字体，颜色，对齐方式
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="startx">开始x行</param>
        /// <param name="starty">开始y列</param>
        /// <param name="endx">结束x行</param>
        /// <param name="endy">结束y列</param>
        /// <param name="size">字体大小如：12</param>
        /// <param name="name">字体名称如：宋体</param>
        /// <param name="color">颜色索引</param>
        /// <param name="horizontalAlignment">对齐方式</param>
        /// <param name="bold">是否加粗</param>
        public void SetCellProperty(string wsn, int startx, int starty, int endx, int endy, int size, string name, ColorIndex color, Excel.Constants horizontalAlignment, bool bold)
        {
            //name = "宋体";
            //size = 12;
            //color = Excel.Constants.xlAutomatic;
            //HorizontalAlignment = Excel.Constants.xlRight;

            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range range = ws.Range[ws.Cells[startx, starty], ws.Cells[endx, endy]];
            range.Font.Name = name;
            range.Font.Size = size;
            range.Font.ColorIndex = color;
            range.HorizontalAlignment = horizontalAlignment;
            range.Font.Bold = bold;
        }

        /// <summary>
        /// 单元格背景色及填充方式
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="color">颜色索引</param>
        public void SetCellsBackColor(string wsn, int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range range = ws.Range[ws.Cells[startRow, startColumn], ws.Cells[endRow, endColumn]];
            range.Interior.ColorIndex = color;
        }

        /// <summary>
        /// 单元格边框
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="lineStyle">边框样式</param>
        public void SetCellsBorders(string wsn, int startRow, int startColumn, int endRow, int endColumn, int lineStyle)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range range = ws.Range[ws.Cells[startRow, startColumn], ws.Cells[endRow, endColumn]];
            range.Borders.LineStyle = lineStyle;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="ws">工作表对象</param>
        /// <param name="x1">起始行</param>
        /// <param name="y1">起始列</param>
        /// <param name="x2">结束行</param>
        /// <param name="y2">结束列</param>
        public void UniteCells(Excel.Worksheet ws, int x1, int y1, int x2, int y2)

        {
            ws.Range[ws.Cells[x1, y1], ws.Cells[x2, y2]].Merge(Type.Missing);
        }
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="ws">工作表名称</param>
        /// <param name="x1">起始行</param>
        /// <param name="y1">起始列</param>
        /// <param name="x2">结束行</param>
        /// <param name="y2">结束列</param>
        public void UniteCells(string ws, int x1, int y1, int x2, int y2)

        {
            GetSheet(ws).Range[GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]].Merge(Type.Missing);
            GetSheet(ws).Range[GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]].HorizontalAlignment = Excel.Constants.xlCenter;

        }

        /// <summary>
        /// 将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时使用
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="wsn">工作表名称</param>
        /// <param name="startX">开始x坐标</param>
        /// <param name="startY">开始y坐标</param>
        public void InsertTable(System.Data.DataTable dt, string wsn, int startX, int startY)

        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(wsn).Cells[startX + i, j + startY] = dt.Rows[i][j].ToString();

                }

            }

        }
        /// <summary>
        /// 将内存中数据表格插入到Excel指定工作表的指定位置
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="ws">工作表对象</param>
        /// <param name="startX">开始x坐标</param>
        /// <param name="startY">开始y坐标</param>
        public void InsertTable(System.Data.DataTable dt, Excel.Worksheet ws, int startX, int startY)

        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[startX + i, j + startY] = dt.Rows[i][j];

                }

            }

        }

        /// <summary>
        /// 将内存中数据表格添加到Excel指定工作表的指定位置
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="wsn">工作表名称</param>
        /// <param name="startX">开始x坐标</param>
        /// <param name="startY">开始y坐标</param>
        public void AddTable(System.Data.DataTable dt, string wsn, int startX, int startY)

        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    GetSheet(wsn).Cells[i + startX, j + startY] = dt.Rows[i][j];

                }

            }

        }
        /// <summary>
        /// 将内存中数据表格添加到Excel指定工作表的指定位置
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="ws">工作表对象</param>
        /// <param name="startX">开始x坐标</param>
        /// <param name="startY">开始y坐标</param>
        public void AddTable(System.Data.DataTable dt, Excel.Worksheet ws, int startX, int startY)

        {


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[i + startX, j + startY] = dt.Rows[i][j];

                }
            }

        }
        /// <summary>
        /// 插入图片操作
        /// </summary>
        /// <param name="filename">图片地址</param>
        /// <param name="wsn">工作表名称</param>
        public void InsertPictures(string filename, string wsn)

        {
            GetSheet(wsn).Shapes.AddPicture(filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            //后面的数字表示位置
        }
        /// <summary>
        /// 插入图片操作
        /// </summary>
        /// <param name="filename">图片地址</param>
        /// <param name="wsn">工作表名称</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void InsertPictures(string filename, string wsn, int height, int width)

        {
            GetSheet(wsn).Shapes.AddPicture(filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            GetSheet(wsn).Shapes.Range[Type.Missing].Height = height;
            GetSheet(wsn).Shapes.Range[Type.Missing].Width = width;
        }
        /// <summary>
        /// 插入图片操作
        /// </summary>
        /// <param name="filename">图片地址</param>
        /// <param name="wsn">工作表名称</param>
        /// <param name="left">左边位置</param>
        /// <param name="top">顶部位置</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void InsertPictures(string filename, string wsn, int left, int top, int height, int width)
        {
            GetSheet(wsn).Shapes.AddPicture(filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            GetSheet(wsn).Shapes.Range[Type.Missing].IncrementLeft(left);
            GetSheet(wsn).Shapes.Range[Type.Missing].IncrementTop(top);
            GetSheet(wsn).Shapes.Range[Type.Missing].Height = height;
            GetSheet(wsn).Shapes.Range[Type.Missing].Width = width;
        }
        /// <summary>
        /// 插入图表操作
        /// </summary>
        /// <param name="chartType">图表类型</param>
        /// <param name="wsn">工作表名称</param>
        /// <param name="dataSourcesX1">数据来源开始x行</param>
        /// <param name="dataSourcesY1">数据来源开始y列</param>
        /// <param name="dataSourcesX2">数据来源结束x行</param>
        /// <param name="dataSourcesY2">数据来源结束y列</param>
        /// <param name="chartDataType">图表数据类型</param>
        public void InsertActiveChart(Excel.XlChartType chartType, string wsn, int dataSourcesX1, int dataSourcesY1, int dataSourcesX2, int dataSourcesY2, Excel.XlRowCol chartDataType)
        {
            //chartDataType = Excel.XlRowCol.xlColumns;
            _wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                _wb.ActiveChart.ChartType = chartType;
                _wb.ActiveChart.SetSourceData(GetSheet(wsn).Range[GetSheet(wsn).Cells[dataSourcesX1, dataSourcesY1], GetSheet(wsn).Cells[dataSourcesX2, dataSourcesY2]], chartDataType);
                _wb.ActiveChart.Location(Excel.XlChartLocation.xlLocationAsObject, wsn);
            }
        }
        /// <summary>
        /// 保存文档
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
                    _app.DisplayAlerts = false;
                    _wb.SaveAs(_mFilename, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
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
        /// <returns></returns>
        public bool SaveAs(object fileName)

        {
            try
            {
                _wb.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
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
            _wb.Save();
            _wb.Close(Type.Missing, Type.Missing, Type.Missing);
            _wbs.Close();
            _app.Quit();
            _wb = null;
            _wbs = null;
            _app = null;
            GC.Collect();
        }

        /// <summary>
        /// 获得最大行列数
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <returns>rowsCount: 最大行,colsCount: 最大列</returns>
        public (int rowsCount, int colsCount) GetMaxCount(string wsn)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            int rowsCount = ws.UsedRange.Rows.Count;
            int colsCount = ws.UsedRange.Columns.Count;
            return (rowsCount, colsCount);
        }


        /// <summary>
        /// 获得最大行数
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        public int GetMaxRowsCount(string wsn)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            return ws.UsedRange.Rows.Count;
        }

        /// <summary>
        /// 获得最大列数
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        public int GetMaxColsCount(string wsn)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            return ws.UsedRange.Columns.Count;
        }
        /// <summary>
        /// 获得单元格文本
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="x">x行</param>
        /// <param name="y">y列</param>
        public string GetCellsValue(string wsn, int x, int y)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            return Convert.ToString(ws.Range[ws.Cells[x, y], ws.Cells[x, y]].Value2);
        }

        /// <summary>
        /// 指定位置插入一行，其他下移
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="x">指定x行</param>
        public void InsertRows(string wsn, int x)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range xlsRows = (Microsoft.Office.Interop.Excel.Range)ws.Rows[x, Type.Missing];
            xlsRows.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Type.Missing);
        }
        /// <summary>
        /// 指定位置插入一列，其余右移
        /// </summary>
        /// <param name="wsn">工作表名称</param>
        /// <param name="y">指定y列</param>
        public void InsertColumns(string wsn, int y)
        {
            object misValue = Type.Missing;
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range xlsColumns = (Microsoft.Office.Interop.Excel.Range)ws.Columns[y, misValue];
            xlsColumns.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, misValue);
        }
        /// <summary>
        /// 常用颜色定义,对就Excel中颜色名
        /// </summary>
        public enum ColorIndex
        {
            无色 = -4142,
            自动 = -4105,
            黑色 = 1,
            褐色 = 53,
            橄榄 = 52,
            深绿 = 51,
            深青 = 49,
            深蓝 = 11,
            靛蓝 = 55,
            灰色80 = 56,
            深红 = 9,
            橙色 = 46,
            深黄 = 12,
            绿色 = 10,
            青色 = 14,
            蓝色 = 5,
            蓝灰 = 47,
            灰色50 = 16,
            红色 = 3,
            浅橙色 = 45,
            酸橙色 = 43,
            海绿 = 50,
            水绿色 = 42,
            浅蓝 = 41,
            紫罗兰 = 13,
            灰色40 = 48,
            粉红 = 7,
            金色 = 44,
            黄色 = 6,
            鲜绿 = 4,
            青绿 = 8,
            天蓝 = 33,
            梅红 = 54,
            灰色25 = 15,
            玫瑰红 = 38,
            茶色 = 40,
            浅黄 = 36,
            浅绿 = 35,
            浅青绿 = 34,
            淡蓝 = 37,
            淡紫 = 39,
            白色 = 2
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
#endif
