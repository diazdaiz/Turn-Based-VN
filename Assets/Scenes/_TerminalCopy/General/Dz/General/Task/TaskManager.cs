using System.Collections.Generic;

/// <summary>
/// Untuk ngejalanin task-task, bedanya dengan task biasa, ini gak akan finish (biar terus ngejalanin task selama running) + bisa add task aja
/// </summary>
public class TaskManager : Task {
    public TaskManager() {
        tasksSequence = new List<Task>();
    }

    public TaskManager(List<Task> tasks) {
        tasksSequence = tasks;
    }

    public void AddTask(Task task) {
        tasksSequence.Add(task);
    }

    public void RemoveTask(Task task) {
        tasksSequence.Remove(task);
    }

    public void Clear() {
        tasksSequence.Clear();
    }

    protected override List<Task> GetCurrentTaskWithDepth() {
        if (tasksSequence == null || tasksSequence.Count == 0) {
            return new() { };
        }
        return tasksSequence[0].CurrentTaskWithDepth;
    }

    public override void Updater(float dt, UpdateMethod updateMethod) {
        if (tasksSequence != null && tasksSequence.Count > 0) {
            if (tasksSequence[0].IsFinished) {
                tasksSequence.RemoveAt(0);
                return;
            }
            if (tasksSequence[0].IsCancelled) {
                tasksSequence.RemoveAt(0);
                return;
            }
            if (tasksSequence.Count <= 0) {
                return;
            }
            if (!tasksSequence[0].IsFinished && !tasksSequence[0].IsRunning && !tasksSequence[0].IsStarted) {
                tasksSequence[0].Start();
            }
            if (tasksSequence[0].IsRunning) {
                updateMethod(dt);
            }
        }
    }
}