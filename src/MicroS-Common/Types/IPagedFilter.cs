using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Types
{
    public interface IPagedFilter<TResult, in TQuery> where TQuery : IQuery
    {
        PagedResult<TResult> Filter(IEnumerable<TResult> values, TQuery query);
    }
}
