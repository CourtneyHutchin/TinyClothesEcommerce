using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyClothes.Models;

namespace TinyClothes.Data
{
    /// <summary>
    /// Contains DB helper methods for Clothing
    /// for <see cref="Models.Clothing" />
    /// </summary>
    public static class ClothingDb
    {
        public static List<Clothing> GetAllClothing()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Adds a clothing object to the database.
        /// Returns the object with the Id populated
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public static async Task<Clothing> Add(Clothing c, StoreContext context)
        {
            await context.AddAsync(c);         // prepares INSERT query
            await context.SaveChangesAsync();  // execute INSERT query

            return c;
        }
    }
}