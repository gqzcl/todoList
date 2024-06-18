using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.SqlTypes;
using Core.utils;
namespace Core.todolist
{
    public class DateTodo
    {
        public int Date { get; set; }
        public List<int> TodoIds { get; set; }
        public List<TodoList> ToDos { get; set; }

        public TodoSql todoSql;

        // 初始化数据库连接字符串
        private readonly string connectionString = "Data Source=yourdatabase.db;Version=3;";

        public DateTodo()
        {
            TodoIds = [];
            ToDos = [];
        }
        // 构造函数
        public DateTodo(int date, TodoSql sql)
        {
            Date = date;
            TodoIds = [];
            ToDos = [];
            todoSql = sql;
        }

        // 根据Date，从tbl_date中拿到todo_id，将字符串todo_id转换为整数数组TodoIds
        public string FetchTodoIdsByDate()
        {
            try
            {
                Result<string> res = todoSql.GetTodosByDate(Date);
                if (res.IsSuccess)
                {
                    string[] numbersArray = res.Data.Split(',');
                    TodoIds = numbersArray.Select(int.Parse).ToList();
                }
                else
                {
                    return "No data found for the specified date.";
                }
                return null;
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        // 根据TodoIds，调用TodoSql类的方法获取到List<TodoList> ToDos
        public string FetchTodosByIds()
        {
            if (TodoIds == null || TodoIds.Count == 0)
            {
                return "TodoIds array is null or empty.";
            }

            try
            {
                Result<List<TodoList>> result = todoSql.GetTodosByIds(TodoIds);
                if (result.IsSuccess)
                {
                    ToDos = result.Data;
                    return null; // No error
                }
                else
                {
                    return result.Error;
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        public string CreateDateTodo(DateTodo dateTodo)
        {
            try
            {
                Result<int> res = todoSql.AddDateTodo(dateTodo);
                return res.Error;
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
    }

}