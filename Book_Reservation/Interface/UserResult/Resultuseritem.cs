using Book_Reservation.Interface.AdminResult;

namespace Book_Reservation.Interface.UserResult
{
    public class Resultuseritem
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public PersonResponse personResponse { get; set; }
    }
    public class PersonResponse
    {
        public int PersonId { get; set; }

        public string? PersonCode { get; set; }

        public string? Password { get; set; }

        public string? PersonName { get; set; }

        public string? PersonTel { get; set; }

        public string? PersonAddress { get; set; }

        public int? RoleId { get; set; }

        public string? RoleName { get; set; }
    }
}
