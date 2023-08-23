using MoviesApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace MoviesApi.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=MotionPictures;Integrated Security=True;";
        public Movie GetMovieById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    Movie tempMovie = null;
                    // Step 2 Open the connection
                    connection.Open();
                    // Step 3 make the sql string
                    string sql = "select * FROM MotionPictures WHERE ID = @movieid;";
                    // Step 4 make a sql command object
                    SqlCommand sqlCommand = new SqlCommand(sql, connection);
                    // Step 5 add the parameters
                    sqlCommand.Parameters.AddWithValue("@movieid", id);
                    // Step 6 Get the data
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        tempMovie = new Movie();
                        tempMovie.ID = Convert.ToInt32(reader["ID"]);
                        tempMovie.Name = Convert.ToString(reader["Name"]);
                        tempMovie.Description = Convert.ToString(reader["Description"]);
                        tempMovie.ReleaseYear = Convert.ToInt32(reader["Release Year"]);
                        tempMovie.DirectorId = reader["DirectorId"] is DBNull ? (int?)null : (int)reader["DirectorId"];
                    }

                    return tempMovie;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Movie> GetMovies()
        {
            string queryString = "SELECT * from MotionPictures;";
            List<Movie> results = new List<Movie>();

            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new Movie()
                        {
                            ID = (int)reader["ID"],
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            ReleaseYear = (int)reader["Release Year"],
                            AcademyAward = (bool)reader["AcademyAward"],
                            DirectorId = reader["DirectorId"] is DBNull ? (int?)null : (int)reader["DirectorId"]
                        });
                    }
                }
            }

            return results;
        }

        public bool InsertMovie(Movie movie)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string queryString = "INSERT INTO MotionPictures (Name, Description, [Release Year], AcademyAward, DirectorId) VALUES (@Name, @Description, @ReleaseYear, @AcademyAward, @DirectorId)";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@Name", movie.Name);
                        command.Parameters.AddWithValue("@Description", movie.Description);
                        command.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                        command.Parameters.AddWithValue("@AcademyAward", movie.AcademyAward);
                        command.Parameters.AddWithValue("@DirectorId", movie.DirectorId ?? (object)DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inserting movie: " + ex.Message);
                    return false;
                }
            }
        }

        public bool MovieExists(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT * FROM MotionPictures WHERE ID = @Id;";
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.Read(); 
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while checking movie existence.", ex);
                }
            }
        }
 
        public Movie UpdateMovie(Movie movie)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE MotionPictures SET Name = @Name, Description = @Description, [Release Year] = @ReleaseYear, AcademyAward = @AcademyAward, DirectorId = @DirectorId WHERE ID = @ID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", movie.ID);
                        command.Parameters.AddWithValue("@Name", movie.Name);
                        command.Parameters.AddWithValue("@Description", movie.Description);
                        command.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                        command.Parameters.AddWithValue("@AcademyAward", movie.AcademyAward);
                        command.Parameters.AddWithValue("@DirectorId", movie.DirectorId ?? (object)DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return movie;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteMovie(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM MotionPictures WHERE ID = @movieid;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@movieid", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
