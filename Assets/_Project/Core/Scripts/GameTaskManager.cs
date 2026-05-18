using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTaskManager : MonoBehaviour
{
    public static GameTaskManager Instance { get; private set; }

    [Serializable]
    public class GameTask
    {
        public string id;
        public string description;
        public bool completed;
    }

    [SerializeField] private List<GameTask> tasks = new List<GameTask>();

    // Event: (taskId, completed, percentComplete)
    public event Action<string, bool, float> OnTaskChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterTask(string id, string description)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (tasks.Exists(t => t.id == id)) return;
        tasks.Add(new GameTask { id = id, description = description, completed = false });
        OnTaskChanged?.Invoke(id, false, GetCompletionPercent());
    }

    public void MarkTaskCompleted(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        var t = tasks.Find(x => x.id == id);
        if (t == null)
        {
            // If task not registered, auto-register and mark complete
            t = new GameTask { id = id, description = string.Empty, completed = true };
            tasks.Add(t);
            OnTaskChanged?.Invoke(id, true, GetCompletionPercent());
            return;
        }

        if (!t.completed)
        {
            t.completed = true;
            OnTaskChanged?.Invoke(id, true, GetCompletionPercent());
        }
    }

    public float GetCompletionPercent()
    {
        if (tasks == null || tasks.Count == 0) return 0f;
        int done = 0;
        for (int i = 0; i < tasks.Count; i++) if (tasks[i].completed) done++;
        return (done / (float)tasks.Count) * 100f;
    }

    public int GetCompletedCount()
    {
        if (tasks == null) return 0;
        int done = 0;
        for (int i = 0; i < tasks.Count; i++) if (tasks[i].completed) done++;
        return done;
    }

    public int GetTotalCount()
    {
        return tasks == null ? 0 : tasks.Count;
    }

    public IReadOnlyList<GameTask> GetAllTasks() => tasks.AsReadOnly();

    public void ResetAll()
    {
        if (tasks == null) return;
        for (int i = 0; i < tasks.Count; i++) tasks[i].completed = false;
        OnTaskChanged?.Invoke(string.Empty, false, GetCompletionPercent());
    }
}
