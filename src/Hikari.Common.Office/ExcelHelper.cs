using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Hikari.Common.Office;

/// <summary>
/// Excel 帮助类
/// </summary>
public class ExcelHelper
{
    /// <summary>
    /// 把DataTable的数据写入到指定的excel文件中
    /// </summary>
    /// <param name="sourceData">要写入的数据</param>
    /// <param name="sheetName">excel表中的sheet的名称，可以根据情况自己起</param>
    /// <param name="isWriteColumnName">是否写入DataTable的列名称</param>
    /// <param name="target">Excel文件的后缀名，默认2007以上版本</param>
    /// <returns>excel文件的二进制流</returns>
    public static byte[] DataTableToExcel(DataTable sourceData, string sheetName = "Sheet1", bool isWriteColumnName = true, string target = "xlsx")
    {
        // 新建工作簿
        IWorkbook workbook = target.ToLower() switch
        {
            "xls" => new HSSFWorkbook(),  // 2003版本的excel
            "xlsx" => new SXSSFWorkbook(),  // 2007版本的excel
            _ => new SXSSFWorkbook()
        };

        workbook.CreateSheet(sheetName);  //新建1个Sheet工作表
        ISheet sheetOne = workbook.GetSheet(sheetName); //获取名称为Sheet1的工作表

        int nameRow = 0;
        //指明需要写入列名，则写入DataTable的列名,第一行写入列名
        if (isWriteColumnName)
        {
            //sheet表创建新的一行,即第一行
            IRow columnNameRow = sheetOne.CreateRow(0); //0下标代表第一行
                                                        //进行写入DataTable的列名
            for (int colunmNameIndex = 0; colunmNameIndex < sourceData.Columns.Count; colunmNameIndex++)
            {
                columnNameRow.CreateCell(colunmNameIndex).SetCellValue(sourceData.Columns[colunmNameIndex].ColumnName);
            }

            nameRow++;
        }

        // 写入数据
        for (int i = 0; i < sourceData.Rows.Count; i++)
        {
            DataRow row = sourceData.Rows[i];
            IRow sheetRow = sheetOne.CreateRow(i + nameRow);
            for (int j = 0; j < row.ItemArray.Length; j++)
            {
                var item = row.ItemArray[j];
                ICell sheetCell = sheetRow.CreateCell(j);
                sheetCell.SetCellValue(item?.ToString());
            }
        }
        using MemoryStream stream = new MemoryStream();
        workbook.Write(stream);
        workbook.Close();

        return stream.ToArray();
    }
    /// <summary>
    /// 把二维数组的数据写入到指定的excel文件中
    /// </summary>
    /// <param name="sourceData">要写入的数据</param>
    /// <param name="sheetName">excel表中的sheet的名称，可以根据情况自己起</param>
    /// <param name="target">目标文件Excel文件的后缀名，默认2007以上版本</param>
    public static byte[] TwoArrayToExcel(List<List<string>> sourceData, string sheetName = "Sheet1", string target = "xlsx")
    {
        // 新建工作簿
        IWorkbook workbook = target.ToLower() switch
        {
            "xls" => new HSSFWorkbook(),  // 2003版本的excel
            "xlsx" => new SXSSFWorkbook(),  // 2007版本的excel
            _ => new SXSSFWorkbook()
        };

        workbook.CreateSheet(sheetName);  //新建1个Sheet工作表
        ISheet sheetOne = workbook.GetSheet(sheetName); //获取名称为Sheet1的工作表

        for (int i = 0; i < sourceData.Count; i++)
        {
            List<string> row = sourceData[i];
            IRow sheetRow = sheetOne.CreateRow(i);
            for (int j = 0; j < row.Count; j++)
            {
                var item = row[j];
                ICell sheetCell = sheetRow.CreateCell(j);
                sheetCell.SetCellValue(item);
            }
        }

        using MemoryStream stream = new MemoryStream();
        workbook.Write(stream);
        workbook.Close();

        return stream.ToArray();
    }

