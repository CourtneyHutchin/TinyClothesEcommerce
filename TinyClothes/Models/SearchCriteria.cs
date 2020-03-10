using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(150)]
        public string Title { get; set; }

        /// <summary>
        /// The minimum price for the product
        /// </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Min Price must be a positive number")]
        [Display(Name = "Min Price")]
        public double? MinPrice { get; set; }

        /// <summary>
        /// The maximum price for the product
        /// </summary>
        [Display(Name = "Max Price")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Max Value must be a positive number")]
        public double? MaxPrice { get; set; }

        /// <summary>
        /// The search results
        /// </summary>
        public List<Clothing> Results { get; set; }

        /// <summary>
        /// Returns true if at least one search criteria is provided
        /// </summary>
        /// <returns></returns>
        public bool IsBeingSearched()
        {
            if (MaxPrice.HasValue || MinPrice.HasValue || Title != null || Type != null || Size != null)
            {
                return true;
            }
            // else
            return false;

        }
    }
}