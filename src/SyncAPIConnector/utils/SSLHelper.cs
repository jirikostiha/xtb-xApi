using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace xAPI.Utils
{
    class SSLHelper
    {
        /// <summary>
        /// Validator that trusts all SSL certificates (all traffic is cyphered).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool TrustAllCertificatesCallback(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            return true;
        }
    }
}
