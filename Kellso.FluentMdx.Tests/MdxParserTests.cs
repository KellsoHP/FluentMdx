﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace FluentMdx.Tests
{
    [TestFixture, Ignore("Wait new parser")]
    public class MdxParserTests
    {
        private MdxParser _parserSut;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            _parserSut = new MdxParser();
        }

        [Test]
        public void ParseQuery_WithSingleAxisParameter_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube]";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(queryString));
        }

        [Test]
        public void ParseQuery_WithSingleAxisParameterAndDimensionProperties_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } " +
                                       "DIMENSION PROPERTIES CATALOG_NAME, CHILDREN_CARDINALITY, CUSTOM_ROLLUP, CUSTOM_ROLLUP_PROPERTIES, DESCRIPTION, " +
                                       "DIMENSION_UNIQUE_NAME, HIERARCHY_UNIQUE_NAME, IS_DATAMEMBER, IS_PLACEHOLDERMEMBER, KEY0, LCID, LEVEL_NUMBER, LEVEL_UNIQUE_NAME, " +
                                       "MEMBER_CAPTION, MEMBER_KEY, MEMBER_NAME, MEMBER_TYPE, MEMBER_UNIQUE_NAME, MEMBER_VALUE, PARENT_COUNT, PARENT_LEVEL, " +
                                       "PARENT_UNIQUE_NAME, SKIPPED_LEVELS, UNARY_OPERATOR, UNIQUE_NAME ON Columns " +
                                       "FROM [Cube]";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(queryString));
        }

        [Test]
        public void ParseQuery_WithMultipleAxisParameters_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure1], [Measures].[Measure2], [Measures].[Measure3] } ON Columns " +
                                       "FROM [Cube]";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(queryString));
        }

        [Test]
        public void ParseQuery_WithNoAxisParameters_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY {  } ON Columns " +
                                       "FROM [Cube]";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(queryString));
        }

        [Test]
        public void ParserQuery_WithMultipleAxes_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns, " +
                                       "NON EMPTY { [Dim Hierarchy].[Dim].ALLMEMBERS } ON Rows " +
                                       "FROM [Cube]";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(queryString));
        }

        [Test]
        public void ParseQuery_WithSingleWhereValueMember_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE [Dim Hierarchy].[Dim].[Dim Key].&[1]";

            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE { ( { [Dim Hierarchy].[Dim].[Dim Key].&[1] } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithSingleWhereRangeMember_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE [Dim Hierarchy].[Dim].[Dim Key].&[1]:[Dim Hierarchy].[Dim].[Dim Key].&[2]";
            
            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE { ( { [Dim Hierarchy].[Dim].[Dim Key].&[1]:[Dim Hierarchy].[Dim].[Dim Key].&[2] } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithSingleWhereRangeMemberDate_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE [Dim Hierarchy].[Date].[Date].&[2010-10-10T00:00:00]:[Dim Hierarchy].[Date].[Date].&[2011-10-10T00:00:00]";

            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE { ( { [Dim Hierarchy].[Date].[Date].&[2010-10-10T00:00:00]:[Dim Hierarchy].[Date].[Date].&[2011-10-10T00:00:00] } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithSingleWhereTuple_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE { [Dim Hierarchy].[Dim].[Dim Key].&[1], [Dim Hierarchy].[Dim].[Dim Key].&[3] }";

            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE { ( { [Dim Hierarchy].[Dim].[Dim Key].&[1], [Dim Hierarchy].[Dim].[Dim Key].&[3] } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT        
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithSingleWhereSet_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE ( [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] )";

            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE { ( { ( [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] ) } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithMultipleSetsInTuple_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE ( [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] )";

            const string expectedString = "SELECT " +
                                          "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                          "FROM [Cube] " +
                                          "WHERE { ( { ( [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] ) } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithMultipleTuplesInSet_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE ( " +
                                       "{ [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[2] }, " +
                                       "{ [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] } " +
                                       ")";

            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM [Cube] " +
                                       "WHERE { ( { ( " +
                                       "{ [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[2] }, " +
                                       "{ [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] } " +
                                       ") } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithInnerQuery_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM ( " +
                                           "SELECT " +
                                           "NON EMPTY { [Dim Hierarchy].[Dim].&[1], [Dim Hierarchy].[Dim].&[2] } ON Columns " +
                                           "FROM [Cube] " +
                                       ") WHERE ( " +
                                       "{ [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[2] }, " +
                                       "{ [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] } " +
                                       ")";

            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Measures].[Measure] } ON Columns " +
                                       "FROM ( " +
                                           "SELECT " +
                                           "NON EMPTY { [Dim Hierarchy].[Dim].&[1], [Dim Hierarchy].[Dim].&[2] } ON Columns " +
                                           "FROM [Cube] " +
                                       ") WHERE { ( { ( " +
                                       "{ [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1], [Dim1 Hierarchy].[Dim1].[Dim1 Key].&[2] }, " +
                                       "{ [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[1], [Dim2 Hierarchy].[Dim2].[Dim2 Key].&[2] } " +
                                       ") } ) }";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithFunctions_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT " +
                                       "NON EMPTY { [Dim Hierarchy1].[Dim1], [Dim Hierarchy1].[Dim2], [Dim Hierarchy1].[Dim3] } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON Columns, " +
                                       "NON EMPTY { [Dim Hierarchy2].[D.im1], ORDER([Dim Hierarchy2].[Dim2].Children, [Dim Hierarchy2].[Dim2].CurrentMember.MEMBER_CAPTION, asc) } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON Rows " +
                                       "FROM [Cube]";

            const string expectedString = "SELECT " +
                                          "NON EMPTY { [Dim Hierarchy1].[Dim1], [Dim Hierarchy1].[Dim2], [Dim Hierarchy1].[Dim3] } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON Columns, " +
                                          "NON EMPTY { [Dim Hierarchy2].[D.im1], ORDER([Dim Hierarchy2].[Dim2].Children, [Dim Hierarchy2].[Dim2].CurrentMember.MEMBER_CAPTION, asc) } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON Rows " +
                                          "FROM [Cube]";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithFunctionsAndExpressions_ReturnsParsedQuery()
        {
            //ARRANGE   
            const string queryString = "SELECT \n\r" +
                                       "NON EMPTY { [Dim Hierarchy1].[Dim1], [Dim Hierarchy1].[Dim2], [Dim Hierarchy1].[Dim3] } ON Columns, " +
                                       "NON EMPTY { [Dim Hierarchy2].[Dim1], ORDER([Dim Hierarchy2].[Dim2].Children, [Dim Hierarchy2].[Dim2].CurrentMember.MEMBER_CAPTION, asc) } ON Rows " +
                                       "FROM ( " +
                                       "SELECT (Filter([Dim Hierarchy2].[Dim1].MEMBERS, NOT [Dim Hierarchy2].[Dim1].CurrentMember.MEMBER_CAPTION = \"V\")) ON 0 FROM [Cube] " +
                                       ")";

            const string expectedString = "SELECT " +
                                       "NON EMPTY { [Dim Hierarchy1].[Dim1], [Dim Hierarchy1].[Dim2], [Dim Hierarchy1].[Dim3] } ON Columns, " +
                                       "NON EMPTY { [Dim Hierarchy2].[Dim1], ORDER([Dim Hierarchy2].[Dim2].Children, [Dim Hierarchy2].[Dim2].CurrentMember.MEMBER_CAPTION, asc) } ON Rows " +
                                       "FROM ( " +
                                       "SELECT { ( Filter([Dim Hierarchy2].[Dim1].MEMBERS, (NOT ([Dim Hierarchy2].[Dim1].CurrentMember.MEMBER_CAPTION = \"V\"))) ) } ON Columns FROM [Cube] " +
                                       ")";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseQuery_WithQueryWithClause_ReturnsParsedQuery()
        {
            const string statement = "SELECT " +
                               "NON EMPTY { [Measures].[Active Days], [Measures].[Distance Sailed], [Measures].[Speed Loss Factor], [Measures].[Ton x nm], [Measures].[Consumed Hfo], [Measures].[Consumed Mgo], [Measures].[Consumed Total], [Measures].[g/ton-nm], [Measures].[kg/nm] } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON COLUMNS, " +
                               "NON EMPTY { [Vessel].[Vessel Name], [Vessel].[Vessel Name].Children } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON ROWS " +
                               "FROM ( SELECT { ( { [Vessel].[Vessel Plant Id].&[BOH], [Vessel].[Vessel Plant Id].&[CRM] }, { [Date].[Date].&[2014-07-21T00:00:00]:[Date].[Date].&[2015-08-22T00:00:00] }, { [Operation Mode].[Operation Mode Text].&[Harbour Out], [Operation Mode].[Operation Mode Text].&[Sea passage], [Operation Mode].[Operation Mode Text].&[Idling], [Operation Mode].[Operation Mode Text].&[Harbour In] }, { [Company].[Company Name].&[FakeVessel] } ) } ON 0 FROM [TransportWork] )";

            //ARRANGE   
            const string queryString = statement;

            const string expectedString = "SELECT " +
                               "NON EMPTY { [Measures].[Active Days], [Measures].[Distance Sailed], [Measures].[Speed Loss Factor], [Measures].[Ton x nm], [Measures].[Consumed Hfo], [Measures].[Consumed Mgo], [Measures].[Consumed Total], [Measures].[g/ton-nm], [Measures].[kg/nm] } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON Columns, " +
                               "NON EMPTY { [Vessel].[Vessel Name], [Vessel].[Vessel Name].Children } DIMENSION PROPERTIES CHILDREN_CARDINALITY, PARENT_UNIQUE_NAME ON Rows " +
                               "FROM ( SELECT { ( { [Vessel].[Vessel Plant Id].&[BOH], [Vessel].[Vessel Plant Id].&[CRM] }, { [Date].[Date].&[2014-07-21T00:00:00]:[Date].[Date].&[2015-08-22T00:00:00] }, { [Operation Mode].[Operation Mode Text].&[Harbour Out], [Operation Mode].[Operation Mode Text].&[Sea passage], [Operation Mode].[Operation Mode Text].&[Idling], [Operation Mode].[Operation Mode Text].&[Harbour In] }, { [Company].[Company Name].&[FakeVessel] } ) } ON Columns FROM [TransportWork] )";

            //ACT
            var query = _parserSut.ParseQuery(queryString);

            //ASSERT
            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo(expectedString));
        }

        [Test]
        public void ParseMember_WithNavigationFx_MemberCreatedAsExpected()
        {
            //ARRANGE
            const string memberString = "[Test].[Test By One].CurrentMember.MEMBER_VALUE";

            //ACT
            var member = _parserSut.ParseMember(memberString);

            //ASSERT
            var navFx = member.NavigationFunctions.ToList();
            var navTitles = member.Titles.ToList();
            Assert.That(member.ToString(), Is.EqualTo(memberString));
            Assert.AreEqual(navFx[0].Title, "CurrentMember");
            Assert.AreEqual(navFx[1].Title, "MEMBER_VALUE");
            Assert.AreEqual(navTitles[0], "Test");
            Assert.AreEqual(navTitles[1], "Test By One");
        }
    }
}
