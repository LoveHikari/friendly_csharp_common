using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace System.MongoDB
{
    public class MongodbBase<T> where T : class, IMongoEntity
    {
        protected MongoServer Server;
        protected MongoDatabase Db;
        protected MongoCollection<T> Collection;
        protected void Init(string dbName)
        {
            ManagerConfig managerConfig = new ManagerConfig();
            var item = managerConfig.ServiceSettings.Mongodbs.FirstOrDefault(p => p.DbName == dbName);
            if (item == null)
            {
                throw new Exception("不存在数据库为: " + dbName + " 的配置对象，请检查");
            }
            else
            {
                Server = new MongoClient(item.HostName).GetServer();
                Db = Server.GetDatabase(item.DbName);
                Collection = Db.GetCollection<T>(typeof(T).Name.Replace("Entity", ""));
            }
        }

        #region 查询
        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetModelById(string id)
        {
            return Collection.FindOneById(id);
        }

        /// <summary>
        /// 获取一条记录(自定义条件)
        /// </summary>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            IMongoQuery query = Query<T>.Where(expression);
            return Collection.FindOne(query);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <returns></returns>
        public T FirstOrDefault()
        {
            return Collection.FindAll().FirstOrDefault();
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public List<T> FindAll()
        {
            return Collection.FindAll().ToList();
        }

        /// <summary>
        /// 获取全部(自定义条件)
        /// </summary>
        /// <returns></returns>
        public List<T> FindAll(Expression<Func<T, bool>> expression)
        {
            IMongoQuery query = Query<T>.Where(expression);
            return Collection.Find(query).ToList();
        }

        /// <summary>
        /// 根据条件获取数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public long GetCount(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return Collection.Count();
            }
            else
            {
                return Collection.Count(Query<T>.Where(expression));
            }
        }

        /// <summary>
        /// 根据ID判断是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(string id)
        {
            return Collection.FindOneById(id) != null;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex">总页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCounts">总记录数</param>
        /// <param name="expression">条件</param>
        /// <param name="isAsc">是否是正序</param>
        /// <param name="orderFiled">排序的字段</param>
        /// <returns></returns>
        public List<T> Page(int pageIndex, int pageSize, out long rowCounts, Expression<Func<T, bool>> expression = null, bool isAsc = true, params string[] orderFiled)
        {
            MongoCursor<T> mongoCursor;

            //条件选择
            if (expression != null)
            {
                rowCounts = Collection.Find(Query<T>.Where(expression)).Count();
                mongoCursor = Collection.Find(Query<T>.Where(expression));
            }
            else
            {
                rowCounts = Collection.FindAll().Count();
                mongoCursor = Collection.FindAll();
            }

            //排序
            if (orderFiled != null && orderFiled.Length > 0)
            {
                //处理主键字段
                for (int i = 0; i < orderFiled.Length; i++)
                {
                    if (orderFiled[i].Equals("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        orderFiled[i] = "_id";
                    }
                }

                if (isAsc)
                {
                    mongoCursor = mongoCursor.SetSortOrder(SortBy.Ascending(orderFiled));
                }
                else
                {
                    mongoCursor = mongoCursor.SetSortOrder(SortBy.Descending(orderFiled));
                }
            }

            return mongoCursor.SetSkip((pageIndex - 1) * pageSize).SetLimit(pageSize).ToList();
        }

        #region 效率低，暂时不用
        ///// <summary>
        ///// 分页
        ///// </summary>
        ///// <returns></returns>
        //public List<T> Page(int PageIndex, int PageSize, out  long RowCounts, Expression<Func<T, bool>> expression = null)
        //{
        //    List<T> ret = new List<T>();
        //    IQueryable<T> queryable;
        //    //条件选择
        //    if (expression != null)
        //    {
        //        queryable = collection.Find(Query<T>.Where(expression)).AsQueryable();
        //    }
        //    else
        //    {
        //        queryable = collection.FindAll().AsQueryable();
        //    }
        //    RowCounts = queryable.Count();
        //    ret = queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        //    return ret;
        //}

        ///// <summary>
        ///// 分页
        ///// </summary>
        ///// <typeparam name="TKey"></typeparam>
        ///// <param name="PageIndex"></param>
        ///// <param name="PageSize"></param>
        ///// <param name="RowCounts"></param>
        ///// <param name="expression"></param>
        ///// <param name="orderBy"></param>
        ///// <param name="IsOrder"></param>
        ///// <returns></returns>
        //public List<T> Page<TKey>(int PageIndex, int PageSize, out  long RowCounts, Expression<Func<T, bool>> expression = null, Expression<Func<T, TKey>> orderBy = null, bool IsOrder = true)
        //{
        //    List<T> ret = new List<T>();
        //    IQueryable<T> queryable;

        //    //条件选择
        //    if (expression != null)
        //    {
        //        queryable = collection.Find(Query<T>.Where(expression)).AsQueryable();
        //    }
        //    else
        //    {
        //        queryable = collection.FindAll().AsQueryable();
        //    }
        //    //排序
        //    if (orderBy != null)
        //    {
        //        if (IsOrder)
        //        {
        //            queryable = queryable.OrderBy(orderBy);
        //        }
        //        else
        //        {
        //            queryable = queryable.OrderByDescending(orderBy);
        //        }
        //    }
        //    RowCounts = queryable.Count();
        //    ret = queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        //    return ret;
        //} 
        #endregion

        #endregion

        #region 删除


        /// <summary>
        /// 带条件的删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public void Delete(Expression<Func<T, bool>> expression)
        {
            IMongoQuery query = Query<T>.Where(expression);
            Collection.Remove(query);
        }
        /// <summary>
        /// 根据模型删除
        /// </summary>
        /// <param name="model"></param>
        public void Delete(T model)
        {
            IMongoQuery query = Query<T>.Where(p => p.Id == model.Id);
            Collection.Remove(query);
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            IMongoQuery query = Query<T>.Where(p => p.Id == id);
            Collection.Remove(query);
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns></returns>
        public void DeleteAll()
        {
            Collection.RemoveAll();
        }
        #endregion

        #region 添加
        /// <summary>
        /// 单模型添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Insert(T model)
        {
            Collection.Insert<T>(model);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void InsertBatch(List<T> model)
        {
            Collection.InsertBatch<T>(model);

        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Update(T model)
        {
            Collection.Save<T>(model);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="model"></param>
        public void UpdateAll(List<T> model)
        {
            model.ForEach(e => Collection.Save<T>(e));
        }
        #endregion

    }
    ///// <summary>
    ///// 业务类例子
    ///// </summary>
    //public class UserServices : MongodbBase<UserEntity>
    //{
    //    public UserServices()
    //    {
    //        this.Init("myDb");
    //    }

    //}
}