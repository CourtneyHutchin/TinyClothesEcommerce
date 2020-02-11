using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Returns the total number of Clothing items
        /// </summary>
        /// <returns></returns>
        public async static Task<int> GetNumClothing(StoreContext context)
        {
            return await context.Clothing.CountAsync();

            // Alternative with query syntax
            //return await (from c in context.Clothing
            //        select c).CountAsync()
        }

        /// <summary>
        /// Returns a specific page of clothing items
        /// sorted by ItemId in ascending order
        /// </summary>
        /// <param name="pageNum">The page</param>
        /// <param name="pageSize">The number of clothing items per page</param>
        public async static Task<List<Clothing>> GetClothingByPage(StoreContext context, int pageNum, int pageSize)
        {
            // If you wanted page 1, we wouldn't skip
            // any rows, so we must offset by 1
            const int pageOffset = 1;
            // LINQ Method Syntax
            List<Clothing> clothes = await context.Clothing
                                                .OrderBy(c => c.ItemId) // Order by the ItemId
                                                .Skip(pageSize * (pageNum - pageOffset)) // Must do Skip then Take
                                                .Take(pageSize)
                                                .ToListAsync();

            return clothes;

            // LINQ Query Syntax - Same as above - Keeping for notes
            //List<Clothing> clothes2 =
            //    await (from c in context.Clothing
            //           orderby c.ItemId ascending
            //           select c)
            //           .Skip(pageSize * (pageNum - pageOffset)) // Must do Skip then Take
            //           .Take(pageSize)
            //           .ToListAsync();
        }


        /// <summary>
        /// Returns a single clothing item or 
        /// null if there is no match
        /// </summary>
        /// <param name="id">The id of the item</param>
        /// <param name="context">DB Context</param>
        public static async Task<Clothing> GetClothingById(int id, StoreContext context)
        {
            Clothing c = await (from clothing in context.Clothing
                                where clothing.ItemId == id
                                select clothing).SingleOrDefaultAsync();
            return c; 
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
        public static async Task<Clothing> Edit(Clothing c, StoreContext context)
        {
            await context.AddAsync(c);
            context.Entry(c).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return c;
        }

        public async static Task Delete(int id, StoreContext context)
        {
            Clothing c = await GetClothingById(id, context);

            // If the product was found, delete it
            if(c != null)
            {
                await context.AddAsync(c);
                context.Entry(c).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}