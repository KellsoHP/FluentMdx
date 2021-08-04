using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentMdx
{
    /// <summary>
    /// Represent Mdx query statement.
    /// </summary>
    public sealed class MdxQuery : MdxExpressionBase
    {
        #region Properties

        /// <summary>
        /// Gets the collection of specified <see cref="MdxAxis"/>s.
        /// </summary>
        public IEnumerable<MdxAxis> Axes => this.axes;

        /// <summary>
        /// Gets the collection of specified <see cref="MdxCube"/>s.
        /// </summary>
        public IEnumerable<MdxCube> Cubes => cubes;

        /// <summary>
        /// Gets the inner <see cref="MdxQuery"/> used as parent source.
        /// </summary>
        public MdxQuery InnerQuery { get; private set; }

        /// <summary>
        /// Gets the collection of specified <see cref="MdxTuple"/>s.
        /// </summary>
        public IEnumerable<MdxTuple> WhereClauseTuples => whereClauseTuples;

        #endregion Properties

        #region Fields

        private readonly IList<MdxAxis> axes;
        private readonly IList<MdxCube> cubes;
        private readonly IList<MdxTuple> whereClauseTuples;
        private readonly IList<MdxDeclaration> withDeclarations;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Appends the specified <see cref="MdxAxis"/> and returns the updated current instance of <see cref="MdxQuery"/>.
        /// It will inner query if specified.
        /// </summary>
        /// <param name="cube">Specified <see cref="MdxCube"/> as query source.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxQuery"/>.</returns>
        public MdxQuery From(MdxCube cube)
        {
            this.InnerQuery = null;

            this.cubes.Add(cube);
            return this;
        }

        /// <summary>
        /// Sets the <see cref="MdxQuery"/> as parent query source and returns the updated current instance of <see cref="MdxQuery"/>.
        /// It will clear any cubes if specified.
        /// </summary>
        /// <param name="innerQuery">Specified <see cref="MdxQuery"/> as query source.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxQuery"/>.</returns>
        public MdxQuery From(MdxQuery innerQuery)
        {
            this.cubes.Clear();

            this.InnerQuery = innerQuery;
            return this;
        }

        /// <summary>
        /// Appends the specified <see cref="MdxAxis"/> and returns the updated current instance of <see cref="MdxQuery"/>.
        /// </summary>
        /// <param name="axis">Specified <see cref="MdxAxis"/>.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxQuery"/>.</returns>
        public MdxQuery On(MdxAxis axis)
        {
            if (axis is null)
                throw new ArgumentNullException(nameof(axis));

            this.axes.Add(axis);
            return this;
        }

        /// <summary>
        /// Appends the <see cref="MdxTuple"/> into where clause and returns the updated current instance of <see cref="MdxQuery"/>.
        /// </summary>
        /// <param name="tuple">Specified <see cref="MdxTuple"/>.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxQuery"/>.</returns>
        public MdxQuery Where(MdxTuple tuple)
        {
            this.whereClauseTuples.Add(tuple);
            return this;
        }

        public MdxQuery With(MdxDeclaration withDeclaration)
        {
            this.withDeclarations.Add(withDeclaration);
            return this;
        }

        #endregion Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="MdxQuery"/>.
        /// </summary>
        public MdxQuery()
        {
            this.axes = new List<MdxAxis>();
            this.cubes = new List<MdxCube>();
            this.whereClauseTuples = new List<MdxTuple>();
            this.withDeclarations = new List<MdxDeclaration>();
        }

        #endregion Constructors

        protected override string GetStringExpression()
        {
            var queryStringBuilder = new StringBuilder();

            if (this.withDeclarations.Any())
                queryStringBuilder.AppendFormat("WITH {0} ", string.Join(" ", withDeclarations));

            queryStringBuilder.AppendFormat("SELECT {0} ", string.Join(", ", Axes));

            if (this.InnerQuery == null)
                queryStringBuilder.AppendFormat("FROM {0}", string.Join(", ", Cubes));
            else
                queryStringBuilder.AppendFormat("FROM ( {0} )", this.InnerQuery);

            if (this.whereClauseTuples.Any())
                queryStringBuilder.AppendFormat(" WHERE {{ ( {0} ) }}", string.Join(", ", whereClauseTuples));

            return queryStringBuilder.ToString();
        }
    }
}