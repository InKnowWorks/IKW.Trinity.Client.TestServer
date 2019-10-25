#pragma warning disable 162,168,649,660,661,1522

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity;
using Trinity.TSL;
using Trinity.TSL.Lib;
using Trinity.Storage;
using System.Linq.Expressions;
using Trinity.Client.TestProtocols.Linq;
using Trinity.Storage.Transaction;
namespace Trinity.Client.TestProtocols
{
    
    #region Internal
    /**
     * <summary>
     * Accepts transformation from TripleStore_Accessor to T.
     * </summary>
     */
    internal class TripleStore_Accessor_local_projector<T> : IQueryable<T>
    {
        private         Expression                                   query_expression;
        private         TripleStore_Accessor_local_query_provider    query_provider;
        internal TripleStore_Accessor_local_projector(TripleStore_Accessor_local_query_provider provider, Expression expression)
        {
            this.query_expression              = expression;
            this.query_provider                = provider;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerator<T>>(query_expression);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public Type ElementType
        {
            get { return typeof(T); }
        }
        public Expression Expression
        {
            get { return query_expression; }
        }
        public IQueryProvider Provider
        {
            get { return query_provider; }
        }
    }
    /**
     * Accepts transformation from TripleStore to T.
     */
    internal class TripleStore_local_projector<T> : IQueryable<T>
    {
        private         Expression                                   query_expression;
        private         TripleStore_local_query_provider             query_provider;
        internal TripleStore_local_projector(TripleStore_local_query_provider provider, Expression expression)
        {
            this.query_expression              = expression;
            this.query_provider                = provider;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerator<T>>(query_expression);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public Type ElementType
        {
            get { return typeof(T); }
        }
        public Expression Expression
        {
            get { return query_expression; }
        }
        public IQueryProvider Provider
        {
            get { return query_provider; }
        }
    }
    internal class TripleStore_AccessorEnumerable : IEnumerable<TripleStore_Accessor>
    {
        private     LocalMemoryStorage              m_storage;
        private     LocalTransactionContext         m_tx;
        private     HashSet<long>                   m_filter_set;
        private     bool                            m_is_positive_filtering;
        private     Func<TripleStore_Accessor,bool> m_filter_predicate;
        internal TripleStore_AccessorEnumerable(LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            this.m_storage     = storage;
            m_filter_set       = null;
            m_filter_predicate = null;
            m_tx               = tx;
        }
        internal void SetPositiveFiltering(HashSet<long> set)
        {
            this.m_filter_set       = set;
            m_is_positive_filtering = true;
        }
        internal void SetNegativeFiltering(HashSet<long> set)
        {
            this.m_filter_set       = set;
            m_is_positive_filtering = false;
        }
        public IEnumerator<TripleStore_Accessor> GetEnumerator()
        {
            if (m_filter_set == null)
            {
                if (m_filter_predicate == null)
                    foreach (var cellInfo in m_storage)
                    {
                        if (cellInfo.CellType == (ushort)CellType.TripleStore)
                        {
                            var accessor = TripleStore_Accessor.AllocIterativeAccessor(cellInfo, m_tx);
                            yield return accessor;
                            accessor.Dispose();
                        }
                    }
                else
                    foreach (var cellInfo in m_storage)
                    {
                        if (cellInfo.CellType == (ushort)CellType.TripleStore)
                        {
                            var accessor = TripleStore_Accessor.AllocIterativeAccessor(cellInfo, m_tx);
                            if (m_filter_predicate(accessor))
                                yield return accessor;
                            accessor.Dispose();
                        }
                    }
            }
            else if (m_is_positive_filtering)
            {
                if (m_filter_predicate == null)
                    foreach (var cellID in m_filter_set)
                    {
                        using (var accessor = m_storage.UseTripleStore(cellID))
                        {
                            yield return accessor;
                        }
                    }
                else
                    foreach (var cellID in m_filter_set)
                    {
                        using (var accessor = m_storage.UseTripleStore(cellID))
                        {
                            if (m_filter_predicate(accessor))
                                yield return accessor;
                        }
                    }
            }
            else
            {
                throw new NotImplementedException("Negative filtering not supported.");
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        internal void SetPredicate(Expression aggregated_where_clause, ParameterExpression parameter)
        {
            m_filter_predicate = Expression.Lambda<Func<TripleStore_Accessor, bool>>(
                aggregated_where_clause,
                parameter
                ).Compile();
        }
    }
    internal class TripleStore_Enumerable : IEnumerable<TripleStore>
    {
        private LocalMemoryStorage      m_storage;
        private HashSet<long>           m_filter_set;
        private bool                    m_is_positive_filtering;
        private Func<TripleStore,bool>  m_filter_predicate;
        private static Type             m_cell_type = typeof(TripleStore);
        private LocalTransactionContext m_tx;
        internal TripleStore_Enumerable(LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            m_storage          = storage;
            m_filter_set       = null;
            m_filter_predicate = null;
            m_tx               = tx;
        }
        internal void SetPositiveFiltering(HashSet<long> set)
        {
            this.m_filter_set       = set;
            m_is_positive_filtering = true;
        }
        internal void SetNegativeFiltering(HashSet<long> set)
        {
            this.m_filter_set       = set;
            m_is_positive_filtering = false;
        }
        public IEnumerator<TripleStore> GetEnumerator()
        {
            if (m_filter_set == null)
            {
                if (m_filter_predicate == null)
                    foreach (var cellInfo in m_storage)
                    {
                        if (cellInfo.CellType == (ushort)CellType.TripleStore)
                        {
                            var accessor = TripleStore_Accessor.AllocIterativeAccessor(cellInfo, m_tx);
                            yield return accessor;
                            accessor.Dispose();
                        }
                    }
                else
                    foreach (var cellInfo in m_storage)
                    {
                        if (cellInfo.CellType == (ushort)CellType.TripleStore)
                        {
                            var accessor = TripleStore_Accessor.AllocIterativeAccessor(cellInfo, m_tx);
                            if (m_filter_predicate(accessor))
                                yield return accessor;
                            accessor.Dispose();
                        }
                    }
            }
            else if (m_is_positive_filtering)
            {
                if (m_filter_predicate == null)
                    foreach (var cellID in m_filter_set)
                    {
                        using (var accessor = m_storage.UseTripleStore(cellID))
                        {
                            yield return accessor;
                        }
                    }
                else
                    foreach (var cellID in m_filter_set)
                    {
                        using (var accessor = m_storage.UseTripleStore(cellID))
                        {
                            if (m_filter_predicate(accessor))
                                yield return accessor;
                        }
                    }
            }
            else
            {
                throw new NotImplementedException("Negative filtering not supported.");
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        internal void SetPredicate(Expression aggregated_where_clause, ParameterExpression parameter)
        {
            m_filter_predicate = Expression.Lambda<Func<TripleStore, bool>>(
                aggregated_where_clause,
                parameter
                ).Compile();
        }
    }
    internal class TripleStore_Accessor_local_query_provider : IQueryProvider
    {
        private static  Type                             s_accessor_type    = typeof(TripleStore_Accessor);
        private static  Type                             s_cell_type        = typeof(TripleStore);
        private static  Type                             s_ienumerable_type = typeof(IEnumerable<>);
        private         TripleStore_AccessorEnumerable   m_accessor_enumerable;
        internal TripleStore_Accessor_local_query_provider(LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            m_accessor_enumerable = new TripleStore_AccessorEnumerable(storage, tx);
        }
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof(TElement) == s_accessor_type)
            {
                return (IQueryable<TElement>)new TripleStore_Accessor_local_selector(this, expression);
            }
            else
            {
                return new TripleStore_Accessor_local_projector<TElement>(this, expression);
            }
        }
        public TResult Execute<TResult>(Expression expression)
        {
            var  visitor              = new RewritableWhereCaluseVisitor<TripleStore_Accessor>(expression);
            var  where_clauses        = visitor.RewritableWhereClauses;
            var  queryable            = m_accessor_enumerable.AsQueryable<TripleStore_Accessor>();
            var  trimmed_expression   = visitor.InjectEnumerator(expression, queryable, typeof(TripleStore_Accessor_local_selector));
            if (where_clauses.Count != 0)
            {
                var subject_rewriter           = new PredicateSubjectRewriter<TripleStore_Accessor>();
                Expression aggregated_predicate = subject_rewriter.Visit(where_clauses.First().Body);
                foreach (var where_clause in where_clauses.Skip(1))
                {
                    Expression predicate = where_clause.Body;
                    aggregated_predicate = Expression.AndAlso(aggregated_predicate, subject_rewriter.Visit(predicate));
                }
                IndexQueryTreeGenerator<TripleStore_Accessor> query_tree_gen       = new IndexQueryTreeGenerator<TripleStore_Accessor>("TripleStore", Index.s_AccessorSubstringIndexAccessMethod, is_cell: false);
                aggregated_predicate                                               = query_tree_gen.Visit(aggregated_predicate);
                var query_tree                                                     = query_tree_gen.QueryTree;
                if (query_tree != null)
                {
                    query_tree = query_tree.Optimize();
                    var query_tree_exec = new IndexQueryTreeExecutor(Index.s_AccessorSubstringQueryMethodTable, Index.s_AccessorSubstringWildcardQueryMethodTable);
                    m_accessor_enumerable.SetPositiveFiltering(query_tree_exec.Execute(query_tree));
                }
                m_accessor_enumerable.SetPredicate(aggregated_predicate, subject_rewriter.Parameter);
            }
            if (trimmed_expression.NodeType == ExpressionType.Constant)
            {
                return (TResult)m_accessor_enumerable.GetEnumerator();
            }
            Type result_type          = typeof(TResult);
            bool result_is_enumerable = (result_type.GenericTypeArguments.Count() == 1);
            Type element_type         = result_is_enumerable ? result_type.GenericTypeArguments[0] : result_type;
            if (result_is_enumerable)
            {
                var  enumerator_type      = s_ienumerable_type.MakeGenericType(element_type);
                var  enumerator_extractor = Expression.Call(trimmed_expression, enumerator_type.GetMethod("GetEnumerator"));
                var  lambda               = Expression.Lambda<Func<TResult>>(enumerator_extractor);
                var  func                 = (lambda).Compile();
                var  result               = func();
                return result;
            }
            else
            {
                var  lambda               = Expression.Lambda<Func<TResult>>(trimmed_expression);
                var  func                 = (lambda).Compile();
                var  result               = func();
                return result;
            }
        }
        #region Not implemented
        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }
        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    internal class TripleStore_local_query_provider : IQueryProvider
    {
        private static  Type                             s_cell_type        = typeof(TripleStore);
        private static  Type                             s_ienumerable_type = typeof(IEnumerable<>);
        private         TripleStore_Enumerable           s_cell_enumerable;
        internal TripleStore_local_query_provider(LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            s_cell_enumerable = new TripleStore_Enumerable(storage, tx);
        }
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof(TElement) == s_cell_type)
            {
                return (IQueryable<TElement>)new TripleStore_local_selector(this, expression);
            }
            else
            {
                return new TripleStore_local_projector<TElement>(this, expression);
            }
        }
        public TResult Execute<TResult>(Expression expression)
        {
            var  visitor              = new RewritableWhereCaluseVisitor<TripleStore>(expression);
            var  where_clauses        = visitor.RewritableWhereClauses;
            var  queryable            = s_cell_enumerable.AsQueryable<TripleStore>();
            var  trimmed_expression   = visitor.InjectEnumerator(expression, queryable, typeof(TripleStore_local_selector));
            if (where_clauses.Count != 0)
            {
                var subject_rewriter           = new PredicateSubjectRewriter<TripleStore>();
                Expression aggregated_predicate = subject_rewriter.Visit(where_clauses.First().Body);
                foreach (var where_clause in where_clauses.Skip(1))
                {
                    Expression predicate = where_clause.Body;
                    aggregated_predicate = Expression.AndAlso(aggregated_predicate, subject_rewriter.Visit(predicate));
                }
                IndexQueryTreeGenerator<TripleStore> query_tree_gen       = new IndexQueryTreeGenerator<TripleStore>("TripleStore", Index.s_CellSubstringIndexAccessMethod, is_cell: true);
                aggregated_predicate                                      = query_tree_gen.Visit(aggregated_predicate);
                var query_tree                                            = query_tree_gen.QueryTree;
                if (query_tree != null)
                {
                    query_tree = query_tree.Optimize();
                    var query_tree_exec = new IndexQueryTreeExecutor(Index.s_CellSubstringQueryMethodTable, Index.s_CellSubstringWildcardQueryMethodTable);
                    s_cell_enumerable.SetPositiveFiltering(query_tree_exec.Execute(query_tree));
                }
                s_cell_enumerable.SetPredicate(aggregated_predicate, subject_rewriter.Parameter);
            }
            if (trimmed_expression.NodeType == ExpressionType.Constant)
            {
                return (TResult)s_cell_enumerable.GetEnumerator();
            }
            Type result_type          = typeof(TResult);
            bool result_is_enumerable = (result_type.GenericTypeArguments.Count() == 1);
            Type element_type         = result_is_enumerable ? result_type.GenericTypeArguments[0] : result_type;
            if (result_is_enumerable)
            {
                var  enumerator_type      = s_ienumerable_type.MakeGenericType(element_type);
                var  enumerator_extractor = Expression.Call(trimmed_expression, enumerator_type.GetMethod("GetEnumerator"));
                var  lambda               = Expression.Lambda<Func<TResult>>(enumerator_extractor);
                var  func                 = (lambda).Compile();
                var  result               = func();
                return result;
            }
            else
            {
                var  lambda               = Expression.Lambda<Func<TResult>>(trimmed_expression);
                var  func                 = (lambda).Compile();
                var  result               = func();
                return result;
            }
        }
        #region Not implemented
        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }
        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion
    #region Public
    /// <summary>
    /// Implements System.Linq.IQueryable{TripleStore_Accessor} and accepts LINQ
    /// queries on <see cref="Trinity.Global.LocalStorage"/>.
    /// </summary>
    public class TripleStore_Accessor_local_selector : IQueryable<TripleStore_Accessor>
    {
        private         Expression                                   query_expression;
        private         TripleStore_Accessor_local_query_provider    query_provider;
        private TripleStore_Accessor_local_selector() { /* nobody should reach this method */ }
        internal TripleStore_Accessor_local_selector(Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            this.query_expression              = Expression.Constant(this);
            this.query_provider                = new TripleStore_Accessor_local_query_provider(storage, tx);
        }
        internal unsafe TripleStore_Accessor_local_selector(TripleStore_Accessor_local_query_provider query_provider, Expression query_expression)
        {
            this.query_expression              = query_expression;
            this.query_provider                = query_provider;
        }
        #region IQueryable<CellAccessor> interfaces
        public IEnumerator<TripleStore_Accessor> GetEnumerator()
        {
            return Provider.Execute<IEnumerator<TripleStore_Accessor>>(query_expression);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public Type ElementType
        {
            get { return typeof(TripleStore_Accessor); }
        }
        public Expression Expression
        {
            get { return this.query_expression; }
        }
        public IQueryProvider Provider
        {
            get { return query_provider; }
        }
        #endregion
        #region PLINQ Wrapper
        public PLINQWrapper<TripleStore_Accessor> AsParallel()
        {
            return new PLINQWrapper<TripleStore_Accessor>(this);
        }
        #endregion
    }
    /// <summary>
    /// Implements System.Linq.IQueryable{TripleStore} and accepts LINQ
    /// queries on <see cref="Trinity.Global.LocalStorage"/>.
    /// </summary>
    public class TripleStore_local_selector : IQueryable<TripleStore>, IOrderedQueryable<TripleStore>
    {
        private         Expression                                   query_expression;
        private         TripleStore_local_query_provider             query_provider;
        private TripleStore_local_selector() { /* nobody should reach this method */ }
        internal TripleStore_local_selector(Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            this.query_expression              = Expression.Constant(this);
            this.query_provider                = new TripleStore_local_query_provider(storage, tx);
        }
        internal unsafe TripleStore_local_selector(TripleStore_local_query_provider query_provider, Expression query_expression)
        {
            this.query_expression              = query_expression;
            this.query_provider                = query_provider;
        }
        #region IQueryable<Cell> interfaces
        public IEnumerator<TripleStore> GetEnumerator()
        {
            return Provider.Execute<IEnumerator<TripleStore>>(query_expression);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator<TripleStore>)this.GetEnumerator();
        }
        public Type ElementType
        {
            get { return typeof(TripleStore); }
        }
        public Expression Expression
        {
            get { return this.query_expression; }
        }
        public IQueryProvider Provider
        {
            get { return query_provider; }
        }
        #endregion
    }
    #endregion
    
