using System;
using System.Collections.Generic;
using System.Linq;

namespace Backup
{
    public class RestorePoint
    {
        private Guid id;
        public Guid Id => id;
        
        private Date creationDate;
        public Date CreationDate => creationDate;
        
        private double restorePointSize;
        public double RestorePointSize => restorePointSize;

        private Guid parent;
        public Guid Parent => parent;
        
        private Guid child;
        public Guid Child => child;
        
        private List<FileInfos> point = new List<FileInfos>();
        public List<FileInfos> Point => point;

        public void NewFullPoint(Backup backup)
        {
            id = Guid.NewGuid();
            creationDate.day = Convert.ToInt32(DateTime.Now.Date.Day);
            creationDate.month = Convert.ToInt32(DateTime.Now.Date.Month);
            creationDate.year = Convert.ToInt32(DateTime.Now.Date.Year);
            
            foreach (var file in backup.FilesToBackup)
            { 
                file.Path = "/BackupPoint/" + file.Path;
                point.Add(file);
                restorePointSize += file.Size;
            }

            child = Guid.Empty;
            parent = Guid.Empty;
            backup.Size += restorePointSize;
            backup.RestorePointsList.Add(id, this);
        }

        public void NewIncrementalPoint(Backup backup)
        {
            id = Guid.NewGuid();
            creationDate.day = Convert.ToInt32(DateTime.Now.Date.Day);
            creationDate.month = Convert.ToInt32(DateTime.Now.Date.Month);
            creationDate.year = Convert.ToInt32(DateTime.Now.Date.Year);
            parent = backup.RestorePointsList.Keys.Last();
            backup.RestorePointsList[parent].child = id;
            
                foreach (var file in backup.FilesToBackup)
                {
                    if (!backup.RestorePointsList[parent].Point.Contains(file))
                    { 
                        file.Path = "/BackupPoint/" + file.Path; 
                        point.Add(file);
                        restorePointSize += file.Size;
                    } 
                }
                
            backup.Size += restorePointSize;
            backup.RestorePointsList.Add(id, this);
        }
    }
}