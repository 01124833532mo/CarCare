namespace CareCare.Core.Application.Abstraction.Common
{
    public class SpecParams
    {

        public string? Sort { get; set; }
        public string? UserId { get; set; }

        private string? search;
        public string? Search { get { return search; } set { search = value?.ToUpper(); } }

        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 10;
        private int pageSize = 5;
        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }

    }
}
