using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Pagination
{
    public class Pagination<T>
    {
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPageCount
        {
            get
            {
                var temp = TotalItemCount / PageSize;
                if (TotalItemCount % PageSize == 0)
                {
                    return temp;
                }
                return temp - 1;
            }
        }
        public int PageIndex { get; set; }
        public bool Previous => PageIndex > 0;
        public ICollection<T> Items { get; set; }
    }
}
