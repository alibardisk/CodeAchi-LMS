using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using System;

using System.Management;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CodeAchi_Library_Management_System
{
    class APIRequest
    {
        string hostUrl = "https://lic.codeachi.com";
        public async Task<string> GenerateUniqueId(string jsonString)
        {
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/reg/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseBody;
        }

        public async Task<string> SendOTP(string jsonString)
        {
            string responseResult = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/otp/send-email/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseResult;
        }

        public async Task<string> VerifyOTP(string jsonString)
        {
            string responseResult = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/otp/verify-email/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseResult;
        }

        public async Task<string> UpdateClient(string jsonString)
        {
            string responseResult = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/reg/update/client/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseResult;
        }

        public async Task<string> UpdateOrganization(string jsonString)
        {
            string responseResult = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/reg/update/organization/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseResult;
        }

        public async Task<string>GetStatus(string jsonString)
        {
            string responseResult = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/reg/status/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseResult;
        }

        public async Task<string> UpdateUsage(string jsonString)
        {
            string responseResult = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/reg/update/usage/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseResult;
        }

        public async Task<string> ActivateLicense(string jsonString)
        {
            string responseResult = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Make a POST request with the data
                    HttpResponseMessage response = await client.PostAsync(hostUrl + "/api/act/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        // Log the response content if available
                        if (response.Content != null)
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response content: {errorResponse}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return responseResult;
        }

        public string GetHardwareId()
        {
            //// Get UUID ID
            string uUID = GetSystemUUID();
            if (uUID == "")
            {
                //// Get cpuID ID
                uUID = "cpuID-" + GetCpuId();
            }
            else
            {
                uUID = "uUID-" + uUID;
            }
            return uUID;
        }

        static string GetCpuId()
        {
            string result = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    result = obj["ProcessorId"].ToString();
                    break; // assuming there's only one CPU
                }
            }
            return result;
        }

        static string GetSystemUUID()
        {
            string result = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    result = obj["UUID"].ToString();
                    break; // assuming there's only one UUID
                }
            }
            return result;
        }
    }
}
