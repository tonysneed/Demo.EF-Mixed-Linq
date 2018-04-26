using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MixedLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to retrieve categories");
            Console.ReadLine();

            List<Category> categories;

            using (var context = new NorthwindSlim())
            {
                context.Database.Log = Console.WriteLine;
                categories = context.Categories
                    .Include(c => c.Products)
                    .Where(c => c.CategoryId == 2
                                || c.CategoryId == 3
                                || c.CategoryId == 7)
                    .ToList();
            }

            Console.WriteLine("\nCategories:");
            categories.ForEach(c => Console.WriteLine($"{c.CategoryId} {c.CategoryName}"));

            Console.WriteLine("\nPress Enter to join products to these categories");
            Console.ReadLine();

            List<Product> products;

            using (var context = new NorthwindSlim())
            {
                try
                {
                    context.Database.Log = Console.WriteLine;

                    // DO NOT DO THIS:
                    // Results in InvalidOperationException:
                    //products = (from p in context.Products
                    //            join c in categories on p.CategoryId equals c.CategoryId
                    //            select p).ToList();

                    // DO NOT DO THIS:
                    // This retrieves ALL the products without remote filtering and performs the join locally
                    products = (from c in categories
                        from p in c.Products
                        join p2 in context.Products on c.CategoryId equals p2.CategoryId
                        select p).Distinct().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine("\nProducts joined to categories:");
            products.ForEach(p => Console.WriteLine($"{p.ProductId} {p.ProductName} Category Id: {p.CategoryId}"));

            var ids = categories.Select(c => (int?)c.CategoryId).ToArray();
            using (var context = new NorthwindSlim())
            {
                try
                {
                    context.Database.Log = Console.WriteLine;

                    // DO THIS:
                    // This gets all the products and performs filter remotely
                    products = context.Products
                        .Where(p => ids.Contains(p.CategoryId)).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine("\nProducts filtered by categories:");
            products.ForEach(p => Console.WriteLine($"{p.ProductId} {p.ProductName} Category Id: {p.CategoryId}"));
        }
    }
}
