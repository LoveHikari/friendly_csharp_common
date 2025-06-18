using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;

namespace Hikari.Common.ElasticSearch;

public class ElasticSearchHelper(string cloudId, string apiKey, string index)
{
    private readonly ElasticsearchClient _elasticClient = new(cloudId, new ApiKey(apiKey));

    /// <summary>
    /// 获得数据列表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    public async Task<List<T>?> GetListAsync<T>(IDictionary<string, string>? query = null) where T : class
    {
        List<Action<QueryDescriptor<T>>> queryList = new List<Action<QueryDescriptor<T>>>();
        if (query != null)
        {
            foreach (KeyValuePair<string, string> pair in query)
            {
                Action<QueryDescriptor<T>> queryName = new Action<QueryDescriptor<T>>(descriptor =>
                {
                    descriptor.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
                });
                //queryName.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
                queryList.Add(queryName);
            }
        }

        var searchRes = await _elasticClient.SearchAsync<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Indices(index));
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

        List<Action<QueryDescriptor<T>>> queryList = new List<Action<QueryDescriptor<T>>>();
        if (query != null)
        {
            foreach (KeyValuePair<string, string> pair in query)
            {
                Action<QueryDescriptor<T>> queryName = new Action<QueryDescriptor<T>>(descriptor =>
                {
                    descriptor.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
                });
                queryList.Add(queryName);
            }
        }


        var searchRes = await _elasticClient.SearchAsync<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ).Size(size).From(from).Indices(index));
        if (searchRes.Total > 0)
        {
            var models = searchRes.Documents.ToList();
            pager.Content = models;
            pager.PageCount = Math.Ceiling(searchRes.Total.ToDouble(0) / pageSize).ToInt32();
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
        var bulkRequest = new BulkRequest(index) { Operations = new List<IBulkOperation>() };
        var id = entity.GetValue("Id") as Id;
        var idxops = list.Select(o => new BulkIndexOperation<T>(o) { Id = id }).Cast<IBulkOperation>().ToList();
        bulkRequest.Operations = idxops;
        var response = await _elasticClient.BulkAsync(bulkRequest);
        return response.IsValidResponse;
    }
    
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>是否成功</returns>
    public async Task<bool> AddAsync<T>(T entity) where T : class
    {
        var list = new List<T> { entity };
        var bulkRequest = new BulkRequest(index) { Operations = new List<IBulkOperation>() };
        var id = entity.GetValue("Id") as Id;
        var idxops = list.Select(o => new BulkIndexOperation<T>(o) { Id = id }).Cast<IBulkOperation>().ToList();
        bulkRequest.Operations = idxops;
        var response = await _elasticClient.BulkAsync(bulkRequest);
        return response.IsValidResponse;

    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns>是否成功</returns>
    public async Task<bool> DeleteAsync<T>(Id id) where T : class
    {
        var response = await _elasticClient.DeleteAsync(index, id);
        return response.IsValidResponse;
    }
    /// <summary>
    /// 获得数据数量
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<long> CountAsync<T>(IDictionary<string, string> query) where T : class
    {
        List<Action<QueryDescriptor<T>>> queryList = new List<Action<QueryDescriptor<T>>>();
        foreach (KeyValuePair<string, string> pair in query)
        {
            Action<QueryDescriptor<T>> queryName = new Action<QueryDescriptor<T>>(descriptor =>
            {
                descriptor.Match(t => t.Field(new Field(pair.Key)).Query(pair.Value));
            });
            queryList.Add(queryName);
        }


        var searchRes = await _elasticClient.CountAsync<T>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Must(queryList.ToArray()))
            ));

        return searchRes.Count;
    }
}