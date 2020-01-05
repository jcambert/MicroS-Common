using System;
using System.Collections.Generic;
using System.Text;

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
