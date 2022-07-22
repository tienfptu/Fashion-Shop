
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Fashion.Library
{
    public static class Helper
    {
        public static MvcHtmlString MyDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionText, bool canEdit)
        {
            if (canEdit) return html.DropDownListFor(expression, selectList, optionText,new { @class = "form-control" });
            return html.DropDownListFor(expression, selectList, optionText, new { @class = "form-control", @disabled="" });
        }
    }
}