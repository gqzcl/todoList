using System;
using System.Collections.Generic;

namespace Core.todolist
{
    public class TodoList
    {
        // Properties
        public string Title { get; set; }
        public string Content { get; set; }
        public List<TodoList>? ChildTodo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; } // 优先级：高中低
        public int Types { get; set; } // 类型
        public int Turns { get; set; } // 顺序
        public int Id { get; set; } // id
        public int Status { get; set; } // 状态

        // Constructor
        public TodoList()
        {
            Title = "";
            Content = "";
        }

        public TodoList(string title, string content, DateTime startDate, DateTime endDate, int priority, int types, int turns, int id, int status)
        {
            Title = title;
            Content = content;
            ChildTodo = null;
            StartDate = startDate;
            EndDate = endDate;
            Priority = priority;
            Types = types;
            Turns = turns;
            Id = id;
            Status = status;
        }

        // Static method
        public static int GetAllTodoList()
        {
            // Placeholder implementation
            return 0;
        }
    }
}

