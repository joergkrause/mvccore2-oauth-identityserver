using JoergIsAGeek.Workshop.FromScratch.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JoergIsAGeek.Workshop.FromScratch.UserStore.Extensions
{
  public static class CustomIdentityServerBuilderExtensions
  {
    public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
    {
      builder.Services.AddSingleton<IUserRepository, UserRepository>();
      builder.AddProfileService<CustomProfileService>();
      builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

      return builder;
    }

    [DebuggerStepThrough]
    public static string GetSubjectId(this IPrincipal principal)
    {
      return principal.Identity.GetSubjectId();
    }

    [DebuggerStepThrough]
    public static string GetSubjectId(this IIdentity identity)
    {
      var id = identity as ClaimsIdentity;
      var claim = id.FindFirst("sub");

      if (claim == null) throw new InvalidOperationException("sub claim is missing");
      return claim.Value;
    }
  }
}
