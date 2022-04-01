using System.Linq;
using System;


namespace Queries
{
    class Program
    {


        static void Main(string[] args)
        {

            ExtensionMethod();
        
        }

        public static void ExtensionMethod()
        {
            var context = new PlutoContext();

            /// REStrictions 
            /// 
            var courses = context.Courses.Where(c => c.Level == 1);


            ///Ordering 
            ///
            var coursesOrdering = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level);


            ///Projection
            var tags = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level)
                .SelectMany(c => c.Tags)
                .Distinct();

            foreach (var t in tags)
            {
                Console.WriteLine(t.Name);
            }

            //SET Operators
            var tagsSet = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level)
                .SelectMany(c => c.Tags)
   ;

            foreach (var t in tagsSet)
            {
                Console.WriteLine(t.Name);
            }

            //Grouping 
            var groups = context.Courses.GroupBy(c => c.Level);
            foreach (var group in groups)
            {
                Console.WriteLine("Key : " + group.Key);
                foreach (var course in group)
                {
                    Console.WriteLine(course.Name);
                }
            }

            // Joining
            context.Courses.Join(context.Authors,
                c => c.AuthorId,
                a => a.Id,
                (course, author) => new {
                    CoureName = course.Name,
                    AuthorName = author.Name
                });


            //Group Join 
            context.Authors.GroupJoin(context.Courses, a => a.Id, c => c.AuthorId, (author, course) => new
            {
                Author = author,
                Courses = course
            });


            //Cross Join 
            context.Authors.SelectMany(a => context.Courses, (author, course) => new
            {
                AuthorName = author.Name,
                CourseName = course.Name
            });
        }

        public static void OtherFunctions()
        {
            var context = new PlutoContext();
         
            //Partitioning 
            var courses = context.Courses.Skip(10).Take(10);

            //Element operators
            context.Courses.OrderBy(c => c.Level).FirstOrDefault();
            context.Courses.OrderBy(c => c.Level).LastOrDefault();
            context.Courses.Single(c => c.Id == 1);
          var allAbove100Dollars =  context.Courses.All(c=>c.FullPrice > 10);
            context.Courses.Any(c => c.Level == 1);

            //Aggregating 
            var count = context.Courses.Count();
            var countLevel1 = context.Courses.Where(c=>c.Level==1).Count();
            context.Courses.Max(c => c.FullPrice);
            context.Courses.Min(c => c.FullPrice);
            context.Courses.Average(c => c.FullPrice);
        }

        public void LinqSyntax()
        {

            var context = new PlutoContext();
            //LINQ SYSNTAX
            //=============


            // RESTRICTION
            var query =
                     from c in context.Courses
                     where c.Level == 1 && c.Author.Id == 1
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
