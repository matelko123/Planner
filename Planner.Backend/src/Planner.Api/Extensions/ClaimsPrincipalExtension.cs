using System.Security.Claims;

namespace Planner.Api.Extensions;

internal static class ClaimsPrincipalExtension
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new InvalidOperationException("User ID claim is missing or invalid.");
        }

        return userId;
    }
    
    public static string GetUserEmail(this ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var emailClaim = user.FindFirst(ClaimTypes.Email);
        if (emailClaim is null)
        {
            throw new InvalidOperationException("Email claim is missing.");
        }

        return emailClaim.Value;
    }
    
    public static string GetUserFirstName(this ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var firstNameClaim = user.FindFirst(ClaimTypes.GivenName);
        if (firstNameClaim is null)
        {
            throw new InvalidOperationException("First name claim is missing.");
        }

        return firstNameClaim.Value;
    }
    
    public static string GetUserLastName(this ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var lastNameClaim = user.FindFirst(ClaimTypes.Surname);
        if (lastNameClaim is null)
        {
            throw new InvalidOperationException("Last name claim is missing.");
        }

        return lastNameClaim.Value;
    }
}