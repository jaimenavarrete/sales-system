using System;
using System.Collections.Generic;

namespace SalesSystem.Presentation.Models.CustomModels
{
    public class PaginatedList<T>
    {
        public int TotalItems { get; }
        public int ItemsPerPage { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
        public int StartPosition => (CurrentPage - 1) * ItemsPerPage + 1;
        public bool IsLastPage => CurrentPage == TotalPages;

        public List<T> Items { get; set; }

        public List<T> PageItems
        {
            get
            {
                if(Items.Count == 0)
                {
                    return Items;
                }

                var currentPageItemsCount = IsLastPage ? TotalItems - StartPosition + 1 : ItemsPerPage;
                var paginatedItems = Items.GetRange(StartPosition - 1, currentPageItemsCount);

                return paginatedItems;
            }
        }

        public PaginatedList(List<T> items)
        {
            Items = items;
            TotalItems = items.Count;
        }
    }
}
