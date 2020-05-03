using System;

namespace MicroS_Common.Dto
{
    public interface IDto
    {
    }

    public class BaseDto : IDto, IDisposable
    {

        public void Dispose()
        {

        }
    }
}
