//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MRS_web.Models.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class TimeSpan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.TimeSpan TimeStart { get; set; }
        public System.TimeSpan TimeEnd { get; set; }
    
        public virtual Tariff Tariff { get; set; }
    }
}
