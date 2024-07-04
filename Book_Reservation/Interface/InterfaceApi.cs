using Book_Reservation.Interface.AdminResult;
using Book_Reservation.Interface.BookResult;
using Book_Reservation.Interface.jwt;
using Book_Reservation.Interface.UserResult;

namespace Book_Reservation.Interface
{
    public interface InterfaceApi
    {
        Task<Resultloginitem> FloginAdminRama(Adminparam adminparams);
        Task<Resultuseritem> FloginUser(Userparam userparams);
        Task<AuthResult> GetToken(UserPass userpass);
        Task<Useritem> GetUserInfo(string token);
        Task LineNotifyAsync(string lineToken, string message);
        Task<Resultbookitem> Freceivebook(Bookparam bookparams);
    }
}
