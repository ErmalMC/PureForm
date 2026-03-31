namespace PureForm.Application.Utilities;

/// <summary>
/// Utility class for common validation methods.
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates that an email address is in a valid format.
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates that a password meets basic requirements:
    /// - At least 8 characters
    /// - Contains uppercase letter
    /// - Contains lowercase letter
    /// - Contains digit
    /// </summary>
    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return false;

        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);

        return hasUpper && hasLower && hasDigit;
    }

    /// <summary>
    /// Validates weight in kilograms (should be between 20kg and 300kg).
    /// </summary>
    public static bool IsValidWeight(decimal weight)
    {
        return weight >= 20m && weight <= 300m;
    }

    /// <summary>
    /// Validates height in centimeters (should be between 100cm and 250cm).
    /// </summary>
    public static bool IsValidHeight(decimal height)
    {
        return height >= 100m && height <= 250m;
    }
}

