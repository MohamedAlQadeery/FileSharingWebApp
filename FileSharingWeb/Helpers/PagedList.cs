using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FileSharingWeb.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(int pageIndex, int pageSize, int totalCount, IEnumerable<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCounts = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCounts / (double)pageSize);
            AddRange(items);
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public int TotalCounts { get; set; }



        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {

            var totalCount = await source.CountAsync();
            if (pageIndex < 1 || pageIndex > (int)Math.Ceiling(totalCount / (double)pageSize)) pageIndex = 1;
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(pageIndex, pageSize, totalCount, items);
        }


    }
}