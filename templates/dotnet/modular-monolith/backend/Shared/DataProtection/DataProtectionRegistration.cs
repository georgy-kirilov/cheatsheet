using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;

namespace Shared.DataProtection;

public static class DataProtectionRegistration
{
    public const string DataProtectionKeysPathInsideDocker = "/root/.aspnet/DataProtection-Keys";

    public static IServiceCollection AddAppDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
        var certificatePath = configuration.GetValueOrThrow<string>(DataProtectionConfigurationSections.CertificatePath);
        var certificatePassword = configuration.GetValueOrThrow<string>(DataProtectionConfigurationSections.CertificatePassword);
        
        var certificate = new X509Certificate2
        (
            certificatePath,
            certificatePassword,
            X509KeyStorageFlags.EphemeralKeySet
        );

        services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(DataProtectionKeysPathInsideDocker))
            .ProtectKeysWithCertificate(certificate);

        return services;
    }
}
