using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Helpers
{
    public class DeviceTimer
    {
        readonly Action _RunningTask;
        readonly Action _StoppingTask;
        readonly List<TaskWrapper> _Tasks = new List<TaskWrapper>();
        readonly TimeSpan _interval;
        public bool IsRecurring { get; }
        public bool IsRunning => _Tasks.Any(t => t.IsRunning);
        private bool IsStopping { get; set; }

        public DeviceTimer(Action runningTask, TimeSpan interval, bool isRecurring = false, bool start = false, Action stoppingTask = null)
        {
            _RunningTask = runningTask;
            _StoppingTask = stoppingTask;
            _interval = interval;
            IsRecurring = isRecurring;
            if (start) Start();
        }

        public void Restart()
        {
            Pause();
            Start();
        }

        public void Start()
        {
            if (IsRunning)
                // Already Running
                return;

            var wrapper = new TaskWrapper(_RunningTask, IsRecurring, true);
            _Tasks.Add(wrapper);

            Device.StartTimer(_interval, wrapper.RunTask);
        }

        public void Pause()
        {
            foreach (var task in _Tasks)
                task.IsRunning = false;
            _Tasks.Clear();
        }

        public void Stop()
        {
            Pause();

            if (!IsStopping) _StoppingTask.Invoke();

            IsStopping = true;
        }

        class TaskWrapper
        {
            public bool IsRunning { get; set; }
            bool _IsRecurring;
            Action _Task;
            public TaskWrapper(Action task, bool isRecurring, bool isRunning)
            {
                _Task = task;
                _IsRecurring = isRecurring;
                IsRunning = isRunning;
            }

            public bool RunTask()
            {
                if (IsRunning)
                {
                    _Task();
                    if (_IsRecurring)
                        return true;
                }

                // No longer need to recur. Stop
                return IsRunning = false;
            }
        }
    }
}
