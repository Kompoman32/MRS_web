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
    
    public partial class Document
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public System.DateTime SigningDate { get; set; }
    
        public virtual Meter Meter { get; set; }
    }
}
