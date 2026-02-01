namespace Inter.DAL.Dto.User
{
    public class ResponseUser : ResponseBaseService
    {
        public string User_Id { get; set; }
        public string Nombres { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string RolId { get; set; }
        public string NombreRol { get; set; }
    }
}
