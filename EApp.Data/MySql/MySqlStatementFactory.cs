﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EApp.Data.MySql
{
    public class MySqlStatementFactory : SqlStatementFactory, ISqlStatementFactory
    {
        private const char Parameter_Prefix = '?';

        private const char Parameter_Token = '`';

        public override string CreateInsertStatement(string tableName, string[] includedColumns)
        {
            string insertSql = @"INSERT INTO `{0}` {1} VALUES ({2})";

            StringBuilder columnNameBuilder = new StringBuilder();
            StringBuilder columnParamNameBuilder = new StringBuilder();

            if (includedColumns != null &&
                includedColumns.Length > 0)
            {
                string includedColumn;

                string dbFieldName;

                string dbFieldParamName;

                for (int columnIndex = 0; columnIndex < includedColumns.Length; columnIndex++)
                {
                    includedColumn = includedColumns[columnIndex].Trim(Parameter_Token, Parameter_Prefix);

                    dbFieldName = string.Format("{0}{1}{0},", Parameter_Token, includedColumn);

                    if (columnIndex == 0)
                    {
                        dbFieldName = "(" + dbFieldName;
                    }

                    if (columnIndex == includedColumns.Length - 1)
                    {
                        dbFieldName = dbFieldName + ")";
                    }

                    dbFieldParamName = string.Format("{0}{1},", Parameter_Prefix, includedColumn);

                    columnNameBuilder.Append(dbFieldName);

                    columnParamNameBuilder.Append(dbFieldParamName);
                }
            }
            else
            {
                columnParamNameBuilder.Append("{0}");
            }

            return string.Format(insertSql, tableName.Trim(Parameter_Token),
                columnNameBuilder.ToString().TrimEnd(',', ' '),
                columnParamNameBuilder.ToString().TrimEnd(',', ' '));
        }

        public override string CreateUpdateStatement(string tableName, string where, string[] includedColumns)
        {
            if (includedColumns == null ||
                includedColumns.Length.Equals(0))
            {
                throw new ArgumentNullException("Columns to be updated cannot be null.");
            }

            string updateSql = @"UPDATE `{0}` SET {1} {2}";

            string includedColumn;

            string fieldUpdateStatement;

            StringBuilder fieldUpdateStatementBuilder = new StringBuilder();

            for (int columnIndex = 0; columnIndex < includedColumns.Length; columnIndex++)
            {
                includedColumn = includedColumns[columnIndex].Trim(Parameter_Token, Parameter_Prefix);

                fieldUpdateStatement = string.Format("{0}{1}{0} = {2}{1},",
                    Parameter_Token, includedColumn, Parameter_Prefix);

                fieldUpdateStatementBuilder.Append(fieldUpdateStatement);
            }

            return string.Format(updateSql,
                                 tableName.Trim(Parameter_Token),
                                 fieldUpdateStatementBuilder.ToString().TrimEnd(',', ' '),
                                 string.IsNullOrEmpty(where.Trim()) ? string.Empty : "WHERE " + where);
        }

        public override string CreateDeleteStatement(string tableName, string where)
        {
            string deleteSql = @"DELETE FROM `{0}` {1}";

            return string.Format(deleteSql,
                                 tableName.Trim(Parameter_Token),
                                 string.IsNullOrEmpty(where.Trim()) ? string.Empty : "WHERE " + where);
        }

        public override string CreateSelectStatement(string tableName, string where, string orderBy, params string[] includedColumns)
        {
            string querySelectSql = "select {0} from {1} {2} {3}";

            StringBuilder selectedFieldNameBuilder = new StringBuilder();

            if (includedColumns != null)
            {
                string includedColumn;

                for (int columnIndex = 0; columnIndex < includedColumns.Length; columnIndex++)
                {
                    includedColumn = includedColumns[columnIndex].Trim(Parameter_Token, Parameter_Prefix);
                    selectedFieldNameBuilder.Append(string.Format("{0}, ", includedColumn));
                }
            }
            else
            {
                selectedFieldNameBuilder.Append("*");
            }

            return string.Format(querySelectSql,
                                 selectedFieldNameBuilder.ToString().TrimEnd(',', ' '),
                                 tableName.Trim(Parameter_Token),
                                 string.IsNullOrEmpty(where.Trim()) ? string.Empty : "WHERE " + where,
                                 string.IsNullOrEmpty(orderBy.Trim()) ? string.Empty : "ORDER BY " + orderBy);
        }

        protected override string CreateSelectTopStatement(string from, string where, string[] columns, string orderBy, string groupBy, int topCount)
        {
            throw new NotImplementedException();
        }

        protected override string CreateSelectRangeStatementForSortedRows(string from, string where, string[] columns, string orderBy, string groupBy, int topCount, int skipCount, string identityColumn, bool isIdentityColumnDesc)
        {
            throw new NotImplementedException();
        }

        protected override string CreateSelectRangeStatementForUnsortedRows(string from, string where, string[] columns, string orderBy, string groupBy, int topCount, int skipCount, string identyColumn)
        {
            throw new NotImplementedException();
        }
    }
}