    /// <summary>
    /// 从Excel中读入数据到DataTable中
    /// </summary>
    /// <param name="sourceFileNamePath">Excel文件的路径</param>
    /// <param name="sheetName">excel文件中工作表名称</param>
    /// <param name="isHasColumnName">文件是否有列名</param>
    /// <returns>从Excel读取到数据的DataTable结果集</returns>
    public static DataTable? ExcelToDataTable(string sourceFileNamePath, string sheetName = "", bool isHasColumnName = true)
    {
        if (!File.Exists(sourceFileNamePath))
        {
            throw new ArgumentException("excel文件的路径不存在或者excel文件没有创建好");
        }

        if (string.IsNullOrEmpty(sheetName))
        {
            throw new ArgumentException("工作表sheet的名称不能为空");
        }

        //打开文件
        using FileStream fs = new FileStream(sourceFileNamePath, FileMode.Open, FileAccess.Read);
        //根据Excel文件的后缀名创建对应的workbook
        string ext = Path.GetExtension(sourceFileNamePath);
        // 新建工作簿
        IWorkbook workbook = ext.ToLower() switch
        {
            ".xls" => new HSSFWorkbook(fs),  // 2003版本的excel
            ".xlsx" => new XSSFWorkbook(fs),  // 2007版本的excel
            _ => new XSSFWorkbook(fs)
        };

        //获取工作表sheet
        ISheet sheet = string.IsNullOrWhiteSpace(sheetName) ? workbook.GetSheetAt(0) : workbook.GetSheet(sheetName);
        //获取不到，直接返回
        if (sheet == null) return null;

        //开始读取的行号
        int startReadRow = 0;
        DataTable targetTable = new DataTable();

        //表中有列名,则为DataTable添加列名
        if (isHasColumnName)
        {
            //获取要读取的工作表的第一行
            IRow columnNameRow = sheet.GetRow(0);   //0代表第一行
                                                    //获取该行的列数(即该行的长度)
            int cellLength = columnNameRow.LastCellNum;

            //遍历读取
            for (int columnNameIndex = 0; columnNameIndex < cellLength; columnNameIndex++)
            {
                //不为空，则读入
                if (columnNameRow.GetCell(columnNameIndex) != null)
                {
                    //获取该单元格的值
                    string cellValue = columnNameRow.GetCell(columnNameIndex).StringCellValue;
                    if (cellValue != null)
                    {
                        //为DataTable添加列名
                        targetTable.Columns.Add(new DataColumn(cellValue));
                    }
                }
            }
            startReadRow++;
        }

        // 开始读取sheet表中的数据
        //获取sheet文件中的行数
        int rowLength = sheet.LastRowNum;
        //遍历一行一行地读入
        for (int i = startReadRow; i < rowLength; i++)
        {
            //获取sheet表中对应下标的一行数据
            IRow currentRow = sheet.GetRow(i);   //RowIndex代表第RowIndex+1行

            if (currentRow == null) continue;  //表示当前行没有数据，则继续
                                               //获取第Row行中的列数，即Row行中的长度
            int currentColumnLength = currentRow.LastCellNum;

            //创建DataTable的数据行
            DataRow dataRow = targetTable.NewRow();
            //遍历读取数据
            for (int j = 0; j < currentColumnLength; j++)
            {
                //没有数据的单元格默认为空
                if (currentRow.GetCell(j) != null)
                {
                    dataRow[j] = currentRow.GetCell(j);
                }
            }
            //把DataTable的数据行添加到DataTable中
            targetTable.Rows.Add(dataRow);
        }

        //释放资源
        workbook.Close();

        return targetTable;
    }
    
    /// <summary>
    /// 从Excel中读入数据到DataTable中
    /// </summary>
    /// <param name="stream">Excel文件流</param>
    /// <param name="sheetName">excel文件中工作表名称</param>
    /// <param name="isHasColumnName">文件是否有列名</param>
    /// <returns>从Excel读取到数据的DataTable结果集</returns>
    public static DataTable? ExcelToDataTable(Stream stream, string sheetName = "", bool isHasColumnName = true)
    {
        // 新建工作簿
        IWorkbook workbook = new XSSFWorkbook(stream);

        //获取工作表sheet
        ISheet sheet = string.IsNullOrWhiteSpace(sheetName) ? workbook.GetSheetAt(0) : workbook.GetSheet(sheetName);
        //获取不到，直接返回
        if (sheet == null) return null;

        //开始读取的行号
        int startReadRow = 0;
        DataTable targetTable = new DataTable();

        //表中有列名,则为DataTable添加列名
        if (isHasColumnName)
        {
            //获取要读取的工作表的第一行
            IRow columnNameRow = sheet.GetRow(0);   //0 代表第一行
            //获取该行的列数(即该行的长度)
            int cellLength = columnNameRow.LastCellNum;

            //遍历读取
            for (int columnNameIndex = 0; columnNameIndex < cellLength; columnNameIndex++)
            {
                //不为空，则读入
                if (columnNameRow.GetCell(columnNameIndex) != null)
                {
                    //获取该单元格的值
                    string cellValue = columnNameRow.GetCell(columnNameIndex).StringCellValue;
                    if (cellValue != null)
                    {
                        //为DataTable添加列名
                        targetTable.Columns.Add(new DataColumn(cellValue));
                    }
                }
            }
            startReadRow++;
        }

        // 开始读取sheet表中的数据
        //获取sheet文件中的行数
        int rowLength = sheet.LastRowNum;
        //遍历一行一行地读入
        for (int i = startReadRow; i < rowLength; i++)
        {
            //获取sheet表中对应下标的一行数据
            IRow currentRow = sheet.GetRow(i);   //RowIndex代表第RowIndex+1行

            if (currentRow == null) continue;  //表示当前行没有数据，则继续
            //获取第Row行中的列数，即Row行中的长度
            int currentColumnLength = currentRow.LastCellNum;

            //创建DataTable的数据行
            DataRow dataRow = targetTable.NewRow();
            //遍历读取数据
            for (int j = 0; j < currentColumnLength; j++)
            {
                //没有数据的单元格默认为空
                if (currentRow.GetCell(j) != null)
                {
                    dataRow[j] = currentRow.GetCell(j);
                }
            }
            //把DataTable的数据行添加到DataTable中
            targetTable.Rows.Add(dataRow);
        }

        //释放资源
        workbook.Close();

        return targetTable;
    }
}
