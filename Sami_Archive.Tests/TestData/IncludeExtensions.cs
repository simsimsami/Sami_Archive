using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sami_Archive.Tests.TestData
{
    public static class IncludeExtensions
    {
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return source;
        }
    }
}
