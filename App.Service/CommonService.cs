using App.Dto;
using App.Repository;
using Humanizer;

namespace App.Service;

public class CommonService
{
    private readonly IServiceProvider provider;
    private readonly CommonRepository repository;

    public CommonService(IServiceProvider provider, CommonRepository repository)
    {
        this.provider = provider;
        this.repository = repository;
    }

    public string GetSettingValueFromDb(string key)
    {
        return repository.GetSettingValueFromDb(key);
    }

    private Dictionary<string, Func<List<DdlOption>>> _StaticDdlFuncs = null;

    public string BeauitfyWord(string Word)
    {
        string NewWord = "";
        Word = Word.Replace("ID", "");
        for (Int32 t = 0; t <= Word.Length - 1; t++)
        {
            if (char.IsUpper(Word[t]))
                NewWord = NewWord + " " + Word.Substring(t, 1);
            else
                NewWord = NewWord + Word.Substring(t, 1);
        }

        return NewWord.Trim();
    }


    public string Plural(string Word)
    {
        return Word?.Pluralize();
    }

    // public static IDdlOption GetDdlSource(string typeName, SqlConnection connection)
    // {
    //     Type type = Type.GetType(typeName);
    //     return (IDdlOption)Activator.CreateInstance(type, connection);
    // }

    public List<DdlOption> GetAllThemeOptions()
    {
        var themes = new List<DdlOption>();
        themes.Add(new DdlOption { Text = "bootstrap-cerulean", Value = "bootstrap-cerulean" });
        themes.Add(new DdlOption { Text = "bootstrap-cyborg", Value = "bootstrap-cyborg" });
        themes.Add(new DdlOption { Text = "bootstrap-darkly", Value = "bootstrap-darkly" });
        themes.Add(new DdlOption { Text = "bootstrap-lumen", Value = "bootstrap-lumen" });
        themes.Add(new DdlOption { Text = "bootstrap-simplex", Value = "bootstrap-simplex" });
        themes.Add(new DdlOption { Text = "bootstrap-slate", Value = "bootstrap-slate" });
        themes.Add(new DdlOption { Text = "bootstrap-spacelab", Value = "bootstrap-spacelab" });
        themes.Add(new DdlOption { Text = "bootstrap-united", Value = "bootstrap-united" });
        return themes;
    }

    public List<DdlOption> GetAllNotificationOptionLayoutOptions()
    {
        var themes = new List<DdlOption>();
        themes.Add(new DdlOption { Text = "topLeft", Value = "topLeft" });
        themes.Add(new DdlOption { Text = "topCenter", Value = "topCenter" });
        themes.Add(new DdlOption { Text = "topRight", Value = "topRight" });
        themes.Add(new DdlOption { Text = "top", Value = "top" });
        themes.Add(new DdlOption { Text = "center", Value = "center" });
        themes.Add(new DdlOption { Text = "bottomLeft", Value = "bottomLeft" });
        themes.Add(new DdlOption { Text = "bottomRight", Value = "bottomRight" });
        themes.Add(new DdlOption { Text = "bottom", Value = "bottom" });
        return themes;
    }

    public List<DdlOption> GetAllNotificationOptionTypeOptions()
    {
        var themes = new List<DdlOption>();
        themes.Add(new DdlOption { Text = "alert", Value = "alert" });
        themes.Add(new DdlOption { Text = "information", Value = "information" });
        themes.Add(new DdlOption { Text = "success", Value = "success" });
        themes.Add(new DdlOption { Text = "error", Value = "error" });
        return themes;
    }

    public List<DdlOption> GetStaticDdl(string key)
    {
        if (_StaticDdlFuncs == null)
        {
            _StaticDdlFuncs = new Dictionary<string, Func<List<DdlOption>>>();
            _StaticDdlFuncs.Add("Theme", GetAllThemeOptions);
            _StaticDdlFuncs.Add("NotificationOptionLayout", GetAllNotificationOptionLayoutOptions);
            _StaticDdlFuncs.Add("NotificationOptionType", GetAllNotificationOptionTypeOptions);
        }

        if (_StaticDdlFuncs.ContainsKey(key))
        {
            return _StaticDdlFuncs[key]();
        }

        return GetDdlSource(key)?.GetAllDdlOptions();
    }


    public IDdlOption GetDdlSource(string typeName)
    {
        Type type = Type.GetType(typeName);
        return provider.GetService(type) as IDdlOption;
    }
}