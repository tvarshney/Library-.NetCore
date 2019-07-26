namespace MvcMovie.Models
{
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
//using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using MySql.Data.EntityFrameworkCore.Extensions;
using System.ComponentModel.DataAnnotations;

    public static class ListHelper
    {
        public static HtmlString CreateList(this IHtmlHelper html, string item)
        {
            TagBuilder ul = new TagBuilder("ul");
            
                TagBuilder li = new TagBuilder("li");
                // добавляем текст в li
                li.InnerHtml.Append(item);
                // добавляем li в ul
                ul.InnerHtml.AppendHtml(li);
           
            ul.Attributes.Add("class", "itemsList");
            var writer = new System.IO.StringWriter();
            ul.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }

        public static HtmlString CreateHrefLink(this IHtmlHelper html, string item)
        {
            TagBuilder a = new TagBuilder("a");

                // добавляем li в ul
                a.InnerHtml.AppendHtml(item);
           
            a.Attributes.Add("class", "itemsList");
            var writer = new System.IO.StringWriter();
            a.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}