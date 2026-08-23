using Microsoft.EntityFrameworkCore;

namespace TeamTaskManager.Specfications
{
    public static class QueryEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> query, 
            ISpecification<T> baseSpecification) where T : class
        {
            var newQuery = query;

            if (baseSpecification.WhereClause != null)
                newQuery = newQuery.Where(baseSpecification.WhereClause);

            foreach (var include in baseSpecification.IncludeClause)
            {
               
                newQuery = newQuery.Include(include);
            }

            return newQuery;
        }
    }
}
