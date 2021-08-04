namespace FluentMdx
{
    /// <summary>
    /// Represents constant value used in Mdx statements.
    /// </summary>
    public sealed class MdxConstantExpression : MdxExpressionBase, IMdxExpression
    {
        /// <summary>
        /// Gets the value of expression.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Sets the value of expression and returns the instance of <see cref="MdxConstantExpression"/>.
        /// </summary>
        /// <param name="value">Value of expression.</param>
        /// <returns>Returns updated current instance of <see cref="MdxConstantExpression"/>.</returns>
        public MdxConstantExpression WithValue(object value)
        {
            this.Value = value;
            return this;
        }

        protected override string GetStringExpression()
        {
            if (this.Value is string strValue)
                return strValue.Equals(MdxConstants.NullConstant, System.StringComparison.OrdinalIgnoreCase)
                    ? null
                    : $"\"{strValue.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim('\"')}\"";

            if (this.Value is int intValue)
                return intValue.ToString();

            if (this.Value is null)
                return "NULL";

            return this.Value?.ToString() ?? MdxConstants.NullConstant;
        }
    }
}
