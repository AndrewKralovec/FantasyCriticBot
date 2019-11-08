using System;
using System.Collections.Generic;
using System.Threading;

namespace FantasyBot.Models
{
    /// <summary>
    /// Abstract <c>Announcement</c> class. Defines how to schedule tasks. 
    /// Abstract because services should manage their own tasks.
    /// </summary>
    public abstract class Announcement
    {
        // Makes sense for now, might need to change when its time to start/stop tasks.
        readonly Dictionary<string, Timer> _tasks;
        /// <summary>
        /// The <c>Announcement</c> class constructor.
        /// </summary>
        public Announcement() => _tasks = new Dictionary<string, Timer>();

        /// <summary>
        /// Initializes a new threaded timer with the requested task. The timer is Registered to <c>_tasks</c> collection.
        /// </summary>
        /// <param name="taskId">Key used to find the task</param>
        /// <param name="task">Operation to be run by timer</param>
        /// <param name="dueTime">When the timer starts</param>
        /// <param name="interval">How often it runs</param>
        public virtual void ScheduleTask(string taskId, TimerCallback task, TimeSpan dueTime, TimeSpan interval)
        {
            if (TaskExists(taskId))
                return;

            var timer = new Timer(task, null, dueTime, interval);
            _tasks.Add(taskId, timer);
        }
        /// <summary>
        /// Update the start and interval time of a task. Assumes the task exists.
        /// </summary>
        /// <param name="taskId">Key used to find the task</param>
        /// <param name="dueTime">When the timer starts</param>
        /// <param name="interval">How often it runs</param>
        public virtual bool UpdateTaskTime(string taskId, TimeSpan dueTime, TimeSpan interval)
        {
            var task = _tasks[taskId];
            return task.Change(dueTime, interval);
        }

        public virtual bool TaskExists(string taskId) => _tasks.ContainsKey(taskId);
    }
}
