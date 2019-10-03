using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDump;
using Rhetos.Configuration.Autofac;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookStore.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more detailed log.
            var rhetosServerPath = @"C:\Users\User\Bookstore\dist\BookstoreRhetosServer";
            Directory.SetCurrentDirectory(rhetosServerPath);
            // Set commitChanges parameter to COMMIT or ROLLBACK the data changes.
            using (var container = new RhetosTestContainer(commitChanges: true))
            {
                var context = container.Resolve<Common.ExecutionContext>();
                var repository = context.Repository;

                var authors = repository.Bookstore.Person.Load();

                repository.Bookstore.Book.Load(book => book.Author.ID != null)
                        .Select(book => new
                        {
                            Title = book.Title,
                            AuthorName = authors.First(author => author.ID == book.AuthorID).Name
                        }).Dump();


                var query = repository.Bookstore.Book.Query()
                       .Where(book => book.AuthorID != null)
                       .Select(book => new { book.Title, book.Author.Name }).Dump();

                query.ToString().Dump("Generated SQL query");




                // Query data from the `Common.Claim` table:

                //var allBooks = repository.Bookstore.Book.Load(t => t.NumberOfPages > 200).Dump();

                //var someBooks = repository.Bookstore.Book.Query().ToString().Dump();


                //var filter = new Bookstore.CommonMisspelling();

                /*
                var id = new Guid("112EDD6C-901E-4697-BF29-3347B6DE88CC");
                repository.Bookstore.Book.Query(new[] {id}).ToSimple().Dump();

                var newBook = new Bookstore.Book
                {
                    Title = "neka knjiga",
                    NumberOfPages = 500
                };
                repository.Bookstore.Book.Insert(newBook);

                Console.WriteLine("Done.");
                */

                Console.ReadLine();


            }
        }
    }
}
