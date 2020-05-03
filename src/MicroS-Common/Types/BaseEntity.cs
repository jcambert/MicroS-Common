using System;

namespace MicroS_Common.Types
{
    public abstract class BaseEntity : IIdentifiable
    {
        [Unique]
        public Guid Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime UpdatedDate { get; protected set; }

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

        /*public static void SetProperty< T>( ref T backingField, T newValue, Expression<Func<T>> propertyExpression)
         

        {
            if (backingField == null && newValue == null)
            {
                return;
            }

            if (backingField == null || !backingField.Equals(newValue))
            {
                backingField = newValue;
            }
        }*/
    }
}
