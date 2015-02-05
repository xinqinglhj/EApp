﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EApp.Data.Queries;

namespace EApp.Data.Oracle
{
    public class OracleWhereClauseBuilder<T> : WhereClauseBuilder<T> where T: class, new()
    {
        public OracleWhereClauseBuilder(IObjectMappingResolver mappingResolver) : base(mappingResolver) { }

        protected internal override char ParameterChar
        {
            get 
            {
                return ':'; 
            }
        }
    }
}
