using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeWorks1
{
    class Program
    {
        readonly static string path = "C:\\Users\\777\\Desktop\\888\\result.txt";

        public static string resultString = "";

        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var tasks = new List<Task<string>>();

            for (int i = 4; i < 14; i++)
            {
                tasks.Add(HttpStart($"https://jsonplaceholder.typicode.com/posts/" + i));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine(resultString);

            //await ResultToTextAsync();

            ResultToTextAsync();

        }


        static async Task<string> HttpStart(string url)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                Result result = JsonSerializer.Deserialize<Result>(responseBody);

                resultString = resultString + result.GetResult();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        static async Task ResultToTextAsync()
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(resultString);
            }
        }

    }

    class Result
    {
        public Result(Int32 userId, Int32 id, string title, string body)
        {
            this.userId = userId;
            this.id = id;
            this.title = title;
            this.body = body;
        }

        public Result()
        {

        }

        public Int32 userId { get; set; }
        public Int32 id { get; set; }
        public object title { get; set; }
        public object body { get; set; }

        public string GetResult()
        {
            return $"{userId}\n{id}\n{title}\n{body}\n\n";
        }

    }
}
