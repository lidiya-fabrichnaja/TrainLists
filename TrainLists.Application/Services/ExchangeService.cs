using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using TrainLists.Application.Contracts;
using TrainLists.Application.Exceptions;
using TrainLists.Application.Models;
using TrainLists.Application.Models.Import;
using TrainLists.Infrastructure;
using TrainLists.Infrastructure.Models;

namespace TrainLists.Application.Services
{
    public interface IExchangeService
    {
        Task<AppResponce> ParseXml(string path);
    }
    public class ExchangeService : IService, IExchangeService
    {
        private readonly ApplicationDbContext _ctx;
        public ExchangeService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<AppResponce> ParseXml(string path)
        {
            if(!File.Exists(path))
                throw new OperationException(ErrorCode.NotFound,"Файл не найден");

            var serializer = new XmlSerializer(typeof(XmlDataFile));
            var xml = File.ReadAllText(path);

            XmlDataFile data = null;
            using (StringReader reader = new StringReader(xml))
            {
                data = serializer.Deserialize(reader) as XmlDataFile;
                if(data == null)
                    throw new OperationException(ErrorCode.InternalError,"Не удалось десериализовать файл");
            }

            await ProcessData(data);
           
            return new AppResponce(true);
            
        }

        private async Task ProcessData(XmlDataFile data)
        {
            await ExtractRoutes(data);

            await ExtractInvoices(data);
 
            await ExtractHandbookData(data);
            
            await ExtracTrainActivities(data);
        }

        private async Task ExtracTrainActivities(XmlDataFile data)
        {
            var trains = data.Rows.GroupBy(x=> new { x.TrainNumber, x.TrainIndexCombined,x.WhenLastOperation
                ,x.LastOperationName,x.LastStationName, x.FromStationName,x.ToStationName });
            
            foreach(var train in trains)
            {
                var lastOperationDate = DateTime.Parse(train.Key.WhenLastOperation);
                int railwayNumber = int.Parse(train.Key.TrainIndexCombined.Split("-")[1]);

                if(await _ctx.TrainActivities.AnyAsync(x=>x.TrainNumber == train.Key.TrainNumber 
                                                       && x.TrainIndexCombined == train.Key.TrainIndexCombined
                                                       && x.LastOperationDate == lastOperationDate) == false)
                {
                    var record = new TrainActivity
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "ExchangeService",
                        TrainNumber = train.Key.TrainNumber,
                        RailwayNumber = railwayNumber,
                        TrainIndexCombined = train.Key.TrainIndexCombined,
                        LastStationName = train.Key.LastStationName,
                        LastOperationDate = lastOperationDate,
                        LastOperationKey = _ctx.HandBooks.First(x=>x.Type == "OperationName" && x.Value == train.Key.LastOperationName).Key,
                        Route = _ctx.Routes.First(x=>x.From == train.Key.FromStationName && x.To == train.Key.ToStationName),
                        TrainActivityDetails = train.Select(detail=> new TrainActivityDetail
                        {
                            CarNumber = detail.CarNumber,
                            FreightTotalWeightKg = detail.FreightTotalWeightKg,
                            PositionInTrain = detail.PositionInTrain,
                            Invoice = _ctx.Invoices.First(x=>x.Number == detail.InvoiceNum),
                            FreightEtsngKey = _ctx.HandBooks.First(x=>x.Type == "FreightEtsngName" && x.Value == detail.FreightEtsngName).Key

                        }).ToList()

                    };

                    _ctx.TrainActivities.Add(record);
                    await _ctx.SaveChangesAsync();

                } 
            }
        }

        private async Task ExtractHandbookData(XmlDataFile data)
        {
            var operations = data.Rows.Select(x=>x.LastOperationName)
                .GroupBy(x=>x)
                .Select(x=>x.Key);
            await ProcessHandBook("OperationName",operations);

            var tmc = data.Rows.Select(x=>x.FreightEtsngName)
                .GroupBy(x=>x)
                .Select(x=>x.Key);
            await ProcessHandBook("FreightEtsngName",tmc);
        }

        private async Task ExtractInvoices(XmlDataFile data)
        {
            var invoices = data.Rows.Select(x=>x.InvoiceNum)
                .GroupBy(x=>x)
                .Select(x=>x.Key);
            foreach(var invoiceNumber in invoices)
            {
                if(await _ctx.Invoices.AnyAsync(x=>x.Number == invoiceNumber) == false){
                    _ctx.Invoices.Add(new Invoice
                    {
                        Number = invoiceNumber
                    });
                       await _ctx.SaveChangesAsync();
                }
            }
        }

        private async Task ExtractRoutes(XmlDataFile data)
        {
            var routes = data.Rows.Select(x=> new { x.FromStationName, x.ToStationName })
                    .GroupBy(x=> new {x.FromStationName,x.ToStationName })
                    .Select(x=>x.Key);

            foreach(var route in routes)
            {
                if(await _ctx.Routes.AnyAsync(x=>x.From == route.FromStationName && x.To == route.ToStationName) == false)
                {
                    _ctx.Routes.Add(new Route 
                    {
                        From = route.FromStationName, 
                        To = route.ToStationName
                    });
                    await _ctx.SaveChangesAsync();
                }
            }
        }

        private async Task ProcessHandBook(string type,IEnumerable<string> values)
        {
            foreach(var value in values) 
            {

                if(await _ctx.HandBooks.AnyAsync(x=>x.Type == type && x.Value == value) == false)
                {
                    int? lastId = await _ctx.HandBooks
                        .Where(x=>x.Type == type)
                        .OrderBy(x=>x.Key)
                           .Select(x=>x.Key)
                        .LastOrDefaultAsync();

                    lastId = (lastId ?? 0) + 1;

                    _ctx.HandBooks.Add(new HandBook 
                    { 
                        Type = type,
                        Key = lastId.Value,
                        Value = value
                    });
                    await _ctx.SaveChangesAsync();
                }
            }

        }

        
    }
}