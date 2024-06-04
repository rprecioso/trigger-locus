using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trigger_locus_03_13_2014.Model;

namespace trigger_locus_03_13_2014.ViewModel
{
    public class TaskHelper : INotifyPropertyChanged
    {
        trigger_locus_DataContext triggerDB;

        public TaskHelper(string connectionString)
        {
            triggerDB = new trigger_locus_DataContext(connectionString);
        }

        private ObservableCollection<TblTask> _allTasks;
        public ObservableCollection<TblTask> allTasks
        {
            get { return _allTasks; }
            set
            {
                _allTasks = value;
                RaisePropertyChanged("allTasks");
            }
        }

        private ObservableCollection<TblTask> _taskList;
        public ObservableCollection<TblTask> taskList
        {
            get { return _taskList; }
            set
            {
                _taskList = value;
                RaisePropertyChanged("taskList");
            }
        }

        private TblTask _selectedTask;
        public TblTask selectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                RaisePropertyChanged("selectedTask");
            }
        }

        public void addTask(TblTask task)
        {
            triggerDB.TblTasks.InsertOnSubmit(task);
            triggerDB.SubmitChanges();
        }

        public void deleteTask(TblTask task)
        {
            triggerDB.TblTasks.DeleteOnSubmit(triggerDB.TblTasks.Where(x => x.Task_id == task.Task_id).First());
            triggerDB.SubmitChanges();
        }

        public void setAsDone(int _task_id, bool active)
        {
            triggerDB.TblTasks.Where(t => t.Task_id == _task_id).First().isActive = active;
            triggerDB.SubmitChanges();
        }

        public void updateTask(TblTask updatedTask)
        {
            TblTask taskQuery = (from tas in triggerDB.TblTasks
                                 where tas.Task_id == updatedTask.Task_id
                                 select tas).First();
            taskQuery.TileWidth = updatedTask.TileWidth;
            taskQuery.Title = updatedTask.Title;
            taskQuery.Description = updatedTask.Description;
            taskQuery.Date_task = updatedTask.Date_task;
            triggerDB.SubmitChanges();
        }

        public void loadTask()
        {
            var tasks = from tas in triggerDB.TblTasks
                        select tas;
            allTasks = new ObservableCollection<TblTask>(tasks);
        }

        public void loadTask(DateTime _datetime)
        {
            var tasks = from tas in allTasks
                        where ((DateTime)tas.Date_task).Year == _datetime.Year &&
                       ((DateTime)tas.Date_task).Month == _datetime.Month &&
                       ((DateTime)tas.Date_task).Day == _datetime.Day
                        select tas;
            taskList = new ObservableCollection<TblTask>(tasks);
        }

        public void loadTask(int _Task_id)
        {
            var tasks = from tas in triggerDB.TblTasks
                        where tas.Task_id == _Task_id
                        select tas;
            selectedTask = tasks.FirstOrDefault();        
        }

        public void resizeTile(int _Task_id)
        {
            var task = (from tas in triggerDB.TblTasks
                        where tas.Task_id == _Task_id
                        select tas).First();

            if (task.TileWidth == 454)
            {
                task.TileWidth = 150;
            }
            else if (task.TileWidth == 302)
            {
                task.TileWidth = 454;
            }
            else if (task.TileWidth == 150)
            {
                task.TileWidth = 302;
            }
            triggerDB.SubmitChanges();
        }

        public void saveChanges()
        {
            triggerDB.SubmitChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
