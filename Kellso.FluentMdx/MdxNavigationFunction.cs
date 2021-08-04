using System.Collections.Generic;
using System.Linq;

namespace FluentMdx
{
    /// <summary>
    /// Represents Mdx navigation function that can be applied to <see cref="MdxMember"/>.
    /// </summary>
    public sealed class MdxNavigationFunction : MdxExpressionBase
    {
        private readonly List<object> functionParameters = new List<object>();

        /// <summary>
        /// Gets the title of <see cref="MdxNavigationFunction"/>.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the collection of specified function parameters.
        /// </summary>
        public IEnumerable<object> FunctionParameters => this.functionParameters;

        /// <summary>
        /// Sets the title and returns the updated current instance of <see cref="MdxNavigationFunction"/>.
        /// </summary>
        /// <param name="title">Navigation function title.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxNavigationFunction"/>.</returns>
        public MdxNavigationFunction Titled(string title)
        {
            this.Title = title;
            return this;
        }

        /// <summary>
        /// Appends the parameters and returns the updated current instance of <see cref="MdxNavigationFunction"/>.
        /// </summary>
        /// <param name="parameters">Collection of navigation function parameters</param>
        /// <returns>Returns the updated current instance of <see cref="MdxNavigationFunction"/>.</returns>
        public MdxNavigationFunction WithParameters(params object[] parameters)
        {
            foreach (var parameter in parameters)
                this.functionParameters.Add(parameter);
            
            return this;
        }

        protected override string GetStringExpression()
        {
            if (!this.functionParameters.Any())
                return this.Title;

            return string.Format("{0}({1})", this.Title, string.Join(", ", this.functionParameters.Select(Mdx.ConstantValue)));
        }
    }
}
