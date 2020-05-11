using System;

namespace MicroS_Common.Types
{
    public abstract class BaseEntityDto : IIdentifiable
    {
        [Unique]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public abstract class BaseEntity : IIdentifiable
    {
        [Unique]
        public Guid Id { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public DateTime UpdatedDate { get;  set; }

        public BaseEntity() : this(Guid.NewGuid())
        {

        }
        public BaseEntity(Guid id)
        {
            Id = id;
            CreatedDate = DateTime.UtcNow;
            SetUpdatedDate();
        }

        protected virtual void SetUpdatedDate(bool update = false)
            => UpdatedDate = update ? DateTime.UtcNow : UpdatedDate;

        
    }
}