    public static class LocalStorageCellSelectorExtension
    {
        
        /// <summary>
        /// Enumerates all the TripleStore within the local memory storage.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> object.</param>
        /// <returns>All the TripleStore within the local memory storage.</returns>
        public static TripleStore_local_selector TripleStore_Selector(this LocalMemoryStorage storage)
        {
            return new TripleStore_local_selector(storage, null);
        }
        /// <summary>
        /// Enumerates all the TripleStore_Accessor within the local memory storage.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> object.</param>
        /// <returns>All the TripleStore_Accessor within the local memory storage.</returns>
        public static TripleStore_Accessor_local_selector TripleStore_Accessor_Selector(this LocalMemoryStorage storage)
        {
            return new TripleStore_Accessor_local_selector(storage, null);
        }
        /// <summary>
        /// Enumerates all the TripleStore within the local memory storage.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> object.</param>
        /// <returns>All the TripleStore within the local memory storage.</returns>
        public static TripleStore_local_selector TripleStore_Selector(this LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            return new TripleStore_local_selector(storage, tx);
        }
        /// <summary>
        /// Enumerates all the TripleStore_Accessor within the local memory storage.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> object.</param>
        /// <returns>All the TripleStore_Accessor within the local memory storage.</returns>
        public static TripleStore_Accessor_local_selector TripleStore_Accessor_Selector(this LocalMemoryStorage storage, LocalTransactionContext tx)
        {
            return new TripleStore_Accessor_local_selector(storage, tx);
        }
        
    }
}

#pragma warning restore 162,168,649,660,661,1522
