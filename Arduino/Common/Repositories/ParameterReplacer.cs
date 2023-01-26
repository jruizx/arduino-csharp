﻿using System.Linq.Expressions;

namespace Arduino.Common.Repositories;

internal class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression parameter;

    protected override Expression VisitParameter(ParameterExpression node)
        => base.VisitParameter(parameter);

    internal ParameterReplacer(ParameterExpression parameter)
    {
        this.parameter = parameter;
    }
}