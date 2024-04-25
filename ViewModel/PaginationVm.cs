using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Exceptions;
using System.Collections;

namespace CodinaxProjectMvc.ViewModel
{
    public class PaginationVm<TEntity> where TEntity : IEnumerable
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public bool HasPrev { get; set; }
        public int Size { get; set; }

        public bool HasNext { get; set; }
        public TEntity? Items { get; set; }
        public PaginationVm(int totalcount, int currentpage, int lastpage, TEntity items, int size)
        {
            if (currentpage < 0) throw new PaginationException();
            TotalCount = totalcount;
            CurrentPage = currentpage;
            LastPage = lastpage;
            Items = items;
            if (currentpage <= lastpage)
            {
                if (currentpage == 1)
                {
                    HasPrev = false;
                }
                else
                {
                    HasPrev = true;
                }
            }
            if (currentpage == lastpage)
            {
                HasNext = false;
            }
            else
            {
                HasNext = true;
            }
            Size = size;
        }

        public PaginationVm()
        {
            TotalCount = 0;
            CurrentPage = 1;
            LastPage = 1;
            Size = 0;
            Items = default;
        }

        public PaginationVm(int total, TEntity paginatedData)
        {
            TotalCount = total;
            Items = paginatedData;
        }

        public PaginationVm(TEntity paginatedData)
        {
            Items = paginatedData;
        }

        public string? BaseUrl { get; set; }
    }
}
