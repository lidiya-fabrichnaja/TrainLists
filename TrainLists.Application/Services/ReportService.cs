using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using TrainLists.Application.Contracts;
using TrainLists.Application.Models.Export;

namespace TrainLists.Application.Services
{
    public interface IReportService
    {
        byte[] CreateXlsx(ReportModel model,string dirPath);

    }

    public class ReportService : IService, IReportService
    {
        public ReportService()
        {
            
        }

        public byte[] CreateXlsx(ReportModel model, string dirPath)
        {

            var filePath = Path.Combine(dirPath,"NL_Template.xlsx");
            FileInfo existingFile = new FileInfo(filePath);

            using(var package = new ExcelPackage(existingFile))
            {
                var worksheet = package.Workbook.Worksheets[0];

                worksheet.Cells["C3"].Value = model.TrainNumber;
                worksheet.Cells["C4"].Value = model.RailwayNumber; 
                worksheet.Cells["E3"].Value = model.Station;
                worksheet.Cells["E4"].Value = model.Date.Date;

                var cell = worksheet.Cells["E4"];

                var rowsCount = model.Rows.Count;
                var rowsLimit = rowsCount +7;

                int n = 0;

                for (int row = 7; row < rowsLimit; row++)
                {
                   if(n >= rowsCount) break;

                    var rowModel = model.Rows[n];

                    worksheet.Cells[row,1].Value = rowModel.Number;
                    worksheet.Cells[row,2].Value = rowModel.CarNumber;
                    worksheet.Cells[row,3].Value = rowModel.InvoiceNum;
                    worksheet.Cells[row,4].Value = rowModel.DateDeparture;
                    worksheet.Cells[row,5].Value = rowModel.FreightEtsngName;
                    worksheet.Cells[row,6].Value = rowModel.FreightTotalWeightT;
                    worksheet.Cells[row,7].Value = rowModel.LastOperationName;

                    n++;
                }

                var groupCount = model.GroupRows.Count;
                var groupLimit = groupCount + rowsLimit;

                n = 0;

                for (int row = rowsLimit; row < groupLimit; row++)
                {
                    if(n >= groupCount) break;                         

                    var rowGroup = model.GroupRows[n];

                    worksheet.Cells[row,2].Value = rowGroup.CountRC;
                    worksheet.Cells[row,5].Value = rowGroup.FreightEtsngName;
                    worksheet.Cells[row,6].Value = rowGroup.TotalWeight;

                    n++;
                }

                int all = 0;
                int cargo = groupCount;
                double allTotalWeight = 0;

                foreach (var gr in model.GroupRows)
                {
                    all += gr.CountRC;
                    allTotalWeight += gr.TotalWeight;
                }

                
                
                worksheet.Cells[$"A{groupLimit}:B{groupLimit}"].Merge = true;
                worksheet.Cells[$"A{groupLimit}"].Value = $"Всего:    {all}";
                worksheet.Cells[$"E{groupLimit}"].Value = cargo;
                worksheet.Cells[$"F{groupLimit}"].Value = allTotalWeight;
                
                worksheet.Cells[$"A7:G{groupLimit}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[$"A7:G{groupLimit}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[$"A7:G{groupLimit}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[$"A7:G{groupLimit}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells[$"A7:G{groupLimit}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{rowsLimit}:G{groupLimit}"].Style.Font.Bold = true;

                worksheet.Cells[$"F7:F{groupLimit}"].Style.Numberformat.Format = "#,##0.00";
                
                return package.GetAsByteArray();
            }

        }
        
    }
}