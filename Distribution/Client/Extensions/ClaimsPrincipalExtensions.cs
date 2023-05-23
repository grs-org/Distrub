using System.Security.Claims;

namespace Distribution.Client.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        internal static string GetEmail(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        internal static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);

        internal static string GetLastName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Surname);

        internal static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

        internal static string GetUserId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        //internal static string GetAnimateur(this ClaimsPrincipal claimsPrincipal)
        //    => claimsPrincipal.FindFirstValue(ClaimTypes.Actor);

        //internal static string GetSiteID(this ClaimsPrincipal claimsPrincipal)
        //    => claimsPrincipal.FindFirstValue("SiteID");

    }
}