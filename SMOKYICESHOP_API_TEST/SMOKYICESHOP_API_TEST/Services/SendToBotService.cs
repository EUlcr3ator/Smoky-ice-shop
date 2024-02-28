using SMOKYICESHOP_API_TEST.DTO.Orders;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Services
{
    public class SendToBotService
    {
        private readonly HttpClient _httpClient;

        public SendToBotService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(configuration["BotApiUrl"]);
        }

        public bool SendOrderToApi(OrderDTO order)
        {
            JsonContent jsonContent = JsonContent.Create(order);
            HttpResponseMessage httpResponseMessage = _httpClient.PostAsync($"/api/bot/new-order", jsonContent).Result;
            return httpResponseMessage.IsSuccessStatusCode;
         
            return false;
        }
    }
}
