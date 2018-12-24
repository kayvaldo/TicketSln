using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRC_WEB.Models
{
    public partial class ScheduleDayOfTheWeek
    {
        public string CheckedStatus()
        {
            var status = IsActive ? "'checked'" : string.Empty;
            return status;
        }
    }
}