﻿

namespace FluentMigrator.Tests.Unit.Generators
{
    using System;
    using System.Data;
    using FluentMigrator.Expressions;
    using FluentMigrator.Model;
    using NUnit.Framework;
    using FluentMigrator.Runner.Generators.SqlServer;
    using NUnit.Should;

    [TestFixture]
    public class SqlServerCeGeneratorTests
    {
        private const string tableName = "NewTable";
        SqlServerCeGenerator generator;

        [SetUp]
        public void SetUp()
        {
            generator = new SqlServerCeGenerator();
        }

        [Test]
        public void DoesNotImplementASchema()
        {
            var expression = GetCreateTableExpression(tableName);
            expression.Columns[0].Type = DbType.String;
            expression.Columns[0].Size = 100;
            var sql = generator.Generate(expression);
            sql.ShouldBe("CREATE TABLE [NewTable] (ColumnName1 NVARCHAR(100) NOT NULL)");
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotCreateASchema()
        {
            var sql = generator.Generate(new CreateSchemaExpression());
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotAlterASchema()
        {
            var sql = generator.Generate(new AlterSchemaExpression());
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotDeleteASchema()
        {
            var sql = generator.Generate(new DeleteSchemaExpression());
        }

        [Test]
        public void CreatesTheCorrectSyntaxToDropAnIndex()
        {
            var expression = new DeleteIndexExpression();
            expression.Index = new IndexDefinition() {Name = "MyColumn", TableName = "MyTable"};
            var sql = generator.Generate(expression);
            sql.ShouldBe("DROP INDEX [MyTable].[MyColumn]");
        }

        private static CreateTableExpression GetCreateTableExpression(string tableName)
        {
            var columnName1 = "ColumnName1";

            var column1 = new ColumnDefinition { Name = columnName1, Type = DbType.String };

            var expression = new CreateTableExpression { TableName = tableName };
            expression.Columns.Add(column1);
            return expression;
        }

    }
}