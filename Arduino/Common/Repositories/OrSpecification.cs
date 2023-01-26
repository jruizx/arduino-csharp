using System.Linq.Expressions;
using Arduino.Common.Repositories;

namespace Arduino.Common.DomainModel;

public class OrSpecification<T> : Specification<T> where T : Entity
{
    private readonly Specification<T> left;
    private readonly Specification<T> right;
    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        this.right = right;
        this.left = left;
    }
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = left.ToExpression();
        var rightExpression = right.ToExpression();

        var paramExpr = Expression.Parameter(typeof(T));
        var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);

        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);


        return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
    }
}