using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MicroS_Common.Types
{
    public interface IEntity
    {

    }
    public abstract class Entity<TKey> : IIdentifiable<TKey>, ICreatable, IUpdatable,IEntity
    {
        public Entity()
        {

        }
        public Entity(TKey id)
        {
            Id = id;
        }
        [Unique,BsonId]
        public virtual TKey Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public abstract class EntityDto<TKey> : IIdentifiable<TKey>
    {
        public EntityDto()
        {

        }
        public EntityDto(TKey id)
        {
            Id = id;
        }
        public virtual TKey Id { get; set; }
       /* public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }*/
    }

    public abstract class BaseEntityDto : EntityDto<Guid>{}
    public abstract class BaseEntity :Entity<Guid>
    {
        
        public BaseEntity() : base(Guid.NewGuid())
        {

        }
    
    }
}
