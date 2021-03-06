//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NRC_WEB.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Schedule()
        {
            this.ScheduleDayOfTheWeeks = new HashSet<ScheduleDayOfTheWeek>();
            this.ScheduleStations = new HashSet<ScheduleStation>();
            this.ScheduleTravelClasses = new HashSet<ScheduleTravelClass>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int DurationInMinutes { get; set; }
        public Nullable<int> TrainTypeId { get; set; }
        public Nullable<System.TimeSpan> TimeOfDay { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScheduleDayOfTheWeek> ScheduleDayOfTheWeeks { get; set; }
        public virtual TrainType TrainType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScheduleStation> ScheduleStations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScheduleTravelClass> ScheduleTravelClasses { get; set; }
    }
}
