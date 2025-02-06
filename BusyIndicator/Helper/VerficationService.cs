using System;
using System.Collections.Generic;
using System.Linq;
namespace BusyIndicator
{
    public static class VerficationService
    {
        private static readonly Dictionary<string, string> _users = new();

        public static bool Login(string? email, string? password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return false;

            return _users.TryGetValue(email, out var storedPassword) && storedPassword == password;
        }

        public static bool Register(string? email, string? password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;  

            if (_users.ContainsKey(email))
                return false;  

            _users[email] = password;  
            return true;
        }
    }
}
