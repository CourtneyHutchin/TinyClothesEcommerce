using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyClothes.Models
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            Results = new List<Clothing>();
        }

        /// <summary>
        /// The size of the product, small/medium/large
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// The type of clothing, shirt/pants/hat/etc
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The title of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The minimum price for the product
        /// </summary>
        public double? MinPrice { get; set; }

        /// <summary>
        /// The maximum price for the product
        /// </summary>
        public double? MaxPrice { get; set; }

        /// <summary>
        /// The search results
        /// </summary>
        public List<Clothing> Results { get; set; }
    }
}