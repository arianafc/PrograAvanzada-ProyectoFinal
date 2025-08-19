using iText.Layout.Element;
using ProyectoFinal.EF;

namespace ProyectoFinal.Models
{
    public class Usuario
    {
        public int ID_USUARIO { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Correo { get; set; }
        public string Contrasenna { get; set; }
        public string ContrasennaAnterior { get; set; }
        public string ConfirmarContrasenna { get; set; }
        public int IdRol {  get; set; }
        public int IdEstado
        {
            get; set;   
        
        }
    }
}