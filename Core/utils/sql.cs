using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

using Core.todolist;

namespace Core.utils
{
    // 定义返回结果类型
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public T? Data { get; set; }

        public Result()
        {
            Error = "";
        }
    }

    // 定义TodoSql类
    public class TodoSql
    {
        private string connectionString;

        public TodoSql(string dbPath)
        {
            connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        // 初始化数据库，创建表
        private void InitializeDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTodoTable = @"
                CREATE TABLE IF NOT EXISTS tbl_todolist (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT,
                    content TEXT,
                    start_date INTEGER,
                    end_date INTEGER,
                    priority INTEGER,
                    types INTEGER,
                    turns INTEGER,
                    status INTEGER
                );";

                string createDateTable = @"
                CREATE TABLE IF NOT EXISTS tbl_date (
                    date INTEGER PRIMARY KEY,
                    todo_id TEXT
                );";

                using (SQLiteCommand command = new SQLiteCommand(createTodoTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand(createDateTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // 查询单个TodoList
        public Result<TodoList> GetTodoById(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM tbl_todolist WHERE [id] = @Id";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TodoList todo = new TodoList
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Title = reader.GetString(reader.GetOrdinal("title")),
                                    Content = reader.GetString(reader.GetOrdinal("content")),
                                    StartDate = new DateTime(reader.GetInt64(reader.GetOrdinal("start_date"))),
                                    EndDate = new DateTime(reader.GetInt64(reader.GetOrdinal("end_date"))),
                                    Priority = reader.GetInt32(reader.GetOrdinal("priority")),
                                    Types = reader.GetInt32(reader.GetOrdinal("types")),
                                    Turns = reader.GetInt32(reader.GetOrdinal("turns")),
                                    Status = reader.GetInt32(reader.GetOrdinal("status"))
                                };
                                return new Result<TodoList> { IsSuccess = true, Data = todo };
                            }
                            else
                            {
                                return new Result<TodoList> { IsSuccess = false, Error = "Todo not found" };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<TodoList> { IsSuccess = false, Error = ex.Message };
            }
        }

        // 批量查询TodoList
        public Result<List<TodoList>> GetTodosByIds(List<int> ids)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM tbl_todolist WHERE [id] IN ({string.Join(",", ids)})";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            List<TodoList> todos = new List<TodoList>();
                            while (reader.Read())
                            {
                                TodoList todo = new TodoList
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Title = reader.GetString(reader.GetOrdinal("title")),
                                    Content = reader.GetString(reader.GetOrdinal("content")),
                                    StartDate = new DateTime(reader.GetInt64(reader.GetOrdinal("start_date"))),
                                    EndDate = new DateTime(reader.GetInt64(reader.GetOrdinal("end_date"))),
                                    Priority = reader.GetInt32(reader.GetOrdinal("priority")),
                                    Types = reader.GetInt32(reader.GetOrdinal("types")),
                                    Turns = reader.GetInt32(reader.GetOrdinal("turns")),
                                    Status = reader.GetInt32(reader.GetOrdinal("status"))
                                };
                                todos.Add(todo);
                            }
                            return new Result<List<TodoList>> { IsSuccess = true, Data = todos };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<List<TodoList>> { IsSuccess = false, Error = ex.Message };
            }
        }

        // 更新TodoList
        public Result<bool> UpdateTodoField(int id, string fieldName, object value)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"UPDATE tbl_todolist SET {fieldName} = @Value WHERE [id] = @Id";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value", value);
                        command.Parameters.AddWithValue("@Id", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new Result<bool> { IsSuccess = true, Data = true };
                        }
                        else
                        {
                            return new Result<bool> { IsSuccess = false, Error = "No rows affected" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<bool> { IsSuccess = false, Error = ex.Message };
            }
        }

        // 根据date查询TodoList
        public Result<string> GetTodosByDate(int date)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT todo_id FROM tbl_date WHERE date = @Date";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Date", date);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            string todoIds = "";
                            while (reader.Read())
                            {
                                todoIds = reader.GetString(reader.GetOrdinal("todo_id"));
                            }
                            return new Result<string> { IsSuccess = true, Data = todoIds };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        // 删除TodoList
        public Result<bool> DeleteTodoById(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM tbl_todolist WHERE [id] = @Id";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new Result<bool> { IsSuccess = true, Data = true };
                        }
                        else
                        {
                            return new Result<bool> { IsSuccess = false, Error = "No rows affected" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<bool> { IsSuccess = false, Error = ex.Message };
            }
        }

        // 插入新的TodoList
        public Result<int> AddTodo(TodoList todo)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO tbl_todolist (title, content, start_date, end_date, priority, types, turns, status)
                    VALUES (@Title, @Content, @StartDate, @EndDate, @Priority, @Types, @Turns, @Status);
                    SELECT last_insert_rowid();";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", todo.Title);
                        command.Parameters.AddWithValue("@Content", todo.Content);
                        command.Parameters.AddWithValue("@StartDate", todo.StartDate.Ticks);
                        command.Parameters.AddWithValue("@EndDate", todo.EndDate.Ticks);
                        command.Parameters.AddWithValue("@Priority", todo.Priority);
                        command.Parameters.AddWithValue("@Types", todo.Types);
                        command.Parameters.AddWithValue("@Turns", todo.Turns);
                        command.Parameters.AddWithValue("@Status", todo.Status);

                        long newId = (long)command.ExecuteScalar();
                        return new Result<int> { IsSuccess = true, Data = (int)newId };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<int> { IsSuccess = false, Error = ex.Message };
            }
        }

        // 插入新的dateTodo
        public Result<int> AddDateTodo(DateTodo dateTodo)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO tbl_date (date, todo_id)
                    VALUES (@Date, @TodoID);
                    SELECT last_insert_rowid();";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        string todoids = string.Join(",", dateTodo.TodoIds);
                        command.Parameters.AddWithValue("@Date", dateTodo.Date);
                        command.Parameters.AddWithValue("@TodoID", todoids);

                        long newId = (long)command.ExecuteScalar();
                        return new Result<int> { IsSuccess = true, Data = (int)newId };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<int> { IsSuccess = false, Error = ex.Message };
            }
        }
    }
}
