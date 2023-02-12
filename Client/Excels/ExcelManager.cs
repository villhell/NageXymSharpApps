using AntDesign.TableModels;
using ClosedXML.Excel;
using nagexymsharpweb.Models;
using System.Reflection;

namespace nagexymsharpweb.Excels
{
    /// <summary>
    /// エクセルを管理するクラス
    /// </summary>
    public class ExcelManager
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        private string _fileName;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExcelManager(string fileName)
        {
            _fileName = fileName;
        }

        public async Task<List<DataItem>> ReadExcelFileAsync()
        {
            var excelRowDataItems = new List<DataItem>();
            try
            {
                await Task.Run(() =>
                {
                    // 行数、先頭行はヘッダーなので2行目から解析する
                    int row = 2;
                    int key = 1;
                    using (var wb = new XLWorkbook(_fileName))
                    {
                        foreach(var ws in wb.Worksheets)
                        {
                            while (row <= ws.LastRowUsed().RowNumber())
                            {
                                var rowdataItem = new DataItem
                                {
                                    Key = key.ToString(),
                                    Check = string.Empty,
                                    Name = ws.Cell(row, 1).GetString(),
                                    Twitter = ws.Cell(row, 2).GetString(),
                                    Namespace = string.Empty,
                                    Address = ws.Cell(row, 5).GetString(),
                                    Xym = ws.Cell(row, 7).GetDouble(),
                                    Message = ws.Cell(row, 7).GetString()
                                };

                                row++;
                                key++;
                                excelRowDataItems.Add(rowdataItem);
                            }
                        }
                    };
                });
            }
            catch (Exception)
            {
                throw;
            }
            return excelRowDataItems;
        }
    }
}
