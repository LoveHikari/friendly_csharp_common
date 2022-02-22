using Elasticsearch.Net;
using Nest;

namespace Hikari.Common.ElasticSearch;

public class ElasticSearchHelper
{
    private readonly ElasticClient _elasticClient;
    private readonly string _index;
    public ElasticSearchHelper(string connectionString, string index) : this(new string[] { connectionString }, index)
    {
    }
    public ElasticSearchHelper(string[] connectionString, string index)
    {
        _elasticClient = GetClient(connectionString);
        _index = index;
    }
    private ElasticClient GetClient(string[] connectionString)
    {
        var nodes = new List<Uri>();
        foreach (string s in connectionString)
        {
            nodes.Add(new Uri(s));
        }

        //var nodes = new Uri[] { new Uri(_connectionString) };
        var pool = new StaticConnectionPool(nodes);
        var connectionSettings = new ConnectionSettings(pool);
        return new ElasticClient(connectionSettings);
    }
    /// <summary>
    /// 获得数据列表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    public async Task<List<T>?> GetListAsync<T>(IDictionary<string, string>? query = null) where T : class
    {
        List<QueryContainerDescriptor<T>> queryList = new List<QueryContainerDescriptor<T>>();
        if (query != null)
        {
            foreach (KeyValuePair<string, string> pair in query)
            {
                QueryContainerDescriptor<T> queryName = new QueryContainerDescriptor<T>();
                queryName.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
                queryList.Add(queryName);
            }
        }
        
        var searchRes = await _elasticClient.SearchAsync<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Index(_index));
        if (searchRes.Total > 0)
        {
            var models = searchRes.Documents.ToList();
            return models;
        }

