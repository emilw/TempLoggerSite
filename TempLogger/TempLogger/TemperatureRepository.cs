using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempLogger
{
    //this.Request.MapPath(this.Request.ApplicationPath)
    public class TemperatureRepository
    {
        private System.IO.FileStream _stream;
        //private System.IO.StreamWriter _writer;

        private string _directoryPath;
        private string _recordType;

        private object writeLock = new object();


        public TemperatureRepository(string directoryPath)
        {
            _directoryPath = directoryPath + "TemperatureRecords\\";
            
            if (!System.IO.Directory.Exists(_directoryPath))
                System.IO.Directory.CreateDirectory(_directoryPath);
                
        }

        private bool checkTargetTypeFile(string targetFileName)
        {
            var blnExists = System.IO.File.Exists(path(targetFileName));

            if(!blnExists)
                logAttempt(targetFileName);

            return blnExists;
        }

        private void logAttempt(string targetFileName)
        {
            var writer = System.IO.File.AppendText(path("NonRegisteredTargetsAttempts"));
            writer.WriteLine(targetFileName + "; " + DateTime.Now);
            writer.Close();
        }

        public void RegisterTarget(string target)
        {
            var filePath = path(target);

            if (!System.IO.File.Exists(filePath))
            {
                _stream = System.IO.File.Create(filePath);
                _stream.Close();
            }
        }

        private void Close()
        {
            _stream.Close();
        }

        private string path(string targetFile)
        {
            return _directoryPath + targetFile + ".txt";
        }

        public IEnumerable<Temperature> Load(string target)
        {
            var records = Read(target);
            var result = new List<Temperature>();

            if (records.Count() == 1 && records[0] == "")
                return result;

            records.ToList().ForEach(x => result.Add(UnMap(x)));

            return result.ToArray();
        }

        public string[] Read(string target)
        {
            //var reader = new System.IO.StreamReader(Open(true));
            if(!checkTargetTypeFile(target))
                return new string[]{""};

            var result = System.IO.File.ReadAllLines(path(target));
            //var result = reader.ReadToEnd();
            //reader.Close();
            //return result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            return result;
        }

        public void Save(Temperature temp, string target)
        {
            temp.TimeStamp = DateTime.Now;

            var record = Map(temp);
            Write(record, target);
        }

        private void Write(string record, string target)
        {
            if (!checkTargetTypeFile(target))
                return;

            /*var writer = new System.IO.StreamWriter(Open());
            
            writer.WriteLine(record);
            writer.Close();*/
            lock(writeLock)
            {
                var writer = System.IO.File.AppendText(path(target));
                writer.WriteLine(record);
                writer.Close();
            }

        }

        private string Map(Temperature temp)
        {
            var record = "{0};{1}";

            return String.Format(record, temp.Value, temp.TimeStamp);
        }

        private Temperature UnMap(string record)
        {
            var cells = record.Split(';');

            var temp = new Temperature();

            temp.Value = Convert.ToDecimal(cells[0]);
            temp.TimeStamp = Convert.ToDateTime(cells[1]);

            return temp;
        }
    }
}