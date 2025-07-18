﻿using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hikari.Common.EntityFrameworkCore.Domain
{
    /// <summary>
    /// Unit of Work：维护受业务事务影响的对象列表，并协调变化的写入和并发问题的解决。工作单元记录在业务事务过程中对数据库有影响的所有变化，操作结束后，作为一种结果，工作单元了解所有需要对数据库做的改变，统一对数据库操作。
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private IDbContextTransaction? _dbTransaction;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
            _dbTransaction = await _dbContext.Database.BeginTransactionAsync();
        }
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public void BeginTransaction()
        {
            _dbTransaction = _dbContext.Database.BeginTransaction();
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public async Task<object?> ExecuteScalarAsync(string sql, params object[] parameters)
        {
            return await ExecuteScalarAsync(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public object? ExecuteScalar(string sql, params object[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;
            return await _dbContext.SaveChangesAsync() > 0;

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public bool Add<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Add(entity);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;
            return _dbContext.SaveChanges() > 0;

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> AddListAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;
            return await _dbContext.SaveChangesAsync() > 0;

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        public bool AddList<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dbContext.Set<TEntity>().AddRange(entities);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;
            return _dbContext.SaveChanges() > 0;

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Update(entity);
            // _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Update(entity);
            // _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UpdateListAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        public bool UpdateList<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
            return _dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 清理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CleanAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Entry(entity).State = EntityState.Unchanged;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 清理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Clean<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Entry(entity).State = EntityState.Unchanged;
            return _dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return _dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteListAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        public bool DeleteList<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return _dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CommitAsync()
        {
            if (_dbTransaction == null)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                await _dbTransaction.CommitAsync();
            return true;
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            if (_dbTransaction == null)
                return _dbContext.SaveChanges() > 0;
            else
                _dbTransaction.Commit();
            return true;
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public async Task RollbackAsync()
        {
            await _dbTransaction?.RollbackAsync()!;
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            _dbTransaction?.Rollback();
        }
        #region 私有方法

        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        private async Task<object?> ExecuteScalarAsync(string sql, CommandType cmdType, params object[] parameters)
        {
            await using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandType = cmdType;
            command.CommandText = sql;
            command.Parameters.AddRange(parameters);

            return await command.ExecuteScalarAsync();
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        private object? ExecuteScalar(string sql, CommandType cmdType, params object[] parameters)
        {
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandType = cmdType;
            command.CommandText = sql;
            command.Parameters.AddRange(parameters);

            return command.ExecuteScalar();
        }
        #endregion
    }
}