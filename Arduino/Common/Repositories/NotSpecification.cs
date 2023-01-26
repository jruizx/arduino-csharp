using System.Linq.Expressions;
using Arduino.Common.Repositories;

namespace Arduino.Common.DomainModel;

public class NotSpecification<T> : Specification<T> where T : Entity
{
    private readonly Specification<T> specification;

    public NotSpecification(Specification<T> specification)
    {
        this.specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = specification.ToExpression();

        var paramExpr = Expression.Parameter(typeof(T));
        var exprBody = Expression.Not(expression.Body);

        exprBody = (UnaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);

        return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
    }
}