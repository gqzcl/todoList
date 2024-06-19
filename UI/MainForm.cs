using Core.todolist;

namespace UI
{
    public class MainForm : Form
    {
        private ListBox todoListBox;
        private TextBox todoTextBox;
        private Button addTodoBtn;
        private Button delTodoBtn;
        public MainForm()
        {
            todoListBox = new ListBox();
            todoTextBox = new TextBox();
            addTodoBtn = new Button();
            delTodoBtn = new Button();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            todoListBox.Location = new Point(12, 12);
            todoListBox.Name = "todoListBox";
            todoListBox.Size = new Size(260, 204);
            todoListBox.TabIndex = 0;

            todoTextBox.Location = new Point(12, 213);
            todoTextBox.Name = "todoTextBox";
            todoTextBox.Size = new Size(260, 27);
            todoTextBox.TabIndex = 1;

            addTodoBtn.Location = new Point(12, 246);
            addTodoBtn.Name = "addTodoBtn";
            addTodoBtn.Size = new Size(111, 27);
            addTodoBtn.TabIndex = 2;
            addTodoBtn.Text = "Add";
            addTodoBtn.Click += AddButtonClick;

            delTodoBtn.Location = new Point(161, 246);
            delTodoBtn.Name = "delTodoBtn";
            delTodoBtn.Size = new Size(111, 27);
            delTodoBtn.TabIndex = 3;
            delTodoBtn.Text = "Del";
            delTodoBtn.Click += DelButtonClick;

            ClientSize = new Size(284, 281);
            Controls.Add(todoListBox);
            Controls.Add(todoTextBox);
            Controls.Add(addTodoBtn);
            Controls.Add(delTodoBtn);
            Name = "MainForm";
            Text = "Todo List";
            ResumeLayout(false);
            PerformLayout();
        }

        private void AddButtonClick(object? sender, EventArgs e)
        {
            var input = todoTextBox.Text.Trim();
            if (input == null) return ;

            TodoList todo = new TodoList();
            todo.Title = input;
            // todo.Id = int.Parse(Guid.NewGuid().ToString());
            todo.StartDate = DateTime.Now;
            todo.EndDate = DateTime.Now + TimeSpan.FromDays(1);

            this.todoListBox.Items.Add(input);
        }

        private void DelButtonClick(object? sender, EventArgs e)
        {
            var input = todoTextBox.Text.Trim();
            if (input == null) return;

            this.todoListBox.Items.Remove(input);
        }
    }
}