        return null;
    }
    /// <summary>
    /// 获得数据列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public List<T>? GetList<T>(IDictionary<string, string>? query = null) where T : class
    {
        List<QueryContainerDescriptor<T>> queryList = new List<QueryContainerDescriptor<T>>();
        if (query != null)
        {
            foreach (KeyValuePair<string, string> pair in query)
            {
                QueryContainerDescriptor<T> queryName = new QueryContainerDescriptor<T>();
                queryName.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
                queryList.Add(queryName);
            }
        }
        
        var searchRes = _elasticClient.Search<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Index(_index));
        if (searchRes.Total > 0)
        {
            var models = searchRes.Documents.ToList();
            return models;
        }

        return null;
    }
    /// <summary>
    /// 获得数据列表
    /// </summary>
    /// <param name="query"></param>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">每页数量</param>
    /// <returns></returns>
    public async Task<Pager<T>> GetListAsync<T>(IDictionary<string, string>? query, int pageIndex, int pageSize) where T : class
    {
        int size = pageSize;  // 显示应该返回的结果数量
        int from = (pageIndex - 1) * pageSize;  // 显示应该跳过的初始结果数量

        Pager<T> pager = new Pager<T>();

        List<QueryContainerDescriptor<T>> queryList = new List<QueryContainerDescriptor<T>>();
        if (query != null)
        {
            foreach (KeyValuePair<string, string> pair in query)
            {
                QueryContainerDescriptor<T> queryName = new QueryContainerDescriptor<T>();
                queryName.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
                queryList.Add(queryName);
            }
        }


        var searchRes = await _elasticClient.SearchAsync<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Size(size).From(from).Index(_index));
        if (searchRes.Total > 0)
        {
            var models = searchRes.Documents.ToList();
            pager.Content = models;
            pager.PageCount = Math.Ceiling(searchRes.Total.ToDouble() / pageSize).ToInt32();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            pager.PrevPage = pageIndex > 0 ? pageIndex - 1 : 0;
            pager.NextPage = pageIndex < pager.PageCount ? pageIndex + 1 : 0;
            pager.TotalRecord = searchRes.Total.ToInt32();
        }

        return pager;
    }
    /// <summary>
    /// 获得数据列表
    /// </summary>
    /// <param name="query"></param>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">每页数量</param>
    /// <returns></returns>
    public Pager<T> GetList<T>(IDictionary<string, string>? query, int pageIndex, int pageSize) where T : class
    {
        int size = pageSize;  // 显示应该返回的结果数量
        int from = (pageIndex - 1) * pageSize;  // 显示应该跳过的初始结果数量

        Pager<T> pager = new Pager<T>();

        List<QueryContainerDescriptor<T>> queryList = new List<QueryContainerDescriptor<T>>();
        if (query != null)
        {
            foreach (KeyValuePair<string, string> pair in query)
            {
                QueryContainerDescriptor<T> queryName = new QueryContainerDescriptor<T>();
                queryName.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
                queryList.Add(queryName);
            }
        }


        var searchRes = _elasticClient.Search<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Size(size).From(from).Index(_index));
        if (searchRes.Total > 0)
        {
            var models = searchRes.Documents.ToList();
            pager.Content = models;
            pager.PageCount = Math.Ceiling(searchRes.Total.ToDouble() / pageSize).ToInt32();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            pager.PrevPage = pageIndex > 0 ? pageIndex - 1 : 0;
            pager.NextPage = pageIndex < pager.PageCount ? pageIndex + 1 : 0;
            pager.TotalRecord = searchRes.Total.ToInt32();
        }

        return pager;
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>是否成功</returns>
    public async Task<bool> UpdateAsync<T>(T entity) where T : class
    {

        var list = new List<T> { entity };
        var bulkRequest = new BulkRequest(_index) { Operations = new List<IBulkOperation>() };
        var id = entity.GetValue("Id") as Nest.Id;
        var idxops = list.Select(o => new BulkIndexOperation<T>(o) { Id = id }).Cast<IBulkOperation>().ToList();
        bulkRequest.Operations = idxops;
        var response = await _elasticClient.BulkAsync(bulkRequest);
        return response.IsValid;
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>是否成功</returns>
    public bool Update<T>(T entity) where T : class
    {
        var list = new List<T> { entity };
        var bulkRequest = new BulkRequest(_index) { Operations = new List<IBulkOperation>() };
        var id = entity.GetValue("Id") as Nest.Id;
        var idxops = list.Select(o => new BulkIndexOperation<T>(o) { Id = id }).Cast<IBulkOperation>().ToList();
        bulkRequest.Operations = idxops;
        var response = _elasticClient.Bulk(bulkRequest);
        return response.IsValid;
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>是否成功</returns>
    public async Task<bool> AddAsync<T>(T entity) where T : class
    {
        var list = new List<T> { entity };
        var bulkRequest = new BulkRequest(_index) { Operations = new List<IBulkOperation>() };
        var id = entity.GetValue("Id") as Nest.Id;
        var idxops = list.Select(o => new BulkIndexOperation<T>(o) { Id = id }).Cast<IBulkOperation>().ToList();
        bulkRequest.Operations = idxops;
        var response = await _elasticClient.BulkAsync(bulkRequest);
        return response.IsValid;

    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>是否成功</returns>
    public bool Add<T>(T entity) where T : class
    {
        var list = new List<T> { entity };
        var bulkRequest = new BulkRequest(_index) { Operations = new List<IBulkOperation>() };
        var id = entity.GetValue("Id") as Nest.Id;
        var idxops = list.Select(o => new BulkIndexOperation<T>(o) { Id = id }).Cast<IBulkOperation>().ToList();
        bulkRequest.Operations = idxops;
        var response = _elasticClient.Bulk(bulkRequest);
        return response.IsValid;

    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns>是否成功</returns>
    public async Task<bool> DeleteAsync<T>(int id) where T : class
    {
        DocumentPath<T> deletePath = new DocumentPath<T>(id);
        deletePath.Index(_index);

        var response = await _elasticClient.DeleteAsync(deletePath);
        return response.IsValid;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns>是否成功</returns>
    public bool Delete<T>(int id) where T : class
    {
        DocumentPath<T> deletePath = new DocumentPath<T>(id);
        deletePath.Index(_index);

        var response = _elasticClient.Delete(deletePath);
        return response.IsValid;
    }
    /// <summary>
    /// 获得数据数量
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<long> CountAsync<T>(IDictionary<string, string> query) where T : class
    {
        List<QueryContainerDescriptor<T>> queryList = new List<QueryContainerDescriptor<T>>();
        foreach (KeyValuePair<string, string> pair in query)
        {
            QueryContainerDescriptor<T> queryName = new QueryContainerDescriptor<T>();
            queryName.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
            queryList.Add(queryName);
        }


        var searchRes = await _elasticClient.CountAsync<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Index(_index));

        return searchRes.Count;
    }
    /// <summary>
    /// 获得数据数量
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public long Count<T>(IDictionary<string, string> query) where T : class
    {
        List<QueryContainerDescriptor<T>> queryList = new List<QueryContainerDescriptor<T>>();
        foreach (KeyValuePair<string, string> pair in query)
        {
            QueryContainerDescriptor<T> queryName = new QueryContainerDescriptor<T>();
            queryName.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
            queryList.Add(queryName);
        }


        var searchRes = _elasticClient.Count<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Index(_index));

        return searchRes.Count;
    }
}