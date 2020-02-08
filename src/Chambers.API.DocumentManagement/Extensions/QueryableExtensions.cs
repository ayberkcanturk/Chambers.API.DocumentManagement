using System;
using System.Linq;
using System.Linq.Expressions;

using Chambers.API.DocumentManagement.Caching;

namespace Chambers.API.DocumentManagement.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string order, string propertyName) => order == Orders.Ascending ? source.OrderByAscending(propertyName) : source.OrderByDescending(propertyName);
        
        public static IOrderedQueryable<T> OrderByAscending<T>(this IQueryable<T> source, string propertyName) =>
            source.OrderBy(ToLambda<T>(propertyName));

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName) =>
            source.OrderByDescending(ToLambda<T>(propertyName));

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            UnaryExpression propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}