using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class ReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Reviews records in the database
        public async Task<List<Review>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spReview_GetAll\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Review>();
                    await sql.OpenAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Function returns the Review with the specified by a user_id and review_id from the database
        public async Task<Review> GetById(int user_id, int review_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spReview_GetById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("review_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = user_id;
                    cmd.Parameters[1].Value = review_id;
                    Review response = null;
                    await sql.OpenAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader);
                        }
                    }

                    return response;
                }
            }
        }

        // Function inserts a Review record in the database
        public async Task Insert(Review review)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spReview_InsertValue\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("review_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("rating", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = review.User_ID;
                    cmd.Parameters[1].Value = review.Review_ID;
                    cmd.Parameters[2].Value = review.Description;
                    cmd.Parameters[3].Value = review.Rating;
                    if (review.Dish_ID == null) cmd.Parameters[4].Value = DBNull.Value;
                    else cmd.Parameters[4].Value = review.Dish_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies a Review record in the database
        public async Task ModifyById(Review review)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spReview_ModifyById\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("review_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("rating", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = review.User_ID;
                    cmd.Parameters[1].Value = review.Review_ID;
                    cmd.Parameters[2].Value = review.Description;
                    cmd.Parameters[3].Value = review.Rating;
                    if (review.Dish_ID == null) cmd.Parameters[4].Value = DBNull.Value;
                    else cmd.Parameters[4].Value = review.Dish_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a Review record in the database
        public async Task DeleteById(int user_id, int review_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spReview_DeleteById\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("review_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = user_id;
                    cmd.Parameters[1].Value = review_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function returns the next review id (in order to keep Review as a weak entity)
        public async Task<int> getNextReviewID(int user_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spReview_getNextReviewID\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = user_id;
                    cmd.Parameters.Add(new NpgsqlParameter("next_review_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    if (Convert.IsDBNull(cmd.Parameters[1].Value)) return 1;
                    else return Convert.ToInt32(cmd.Parameters[1].Value);
                }
            }
        }

        // Mapper used to map between the reader object and our Review model
        private Review MapToValue(NpgsqlDataReader reader)
        {
            int? t = null;
            if (!Convert.IsDBNull(reader["Dish_ID"]))
            {
                t = (int?)reader["Dish_ID"];
            }

            return new Review()
            {
                User_ID = (int)reader["User_ID"],
                Review_ID = (int)reader["Review_ID"],
                Description = reader["Description"].ToString(),
                Rating = (int)reader["Rating"],
                Dish_ID = t
            };
        }
    }
}
