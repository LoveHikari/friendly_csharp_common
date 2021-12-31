using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Hikari.Dapper.Contrib
{
    public class DapperMap
    {
        public static void Init(Assembly assembly)
        {
            Init(new[] { assembly });
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
    /// Uses the Name value of the <see cref="ColumnAttribute"/> specified to determine
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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
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