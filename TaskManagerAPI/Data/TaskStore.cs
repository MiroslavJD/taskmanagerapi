using TaskManagerAPI.Models;



namespace TaskManagerAPI.Data
{
    public static class TaskStore
    {

        private static readonly List<TodoTask> _tasks = new()
        {
            new TodoTask {Id = 1, Title = "Learn ASP.NET Core", IsDone = false},
            new TodoTask {Id = 2, Title = "Build CRUD API", IsDone=false}
        };

        private static int _nextId = 3;

        public static List<TodoTask> GetAll() => _tasks;
        public static TodoTask? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);
        
        public static TodoTask Add(string title)
        {
            var task  = new TodoTask { Id = _nextId, Title = title };
            _tasks.Add(task);
            return task;
        }

        public static bool Update(int id, string title,bool isDone)
        {
            var task = GetById(id);
            if (task is null) return false;
            task.Title = title;
            task.IsDone = isDone;
            return true;
        }

        public static bool Delete(int id)
        {
            var task = GetById(id);
            if (task is null) return false;
            _tasks.Remove(task);
            return true;
        }

    }
}
