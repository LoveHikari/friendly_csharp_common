namespace Hikari.Common.IdGenerator.Contract;

    public interface IIdGenerator
    {
        /// <summary>
        /// 生成新的long型Id
        /// </summary>
        /// <returns></returns>
        long NewLong();
    }
