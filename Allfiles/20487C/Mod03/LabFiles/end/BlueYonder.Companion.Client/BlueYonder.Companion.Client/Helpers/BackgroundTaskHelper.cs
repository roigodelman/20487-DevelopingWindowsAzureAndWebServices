using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace BlueYonder.Companion.Client.Helpers
{
    //Module 10 - Background Tasks
    //The student will be able to create and consume background tasks.
    public class BackgroundTaskHelper
    {
        public static IBackgroundTaskRegistration RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundCondition condition, params IBackgroundTrigger[] triggers)
        {
            IBackgroundTaskRegistration _task = IsTaskRegistered(name);
            if (_task == null)
            {
                var builder = new BackgroundTaskBuilder();

                builder.Name = name;
                builder.TaskEntryPoint = taskEntryPoint;

                foreach (var trigger in triggers)
                {
                    builder.SetTrigger(trigger);
                }

                if (condition != null)
                {
                    builder.AddCondition(condition);
                }

                BackgroundTaskRegistration task = builder.Register();

                var settings = ApplicationData.Current.LocalSettings;
                var key = task.TaskId.ToString();
                settings.Values[name] = key;

                return task;
            }
            return _task;
        }

        public static void UnregisterWeatherTask(String name)
        {
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    cur.Value.Unregister(true);
                    var settings = ApplicationData.Current.LocalSettings;
                    settings.Values[name] = string.Empty;
                }
            }
        }

        private static IBackgroundTaskRegistration IsTaskRegistered(string taskName)
        {
            foreach (var task in Windows.ApplicationModel.Background.BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    return task.Value;
                }
            }
            return null;
        }
    }
}
