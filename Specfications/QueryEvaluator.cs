using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

            Console.WriteLine("IncludeClause: {0}" + baseSpecification.IncludeClause);
            Console.WriteLine("IncludeClause Count: {0}" + baseSpecification.IncludeClause.Count); 
            foreach (var include in baseSpecification.IncludeClause)
            {
                Console.WriteLine("Adding Incude for: {0}", include);
                newQuery = newQuery.Include(include);
            }

            return newQuery;
        }
    }
}
