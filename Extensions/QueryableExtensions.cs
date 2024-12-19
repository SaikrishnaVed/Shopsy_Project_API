using System;
using System.Linq;
using System.Linq.Expressions;

namespace Shopsy_Project.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string orderByProperty, bool ascending)
        {
            var type = typeof(T);
            var property = type.GetProperty(orderByProperty);
            if (property == null) throw new ArgumentException($"Property '{orderByProperty}' not found on type '{type.Name}'.");

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName,
                new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}