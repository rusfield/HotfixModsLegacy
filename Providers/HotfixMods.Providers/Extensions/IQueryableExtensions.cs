using HotfixMods.Providers.Models;
using System.Linq.Expressions;

namespace HotfixMods.Providers.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WhereDbParameters<T>(this IEnumerable<T> source, params DbParameter[] parameters)
        {
            var query = source.AsQueryable();
            var paramExpr = Expression.Parameter(typeof(T), "x");
            Expression? whereExpr = null;

            foreach (var p in parameters)
            {
                var propertyExpr = Expression.Property(paramExpr, p.Property);
                var valueExpr = Expression.Constant(p.Value);
                Expression? binaryExpr = null;

                switch (p.Operator)
                {
                    case DbParameter.DbOperator.EQ:
                        binaryExpr = Expression.Equal(propertyExpr, valueExpr);
                        break;
                        // Other cases here
                }

                if (whereExpr == null)
                {
                    whereExpr = binaryExpr;
                }
                else
                {
                    whereExpr = Expression.AndAlso(whereExpr, binaryExpr);
                }
            }

            var lambdaExpr = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);
            return query.Where(lambdaExpr);
        }
    }

}
