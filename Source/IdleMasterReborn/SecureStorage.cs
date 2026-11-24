using System;
using System.Security.Cryptography;
using System.Text;

namespace IdleMasterReborn
{
    internal static class SecureStorage
    {
        private const string Prefix = "dpapi:";

        internal static string Protect(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            if (plainText.StartsWith(Prefix, StringComparison.Ordinal))
            {
                return plainText;
            }

            var data = Encoding.UTF8.GetBytes(plainText);
            var protectedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Prefix + Convert.ToBase64String(protectedData);
        }

        internal static string Unprotect(string storedValue)
        {
            if (string.IsNullOrEmpty(storedValue))
            {
                return string.Empty;
            }

            if (!storedValue.StartsWith(Prefix, StringComparison.Ordinal))
            {
                return storedValue;
            }

            try
            {
                var base64 = storedValue.Substring(Prefix.Length);
                var protectedData = Convert.FromBase64String(base64);
                var data = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
