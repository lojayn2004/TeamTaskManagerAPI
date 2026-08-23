using System.Linq.Expressions;

namespace TeamTaskManager.Specfications
{
    public interface ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? WhereClause { get; }

        public IEnumerable<Expression<Func<T, object>>> IncludeClause { get; } 

    }
}
