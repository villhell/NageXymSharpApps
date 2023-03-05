using ClosedXML.Excel;
using nagexymsharpweb.Models;

namespace nagexymsharpweb.Excels
{
    /// <summary>
    /// エクセルを管理するクラス
    /// </summary>
    public class ExcelManager
    {
        #region 列番号
        /// <summary>
        /// 列：名前
        /// </summary>
        private const int COL_NAME = 1;
        /// <summary>
        /// 列：Twitter
        /// </summary>
        private const int COL_TWITTER = 2;
        /// <summary>
        /// 列：アドレス/ネームスペース
        /// </summary>
        private const int COL_ADDRESS_NAMESPACE = 5;
        /// <summary>
        /// 列：モザイク
        /// </summary>
        private const int COL_MOSAIC = 7;
        /// <summary>
        /// 列：数量
        /// </summary>
        private const int COL_AMOUNT = 8;
        /// <summary>
        /// 列：メッセージ
        /// </summary>
        private const int COL_MESSAGE = 9;
        /// <summary>
        /// 列：Check
        /// </summary>
        private const int COL_CHECK = 10;
        #endregion

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

        /// <summary>
        /// Excelファイルを読込む
        /// </summary>
        /// <returns></returns>
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
                        foreach (var ws in wb.Worksheets)
                        {
                            while (row <= ws.LastRowUsed().RowNumber())
                            {
                                var rowdataItem = new DataItem
                                {
                                    Key = key.ToString(),
                                    Check = string.Empty,
                                    Name = ws.Cell(row, COL_NAME).GetString(),
                                    Twitter = ws.Cell(row, COL_TWITTER).GetString(),
                                    AddressNamespace = string.Empty,
                                    Address = ws.Cell(row, COL_ADDRESS_NAMESPACE).GetString(),
                                    MosaicNamespace = ws.Cell(row, COL_MOSAIC).GetString(),
                                    MosaicId = string.Empty,
                                    Amount = ws.Cell(row, COL_AMOUNT).GetDouble(),
                                    Message = ws.Cell(row, COL_MESSAGE).GetString()
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
