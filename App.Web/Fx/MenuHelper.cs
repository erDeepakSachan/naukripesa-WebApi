using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace App.Web.Fx
{
    public class MenuNode
    {
        public MenuNode()
        {
            Nodes = new List<MenuNode>();
        }

        public string Text { get; set; }
        public string Icon { get; set; }
        public string IconCss { get; set; }
        public string IconColor { get; set; }
        public string ControllerName { get; set; }
        public string NavUrl { get; set; }
        public List<MenuNode> Nodes { get; set; }

        public string StartUlTag()
        {
            if (this.Nodes.Count > 0)
            {
                return "<ul class='nav nav-pills nav-stacked'>";
            }
            return "";
        }

        public string EndUlTag()
        {
            if (this.Nodes.Count > 0)
            {
                return "</ul>";
            }
            return "";
        }

        public string StartLiTag(IUrlHelper urlHelper, string sid, string iClazz = "", string color = "")
        {
            if (string.IsNullOrWhiteSpace(iClazz))
            {
                iClazz = "glyphicon glyphicon-plus";
            }
            if (string.IsNullOrWhiteSpace(color))
            {
                color = "fa-none";
            }
            var clazz = "";
            var navUrl = "";
            if (this.Nodes.Count > 0)
            {
                navUrl = "#";
                clazz = "accordion";
            }
            else
            {
                navUrl = urlHelper.Content("~/" + this.ControllerName + "/Index?sid=" + sid);
                if (String.IsNullOrWhiteSpace(this.ControllerName))
                {
                    navUrl = "#";
                }
            }
            return "<li class='" + clazz + "' ><a href = '" + navUrl + "' ><i class='" + iClazz + "' style='color:" + color + ";'></i><span> " + this.Text + "</span></a>";
        }

        public string EndLiTag()
        {
            return "</li>";
        }
    }
}