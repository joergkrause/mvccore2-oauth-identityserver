using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JoergIsAGeek.Workshop.IdentityClient
{
  class Program
  {
    // C# 7.2+
    public static async Task Main(string[] args)
    {
      // discover endpoints from metadata
      var discoClient = new DiscoveryClient("http://realname.local:5000");
      discoClient.Policy.RequireHttps = false;
      discoClient.Policy.ValidateIssuerName = false;
      var disco = await discoClient.GetAsync();
      if (disco.IsError)
      {
        Console.WriteLine(disco.Error);
        Console.ReadLine();
        return;
      }

      // request token
      var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
      var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

      if (tokenResponse.IsError)
      {
        Console.WriteLine(tokenResponse.Error);
        Console.ReadLine();
        return;
      }

      Console.WriteLine(tokenResponse.Json);
      Console.WriteLine("\n\n");

      // call api
      var client = new HttpClient();
      client.SetBearerToken(tokenResponse.AccessToken);

      var response = await client.GetAsync("http://realname.local:5001/identity");
      if (!response.IsSuccessStatusCode)
      {
        Console.WriteLine(response.StatusCode);
      }
      else
      {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(JArray.Parse(content));
      }
      Console.ReadLine();
    }

  }
}
