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
    
    public partial class ACTIVIDADES_TB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACTIVIDADES_TB()
        {
            this.USUARIO_ACTIVIDAD_TB = new HashSet<USUARIO_ACTIVIDAD_TB>();
        }
    
        public int ID_ACTIVIDAD { get; set; }
        public string DESCRIPCION { get; set; }
        public System.DateTime FECHA { get; set; }
        public decimal PRECIO_BOLETO { get; set; }
        public Nullable<int> TICKETS_DISPONIBLES { get; set; }
        public Nullable<int> TICKETS_VENDIDOS { get; set; }
        public int ID_ESTADO { get; set; }
        public string IMAGEN { get; set; }
        public string TIPO { get; set; }
    
        public virtual ESTADOS_TB ESTADOS_TB { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_ACTIVIDAD_TB> USUARIO_ACTIVIDAD_TB { get; set; }
    }
}
