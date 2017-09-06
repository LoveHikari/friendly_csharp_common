using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace System.MongoDB
{
    /// <summary>
    /// 底层实体基类
    /// </summary>
    public class BaseModel : IMongoEntity
    {
        [BsonIgnore]
        public string Id
        {
            get
            {
                if (_id == ObjectId.Empty)
                    _id = ObjectId.GenerateNewId(DateTime.Now);
                return _id.ToString();
            }
        }
        [BsonId]
        private ObjectId _id;
    }
    ///// <summary>
    ///// 实体类的例子
    ///// </summary>
    //public class UserEntity : BaseModel
    //{
    //    public string UserName { get; set; }

    //    public int Num { get; set; }

    //    //MongoDB中存储的时间是标准时间UTC +0:00  (相差了8个小时)
    //    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    //    public DateTime PostTime { get; set; }

    //}
}