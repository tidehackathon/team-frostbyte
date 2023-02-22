using System.Net;
using System.Runtime.InteropServices;

namespace Downloader.Commons
{
    public static class Utils
    {
        // This session cookie should be changed on every program running (fix: add user-agent to request which retrive the cookies)
        private static Cookie sessionCookie = new Cookie()
        {
            Name = "tide.act.nato.int-proxy-session",
            Value = "s%3AzmTMvD4cAeKs5I4mOtZRbXEvhCbnR5lq.6bP46svQoQIa1OJkfcfsxh9F9X5V%2FXXdarGNEEF90UE",
            Domain = "tide.act.nato.int",
            Path = "/",
            Expires = DateTime.Now.AddYears(3)
        };

        private const int maxDegreeOfParallelism = 20;
        private const int delay = 3;

        public static string CreateIfNotExist(string subDirectory, string outputDirectory)
        {
            string fullPath = Path.Combine(outputDirectory, subDirectory);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            return fullPath;
        }

        public static string CompatiblePath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
                return path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){
                return path.Replace('/', System.IO.Path.DirectorySeparatorChar);
            }

            return path;
        }

        public static async Task Download(string[] urls, string outputDirectory, HttpClient client)
        {
            WriteOutcome($"Downloading to {outputDirectory}. Batches: {(urls.Length / maxDegreeOfParallelism)}. Estimated time: {(urls.Length / maxDegreeOfParallelism) * delay} s");

            await DownloadInternal(urls, async (response, url) =>
            {
                string outputPath = Path.Combine(outputDirectory, $"{Uri.EscapeDataString(url)}.html");
             
                using (FileStream fileStream = new FileStream(outputPath, FileMode.OpenOrCreate))
                {
                    await response.Content.ReadAsStream().CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }

                WriteOutcome(outputPath);
            
            }, client);
        }

        public static async Task<string[]> Download(string[] urls, HttpClient client)
        {
            List<string> files = new List<string>();

            await DownloadInternal(urls, async (response, url) =>
            {
                files.Add(await response.Content.ReadAsStringAsync());
            }, client);

            return files.ToArray();
        }

        public static async Task<HttpClient> GetClient(string baseAddress, string authenticationUrl, string username, string password)
        {
            var baseUri = new Uri(baseAddress);
            var authenticationUri = new Uri(authenticationUrl);
            var authenticationData = await GetAuthenticationData();

            var cookieContainer = new CookieContainer();

            string result = await authenticationData.Content.ReadAsStringAsync();

            cookieContainer.SetCookies(baseUri, string.Join(",", authenticationData.Headers.GetValues("Set-Cookie")));
            cookieContainer.Add(sessionCookie);

            var handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };

            return new HttpClient(handler);

            async Task<HttpResponseMessage> GetAuthenticationData()
            {
                // Retrieve authentication data.
                HttpClient authenticationClient = new HttpClient();

                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string,string>("username", username),
            new KeyValuePair<string,string>("password", password),
        });


                var response = await authenticationClient.PostAsync(authenticationUri, content);

                return response;
            }
        }


        #region Utils

        public static void WriteOutcome(string message, bool succeed = true)
        {
            Console.ForegroundColor = succeed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintUrls(string[] urls)
        {
            foreach (var url in urls)
            {
                Console.WriteLine(url);
            }
        }

       public static (int Start, int End)[] ParseRangeString(string rangeString) => rangeString.Split(";", StringSplitOptions.RemoveEmptyEntries)
                                                                              .Select(range =>
                                                                              {
                                                                                  string[] tokens = range.Split("-", StringSplitOptions.RemoveEmptyEntries);

                                                                                  if(tokens.Length != 2)
                                                                                  {
                                                                                      return (Start: 0, End: 0);
                                                                                  }

                                                                                  return (Start: Convert.ToInt32(tokens[0]), End: Convert.ToInt32(tokens[1]));
                                                                              })
                                                                              .ToArray();

        public static string[] ParseValueString(string valueString) => valueString.Split(";", StringSplitOptions.RemoveEmptyEntries);

        #endregion

        private static async Task DownloadInternal(string[] urls, Func<HttpResponseMessage, string, Task> writeToOutput, HttpClient client)
        {
            await Parallel.ForEachAsync(
                source: urls,
                parallelOptions: new ParallelOptions()
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                },
                body: async (url, ct) =>
                {
                    try
                    {

                        var response = await client.GetAsync(url);

                        await writeToOutput(response, url);

                        await Task.Delay(delay * 1000);
                    }
                    catch
                    {
                        WriteOutcome($"Failed for url {url}", succeed: false);
                    }

                });


        }

    }
}