using System;

namespace Backup
{
    class Program
    {
        static void Main(string[] args)
        {
           Backup data = new Backup();

           data.AddFile("/Users/vladislavagilde/Documents/2 semester/English/Essay Fashion.docx");
           data.AddFile("/Users/vladislavagilde/Documents/2 semester/English/ESSAY_Adventure_time.docx");
           data.AddFile("/Users/vladislavagilde/Documents/2 semester/English/Film.docx");
           
           data.RemoveFile("Essay Fashion.docx");

           RestorePoint restorePoint1 = new RestorePoint();
           restorePoint1.NewFullPoint(data);
           
           data.AddFile("/Users/vladislavagilde/Desktop/Screenshot 2020-12-15 at 22.13.25.png");
           RestorePoint restorePoint2 = new RestorePoint();
           restorePoint2.NewIncrementalPoint(data);
           
           RestorePoint restorePoint3 = new RestorePoint();
           restorePoint3.NewFullPoint(data);
           
           data.AddFile("/Users/vladislavagilde/Desktop/Screenshot 2020-12-15 at 22.14.00.png");
           RestorePoint restorePoint4 = new RestorePoint();
           restorePoint4.NewIncrementalPoint(data);
           
           RestorePoint restorePoint5 = new RestorePoint();
           restorePoint5.NewFullPoint(data);
           RestorePoint restorePoint6 = new RestorePoint();
           restorePoint6.NewFullPoint(data);

           data.BackupState();
           
           Console.WriteLine("V AMOUNT ALGO V");
           RemoveAlgo amountalgo = new PointAmountAlgo(5);
           amountalgo.Remove(data);
           data.BackupState();

           Console.WriteLine("V DATE ALGO V");
           RemoveAlgo datealgo = new PointDateAlgo("14/12/2020");
           datealgo.Remove(data);           
           data.BackupState();

           Console.WriteLine("V SIZE ALGO V");
           RemoveAlgo sizealgo = new PointSizeAlgo(10000000);
           sizealgo.Remove(data);
           data.BackupState();
           
           Console.WriteLine("V HYBRID ALGO V");
           RemoveAlgo hybridalgo = new PointHybridAlgo(PointHybridAlgo.HybridType.ONE, PointHybridAlgo.MinMaxHybridRemoveType.MAX, new PointAmountAlgo(1), new PointSizeAlgo(10000000));
           hybridalgo.Remove(data);
           data.BackupState();
           
        }
    }
}