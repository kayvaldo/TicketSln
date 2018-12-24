using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRC_WEB.Models.ViewModels
{
    public class TrainTypeViewModel
    {
        public TrainType TrainType { get; set; }
        public List<TrainTypeTravelClass> TrainTypeTravelClasses { get; set; }
    }
}