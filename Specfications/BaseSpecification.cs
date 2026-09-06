using System.Linq.Expressions;

namespace TeamTaskManager.Specfications
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? WhereClause { get; set; }

        public List<Expression<Func<T, object>>> IncludeClause { get; set; } = new List<Expression<Func<T, object>>>();


        public BaseSpecification(Expression<Func<T, bool>>? whereClause)
        {
            WhereClause = whereClause;
           
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            IncludeClause.Add(includeExpression);
            Console.WriteLine("Size After: " + IncludeClause.Count);
        }
    }
}
