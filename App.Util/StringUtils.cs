using App.Core;

namespace App.Util;

public static class StringUtils
{
    public static EfProviders ToEfProvider(this string providerName)
    {
        providerName = providerName.Trim();
        var result = EfProviders.None;
        switch (providerName)
        {
            case "Microsoft.EntityFrameworkCore.SqlServer":
                result = EfProviders.SqlServer;
                break;
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                result = EfProviders.PostgreSql;
                break;
            case "Pomelo.EntityFrameworkCore.MySql":
                result = EfProviders.MySql;
                break;
            default:
                break;
        }
        
        return result;
    }
}