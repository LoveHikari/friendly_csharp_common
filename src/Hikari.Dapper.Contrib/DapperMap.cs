﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Dapper;

namespace Hikari.Dapper.Contrib
{
    public class DapperMap
    {
        internal static string ConnectionString { get; set; }
        internal static DbProviderEnum? DbProvider { get; set; }
        public static void Init(Assembly assembly, string connectionString = "", DbProviderEnum? dbProvider = null)
        {
            Init(new[] { assembly });
            ConnectionString = connectionString;
            DbProvider = dbProvider;
        }
        public static void Init(Assembly[] assemblies)
        {
            foreach (var item in assemblies)
            {
                var classes = item.GetTypes().Where(x => x.GetCustomAttribute<TableAttribute>() != null);
                foreach (var cls in classes)
                {
                    SqlMapper.SetTypeMap(cls, new ColumnAttributeTypeMapper(cls));
                }
            }
        }
    }
    /// <summary>
    /// Uses the Name value of the <see cref="System.ComponentModel.DataAnnotations.Schema.ColumnAttribute"/> specified to determine
    /// the association between the name of the column in the query results and the member to
    /// which it will be extracted. If no column mapping is present all members are mapped as
    /// usual.
    /// </summary>
    /// <typeparam name="T">The type of the object that this association between the mapper applies to.</typeparam>
    class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
            {
                new CustomPropertyTypeMap(
                    typeof(T),
                    (type, columnName) =>
                        type.GetProperties().FirstOrDefault(prop =>
                            prop.GetCustomAttributes(false)
                                .OfType<ColumnAttribute>()
                                .Any(attr => attr.Name == columnName)
                        )
                ),
                new DefaultTypeMap(typeof(T))
            })
        {
        }

    }

    class ColumnAttributeTypeMapper : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper(Type t)
            : base(new SqlMapper.ITypeMap[]
            {
                new CustomPropertyTypeMap(
                    t,
                    (type, columnName) =>
                        type.GetProperties().FirstOrDefault(prop =>
                            prop.GetCustomAttributes(false)
                                .OfType<ColumnAttribute>()
                                .Any(attr => attr.Name == columnName)
                        )
                ),
                new DefaultTypeMap(t)
            })
        {
        }

    }

    class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;


        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }


        public ConstructorInfo? FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public ConstructorInfo? FindExplicitConstructor()
        {
            return _mappers
                .Select(mapper => mapper.FindExplicitConstructor())
                .FirstOrDefault(result => result != null);
        }

        public SqlMapper.IMemberMap? GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap? GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }
    }
}