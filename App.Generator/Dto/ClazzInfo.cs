using System;
using System.Collections.Generic;
using Humanizer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace App.Generator.Dto;

public class ClazzInfo
{
    public ClazzInfo()
    {
        Columns = new List<string>();
    }

    public Type UnderlineType { get; set; }
    public string Name { get; set; }
    public string NameLower => Name?.ToLowerInvariant();
    public string DisplayName => Name.Humanize();
    public List<string> Columns { get; set; }
}