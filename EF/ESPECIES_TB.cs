//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoFinal.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class ESPECIES_TB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ESPECIES_TB()
        {
            this.RAZAS_TB = new HashSet<RAZAS_TB>();
        }
    
        public int ID_ESPECIE { get; set; }
        public string NOMBRE { get; set; }
        public int ID_ESTADO { get; set; }
    
        public virtual ESTADOS_TB ESTADOS_TB { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RAZAS_TB> RAZAS_TB { get; set; }
    }
}
