using EnumsNET;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Queries;
using EPiServer.Find.Cms;
using EPiServer.Find.Helpers;
using EPiServer.Find.Statistics;
using EPiServer.Find.Statistics.Api;
using EPiServer.Find.Tracing;
using EPiServer.Find.UnifiedSearch;
using NRL.Shared.Collections;
using NRL.Shared.Expressions;
using NRL.Shared.Extensions;
using NRL.Shared.Types;
using NRL.Web.Business.Find;
using NRL.Web.Business.Models;
using NRL.Web.Business.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public static class ITypeSearchExtensions
{
   public static ITypeSearch<T> AndAny<T, TVal>(this ITypeSearch<T> query, IEnumerable<TVal> values, Expression<Func<T, TVal, Filter>> filterExpression)
        {
            if (values.IsNullOrEmpty())
                return query;

            var builder = new FilterBuilder<T>(query.Client).Any(values, filterExpression);
            return query.Filter(builder);
        }

        public static ITypeSearch<T> OrAny<T, TVal>(this ITypeSearch<T> query, IEnumerable<TVal> values, Expression<Func<T, TVal, Filter>> filterExpression)
        {
            if (values.IsNullOrEmpty())
                return query;

            var builder = new FilterBuilder<T>(query.Client).Any(values, filterExpression);
            return query.OrFilter(builder);
        }
		
		 private static FilterBuilder<T> Any<T, TVal>(this FilterBuilder<T> builder, IEnumerable<TVal> values, Expression<Func<T, TVal, Filter>> filterExpression)
        {
            // Expression visitor to modify the parameter in the given filter expression 
            var visitor = new ParameterReplacerVisitor<Func<T, TVal, Filter>, Func<T, Filter>>();

            // Parameter to be replaced with constant
            var parameter = filterExpression.Parameters[1];
            foreach (var value in values)
            {
                visitor.Source = parameter;
                visitor.Value = value;

                builder = builder.Or(visitor.ReplaceValue(filterExpression));
            }

            return builder;
        }
}