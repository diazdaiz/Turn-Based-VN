using System;
using System.Collections.Generic;
using UnityEngine;

public class Task {
    public virtual bool IsRunning { get; set; }
    public virtual bool IsStarted { get; set; }
    public virtual bool IsFinished { get; set; }
    public virtual bool IsCancelled { get; set; }
    public virtual bool FinishWhenLastTaskInTaskSequenceCompleted { get; set; }
    public virtual bool CancelWhenTaskInTaskSequenceCanceled { get; set; }
    protected List<Task> tasksSequence;

    /// <summary>
    /// Task bisa punya depth berdasarkan berapa kali task ini di break <br/><br/>
    /// contoh: <br/>
    /// Mandi mandi = new Mandi(); mandi.BreakTask(new ListTask>() { new Nyabun(), new Nyampo(), new NyikatGigi() });<br/>
    /// TaskManager kegiatan = new TaskManager(); kegiatan.AddTask(mandi);<br/>
    /// kegiatan.CurrentTask bakal return { mandi, nyabun/nyampo/nyikatGigi }
    /// </summary>
    public List<Task> CurrentTaskWithDepth {
        get {
            return GetCurrentTaskWithDepth();
        }
    }

    protected virtual List<Task> GetCurrentTaskWithDepth() {
        if (tasksSequence == null || tasksSequence.Count <= 0) {
            return new() { this };
        }
        else {
            List<Task> returnedCurrentTasks = new() { this };
            returnedCurrentTasks.AddRange(tasksSequence[0].CurrentTaskWithDepth);
            return returnedCurrentTasks;
        }
    }

    public Task() {

    }

    public Task(List<Task> tasks, bool finishWhenLastTaskInTaskSequenceCompleted = true, bool cancelWhenLastTaskInTaskSequenceCanceled = true) {
        tasksSequence = tasks;
        FinishWhenLastTaskInTaskSequenceCompleted = finishWhenLastTaskInTaskSequenceCompleted;
        CancelWhenTaskInTaskSequenceCanceled = cancelWhenLastTaskInTaskSequenceCanceled;
    }

    public void BreakTask(List<Task> tasks, bool finishWhenLastTaskInTaskSequenceCompleted = true, bool cancelWhenLastTaskInTaskSequenceCanceled = true) {
        tasksSequence = tasks;
        FinishWhenLastTaskInTaskSequenceCompleted = finishWhenLastTaskInTaskSequenceCompleted;
        CancelWhenTaskInTaskSequenceCanceled = cancelWhenLastTaskInTaskSequenceCanceled;
    }

    /// <summary>
    /// insert index 0 before, 1 after
    /// </summary>
    void FindAndInsertTaskAt(Task current, Task taskToAdd, int insertIndex, bool sameType, Task atTask = null, Type atType = null) {
        if (current.tasksSequence == null || current.tasksSequence.Count <= 0) {
            return;
        }
        for (int i = 0; i < current.tasksSequence.Count; i++) {
            if ((!sameType && current.tasksSequence[i] == atTask) || (sameType && (current.tasksSequence[i].GetType().IsSubclassOf(atType) || current.tasksSequence[i].GetType() == atType))) {
                current.tasksSequence.Insert(i + insertIndex, taskToAdd);
                return;
            }
            if (current.tasksSequence[i].tasksSequence != null && current.tasksSequence[i].tasksSequence.Count > 0) {
                FindAndInsertTaskAt(current.tasksSequence[i], taskToAdd, insertIndex, sameType, atTask, atType);
            }
        }
    }

    Task FindAndGetTaskOfType(Task current, Type type) {
        if (GetType() == type) {
            return this;
        }
        for (int i = 0; i < current.tasksSequence.Count; i++) {
            if (current.tasksSequence[i].GetType().IsSubclassOf(type) || current.tasksSequence[i].GetType() == type) {
                return current.tasksSequence[i];
            }
            if (current.tasksSequence[i].tasksSequence != null && current.tasksSequence[i].tasksSequence.Count > 0) {
                Task task = FindAndGetTaskOfType(current.tasksSequence[i], type);
                if (task != null) {
                    return task;
                }
            }
        }
        return null;
    }

