using System;
using System.IO;
using System.Collections.Generic;

namespace Backup
{
    public class Backup
    {
        private double fileSize;
        private double size;
        public double Size
        {
            get => size;
            set => size = value;
        }

        private List<FileInfos> filesToBackup = new List<FileInfos>();
        public List<FileInfos> FilesToBackup => filesToBackup;
        
        private Dictionary<Guid, RestorePoint> restorePointsList = new Dictionary<Guid, RestorePoint>();
        public Dictionary<Guid, RestorePoint> RestorePointsList => restorePointsList;

        public void AddFile(string path)
        {
            FileInfo file = new FileInfo(path);
            FileInfos fileinfo = new FileInfos(file.Name, file.Length, file.FullName);
            filesToBackup.Add(fileinfo);
            size += file.Length;
            fileSize += file.Length;
        }

        public void RemoveFile(string name)
        {
            var flag = 1;
            for(var i = 0; i < filesToBackup.Count; i++)
            {
                var file = filesToBackup[i];
                if (file.Name == name)
                {
                    filesToBackup.Remove(file);
                    size -= file.Size;
                    fileSize -= file.Size;
                    flag = 0;
                }
            }

            if (flag == 1)
            {
                throw new Exception("ERROR: Can't remove file that does not exist");
            }
            else
            {
                Console.WriteLine("Sucess: file was removed");
            }
        }

        public void BackupState()
        {
            Console.WriteLine("===================================");
            Console.WriteLine($"Backup size:   {size}");
            Console.WriteLine($"RestorePoints count:  {restorePointsList.Count}");
            Console.WriteLine($"FilesToBU count:   {filesToBackup.Count}");
            Console.WriteLine("===================================\n");
        }
    }
}