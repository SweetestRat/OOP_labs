using System;
using System.Linq;
using System.Collections.Generic;

namespace Backup
{
    public interface RemoveAlgo
    { 
        void Remove(Backup backup) {}
    }

    public interface RemoveForHybrid : RemoveAlgo
    {
        bool WithinLimit(Backup backup);

        Dictionary<Guid, RestorePoint> RemoveForHybrid(Backup backup, Dictionary<Guid, RestorePoint> PointsToRemove)
        {
            return PointsToRemove;
        }
    }

    public class PointAmountAlgo : RemoveForHybrid
    {
        private int pointsAmount;
        
        public PointAmountAlgo(int amount)
        {
            pointsAmount = amount;
        }

        public bool WithinLimit(Backup backup)
        {
            return backup.RestorePointsList.Count <= pointsAmount;
        }
        
        public void Remove(Backup backup)
        {
            while (backup.RestorePointsList.Count > pointsAmount)
            {
                if (backup.RestorePointsList.Count == 0)
                {
                    throw new Exception("No restore points have been created yet or all points have already been deleted");
                }
                var cur = backup.RestorePointsList.Keys.FirstOrDefault();
                if(!backup.RestorePointsList[cur].Parent.Equals(Guid.Empty)) // if parent exists
                {
                    var parent = backup.RestorePointsList[cur].Parent;
                    backup.Size -= backup.RestorePointsList[parent].RestorePointSize;
                    backup.RestorePointsList.Remove(backup.RestorePointsList[cur].Parent);
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    backup.RestorePointsList.Remove(cur);
                }
                else if (!backup.RestorePointsList[cur].Child.Equals(Guid.Empty)) // if child exists
                {
                    var child = backup.RestorePointsList[cur].Child;
                    backup.Size -= backup.RestorePointsList[child].RestorePointSize;
                    backup.RestorePointsList.Remove(backup.RestorePointsList[cur].Child);
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    backup.RestorePointsList.Remove(cur);
                }
                else // if point is an orphan :(
                {
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    backup.RestorePointsList.Remove(cur);
                }
            }
        }

