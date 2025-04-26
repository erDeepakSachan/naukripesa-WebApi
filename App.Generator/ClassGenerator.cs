using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using App.Entity;
using App.Generator.Dto;
using App.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFormat;
using Humanizer;

namespace App.Generator;

[TestClass]
public class ClassGenerator
{
    [TestMethod]
    public void Run()
    {
        var excludedFiles = new List<string>()
        {

        };
        var clazzes = typeof(User)
            .Assembly.GetTypes()
            .Where(p => !p.IsInterface
                        && !excludedFiles.Contains(p.Name)
                        && p.FullName.StartsWith("App.Entity")
                        && p.BaseType == typeof(object))
            .Select(p => new ClazzInfo()
            {
                Name = p.Name, UnderlineType = p,
                Columns = GetPublicProps(p)
            })
            .ToList();
        //GenerateRepository(clazzes);
        //GenerateService(clazzes);
        //GenerateController(clazzes);
        GenerateAngularFiles(clazzes);
    }
    
    private static List<string> GetPublicProps(Type t)
    {
        return t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.DeclaringType.Namespace.StartsWith("App.Entity"))
            .Select(p => p.Name).ToList();
    }

    private void GenerateRepository(List<ClazzInfo> clazzes)
    {
        var template = File.ReadAllText("Template/RepositoryTemplate.cs.txt");
        var directory = "../../../Repository";
        foreach (var clazz in clazzes)
        {
            try
            {
                var clazzDef = Smart.Format(template, new { clazzName = clazz.Name });
                var clazzFile = Path.Combine(directory, $"{clazz.Name}Repository.cs");
                File.WriteAllText(clazzFile, clazzDef);
            }
            catch (Exception)
            {
            }
        }
    }

    private void GenerateService(List<ClazzInfo> clazzes)
    {
        var template = File.ReadAllText("Template/ServiceTemplate.cs.txt");
        var directory = "../../../Service";
        foreach (var clazz in clazzes)
        {
            try
            {
                var clazzDef = Smart.Format(template, new { clazzName = clazz.Name });
                var clazzFile = Path.Combine(directory, $"{clazz.Name}Service.cs");
                File.WriteAllText(clazzFile, clazzDef);
            }
            catch (Exception)
            {

            }
        }
    }

    private void GenerateController(List<ClazzInfo> clazzes)
    {
        var template = File.ReadAllText("Template/ControllerTemplate.cs.txt");
        var directory = "../../../Controller";
        foreach (var clazz in clazzes)
        {
            try
            {
                var clazzDef = Smart.Format(template, new { clazzName = clazz.Name, DisplayName = clazz.DisplayName });
                var clazzFile = Path.Combine(directory, $"{clazz.Name}Controller.cs");
                File.WriteAllText(clazzFile, clazzDef);
            }
            catch (Exception)
            {

            }
        }
    }

    private void GenerateAngularFiles(List<ClazzInfo> clazzes)
    {
        var directory = "../../../AngularPage";
        var clazzDir = "";
        var _import = new StringBuilder();
        var _routes = new StringBuilder();
        foreach (var clazz in clazzes)
        {
            try
            {
                clazzDir = Path.Combine(directory, $"{clazz.NameLower}");
                if (!Directory.Exists(clazzDir))
                {
                    Directory.CreateDirectory(clazzDir);
                }

                GenerateEntity(clazz);
                GenerateService(clazz);
                GenerateComponentTs(clazz);
                GenerateComponentHtml(clazz);
                GenerateComponentAddEdit(clazz);
                GenerateComponentAddEdit(clazz, true);
                GenerateRoute(clazz, ref _import, ref _routes);
            }
            catch (Exception)
            {
            }
        }
        
        Console.WriteLine(_import.ToString());
        Console.WriteLine(_routes.ToString());

        void GenerateService(ClazzInfo clazz)
        {
            var template = File.ReadAllText("Template/service-template.ts.txt");
            var clazzDef = Smart.Format(template, new { __NAME_PROP__ = clazz.Name, __NAME_LOW__ = clazz.NameLower });
            var clazzFile = Path.Combine(clazzDir, $"{clazz.NameLower}.service.ts");
            File.WriteAllText(clazzFile, clazzDef);
            var cssFile = Path.Combine(clazzDir, $"{clazz.NameLower}.component.css");
            File.WriteAllText(cssFile, "");
        }
        
        void GenerateComponentTs(ClazzInfo clazz)
        {
            var template = File.ReadAllText("Template/component-template.ts.txt");
            var clazzDef = Smart.Format(template, new { __NAME_PROP__ = clazz.Name, __NAME_LOW__ = clazz.NameLower });
            var clazzFile = Path.Combine(clazzDir, $"{clazz.NameLower}.component.ts");
            File.WriteAllText(clazzFile, clazzDef);
        }

        void GenerateComponentHtml(ClazzInfo clazz)
        {
            var template = File.ReadAllText("Template/component-template.html.txt");
            var columnList = new StringBuilder();
            var columnListValue = new StringBuilder();
            foreach (var column in clazz.Columns)
            {
                columnList.AppendLine($"<th>{column}</th>");
                columnListValue.AppendLine($"<td>{{{{ item.{column} }}}}</td>");
            }
            var clazzDef = Smart.Format(template, new
            {
                __NAME_PROP__ = clazz.Name, __NAME_LOW__ = clazz.NameLower, __PK__ = clazz.Columns.FirstOrDefault(),
                __COLUMN_LIST__ = columnList.ToString(), __COLUMN_VALUE_LIST__ = columnListValue.ToString()
            });
            var clazzFile = Path.Combine(clazzDir, $"{clazz.NameLower}.component.html");
            File.WriteAllText(clazzFile, clazzDef);
        }

        void GenerateComponentAddEdit(ClazzInfo clazz, bool edit = false)
        {
            var template = File.ReadAllText("Template/component-template.add.ts.txt");
            if (edit)
            {
                template = File.ReadAllText("Template/component-template.edit.ts.txt");
            }

            var columnList = new StringBuilder();
            var columnListValue = new StringBuilder();
            var columnListInput = new StringBuilder();
            foreach (var column in clazz.Columns)
            {
                columnList.AppendLine($"<th>{column}</th>");
                columnListValue.AppendLine($"<td>{{{{ item.{column} }}}}</td>");
                columnListInput.AppendLine(@$"
                <div class=""col-lg-12"">
                  <div class=""form-group"">
                    <label>{column} <span class=""field-validation-valid"" data-valmsg-for=""{column}"" data-valmsg-replace=""true""></span>
                    </label>
                    <input [(ngModel)]=""obj.{column}"" type=""text"" name=""{column}"" placeholder=""{column}"" class=""form-control"" data-val=""true"" data-val-required=""The {column} field is required."" autocomplete=""off"" />
                  </div>
                </div>
                ");
            }

            var clazzDef = Smart.Format(template, new
            {
                __NAME_PROP__ = clazz.Name, __NAME_LOW__ = clazz.NameLower, __PK__ = clazz.Columns.FirstOrDefault(),
                __COLUMN_LIST__ = columnList.ToString(), __COLUMN_VALUE_LIST__ = columnListValue.ToString(),
                __COLUMN_INPUT_LIST__ = columnListInput.ToString()
            });
            var clazzFile = Path.Combine(clazzDir, $"{clazz.NameLower}.component.add.ts");
            if (edit)
            {
                clazzFile = Path.Combine(clazzDir, $"{clazz.NameLower}.component.edit.ts");
            }
            File.WriteAllText(clazzFile, clazzDef);
        }

        void GenerateRoute(ClazzInfo clazz,ref StringBuilder _import,ref StringBuilder _route)
        {
            _import.AppendLine($"import {{ {clazz.Name}Component }} from './../pages/{clazz.NameLower}/{clazz.NameLower}.component';");
            _route.AppendLine($", {{ path: '{clazz.NameLower}s', component: {clazz.Name}Component }}");
        }
        
        void GenerateEntity(ClazzInfo clazz)
        {
            var def = new StringBuilder();
            def.AppendLine($"export interface {clazz.Name} {{");
            var defEmpty = new StringBuilder();
            defEmpty.AppendLine(@$"export const empty{clazz.Name} = (): {clazz.Name} => {{
    return {{");
            foreach (var prop in clazz.Columns)
            {
                def.AppendLine($"{prop}: any,");
                defEmpty.AppendLine($"{prop}: '',");
            }
            def.AppendLine($"}}");
            defEmpty.AppendLine($@"    }}
}}");
            var text = String.Concat(def.ToString(), Environment.NewLine, defEmpty.ToString());
            var clazzFile = Path.Combine(clazzDir, "../Entity", $"{clazz.NameLower}.entity.ts");
            File.WriteAllText(clazzFile, text);
        }
    }
}