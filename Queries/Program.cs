using System.Linq;
using System;


namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PlutoContext();

            //LINQ SYSNTAX
            //=============


            // RESTRICTION
            var query =
                     from c in context.Courses
                     where c.Level == 1 && c.Author.Id ==1
                     select c;



            //ORDERING
            var queryOrdering =
                    from c in context.Courses
                    where c.Author.Id == 1
                    orderby c.Level descending, c.Name
                    select c;


            //Projection
            var queryProjection =
                   from c in context.Courses
                   where c.Author.Id == 1
                   orderby c.Level descending, c.Name
                   select new { Name = c.Name, Author = c.Author.Name };

            //Grouping 
            var queryGroup =
                from c in context.Courses
                group c by c.Level
                into g
                select g;

            foreach (var group in queryGroup)
            {
                Console.WriteLine(group.Key);
                foreach (var course in group)
                {
                    Console.WriteLine("\t{0}", course.Name);
                }
            }

            foreach (var group in queryGroup)
            {
                    Console.WriteLine("{0} ({1})", group.Key, group.Count());
            }


            // Joining 
            // +++++++++++
            // Innner join 
            // group join 
            // cross join

            var queryInnerJoin =
               from c in context.Courses
               join a in context.Authors on c.AuthorId equals a.Id
               select new { CourseName = c.Name, AuthorName = c.Author.Name };


            var queryGroupJoin =
                from a in context.Authors
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new { AuthorName = a.Name, Courses = g.Count() };

            foreach (var x in queryGroupJoin)
                Console.WriteLine("{0} ({1})", x.AuthorName, x.Courses);


            var queryCrossJoin =
                from a in context.Authors
                from c in context.Courses
                select new { AuthorName = a.Name, CourseName = c.Name };

            foreach (var x in queryCrossJoin)
            {
                Console.WriteLine("{0} - {1}", x.AuthorName, x.CourseName);
            }
        }


    }
}
