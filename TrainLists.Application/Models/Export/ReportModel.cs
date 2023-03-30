using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainLists.Application.Models.Export
{
    public class ReportModel
    {
        public int TrainNumber { get; set; }
        public int RailwayNumber { get; set; }
        public string Station { get; set; }
        public DateTime Date { get; set; }

        public List<Row> Rows { get; set;} = new List<Row>();
        public List<GroupRow> GroupRows { get; set; } = new List<GroupRow>();

        
    }

    public class Row
    {
        public int Number { get; set; }
        public int CarNumber { get; set; }
        public string InvoiceNum { get; set; }  
        public string DateDeparture { get; set; }
        public string FreightEtsngName { get; set; }    
        public double FreightTotalWeightT { get; set; }
        public string LastOperationName { get; set; }
    }

    public class GroupRow
    {
        public int CountRC { get; set; }
        public string FreightEtsngName { get; set; }  
        public double TotalWeight { get; set; }

    }

}