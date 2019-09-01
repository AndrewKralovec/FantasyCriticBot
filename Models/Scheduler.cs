using System;
using System.Collections.Generic;
using System.Threading;

namespace FantasyBot.Models
{
    public abstract class Scheduler
    {
        public readonly Dictionary<string, Timer> JobTasks;
        public Scheduler()
            => JobTasks = new Dictionary<string, Timer>();
        public virtual void ScheduleTask(string taskId, TimerCallback task, TimeSpan dueTime, TimeSpan interval)
        {
            var timer = new Timer(task, null, dueTime, interval);
            JobTasks.Add(taskId, timer);
        }
    }
}
