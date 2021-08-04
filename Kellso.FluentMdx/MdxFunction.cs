using System.Collections.Generic;

namespace FluentMdx
{
    /// <summary>
    /// Represents Mdx function specificion.
    /// </summary>
    public sealed class MdxFunction : MdxExpressionBase, IMdxMember, IMdxExpression
    {
        #region Properties

        /// <summary>
        /// Gets the collection of applied function parameters.
        /// </summary>
        public IEnumerable<IMdxExpression> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Gets the collection of function titles.
        /// </summary>
        public IEnumerable<string> Titles
        {
            get { return _titles; }
        }

        #endregion Properties

        #region Fields

        private readonly IList<IMdxExpression> _parameters;
        private readonly IList<string> _titles;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Appends titles and returns updated current instance of <see cref="MdxFunction"/>.
        /// </summary>
        /// <param name="titles">Collection of titles.</param>
        /// <returns>Returns updated current instance of <see cref="MdxFunction"/>.</returns>
        //TODO: Change methods argumets for only name, without titles.
        public MdxFunction Titled(params string[] titles)
        {
            foreach (var title in titles)
                _titles.Add(title);

            return this;
        }

        /// <summary>
        /// Appends specified parameters and returns updated current instance of <see cref="MdxFunction"/>.
        /// </summary>
        /// <param name="parameters">Collection of <see cref="IMdxExpression"/> parameters.</param>
        /// <returns>Returns updated current instance of <see cref="MdxFunction"/></returns>
        public MdxFunction WithParameters(params IMdxExpression[] parameters)
        {
            foreach (var parameter in parameters)
                _parameters.Add(parameter);

            return this;
        }

        protected override string GetStringExpression()
        {
            return string.Format("{0}({1})",
                string.Join(".", Titles),
                string.Join(", ", Parameters));
        }

        #endregion Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="MdxFunction"/>.
        /// </summary>
        public MdxFunction()
        {
            _titles = new List<string>();
            _parameters = new List<IMdxExpression>();
        }

        #endregion Constructors
    }
}
