using Book_Reservation.Interface.AdminResult;
using Book_Reservation.Interface.BookResult;
using Book_Reservation.Interface.jwt;
using Book_Reservation.Interface.UserResult;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Book_Reservation.Interface
{
    public class DataApi : InterfaceApi
    {
        public async Task<Resultloginitem> FloginAdminRama(Adminparam adminparams)
        {
            Resultloginitem resultloginitems = new Resultloginitem();

            try
            {
                HttpContent content = null;

                string Apiurl = "http://wsback.rama.mahidol.ac.th/VerifyUser/api/Rama/ITLogin";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Apiurl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(adminparams);
                    content = new StringContent(json, Encoding.UTF8, "application/json");

                    var result1 = await client.PostAsync(Apiurl, content).ConfigureAwait(false);

                    var responseStr = result1.Content.ReadAsStringAsync();
                    resultloginitems = JsonConvert.DeserializeObject<Resultloginitem>(await responseStr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return resultloginitems;
        }

        public async Task<Resultuseritem> FloginUser(Userparam userparams)
        {
            Resultuseritem resultuseritems = new Resultuseritem();

            try
            {
                HttpContent content = null;

                string Apiurl = "http://localhost/WebAPI/api/Persons";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Apiurl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(userparams);
                    content = new StringContent(json, Encoding.UTF8, "application/json");

                    var result1 = await client.PostAsync(Apiurl, content).ConfigureAwait(false);

                    var responseStr = await result1.Content.ReadAsStringAsync();
                    resultuseritems = JsonConvert.DeserializeObject<Resultuseritem>(responseStr);
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"An error occurred during FloginUser: {ex.Message}");
            }
            return resultuseritems;
        }

        public async Task<AuthResult> GetToken(UserPass userpass)
        {
            AuthResult resultToken = new AuthResult();

            try
            {
                HttpContent content = null;
                string Url = "http://localhost/WebAPI/api/Authentication/login"; //แก้ link นี้
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(userpass);
                    content = new StringContent(json, Encoding.UTF8, "application/json");

                    var result1 = await client.PostAsync(Url, content).ConfigureAwait(false);

                    var tokenValue = await result1.Content.ReadAsStringAsync();
                    resultToken = JsonConvert.DeserializeObject<AuthResult>(tokenValue);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return resultToken;
        }

        public async Task<Useritem> GetUserInfo(string token)
        {
            Useritem ResultUser = new Useritem();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://localhost/WebAPI/api/Authentication/restricted");

                request.Method = "GET";
                request.ContentType = "application/json"; // Assuming you're sending JSON data
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var responseString = streamReader.ReadToEnd();
                        ResultUser = JsonConvert.DeserializeObject<Useritem>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Useritem()
                {
                    PersonCode = "ไม่ให้ผ่าน"
                };
            }

            return ResultUser;
        }

        public async Task LineNotifyAsync(string lineToken, string message)
        {
            try
            {
                message = System.Web.HttpUtility.UrlEncode(message, Encoding.UTF8);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + lineToken);
                    var content = new StringContent("message=" + message, Encoding.UTF8, "application/x-www-form-urlencoded");
                    var response = await client.PostAsync("https://notify-api.line.me/api/notify", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Failed to send Line Notify notification. Response: " + responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<Resultbookitem> Freceivebook(Bookparam bookparams)
        {
            Resultbookitem resultbookitem = new Resultbookitem();

            try
            {
                HttpContent content = null;

                string Apiurl = "http://10.6.165.181/WebAPI/api/Books";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Apiurl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(bookparams);
                    content = new StringContent(json, Encoding.UTF8, "application/json");

                    var result1 = await client.PostAsync(Apiurl, content).ConfigureAwait(false);

                    var responseStr = await result1.Content.ReadAsStringAsync();
                    resultbookitem = JsonConvert.DeserializeObject<Resultbookitem>(responseStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during Freceivebook: {ex.Message}");
                throw;
            }
            return resultbookitem;
        }
    }
}
