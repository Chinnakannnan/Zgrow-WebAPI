
namespace DataAccess
{
    using Dapper;
    using DapperExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class DapperRepository<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperRepository{TModel}"/> class.
        /// </summary>
        /// <param name="databaseConfig">configuration information</param>
        protected DapperRepository(IDatabaseConfig databaseConfig)
        {
            this.DatabaseConfig = databaseConfig;
        }

        /// <summary>
        /// Gets private field for database Configuration.
        /// </summary>
        private IDatabaseConfig DatabaseConfig { get; }

        /// <summary>
        /// Gets gets or sets the database conttext
        /// </summary>
        private SqlConnection Context
        {
            get
            {
                var cn = new SqlConnection(this.DatabaseConfig.ConnectionString);
                cn.Open();
                return cn;
            }
        }

        /// <summary>
        /// insert the records to the table.
        /// </summary>
        /// <param name="model">.NET object</param>
        /// <returns> This will return the status</returns>
        protected bool CreateList(IEnumerable<TModel> model)
        {
            bool savestatus = false;
            using (var db = this.Context)
            {
                db.Insert<TModel>(model);
                savestatus = true;
            }

            return savestatus;
        }

        /// <summary>
        /// Update the records to the table.
        /// </summary>
        /// <param name="model">.NET object</param>
        /// <returns>Return the Update status</returns>
        protected bool UpdateSingle(TModel model)
        {
            using (var db = this.Context)
            {
                return db.Update(model);
            }
        }

        /// <summary>
        /// Delete the records to the table.
        /// </summary>
        /// <param name="model">.NET object</param>f
        /// <returns> Return the delete status</returns>
        protected bool Delete(TModel model)
        {
            using (var db = this.Context)
            {
                return db.Delete(model);
            }
        }

        /// <summary>
        /// Retreive Many information from the List
        /// </summary>
        /// <param name="filter">fitler conditon</param>
        /// <param name="value">value of conditon</param>
        /// <returns>array object</returns>
        protected TModel[] RetrieveMany(Expression<Func<TModel, object>> filter, object value)
        {
            using (var db = this.Context)
            {
                var predicate = Predicates.Field(filter, Operator.Eq, value);
                return db.GetList<TModel>(predicate).ToArray();
            }
        }

