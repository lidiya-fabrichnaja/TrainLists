using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainLists.Application.Contracts;
using TrainLists.Infrastructure;
using TrainLists.Application.Models.Export;
using TrainLists.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace TrainLists.Application.Services
{

    public interface ISearchService
    {
        Task<AppResponce<ReportModel>> Search(int trainNumber);
    }
    public class SearchService : IService, ISearchService
    {
        private readonly ApplicationDbContext _ctx;

        private readonly Dictionary<int,string> _hbFreightEtsngName;

        private readonly Dictionary<int,string> _hbOperationName;

        

        public SearchService(ApplicationDbContext ctx)
        {
            _ctx = ctx;

            _hbFreightEtsngName = _ctx.HandBooks.Where(x=>x.Type == "FreightEtsngName").ToDictionary(x=> x.Key, y => y.Value );
            _hbOperationName = _ctx.HandBooks.Where(x=>x.Type == "OperationName").ToDictionary(x=> x.Key, y => y.Value );
        }

        public async Task<AppResponce<ReportModel>> Search(int trainNumber)
        {
            if(!await _ctx.TrainActivities.AnyAsync(x=>x.TrainNumber == trainNumber)) 
                return new AppResponce<ReportModel>(new AppError("Не найдено"));

            var trains = await _ctx.TrainActivities
                            .AsNoTracking()
                            .Include(x=>x.Route)
                            .Include(x=> x.TrainActivityDetails)
                            .ThenInclude(x=> x.Invoice)
                            .Where(x=>x.TrainNumber == trainNumber)
                            .OrderByDescending(x=>x.LastOperationDate)
                            .ToListAsync();
                            ;

            var train = trains[0];


            var model = new ReportModel();

            model.TrainNumber = train.TrainNumber;
            model.RailwayNumber = train.RailwayNumber;
            model.Date = train.LastOperationDate ?? new DateTime(1900,1,1);
            model.Station = train.LastStationName;  
            
            var details = train.TrainActivityDetails.OrderBy(x=>x.PositionInTrain);
            int n = 1;
            foreach (var detail in details)
            {
                model.Rows.Add(new Row{
                    Number = n,
                    CarNumber = detail.CarNumber,
                    InvoiceNum = detail.Invoice.Number,
                    DateDeparture = trains
                                        .Where(x=>x.TrainNumber == train.TrainNumber) 
                                        .Where(x=>x.RailwayNumber == train.RailwayNumber)
                                        .OrderBy(x=>x.LastOperationDate)
                                        .First()
                                        .LastOperationDate.GetValueOrDefault()
                                        .Date.ToString("dd:MM:yyyy"),
                    FreightEtsngName = _hbFreightEtsngName[detail.FreightEtsngKey],
                    FreightTotalWeightT = (double)detail.FreightTotalWeightKg/1000d,
                    LastOperationName = _hbOperationName[train.LastOperationKey]

                });

                n++;
            }

            var groups = details.GroupBy(x=>x.FreightEtsngKey);

            foreach (var group in groups)
            {
                double totalWeight = 0;
                foreach (var item in group)
                {
                    totalWeight += (double)item.FreightTotalWeightKg / 1000d;
                }
                    model.GroupRows.Add(new GroupRow{
                        CountRC = group.Count(),
                        FreightEtsngName = _hbFreightEtsngName[group.First().FreightEtsngKey],
                        TotalWeight = totalWeight
                    });
            }

            
            return new AppResponce<ReportModel>(model);

            
        }
        
    }
}