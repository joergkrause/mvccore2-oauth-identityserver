using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JoergIsAGeek.Workshop.FromScratch.Controllers
{
  [Route("api/[controller]")]
  public class AppController : Controller
  {
    // GET: api/<controller>
    [HttpGet]
    public IActionResult Get()
    {
      var options = new DbContextOptions<PersistedGrantDbContext>();
      
      var store = new OperationalStoreOptions();
      using (var ctx = new PersistedGrantDbContext(options, store))
      {
        ctx.Add<Client>(new Client
        {
          ClientId = "c1",
          ClientSecrets = new List<ClientSecret> { new ClientSecret() { Type = "SharedSecret", Value = "secret" } },
          AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_id" } },
          AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "api1" } }
        });
        ctx.SaveChanges();
      }

      return Ok();
    }
  }
}