        public Dictionary<Guid, RestorePoint> RemoveForHybrid(Backup backup, Dictionary<Guid, RestorePoint> PointsToRemove)
        {
            var backupsize = backup.Size;
            var backupcount = backup.RestorePointsList.Count;

            foreach (var point in backup.RestorePointsList.Keys)
            {
                if (backupsize > pointsAmount)
                {
                    if(!backup.RestorePointsList[point].Parent.Equals(Guid.Empty)) // if parent exists
                    {
                        var parent = backup.RestorePointsList[point].Parent;
                        backupsize -= backup.RestorePointsList[parent].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[backup.RestorePointsList[point].Parent].Id, backup.RestorePointsList[backup.RestorePointsList[point].Parent]);
                        backupcount -= 1;
                        backupsize -= backup.RestorePointsList[point].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[point].Id, backup.RestorePointsList[point]);
                        backupcount -= 1;
                    }
                    else if (!backup.RestorePointsList[point].Child.Equals(Guid.Empty)) // if child exists
                    {
                        var child = backup.RestorePointsList[point].Child;
                        backupsize -= backup.RestorePointsList[child].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[backup.RestorePointsList[point].Child].Id, backup.RestorePointsList[backup.RestorePointsList[point].Child]);
                        backupcount -= 1;
                        backupsize -= backup.RestorePointsList[point].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[point].Id, backup.RestorePointsList[point]);
                        backupcount -= 1;
                    }
                    else // if point is an orphan :(
                    {
                        backupsize -= backup.RestorePointsList[point].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[point].Id, backup.RestorePointsList[point]);
                        backupcount -= 1;
                    }
                }
            }

            return PointsToRemove;
        }
    }

    public struct Date
    {
        public int day;
        public int month;
        public int year;
    }
    
    class PointDateAlgo : RemoveForHybrid
    {
        private Date pointsDate;

        public PointDateAlgo(string date)
        {
            string[] dateparse = date.Split("/");
            int day = Convert.ToInt32(dateparse[0]);
            int month = Convert.ToInt32(dateparse[1]);
            int year = Convert.ToInt32(dateparse[2]);
            
            if (year <= 2020)
            {
                if (month <= 12)
                {
                    if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                    {
                        if (day >= 1 && day <= 31)
                        {
                            pointsDate.day = day;
                            pointsDate.month = month;
                            pointsDate.year = year;
                        }
                        else
                        {
                            throw new Exception("Invalid date format");
                        }
                    }
                    else if (month == 4 || month == 6 || month == 9 || month == 11)
                    {
                        if (day >= 1 && day <= 30)
                        {
                            pointsDate.day = day;
                            pointsDate.month = month;
                            pointsDate.year = year;
                        }
                        else
                        {
                            throw new Exception("Invalid date format");
                        }
                    }
                    else if (month == 2)
                    {
                        if (day >= 1 && day <= 28)
                        {
                            pointsDate.day = day;
                            pointsDate.month = month;
                            pointsDate.year = year;
                        }
                        else
                        {
                            throw new Exception("Invalid date");
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid date format");
                }
            }
            else
            {
                throw new Exception("This year has not come yet");
            }
        }

        public bool WithinLimit(Backup backup)
        {
            int count = 0;
            foreach (var restorepoint in backup.RestorePointsList)
            {
                if (restorepoint.Value.CreationDate.year < pointsDate.year ||
                    restorepoint.Value.CreationDate.year == pointsDate.year &&
                    restorepoint.Value.CreationDate.month < pointsDate.month ||
                    restorepoint.Value.CreationDate.year == pointsDate.year &&
                    restorepoint.Value.CreationDate.month == pointsDate.month &&
                    restorepoint.Value.CreationDate.day < pointsDate.day)
                {
                    count += 1;
                }
            }
        
            return count == 0;
        }

        public void Remove(Backup backup)
        {
            if (backup.RestorePointsList.Count == 0)
            {
                throw new Exception("No restore points have been created yet or all points have already been deleted");
            }
            foreach (var restorepoint in backup.RestorePointsList)
            {
                var cur = restorepoint.Key;
                if (restorepoint.Value.CreationDate.year < pointsDate.year ||
                    restorepoint.Value.CreationDate.year == pointsDate.year &&
                    restorepoint.Value.CreationDate.month < pointsDate.month ||
                    restorepoint.Value.CreationDate.year == pointsDate.year &&
                    restorepoint.Value.CreationDate.month == pointsDate.month &&
                    restorepoint.Value.CreationDate.day < pointsDate.day)
                {
                    if (!backup.RestorePointsList[cur].Parent.Equals(Guid.Empty)) // if parent exists
                    {
                        var parent = backup.RestorePointsList[cur].Parent;
                        backup.Size -= backup.RestorePointsList[parent].RestorePointSize;
                        backup.RestorePointsList.Remove(backup.RestorePointsList[cur].Parent);
                        backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                        backup.RestorePointsList.Remove(cur);
                    }
                    else if (!backup.RestorePointsList[cur].Child.Equals(Guid.Empty)) // if child exists
                    {
                        var child = backup.RestorePointsList[cur].Child;
                        backup.Size -= backup.RestorePointsList[child].RestorePointSize;
                        backup.RestorePointsList.Remove(backup.RestorePointsList[cur].Child);
                        backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                        backup.RestorePointsList.Remove(cur);
                    }
                    else // if point is an orphan :(
                    {
                        backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                        backup.RestorePointsList.Remove(cur);
                    }
                }
            }
        }

        public Dictionary<Guid, RestorePoint> RemoveForHybrid(Backup backup, Dictionary<Guid, RestorePoint> PointsToRemove)
        {
            foreach (var restorepoint in backup.RestorePointsList)
            {
                var cur = restorepoint.Key;
                if (restorepoint.Value.CreationDate.year < pointsDate.year ||
                    restorepoint.Value.CreationDate.year == pointsDate.year &&
                    restorepoint.Value.CreationDate.month < pointsDate.month ||
                    restorepoint.Value.CreationDate.year == pointsDate.year &&
                    restorepoint.Value.CreationDate.month == pointsDate.month &&
                    restorepoint.Value.CreationDate.day < pointsDate.day)
                {
                    if (!backup.RestorePointsList[cur].Parent.Equals(Guid.Empty)) // if parent exists
                    {
                        var parent = backup.RestorePointsList[cur].Parent;
                        backup.Size -= backup.RestorePointsList[parent].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[backup.RestorePointsList[cur].Parent].Id, backup.RestorePointsList[backup.RestorePointsList[cur].Parent]);
                        backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[cur].Id, backup.RestorePointsList[cur]);
                    }
                    else if (!backup.RestorePointsList[cur].Child.Equals(Guid.Empty)) // if child exists
                    {
                        var child = backup.RestorePointsList[cur].Child;
                        backup.Size -= backup.RestorePointsList[child].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[backup.RestorePointsList[cur].Child].Id, backup.RestorePointsList[backup.RestorePointsList[cur].Child]);
                        backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[cur].Id, backup.RestorePointsList[cur]);
                    }
                    else // if point is an orphan :(
                    {
                        backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                        PointsToRemove.Add(backup.RestorePointsList[cur].Id, backup.RestorePointsList[cur]);
                    }
                }
            }
            
            return PointsToRemove;
        }
    }

    class PointSizeAlgo : RemoveForHybrid
    {
        private double limitsize;

        public PointSizeAlgo(double size)
        {
            limitsize = size;
        }

        public bool WithinLimit(Backup backup)
        {
            return !(backup.Size > limitsize);
        }

        public void Remove(Backup backup)
        {
            while (backup.Size > limitsize)
            {
                if (backup.RestorePointsList.Count == 0)
                {
                    Console.WriteLine($"RestorePoints count: {backup.RestorePointsList.Count}");
                    throw new Exception("No restore points have been created yet or all points have already been deleted");
                }
                var cur = backup.RestorePointsList.Keys.FirstOrDefault();
                if(!backup.RestorePointsList[cur].Parent.Equals(Guid.Empty)) // if parent exists
                {
                    var parent = backup.RestorePointsList[cur].Parent;
                    backup.Size -= backup.RestorePointsList[parent].RestorePointSize;
                    backup.RestorePointsList.Remove(backup.RestorePointsList[cur].Parent);
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    backup.RestorePointsList.Remove(cur);
                }
                else if (!backup.RestorePointsList[cur].Child.Equals(Guid.Empty)) // if child exists
                {
                    var child = backup.RestorePointsList[cur].Child;
                    backup.Size -= backup.RestorePointsList[child].RestorePointSize;
                    backup.RestorePointsList.Remove(backup.RestorePointsList[cur].Child);
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    backup.RestorePointsList.Remove(cur);
                }
                else // if point is an orphan :(
                {
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    backup.RestorePointsList.Remove(cur);
                }
            }
        }

        public Dictionary<Guid, RestorePoint> RemoveForHybrid(Backup backup, Dictionary<Guid, RestorePoint> PointsToRemove)
        {
            while (backup.Size > limitsize)
            {
                var cur = backup.RestorePointsList.Keys.FirstOrDefault();
                if(!backup.RestorePointsList[cur].Parent.Equals(Guid.Empty)) // if parent exists
                {
                    var parent = backup.RestorePointsList[cur].Parent;
                    backup.Size -= backup.RestorePointsList[parent].RestorePointSize;
                    PointsToRemove.Add(backup.RestorePointsList[backup.RestorePointsList[cur].Parent].Id, backup.RestorePointsList[backup.RestorePointsList[cur].Parent]);
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    PointsToRemove.Add(backup.RestorePointsList[cur].Id, backup.RestorePointsList[cur]);
                }
                else if (!backup.RestorePointsList[cur].Child.Equals(Guid.Empty)) // if child exists
                {
                    var child = backup.RestorePointsList[cur].Child;
                    backup.Size -= backup.RestorePointsList[child].RestorePointSize;
                    PointsToRemove.Add(backup.RestorePointsList[backup.RestorePointsList[cur].Child].Id, backup.RestorePointsList[backup.RestorePointsList[cur].Child]);
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    PointsToRemove.Add(backup.RestorePointsList[cur].Id, backup.RestorePointsList[cur]);
                }
                else // if point is an orphan :(
                {
                    backup.Size -= backup.RestorePointsList[cur].RestorePointSize;
                    PointsToRemove.Add(backup.RestorePointsList[cur].Id, backup.RestorePointsList[cur]);
                }
            }
            
            return PointsToRemove;
        }
    }

    class PointHybridAlgo : RemoveAlgo
    {
        internal enum HybridType
        {
            ALL,
            ONE
        }

        internal enum MinMaxHybridRemoveType
        {
            MIN,
            MAX
        }

        private HybridType hybridType;
        private MinMaxHybridRemoveType minmaxType;
        private RemoveForHybrid algo1;
        private RemoveForHybrid algo2;
        private Dictionary<Guid, RestorePoint> PointsToRemove1 = new Dictionary<Guid, RestorePoint>();
        private Dictionary<Guid, RestorePoint> PointsToRemove2 = new Dictionary<Guid, RestorePoint>();
        public PointHybridAlgo(HybridType type, MinMaxHybridRemoveType mmtype, RemoveAlgo _algo1, RemoveAlgo _algo2)
        {
            hybridType = type;
            minmaxType = mmtype;
            algo1 = (RemoveForHybrid) _algo1;
            algo2 = (RemoveForHybrid) _algo2;
        }

        public void Remove(Backup backup)
        {
            switch (hybridType)
            {
                case HybridType.ALL:
                {
                    if (algo1.WithinLimit(backup) == false && algo2.WithinLimit(backup) == false)
                    {
                        PointsToRemove1 = algo1.RemoveForHybrid(backup, PointsToRemove1);
                        PointsToRemove2 = algo2.RemoveForHybrid(backup, PointsToRemove2);

                        RemoveAlgo greater;
                        RemoveAlgo less;
                        if (PointsToRemove1.Count > PointsToRemove2.Count)
                        {
                            greater = algo1;
                            less = algo2;
                        }
                        else
                        {
                            greater = algo2;
                            less = algo1;
                        }
                    
                        switch (minmaxType)
                        {
                            case MinMaxHybridRemoveType.MAX:
                                greater.Remove(backup);
                                break;
                            case MinMaxHybridRemoveType.MIN:
                                less.Remove(backup);
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("One or both limits have not been exceeded");
                    }

                    break;
                }
                case HybridType.ONE:
                {
                    if (algo1.WithinLimit(backup) == false || algo2.WithinLimit(backup) == false)
                    {
                        PointsToRemove1 = algo1.RemoveForHybrid(backup, PointsToRemove1);
                        PointsToRemove2 = algo2.RemoveForHybrid(backup, PointsToRemove2);

                        if (PointsToRemove1.Count == 0 && PointsToRemove2.Count == 0)
                        {
                            throw new Exception("All is within limits");
                        }
                        // ReSharper disable once RedundantIfElseBlock
                        else if (PointsToRemove1.Count != 0)
                        {
                            algo1.Remove(backup);
                        }
                        else
                        {
                            algo2.Remove(backup);
                        }
                    }

                    break;
                }
            }
        }
    }
}