        /// <summary>
        /// Retreive Many information from the single
        /// </summary>
        /// <param name="filter">fitler conditon</param>
        /// <param name="value">value of conditon</param>
        /// <returns>array object</returns>
        protected TModel RetrieveSingle(Expression<Func<TModel, object>> filter, object value)
        {
            using (var db = this.Context)
            {
                var predicate = Predicates.Field(filter, Operator.Eq, value);
                return db.GetList<TModel>(predicate).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retreive Many information from the single
        /// </summary>
        /// <returns>array object</returns>
        protected TModel[] RetrieveAll()
        {
            using (var db = this.Context)
            {
                return db.GetList<TModel>().ToArray();
            }
        }

        /// <summary>
        /// Retreive Many information from the single
        /// </summary>
        /// <param name="predicate">GetMutiplePredicate</param>
        /// <returns>IMultipleResultReader</returns>
        protected IMultipleResultReader GetInfoWithMultiplePredicate(GetMultiplePredicate predicate)
        {
            using (var db = this.Context)
            {
                return db.GetMultiple(predicate);
            }
        }

        /// <summary>
        /// To Execute Store Procedure
        /// </summary>
        /// <typeparam name="T">.NET OBject</typeparam>
        /// <param name="storedProcedure">  Name of the store procedure </param>
        /// <param name="param">Store Procedure Parameters</param>
        /// <returns> REturn the List</returns>
        protected IEnumerable<T> QuerySP<T>(string storedProcedure, object param = null)
        {
            using (var db = this.Context)
            {
                var output = db.Query<T>(storedProcedure, param: param, commandType: CommandType.StoredProcedure, commandTimeout: 32767);
                return output;
            }
        }
        /// <summary> Ajith
        /// To Execute Store Procedure
        /// </summary>
        /// <typeparam name="T">.NET OBject</typeparam>
        /// <param name="storedProcedure">  Name of the store procedure </param>
        /// <param name="param">Store Procedure Parameters</param>
        /// <returns> REturn True/False </returns>
        public async Task<bool> QuerySPTask(string storedProcedure, object param = null)
        {
            try
            {
                using (var db = this.Context)
                {
                    var output = db.Query(storedProcedure, param: param, commandType: CommandType.StoredProcedure, commandTimeout: 32767);
                    return true;
                }
            }
            catch (Exception ex) { return false; }          

        }
        /// <summary>
        /// SQL Inline Query to fetch results.
        /// </summary>
        /// <typeparam name="T"> Type object for return value</typeparam>
        /// <param name="sql"> inline query </param>
        /// <param name="parameters"> number of parameters</param>
        /// <param name="cmdType"> command text</param>
        /// <returns>rows as list </returns>
        protected List<T> QuerySQL<T>(string sql, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            List<T> resultList = null;
            using (var db = this.Context)
            {
                resultList = db.Query<T>(sql, param: parameters, commandType: cmdType).ToList();
            }

            return resultList;
        }

        /// <summary>
        /// SQL Inline Query to fetch main and sub entity (many/single) using dapper split on
        /// </summary>
        /// <typeparam name="T">Main Entity type</typeparam>
        /// <typeparam name="TSubEntity">Sub Entity type</typeparam>
        /// <param name="sql">SQL query</param>
        /// <param name="map">callback</param>
        /// <param name="splitOn">spliton</param>
        /// <param name="parameters">parameters for the query</param>
        /// <param name="commandType">Command type defaulted to text</param>
        /// <returns>List of main entity</returns>
        protected List<T> QuerySQL<T, TSubEntity>(string sql, Func<T, List<TSubEntity>, T> map, string splitOn, object parameters = null, CommandType commandType = CommandType.Text)
        {
            List<T> mainEntityList = null;

            using (var db = this.Context)
            {
                mainEntityList = db.Query<T, List<TSubEntity>, T>(sql, map, param: parameters, commandType: commandType, splitOn: splitOn).ToList();
            }

            return mainEntityList;
        }

        /// <summary>
        /// SQL Inline query to fetch main and sub entity using dapper split on for two nested entities
        /// </summary>
        /// <typeparam name="T">Main type</typeparam>
        /// <typeparam name="T1">Sub type 1</typeparam>
        /// <typeparam name="T2">Sub type 2</typeparam>
        /// <param name="sql">SQL Query</param>
        /// <param name="map">mapper</param>
        /// <param name="splitOn">splitOn columns</param>
        /// <param name="parameters">SQL query parameters</param>
        /// <param name="cmdType">Command type defaulted to text</param>
        /// <returns>List of main entity</returns>
        protected List<T> QuerySQL<T, T1, T2>(string sql, Func<T, T1, T2, T> map, string splitOn, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            List<T> mainEntityList = null;

            using (var db = this.Context)
            {
                mainEntityList = db.Query<T, T1, T2, T>(sql, map: map, param: parameters, splitOn: splitOn, commandType: cmdType).ToList();
            }

            return mainEntityList;
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="T">query</typeparam>
        /// <param name="query">query param</param>
        /// <returns>Query</returns>
        protected IEnumerable<T> QueryList<T>(string query)
        {
            using (var db = this.Context)
            {
                var output = db.Query<T>(query);
                return output;
            }
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="query">query param</param>
        /// <param name="parameters"> number of parameters</param>
        /// <param name="cmdType"> command text</param>
        /// <returns>Query</returns>
        protected string GetSingleField(string query, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var db = this.Context)
            {
                var output = db.QueryFirstOrDefault<string>(query, param: parameters, commandType: cmdType);
                return output;
            }
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="query">query param</param>
        /// <param name="parameters"> number of parameters</param>
        /// <param name="cmdType"> command text</param>
        /// <returns>Query</returns>
        protected int GetSingleFieldInteger(string query, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var db = this.Context)
            {
                var output = db.QueryFirstOrDefault<int>(query, param: parameters, commandType: cmdType);
                return output;
            }
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="T">query</typeparam>
        /// <param name="query">query param</param>
        /// <param name="parameters"> number of parameters</param>
        /// <param name="cmdType"> command text</param>
        /// <returns>Query</returns>
        protected T QuerySQLSingleModel<T>(string query, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var db = this.Context)
            {
                var output = db.QueryFirstOrDefault<T>(query, param: parameters, commandType: cmdType);
                return output;
            }
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="T">query</typeparam>
        /// <param name="predicate">query Group or Single Predicate</param>
        /// <returns>Query</returns>
        protected IEnumerable<T> GetList<T>(object predicate)
            where T : class
        {
            using (var db = this.Context)
            {
                var output = db.GetList<T>(predicate);
                return output.ToList();
            }
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="query">query param</param>
        /// <param name="parameters"> number of parameters</param>
        /// <param name="cmdType"> command text</param>
        /// <returns>Query</returns>
        protected int ExecuteQuery(string query, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var db = this.Context)
            {
                var output = db.Execute(query, param: parameters, commandType: cmdType);
                return output;
            }
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="query">query param</param>
        /// <param name="parameters"> number of parameters</param>
        /// <param name="cmdType"> command text</param>
        /// <returns>Query</returns>
        protected List<string> GetListofString(string query, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var db = this.Context)
            {
                var output = db.QueryFirstOrDefault<List<string>>(query, param: parameters, commandType: cmdType);
                return output;
            }
        }


        protected Tuple<List<T1>, List<T2>> GetSelectMany<T1, T2>(string query, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var db = this.Context)
            {
                var output = db.QueryMultiple(query, param: parameters, commandType: cmdType);
                var t1 = output.Read<T1>().ToList();
                var t2 = output.Read<T2>().ToList();
                return new Tuple<List<T1>, List<T2>>(t1, t2);
            }
        }

        protected bool Isany(string query, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var db = this.Context)
            {
                var output = db.QueryFirstOrDefault<string>(query, param: parameters, commandType: cmdType);
                if (output == string.Empty || output == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
