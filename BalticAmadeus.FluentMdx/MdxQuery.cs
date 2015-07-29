using System;
using System.Collections.Generic;
using System.Linq;

namespace BalticAmadeus.FluentMdx
{
    public class MdxQuery : IMdxExpression
    {
        private MdxQuery _innerQuery;
        private readonly IList<MdxCube> _cubes;
        private readonly IList<MdxAxis> _axes;
        private readonly IList<MdxTuple> _whereClauseTuples;

        public MdxQuery()
        {
            _axes = new List<MdxAxis>();
            _cubes = new List<MdxCube>();
            _whereClauseTuples = new List<MdxTuple>();            
        }

        public IEnumerable<MdxCube> Cubes
        {
            get { return _cubes; }
        }

        public MdxQuery InnerQuery
        {
            get { return _innerQuery; }
        }

        public IEnumerable<MdxAxis> Axes
        {
            get { return _axes; }
        }

        public IEnumerable<MdxTuple> WhereClauseTuples
        {
            get { return _whereClauseTuples; }
        }

        public MdxQuery On(MdxAxis axis)
        {
            if (axis == null)
                throw new ArgumentNullException("axis");

            _axes.Add(axis);
            return this;
        }

        public MdxQuery From(MdxCube cube)
        {
            if (_innerQuery != null)
                throw new InvalidOperationException("Cannot apply selection from OLAP Cube, when there is inner query applied!");

            _cubes.Add(cube);
            return this;
        }

        public MdxQuery From(MdxQuery innerQuery)
        {
            if (_cubes.Any())
                throw new InvalidOperationException("Cannot apply selection inner query, when there is OLAP Cube applied!");

            _innerQuery = innerQuery;
            return this;
        }

        public MdxQuery Where(MdxTuple tuple)
        {
            _whereClauseTuples.Add(tuple);
            return this;
        }

        public string GetStringExpression()
        {
            if (_innerQuery == null)
            {
                if (!WhereClauseTuples.Any())
                    return string.Format(@"SELECT {0} FROM {1}",
                        string.Join(", ", Axes),
                        string.Join(", ", Cubes));

                return string.Format(@"SELECT {0} FROM {1} WHERE {{ ( {2} ) }}",
                    string.Join(", ", Axes),
                    string.Join(", ", Cubes),
                    string.Join(", ", WhereClauseTuples));
            }

            if (!WhereClauseTuples.Any())
                return string.Format(@"SELECT {0} FROM ( {1} )",
                    string.Join(", ", Axes),
                    InnerQuery);

            return string.Format(@"SELECT {0} FROM ( {1} ) WHERE {{ ( {2} ) }}",
                string.Join(", ", Axes),
                InnerQuery,
                string.Join(", ", WhereClauseTuples));
        }

        public override string ToString()
        {
            return GetStringExpression();
        }
    }
}