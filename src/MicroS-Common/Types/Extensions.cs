using MicroS_Common.Domain;
using System;

namespace MicroS_Common.Types
{
#pragma warning disable IDE0060
    public static class Extensions
    {
        public static void Validate<TEntity,T>(this TEntity entity, ref T backingField, Func<TEntity,T> transform,Func<T,bool> validate,string code,string reason,IValidateContext context)
            where TEntity : BaseEntity
        {
            try
            {
                T value = transform(entity);
                entity.SetProperty(ref backingField, value, validate, code, reason);
            }catch(ValidationException e)
            {
                context.AddMessage(e.Message);
            }
        }
        public static void Validate<TEntity, T>(this TEntity entity, Func<T> transform, Func<T, bool> validate, string code, string reason)
            where TEntity : BaseEntity
        {
            if (validate(transform()))
            {
                throw new ValidationException(code, reason);
            }
        }

        public static bool SetProperty<TEntity, T>(this TEntity entity, ref T backingField, T newValue, Func<T, bool> check, string code, string reason, Action update = null)
         where TEntity : BaseEntity

        {
            entity.Validate(() => newValue, check, code, reason);
            try
            {
                if (backingField == null && newValue == null)
                {
                    return false;
                }


                backingField = newValue;
                update?.Invoke();
                return true;

            }
            catch (Exception e)
            {
                throw new MicroSException(e, "Unattended Exception!!" + e.Message);
            }
        }

        public static bool SetProperty<TEntity, T>(this TEntity entity, ref T backingField, T newValue, Action update = null)
         where TEntity : BaseEntity

        {
            try
            {
                if (backingField == null && newValue == null)
                {
                    return false;
                }


                backingField = newValue;
                update?.Invoke();
                return true;

            }
            catch (Exception e)
            {
                throw new MicroSException(e, "Unattended Exception!!" + e.Message);
            }
        }
    }
#pragma warning restore IDE0060
}