    public void AddTaskBefore(Task task, Task before) {
        FindAndInsertTaskAt(this, task, 0, false, atTask: before);
    }

    public void AddTaskBefore(Task task, Type beforeType) {
        FindAndInsertTaskAt(this, task, 0, true, atType: beforeType);
    }

    public void AddTaskAfter(Task task, Task after) {
        FindAndInsertTaskAt(this, task, 1, false, atTask: after);
    }

    public void AddTaskAfter(Task task, Type afterType) {
        FindAndInsertTaskAt(this, task, 1, true, atType: afterType);
    }

    public void AddTaskOnFirst(Task task, Type onType) {
        Task on = FindAndGetTaskOfType(this, onType);
        if (on != null) {
            if (on.tasksSequence == null) {
                on.tasksSequence = new List<Task>();
            }
            on.tasksSequence.Insert(0, task);
        }
        else {
            Debug.Log($"Type not found, will not add task on first type {onType.Name}");
        }
    }

    public void AddTaskOnFirst(Task task, Task on) {
        if (on != null) {
            if (on.tasksSequence == null) {
                on.tasksSequence = new List<Task>();
            }
            on.tasksSequence.Insert(0, task);
        }
        else {
            Debug.Log($"Task not found, will not add task on first task of type {on.GetType().Name}");
        }
    }

    public void AddTaskOnLast(Task task, Type onType) {
        Task on = FindAndGetTaskOfType(this, onType);
        if (on != null) {
            if (on.tasksSequence == null) {
                on.tasksSequence = new List<Task>();
            }
            on.tasksSequence.Insert(on.tasksSequence.Count, task);
        }
        else {
            Debug.Log($"Type not found, will not add task on last type {onType.Name}");
        }
    }

    public void AddTaskOnLast(Task task, Task on) {
        if (on != null) {
            if (on.tasksSequence == null) {
                on.tasksSequence = new List<Task>();
            }
            on.tasksSequence.Insert(on.tasksSequence.Count, task);
        }
        else {
            Debug.Log($"Task not found, will not add task on last task of type {on.GetType().Name}");
        }
    }

    public virtual void Start() {
        IsRunning = true;
        IsStarted = true;
        IsFinished = false;
        if (tasksSequence != null && tasksSequence.Count > 0) {
            tasksSequence[0].Start();
        }
    }

    public delegate void UpdateMethod(float dt);

    public virtual void Update(float dt) {
        if (tasksSequence == null || tasksSequence.Count <= 0 || !IsRunning) {
            return;
        }
        Updater(dt, tasksSequence[0].Update);
    }

    public virtual void FixedUpdate(float dt) {
        if (tasksSequence == null || tasksSequence.Count <= 0 || !IsRunning) {
            return;
        }
        Updater(dt, tasksSequence[0].FixedUpdate);
    }

    public virtual void Updater(float dt, UpdateMethod updateMethod) {
        if (tasksSequence != null && tasksSequence.Count > 0) {
            if (tasksSequence[0].IsFinished) {
                tasksSequence.RemoveAt(0);
                if (FinishWhenLastTaskInTaskSequenceCompleted && tasksSequence.Count == 0) {
                    Finish();
                    return;
                }
            }
            else if (tasksSequence[0].IsCancelled) {
                tasksSequence.RemoveAt(0);
                if (CancelWhenTaskInTaskSequenceCanceled) {
                    Cancel();
                    return;
                }
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

    /// <summary>
    /// reminder: this function only stop the task without finishing
    /// </summary>
    public virtual void Cancel() {
        IsRunning = false;
        IsCancelled = true;
    }

    public virtual void Finish() {
        IsRunning = false;
        IsFinished = true;
    }
}
