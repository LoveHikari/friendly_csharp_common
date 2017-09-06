#if !NETSTANDARD2_0
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
/******************************************************************************************************************
 * 
 * 
 * ˵������ Excel����-΢���(�汾��Version1.0.0)
 * �����ߣ�YuXiaoWei
 * �ա��ڣ�2016/05/31
 * �ޡ��ģ�
 * �Ρ�����http://www.knowsky.com/604102.html �� http://www.cnblogs.com/asxinyu/p/4374015.html
 * ����ע����Ҫ����Microsoft.Office.Interop.Excel.dll��COM�����Microsoft Office 16.0 Object Library
 *        office�汾��ͬ�����õİ汾Ҳ��ͬ
 * 
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// MicrosoftExcelHelper ��ժҪ˵����һ��C#����Excel�࣬���ܱȽ�ȫ��
    /// ��Ҫ����Microsoft.Office.Interop.Excel.dll��COM�����Microsoft Office 16.0 Object Library
    /// office�汾��ͬ�����õİ汾Ҳ��ͬ
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
        /// �����1��1��ʼ
        /// </summary>
        public MicrosoftExcelHelper()
        {

        }
        /// <summary>
        /// ����һ��Excel����
        /// </summary>
        public void Create()
        {
            _app = new Excel.Application();
            _wbs = _app.Workbooks;
            _wb = _wbs.Add(true);
        }
        /// <summary>
        /// ��һ��Excel�ļ�
        /// </summary>
        /// <param name="fileName">�ļ���ַ</param>
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
        /// //��ȡһ��������
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <returns></returns>
        public Excel.Worksheet GetSheet(string wsn)
        {
            Excel.Worksheet s = (Excel.Worksheet)_wb.Worksheets[wsn];
            return s;
        }
        /// <summary>
        /// ���һ��������
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <returns></returns>
        public Excel.Worksheet AddSheet(string wsn)
        {
            Excel.Worksheet s = (Excel.Worksheet)_wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = wsn;
            return s;
        }
        /// <summary>
        /// ɾ��һ��������
        /// </summary>
        /// <param name="wsn"></param>
        public void DelSheet(string wsn)
        {
            ((Excel.Worksheet)_wb.Worksheets[wsn]).Delete();
        }
        /// <summary>
        /// ������һ��������
        /// </summary>
        /// <param name="oldSheetName">ԭ������������</param>
        /// <param name="newSheetName">�¹���������</param>
        /// <returns></returns>
        public Excel.Worksheet ReNameSheet(string oldSheetName, string newSheetName)
        {
            Excel.Worksheet s = (Excel.Worksheet)_wb.Worksheets[oldSheetName];
            s.Name = newSheetName;
            return s;
        }
        /// <summary>
        /// ������һ��������
        /// </summary>
        /// <param name="sheet">ԭ���������</param>
        /// <param name="newSheetName">�¹���������</param>
        /// <returns></returns>
        public Excel.Worksheet ReNameSheet(Excel.Worksheet sheet, string newSheetName)
        {

            sheet.Name = newSheetName;

            return sheet;
        }
        /// <summary>
        /// ���õ�Ԫ���ֵ
        /// </summary>
        /// <param name="ws">���������</param>
        /// <param name="x">X��</param>
        /// <param name="y">Y��</param>
        /// <param name="value">value ֵ</param>
        public void SetCellValue(Excel.Worksheet ws, int x, int y, object value)
        {
            if (value == null)
            {
                value = "";
            }
            ws.Cells[x, y] = value;
        }
        /// <summary>
        /// ���õ�Ԫ���ֵ
        /// </summary>
        /// <param name="wsn">�����������</param>
        /// <param name="x">X��</param>
        /// <param name="y">Y��</param>
        /// <param name="value">value ֵ</param>
        public void SetCellValue(string wsn, int x, int y, object value)
        {
            if (value == null)
            {
                value = "";
            }
            GetSheet(wsn).Cells[x, y] = value;
        }

        /// <summary>
        /// ����һ����Ԫ������ԣ����壬��С����ɫ�����뷽ʽ
        /// </summary>
        /// <param name="ws">���������</param>
        /// <param name="startx">��ʼx��</param>
        /// <param name="starty">��ʼy��</param>
        /// <param name="endx">����x��</param>
        /// <param name="endy">����y��</param>
        /// <param name="size">�����С</param>
        /// <param name="name">�����ͺ�</param>
        /// <param name="color">��ɫ</param>
        /// <param name="horizontalAlignment">���뷽ʽ</param>
        /// <param name="bold">�Ƿ�Ӵ�</param>
        public void SetCellProperty(Excel.Worksheet ws, int startx, int starty, int endx, int endy, int size, string name, Excel.Constants color, Excel.Constants horizontalAlignment, bool bold)

        {
            //name = "����";
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
        /// ����һ����Ԫ������ԣ���С�����壬��ɫ�����뷽ʽ
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <param name="startx">��ʼx��</param>
        /// <param name="starty">��ʼy��</param>
        /// <param name="endx">����x��</param>
        /// <param name="endy">����y��</param>
        /// <param name="size">�����С�磺12</param>
        /// <param name="name">���������磺����</param>
        /// <param name="color">��ɫ����</param>
        /// <param name="horizontalAlignment">���뷽ʽ</param>
        /// <param name="bold">�Ƿ�Ӵ�</param>
        public void SetCellProperty(string wsn, int startx, int starty, int endx, int endy, int size, string name, ColorIndex color, Excel.Constants horizontalAlignment, bool bold)
        {
            //name = "����";
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
        /// ��Ԫ�񱳾�ɫ����䷽ʽ
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="color">��ɫ����</param>
        public void SetCellsBackColor(string wsn, int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range range = ws.Range[ws.Cells[startRow, startColumn], ws.Cells[endRow, endColumn]];
            range.Interior.ColorIndex = color;
        }

        /// <summary>
        /// ��Ԫ��߿�
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="lineStyle">�߿���ʽ</param>
        public void SetCellsBorders(string wsn, int startRow, int startColumn, int endRow, int endColumn, int lineStyle)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range range = ws.Range[ws.Cells[startRow, startColumn], ws.Cells[endRow, endColumn]];
            range.Borders.LineStyle = lineStyle;
        }

        /// <summary>
        /// �ϲ���Ԫ��
        /// </summary>
        /// <param name="ws">���������</param>
        /// <param name="x1">��ʼ��</param>
        /// <param name="y1">��ʼ��</param>
        /// <param name="x2">������</param>
        /// <param name="y2">������</param>
        public void UniteCells(Excel.Worksheet ws, int x1, int y1, int x2, int y2)

        {
            ws.Range[ws.Cells[x1, y1], ws.Cells[x2, y2]].Merge(Type.Missing);
        }
        /// <summary>
        /// �ϲ���Ԫ��
        /// </summary>
        /// <param name="ws">����������</param>
        /// <param name="x1">��ʼ��</param>
        /// <param name="y1">��ʼ��</param>
        /// <param name="x2">������</param>
        /// <param name="y2">������</param>
        public void UniteCells(string ws, int x1, int y1, int x2, int y2)

        {
            GetSheet(ws).Range[GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]].Merge(Type.Missing);
            GetSheet(ws).Range[GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]].HorizontalAlignment = Excel.Constants.xlCenter;

        }

        /// <summary>
        /// ���ڴ������ݱ����뵽Excelָ���������ָ��λ�� Ϊ��ʹ��ģ��ʱ���Ƹ�ʽʱʹ��
        /// </summary>
        /// <param name="dt">����</param>
        /// <param name="wsn">����������</param>
        /// <param name="startX">��ʼx����</param>
        /// <param name="startY">��ʼy����</param>
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
        /// ���ڴ������ݱ����뵽Excelָ���������ָ��λ��
        /// </summary>
        /// <param name="dt">����</param>
        /// <param name="ws">���������</param>
        /// <param name="startX">��ʼx����</param>
        /// <param name="startY">��ʼy����</param>
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
        /// ���ڴ������ݱ����ӵ�Excelָ���������ָ��λ��
        /// </summary>
        /// <param name="dt">����</param>
        /// <param name="wsn">����������</param>
        /// <param name="startX">��ʼx����</param>
        /// <param name="startY">��ʼy����</param>
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
        /// ���ڴ������ݱ����ӵ�Excelָ���������ָ��λ��
        /// </summary>
        /// <param name="dt">����</param>
        /// <param name="ws">���������</param>
        /// <param name="startX">��ʼx����</param>
        /// <param name="startY">��ʼy����</param>
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
        /// ����ͼƬ����
        /// </summary>
        /// <param name="filename">ͼƬ��ַ</param>
        /// <param name="wsn">����������</param>
        public void InsertPictures(string filename, string wsn)

        {
            GetSheet(wsn).Shapes.AddPicture(filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            //��������ֱ�ʾλ��
        }
        /// <summary>
        /// ����ͼƬ����
        /// </summary>
        /// <param name="filename">ͼƬ��ַ</param>
        /// <param name="wsn">����������</param>
        /// <param name="height">�߶�</param>
        /// <param name="width">���</param>
        public void InsertPictures(string filename, string wsn, int height, int width)

        {
            GetSheet(wsn).Shapes.AddPicture(filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            GetSheet(wsn).Shapes.Range[Type.Missing].Height = height;
            GetSheet(wsn).Shapes.Range[Type.Missing].Width = width;
        }
        /// <summary>
        /// ����ͼƬ����
        /// </summary>
        /// <param name="filename">ͼƬ��ַ</param>
        /// <param name="wsn">����������</param>
        /// <param name="left">���λ��</param>
        /// <param name="top">����λ��</param>
        /// <param name="height">�߶�</param>
        /// <param name="width">���</param>
        public void InsertPictures(string filename, string wsn, int left, int top, int height, int width)
        {
            GetSheet(wsn).Shapes.AddPicture(filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            GetSheet(wsn).Shapes.Range[Type.Missing].IncrementLeft(left);
            GetSheet(wsn).Shapes.Range[Type.Missing].IncrementTop(top);
            GetSheet(wsn).Shapes.Range[Type.Missing].Height = height;
            GetSheet(wsn).Shapes.Range[Type.Missing].Width = width;
        }
        /// <summary>
        /// ����ͼ�����
        /// </summary>
        /// <param name="chartType">ͼ������</param>
        /// <param name="wsn">����������</param>
        /// <param name="dataSourcesX1">������Դ��ʼx��</param>
        /// <param name="dataSourcesY1">������Դ��ʼy��</param>
        /// <param name="dataSourcesX2">������Դ����x��</param>
        /// <param name="dataSourcesY2">������Դ����y��</param>
        /// <param name="chartDataType">ͼ����������</param>
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
        /// �����ĵ�
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
        /// �ĵ����Ϊ
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
        /// �ر�һ��Excel�������ٶ���
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
        /// ������������
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <returns>rowsCount: �����,colsCount: �����</returns>
        public (int rowsCount, int colsCount) GetMaxCount(string wsn)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            int rowsCount = ws.UsedRange.Rows.Count;
            int colsCount = ws.UsedRange.Columns.Count;
            return (rowsCount, colsCount);
        }


        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="wsn">����������</param>
        public int GetMaxRowsCount(string wsn)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            return ws.UsedRange.Rows.Count;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="wsn">����������</param>
        public int GetMaxColsCount(string wsn)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            return ws.UsedRange.Columns.Count;
        }
        /// <summary>
        /// ��õ�Ԫ���ı�
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <param name="x">x��</param>
        /// <param name="y">y��</param>
        public string GetCellsValue(string wsn, int x, int y)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            return Convert.ToString(ws.Range[ws.Cells[x, y], ws.Cells[x, y]].Value2);
        }

        /// <summary>
        /// ָ��λ�ò���һ�У���������
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <param name="x">ָ��x��</param>
        public void InsertRows(string wsn, int x)
        {
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range xlsRows = (Microsoft.Office.Interop.Excel.Range)ws.Rows[x, Type.Missing];
            xlsRows.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Type.Missing);
        }
        /// <summary>
        /// ָ��λ�ò���һ�У���������
        /// </summary>
        /// <param name="wsn">����������</param>
        /// <param name="y">ָ��y��</param>
        public void InsertColumns(string wsn, int y)
        {
            object misValue = Type.Missing;
            Excel.Worksheet ws = GetSheet(wsn);
            Excel.Range xlsColumns = (Microsoft.Office.Interop.Excel.Range)ws.Columns[y, misValue];
            xlsColumns.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, misValue);
        }
        /// <summary>
        /// ������ɫ����,�Ծ�Excel����ɫ��
        /// </summary>
        public enum ColorIndex
        {
            ��ɫ = -4142,
            �Զ� = -4105,
            ��ɫ = 1,
            ��ɫ = 53,
            ��� = 52,
            ���� = 51,
            ���� = 49,
            ���� = 11,
            ���� = 55,
            ��ɫ80 = 56,
            ��� = 9,
            ��ɫ = 46,
            ��� = 12,
            ��ɫ = 10,
            ��ɫ = 14,
            ��ɫ = 5,
            ���� = 47,
            ��ɫ50 = 16,
            ��ɫ = 3,
            ǳ��ɫ = 45,
            ���ɫ = 43,
            ���� = 50,
            ˮ��ɫ = 42,
            ǳ�� = 41,
            ������ = 13,
            ��ɫ40 = 48,
            �ۺ� = 7,
            ��ɫ = 44,
            ��ɫ = 6,
            ���� = 4,
            ���� = 8,
            ���� = 33,
            ÷�� = 54,
            ��ɫ25 = 15,
            õ��� = 38,
            ��ɫ = 40,
            ǳ�� = 36,
            ǳ�� = 35,
            ǳ���� = 34,
            ���� = 37,
            ���� = 39,
            ��ɫ = 2
        }

        /// <summary>
        /// HTML��CSV��TEXT��EXCEL��XML
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
