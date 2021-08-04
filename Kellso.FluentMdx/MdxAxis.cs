using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentMdx
{
    /// <summary>
    /// Represents Mdx query axis specification.
    /// </summary>
    public sealed class MdxAxis : MdxExpressionBase
    {
        #region Properties

        /// <summary>
        /// Gets the axis number.
        /// </summary>
        public MdxAxisType AxisIdentifier { get; private set; }

        /// <summary>
        /// Gets the axis slicer.
        /// </summary>
        public MdxTuple AxisSlicer { get; private set; }

        /// <summary>
        /// Gets the value if the axis is specified as non-empty.
        /// </summary>
        public bool IsNonEmpty { get; private set; }

        /// <summary>
        /// Gets the collection of axis properties.
        /// </summary>
        public IEnumerable<string> Properties => this.properties;

        #endregion Properties

        #region Fields

        private readonly List<string> properties;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Marks axis as empty and returns the updated current instance of <see cref="MdxAxis"/>.
        /// </summary>
        /// <returns>Returns the updated current instance of <see cref="MdxAxis"/>.</returns>
        public MdxAxis AsEmpty()
        {
            this.IsNonEmpty = false;
            return this;
        }

        /// <summary>
        /// Marks axis as non-empty and returns the updated current instance of <see cref="MdxAxis"/>.
        /// </summary>
        /// <returns>Returns the updated current instance of <see cref="MdxAxis"/>.</returns>
        public MdxAxis AsNonEmpty()
        {
            this.IsNonEmpty = true;
            return this;
        }

        /// <summary>
        /// Get set member by member name
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns>Returns set member</returns>
        public MdxMember GetMember(string memberName)
        {
            return this.AxisSlicer.GetMember(memberName);
        }

        /// <summary>
        /// Sets the title for axis and returns the updated current instance of <see cref="MdxAxis"/>.
        /// </summary>
        /// <param name="title">Axis title.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxAxis"/>.</returns>
        public MdxAxis Titled(string title)
        {
            if (int.TryParse(title, out var number))
                return this.Titled(number);

            if (Enum.TryParse<MdxAxisType>(title, true, out var type))
                return this.Titled(type);

            if (!Regex.IsMatch(title, "^AXIS\\(\\d+\\)$", RegexOptions.IgnoreCase))
                throw new ArgumentException("Invalid title specified!");

            var numberMatch = Regex.Match(title, "\\d+");
            if (int.TryParse(numberMatch.Value, out number))
                return this.Titled((MdxAxisType)number);

            return this;
        }

        /// <summary>
        /// Sets the title for axis and returns the updated current instance of <see cref="MdxAxis"/>.
        /// </summary>
        /// <param name="type">Axis type.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxAxis"/>.</returns>
        public MdxAxis Titled(MdxAxisType type)
        {
            this.AxisIdentifier = type;
            return this;
        }

        /// <summary>
        /// Sets the title for axis and returns the updated current instance of <see cref="MdxAxis"/>.
        /// </summary>
        /// <param name="number">Axis number.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxAxis"/>.</returns>
        public MdxAxis Titled(int number)
        {
            this.AxisIdentifier = (MdxAxisType)number;
            return this;
        }

        /// <summary>
        /// Applies axis properties and returns the updated current instance of <see cref="MdxAxis"/>.
        /// </summary>
        /// <param name="properties">Collection of axis properties.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxAxis"/>.</returns>
        public MdxAxis WithProperties(params string[] properties)
        {
            this.properties.AddRange(properties);
            return this;
        }

        /// <summary>
        /// Sets the slicer for axis and returns the updated current instance of <see cref="MdxAxis"/>.
        /// </summary>
        /// <param name="slicer">Axis slicer.</param>
        /// <returns>Returns the updated current instance of <see cref="MdxAxis"/>.</returns>
        public MdxAxis WithSlicer(MdxTuple slicer)
        {
            this.AxisSlicer = slicer;
            return this;
        }

        #endregion Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="MdxAxis"/>.
        /// </summary>
        public MdxAxis()
        {
            this.properties = new List<string>();

            this.AxisIdentifier = MdxAxisType.Columns;
            this.AxisSlicer = null;
            this.IsNonEmpty = false;
        }

        #endregion Constructors

        protected override string GetStringExpression()
        {
            var stringBuilder = new StringBuilder();

            if (this.IsNonEmpty)
                stringBuilder.Append("NON EMPTY ");

            //TODO: Potential NRE.
            stringBuilder.AppendFormat("{0} ", this.AxisSlicer);

            if (this.Properties.Any())
            {
                var propertiesString = string.Join(", ", this.Properties);
                stringBuilder.AppendFormat("DIMENSION PROPERTIES {0} ", propertiesString);
            }

            stringBuilder.AppendFormat("ON {0}", this.AxisIdentifier);
            return stringBuilder.ToString();
        }
    }
}