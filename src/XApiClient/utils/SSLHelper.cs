using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Xtb.XApiClient.Utils;

internal static class SslHelper
{
    /// <summary>
    /// A certificate validation callback that trusts all SSL certificates, regardless of their validity.
    /// This allows all SSL/TLS traffic to proceed without certificate validation.
    /// </summary>
    /// <param name="sender">An object that contains state information for this validation.</param>
    /// <param name="certificate">The certificate to be validated.</param>
    /// <param name="chain">The chain of certificate authorities associated with the certificate.</param>
    /// <param name="errors">Any SSL policy errors encountered during validation.</param>
    /// <returns>Always returns <c>true</c>, effectively trusting all certificates.</returns>
    public static bool TrustAllCertificatesCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

    /// <summary>
    /// A certificate validation callback that checks the validity of an SSL certificate.
    /// </summary>
    /// <param name="sender">An object containing state information about the validation request.</param>
    /// <param name="certificate">The SSL certificate to be validated.</param>
    /// <param name="chain">The chain of certificate authorities associated with the certificate.</param>
    /// <param name="errors">The SSL policy errors that occurred during the validation process, if any.</param>
    /// <returns>
    /// Returns <c>true</c> if the certificate is valid and no SSL policy errors are found;
    /// otherwise, returns <c>false</c> if the certificate is invalid or errors are detected.
    /// </returns>
    public static bool ValidateCertificateCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        if (errors != SslPolicyErrors.None)
        {
            return false;
        }

        X509Chain trustedChain = new();
        trustedChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
        trustedChain.ChainPolicy.RevocationFlag = X509RevocationFlag.EndCertificateOnly;
        trustedChain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreInvalidBasicConstraints;
        trustedChain.ChainPolicy.VerificationTime = DateTime.Now;

        bool isValid = trustedChain.Build(new X509Certificate2(certificate));
        if (isValid)
        {
            return true;
        }

        return false;
    }
}