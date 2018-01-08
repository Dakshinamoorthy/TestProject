using System;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    internal class ExcelDataHelper
    {
        private int _spreadsheetColNo;
        private int _spreadsheetNo;
        private string _returnDomainNames;
        public int RowCount;
        internal string DataFletcherFromExcel(string type)
        {
            switch (EnumHelper.ParseEnum<EnumHelper.ExcelDataEnum>(type))
            {
                case EnumHelper.ExcelDataEnum.ProductionEnomTlDs:
                    _spreadsheetNo = 1;
                    _spreadsheetColNo = 1;
                    break;
                case EnumHelper.ExcelDataEnum.ProductionReTlDs:
                    _spreadsheetNo = 1;
                    _spreadsheetColNo = 3;
                    break;
                case EnumHelper.ExcelDataEnum.SandboxEnomTlDs:
                    _spreadsheetNo = 1;
                    _spreadsheetColNo = 5;
                    break;
                case EnumHelper.ExcelDataEnum.SandboxReTlDs:
                    _spreadsheetNo = 1;
                    _spreadsheetColNo = 7;
                    break;
                case EnumHelper.ExcelDataEnum.BsbEnomTlDs:
                    _spreadsheetNo = 1;
                    _spreadsheetColNo = 9;
                    break;
                case EnumHelper.ExcelDataEnum.BsbReTlDs:
                    _spreadsheetNo = 1;
                    _spreadsheetColNo = 11;
                    break;
                case EnumHelper.ExcelDataEnum.BannedDomains:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 1;
                    break;
                case EnumHelper.ExcelDataEnum.WhoisLookupDomains:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 3;
                    break;
                case EnumHelper.ExcelDataEnum.Creditcardnumbers:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 5;
                    break;
                case EnumHelper.ExcelDataEnum.AddressNameType:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 7;
                    break;
                case EnumHelper.ExcelDataEnum.UserFirstName:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 9;
                    break;
                case EnumHelper.ExcelDataEnum.UserLastName:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 11;
                    break;
                case EnumHelper.ExcelDataEnum.CountryNames:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 13;
                    break;
                case EnumHelper.ExcelDataEnum.EmailServer:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 15;
                    break;
                case EnumHelper.ExcelDataEnum.ExtendedTlds:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 17;
                    break;
                case EnumHelper.ExcelDataEnum.AddNewAddressFrom:
                    _spreadsheetNo = 2;
                    _spreadsheetColNo = 19;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("In Excel Data Helper class 'DataFletcherFromExcel' method dont have " + type + " case type in switch");
            }
            _returnDomainNames = GetDataFromExcel(_spreadsheetNo, _spreadsheetColNo);
            return _returnDomainNames;
        }
        protected string GetDataFromExcel(int spreadsheetNo, int spreadsheetColNo)
        {
            string returnValue;
            const string spreadSheetName = "UiConstantHelperList.xls";
            var projectSolutionDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", string.Empty);
            var file = new FileStream(@projectSolutionDirectory +"\\"+ spreadSheetName, FileMode.Open, FileAccess.ReadWrite);
            var workbook = new HSSFWorkbook(file);
            var sheet = (HSSFSheet)workbook.GetSheetAt(spreadsheetNo);
            for (var spreadsheetRowno = UiConstantHelper.TwoNumber; ; spreadsheetRowno++)
            {
                var row = sheet.GetRow(spreadsheetRowno);
                var cell = row.GetCell(spreadsheetColNo, MissingCellPolicy.RETURN_BLANK_AS_NULL);
                if (cell != null)
                {
                    RowCount++;
                    continue;
                }
                var rowRandomNo = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(spreadsheetRowno, 2);
                returnValue = sheet.GetRow(rowRandomNo).GetCell(spreadsheetColNo).StringCellValue;
              break;
            }
            return returnValue;
        }
    }
}