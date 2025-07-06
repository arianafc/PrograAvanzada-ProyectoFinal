namespace ProyectoFinal.Models
{
    public class Usuario
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }

        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }

        public string Correo { get; set; }
        public string Contrasenna { get; set; }
        public string ContrasennaAnterior { get; set; }

        public string ConfirmarContrasenna { get; set; }

        public int idRol {  get; set; }

        public int idEstado { 
        get; set;   
        
        }
    }
}