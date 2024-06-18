using Core.todolist;
using Core.utils;

namespace Core
{
    public class Program
    {
        public static void Main()
        {
            string dataPath = "./test.sql";
            // 初始化TodoSql实例
            TodoSql todoSql = new TodoSql(dataPath);

            // 创建DateTodo实例
            DateTodo dateTodo = new DateTodo(20240101, todoSql); // 示例日期

            TodoList todo1 = new TodoList("test1", "test1", DateTime.Now, DateTime.Now, 1, 1, 1, 1, 1);
            // 插入一个新的todo
            Result<int> res = todoSql.AddTodo(todo1);
            if (!res.IsSuccess)
            {
                Console.WriteLine("add Todos to sql err : " + res.Error);
                return;
            }
            dateTodo.TodoIds.Add(todo1.Id);
            // 在数据库中插入datetodo
            string res1 = dateTodo.CreateDateTodo(dateTodo);
            Console.WriteLine("res1: " + res1);
            // 根据Date获取TodoIds
            string error = dateTodo.FetchTodoIdsByDate();
            if (error != null)
            {
                Console.WriteLine("Error fetching TodoIds: " + error);
                return;
            }

            // 根据TodoIds获取ToDos
            error = dateTodo.FetchTodosByIds();
            if (error != null)
            {
                Console.WriteLine("Error fetching Todos: " + error);
                return;
            }

            // 输出ToDos
            foreach (var todo in dateTodo.ToDos)
            {
                Console.WriteLine($"Todo ID: {todo.Id}, Title: {todo.Title}");
            }
        }
    }
}
