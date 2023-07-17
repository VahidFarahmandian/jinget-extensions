﻿using System.Collections.Generic;
using System.Linq;
using Jinget.Core.ExpressionToSql.Internal;

namespace Jinget.Core.ExtensionMethods.ExpressionToSql
{
    public static class OrderByExtensions
    {
        public static string Stringfy(this List<OrderBy> lstOrderBy) =>
            lstOrderBy.Any()
                ? $"ORDER BY {string.Join(",", lstOrderBy.Select(x => x.ToString()))}"
                : string.Empty;

        public static string GetSorting(this List<OrderBy> lstOrderBy)
        {
            lstOrderBy ??= new List<OrderBy>();

            return lstOrderBy.Any() ? lstOrderBy.Stringfy() : string.Empty;
        }
    }
}
