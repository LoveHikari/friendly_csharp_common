#if !NETSTANDARD2_0
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

/******************************************************************************************************************
 * 
 * 
 * 标  题： Dynamic 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/04/28
 * 修  改：
 * 参  考： http://blog.zhaojie.me/2010/05/asp-net-mvc-dynamic-view-model-binding-error-with-anonymous-types.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// Dynamic 扩展类
    /// </summary>
    public static class DynamicFactory
    {
        private static ConcurrentDictionary<Type, Type> s_dynamicTypes = new ConcurrentDictionary<Type, Type>();

        private static Func<Type, Type> s_dynamicTypeCreator = new Func<Type, Type>(CreateDynamicType);
        /// <summary>
        /// 从任意对象扩展出一个动态类型的对象，这个动态类型会根据输入对象中的属性信息，生成对应的公有字段
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static object ToDynamic(this object entity)
        {
            var entityType = entity.GetType();
            var dynamicType = s_dynamicTypes.GetOrAdd(entityType, s_dynamicTypeCreator);

            var dynamicObject = Activator.CreateInstance(dynamicType);
            foreach (var entityProperty in entityType.GetProperties())
            {
                var value = entityProperty.GetValue(entity, null);
                dynamicType.GetField(entityProperty.Name).SetValue(dynamicObject, value);
            }

            return dynamicObject;
        }
        /// <summary>
        /// 创建动态类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>类型</returns>
        private static Type CreateDynamicType(Type entityType)
        {
            var asmName = new AssemblyName("DynamicAssembly_" + Guid.NewGuid());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule("DynamicModule_" + Guid.NewGuid());

            var typeBuilder = moduleBuilder.DefineType(
                entityType + "$DynamicType",
                TypeAttributes.Public);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            foreach (var entityProperty in entityType.GetProperties())
            {
                typeBuilder.DefineField(entityProperty.Name, entityProperty.PropertyType, FieldAttributes.Public);
            }

            return typeBuilder.CreateType();
        }
    }
}
#endif



