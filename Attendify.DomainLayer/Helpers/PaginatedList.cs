using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool MatchesFound { get; set; } // New property to indicate if matches were found

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize, bool matchesFound)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            MatchesFound = matchesFound; // Initialize the MatchesFound property

            AddRange(items);
        }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// For entity framework collections
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            bool matchesFound = count > 0; // If count is greater than 0, matches were found

            return new PaginatedList<T>(items, count, pageNumber, pageSize, matchesFound);
        }

        /// <summary>
        /// for my dtos or other options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        public static Task<PaginatedList<T>> CreateAsync<T>(IEnumerable<T> source, int pageNumber, int pageSize)
        {

            var count = source.Count();


            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            bool matchesFound = count > 0;


            return Task.FromResult(new PaginatedList<T>(items, count, pageNumber, pageSize, matchesFound));
        }
    }
}
