//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class MenuAccess
    {
        public System.Guid AccessID { get; set; }
        public Nullable<System.Guid> MenuID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Menu Menu { get; set; }
        public virtual User User { get; set; }
    }
}
