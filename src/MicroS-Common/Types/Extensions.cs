using System;

namespace MicroS_Common.Types
{
#pragma warning disable IDE0060
    public static class Extensions
    {
        public static void Validate<TEntity, T>(this TEntity entity, Func<T> p, Func<T, bool> check, string code, string reason)
            where TEntity : BaseEntity
        {
            if (check(p()))
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

            }catch(Exception e)
            {
                throw new MicroSException(e, "Unattended Exception!!" + e.Message);
            }
        }
    }
#pragma warning restore IDE0060
}
