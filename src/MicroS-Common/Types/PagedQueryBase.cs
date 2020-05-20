
namespace MicroS_Common.Types
{
    public abstract class PagedQueryBase : IPagedQuery
    {
        [DisableSearchFilter]
        public int Page { get; set; } = 1;
        [DisableSearchFilter]
        public int Results { get; set; } = 10;
        [DisableSearchFilter]
        public string OrderBy { get; set; } = string.Empty;
        [DisableSearchFilter]
        public string SortOrder { get; set; } = string.Empty;

        public string Q { get; set; } = string.Empty;

    }
}
