﻿using System;
using System.Linq.Expressions;

namespace Hikari.Common
{
    /// <summary>
    /// Linq扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class PredicateExtensions
    {
        /// <summary>
        /// 与
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        /// <summary>
        /// 或
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            var body = Expression.Or(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        internal class ParameterReplacer : ExpressionVisitor
        {
            public ParameterReplacer(ParameterExpression paramExpr)
            {
                this.ParameterExpression = paramExpr;
            }

            public ParameterExpression ParameterExpression { get; private set; }

            public Expression Replace(Expression expr)
            {
                return this.Visit(expr);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                return this.ParameterExpression;
            }
        }
    }
}