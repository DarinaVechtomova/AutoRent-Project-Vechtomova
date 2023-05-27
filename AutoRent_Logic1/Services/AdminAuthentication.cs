using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace AutoRent_Presentation.Services
{
    public class AdminAuthentication
    {
        public static bool IsAuthenticated { get; set; } = false;
    }
}
