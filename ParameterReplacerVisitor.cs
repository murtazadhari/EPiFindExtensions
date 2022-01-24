/// <summary>
    /// Custom expression visitor to replace the parameter value in the given expression
    /// </summary>
    public class ParameterReplacerVisitor<TIn, TOut> : ExpressionVisitor
    {
        public ParameterExpression Source { get; set; }

        public object Value { get; set; }

        public ParameterReplacerVisitor()
        { }

        /// <summary>
        /// Replace the parameter value in the given expression
        /// </summary>
        public Expression<TOut> ReplaceValue(Expression<TIn> exp)
        {
            var expNew = Visit(exp) as LambdaExpression;
            return Expression.Lambda<TOut>(expNew.Body, expNew.Parameters);
        }

        /// <summary>
        /// This function will replace the parameter expression with constant values if parameter name and type matches with provided value
        /// for e.g (num1, num2) => num1 + num2 will be replaced (num1) => num1 + 3. if we want to replace num2 with 3.
        /// </summary>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            // Replace the source with the target, visit other params as usual.
            if (node.Type == Source.Type && node.Name == Source.Name)
                return Expression.Constant(Value);

            return base.VisitParameter(node);
        }

        /// <summary>
        /// This functoin will replace the member expression with constant values if parameter name and type matches with provided value
        /// for e.g (num1, classObj) => num1 + classObj.num2 will be replaced (num1) => num1 + 3. if we want to replace classObj.num2 with 3.
        /// </summary>
        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null
                && m.Expression.NodeType == ExpressionType.Parameter
                && m.Expression.Type == Source.Type && ((ParameterExpression)m.Expression).Name == Source.Name)
            {
                object newVal;
                if (m.Member is FieldInfo)
                    newVal = ((FieldInfo)m.Member).GetValue(Value);
                else if (m.Member is PropertyInfo)
                    newVal = ((PropertyInfo)m.Member).GetValue(Value, null);
                else
                    newVal = null;

                return Expression.Constant(newVal);
            }

            return base.VisitMember(m);
        }

        /// <summary>
        /// In this function we are reducing the number of parameters in lambda expression
        /// for e.g as we are replacing parameter with constants therefore, (num1, num2) => will become (num1) =>
        /// </summary>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Where(p => p.Name != Source.Name || p.Type != Source.Type).ToList();
            return Expression.Lambda(Visit(node.Body), parameters);
        }
    }