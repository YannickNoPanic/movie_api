using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Movie_API
{
    public static class DataAccess
    {

        public static List<Movie> GetMovie(int movieId)
        {            
            return GetMovies().Where(m => m.MovieId == movieId) //first get the right movies
                .OrderByDescending(m => m.Id) //get the order descending so we have the highest id
                .GroupBy(m => m.Language) //group by language to return each language
                .Select(m => m.FirstOrDefault())
                .ToList();
        }        

        public static void SaveMovie(Movie movie)
        {
            movie.Id = GetLastMovieId() + 1;
            //File.AppendText
            using (var writer = File.AppendText(@"Data/metadata.csv"))
            {
                writer.WriteLine(movie.ToString());
                writer.Close();
            }
        }

        private static int GetLastMovieId()
        {
            //get the last line, and get the int
            return Int32.Parse(File.ReadLines(@"Data/metadata.csv").Last().Split(',')[0]);
        }

        private static List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            //movies is empty, fill er up

            using (var reader = new StreamReader(@"Data/metadata.csv"))
            {
                //get the first line of columns out of the way;
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    List<string> values = new List<string>();

                    var quoteValues = line.Split("\"");//split by quote
                    for (int i = 0; i < quoteValues.Length; i++)
                    {
                        //split the normal values
                        if (i % 2 == 0)
                        {
                            //first remove commas leading or trailing
                            string s = quoteValues[i].TrimEnd(',').TrimStart(',');
                            values.AddRange(s.Split(',').ToList());
                        }
                        else //add the quoted value
                        {
                            values.Add(quoteValues[i]);
                        }
                    }

                    movies.Add(new Movie()
                    {
                        Id = Int32.Parse(values[0]),
                        MovieId = Int32.Parse(values[1]),
                        Title = values[2],
                        Language = values[3],
                        Duration = values[4],
                        ReleaseYear = Int32.Parse(values[5]),
                    });
                }
            }
            return movies;
        }        
    }
}
