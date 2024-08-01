using EventMatcha.BackgroundServiceCore.Features.SMS.Models;
using Microsoft.Extensions.Options;

namespace EventMatcha.BackgroundServiceCore.Features.SMS.Service
{
    public class SmsService
    {
        private readonly SMSLive247Options _smsOptions;
        private readonly HttpClient _httpClient;

        public SmsService(IOptions<SMSLive247Options> smsOptions, HttpClient httpClient)
        {
            _smsOptions = smsOptions.Value;
            _httpClient = httpClient;
        }

        public async Task<string> SendSmsAsync(string toPhoneNumber, string message)
        {
            var url = $"{_smsOptions.ApiUrl}/http/index.aspx?"
                      + "cmd=sendquickmsg"
                      + $"&owneremail={Uri.EscapeDataString(_smsOptions.OwnerEmail)}"
                      + $"&subacct={Uri.EscapeDataString(_smsOptions.SubAccount)}"
                      + $"&subacctpwd={Uri.EscapeDataString(_smsOptions.SubAccountPassword)}"
                      + $"&message={Uri.EscapeDataString(message)}"
                      + $"&sender={Uri.EscapeDataString(_smsOptions.Sender)}"
                      + $"&sendto={Uri.EscapeDataString(toPhoneNumber)}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                if (response.StartsWith("+"))
                {
                    return $"An error has occurred: [{response}].";
                }
                else
                {
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return $"Error: URL could not be opened. {ex.Message}";
            }
        }
    }
}
