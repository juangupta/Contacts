using CheckTicketJG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckTicketJG.Services
{
    public class ApiService
    {
        public async Task<Response> Login(string urlBase, string servicePrefix,
                                            string controller, User user)
        {
            try
            {
                var request = JsonConvert.SerializeObject(user);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var userLog = JsonConvert.DeserializeObject<User>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Login OK",
                    Result = userLog,

                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

    }
}
