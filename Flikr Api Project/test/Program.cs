using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            callMe();
            Console.Read();
        }

       async static void callMe()
        {
            HttpClient client = new HttpClient();
             string flickrApiKey = "656e36e5e04aed1878962f02cb2a661d";
       // var baseUrl = string.Format("https://api.flickr.com/services/rest/?method=flickr.photos.search&license=4%2C5%2C6%2C7&api_key={0}&format=json&nojsoncallback=1", flickrApiKey);
        string keyword = "jalaj";
       // if (!string.IsNullOrWhiteSpace(keyword))
         //   baseUrl += string.Format("&text=%22{0}%22", keyword);
        string baseUrl = "https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=258dcad235f3e2b874521979d8b583bb&tags=" + keyword + "&format=json&nojsoncallback=1";
            string msg =  await client.GetStringAsync(baseUrl);
            Console.Read();
           return ;
        }
    }
}
