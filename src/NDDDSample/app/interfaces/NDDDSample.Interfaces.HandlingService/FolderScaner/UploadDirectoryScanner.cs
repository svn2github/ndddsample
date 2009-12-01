namespace NDDDSample.Interfaces.HandlingService.FolderScaner
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Application;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Infrastructure.Builders;
    using Infrastructure.Log;
    using Infrastructure.Utils;

    #endregion

    /// <summary>
    /// Periodically scans a certain directory for files and attempts
    /// to parse handling event registrations from the contents.
    /// 
    /// Files that fail to parse are moved into a separate directory,
    /// succesful files are deleted.
    /// </summary>
    public class UploadDirectoryScanner
    {
        private readonly uint ScanPeriod;
        private readonly IApplicationEvents AppEvents;
        private readonly DirectoryInfo UploadDirectory;
        private readonly DirectoryInfo ParseFailureDirectory;
        private readonly ILog logger = LogFactory.GetExternalServiceLogger();
        private Timer timer;
        private bool TimerCanceled;

        public UploadDirectoryScanner(uint scanPeriod, IApplicationEvents appEvents, DirectoryInfo uploadDirectory,
                                      DirectoryInfo parseFailureDirectory)
        {
            ScanPeriod = scanPeriod;
            AppEvents = appEvents;
            UploadDirectory = uploadDirectory;
            ParseFailureDirectory = parseFailureDirectory;
            TimerCanceled = false;
        }

        public void Run()
        {
            SetProperties();
            timer = new Timer(TimerTaskCallBack, null, 0, ScanPeriod);
        }

        public void CancelTask()
        {
            TimerCanceled = true;
        }

        private void TimerTaskCallBack(object state)
        {
            lock (typeof(UploadDirectoryScanner))
            {
                foreach (FileInfo file in UploadDirectory.GetFiles())
                {
                    try
                    {
                        Parse(file);
                        Delete(file);
                        logger.Info("Import of " + file.Name + " complete");
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, e);
                        Move(file);
                    }
                }

                if (TimerCanceled)
                {
                    timer.Dispose();
                    logger.Info("UploadDirectoryScanner Task Done  " + DateTime.Now);
                }
            }
        }

        private void Parse(FileInfo file)
        {
            IList<string> lines = FileUtils.ReadLines(file);
            var rejectedLines = new List<string>();

            foreach (string line in lines)
            {
                try
                {
                    ParseLine(line);
                }
                catch (Exception e)
                {
                    logger.Error("Rejected line \n" + line + "\nReason is: " + e);
                    rejectedLines.Add(line);
                }
            }
            if (!rejectedLines.IsEmpty())
            {
                WriteRejectedLinesToFile(ToRejectedFilename(file), rejectedLines);
            }
        }

        private static string ToRejectedFilename(FileSystemInfo file)
        {
            return file.Name + ".reject";
        }

        private void WriteRejectedLinesToFile(string filename, IList<string> rejectedLines)
        {
            FileUtils.WriteLines(ParseFailureDirectory, filename, rejectedLines);
        }

        private void ParseLine(string line)
        {
            string[] columns = line.Split('\t');
            if (columns.Length == 5)
            {
                QueueAttempt(columns[0], columns[1], columns[2], columns[3], columns[4]);
            }
            else if (columns.Length == 4)
            {
                QueueAttempt(columns[0], columns[1], "", columns[2], columns[3]);
            }
            else
            {
                throw new ArgumentException("Wrong number of columns on line: " + line + ", must be 4 or 5");
            }
        }

        private void QueueAttempt(string completionTimeStr, string trackingIdStr, string voyageNumberStr,
                                  string unLocodeStr, string eventTypeStr)
        {
            var errors = new List<string>();

            DateTime date = HandlingReportParser.ParseDate(completionTimeStr, errors);
            TrackingId trackingId = HandlingReportParser.ParseTrackingId(trackingIdStr, errors);
            VoyageNumber voyageNumber = HandlingReportParser.ParseVoyageNumber(voyageNumberStr, errors);
            HandlingType eventType = HandlingReportParser.ParseEventType(eventTypeStr, errors);
            UnLocode unLocode = HandlingReportParser.ParseUnLocode(unLocodeStr, errors);

            if (errors.IsEmpty())
            {
                var attempt = new HandlingEventRegistrationAttempt(DateTime.Now, date,
                                                                   trackingId, voyageNumber,
                                                                   eventType, unLocode);
                AppEvents.ReceivedHandlingEventRegistrationAttempt(attempt);
            }
            else
            {
                throw new Exception(ToStringBuilder.ReflectionToString(errors));
            }
        }

        private void Delete(FileSystemInfo file)
        {
            try
            {
                file.Delete();
            }
            catch (Exception)
            {
                logger.Error("Could not delete " + file.Name);
                throw;
            }
        }

        private void Move(FileSystemInfo file)
        {
            string destFileName = ParseFailureDirectory.FullName + file.Name;
            try
            {
                File.Move(file.FullName, destFileName);
            }
            catch (Exception)
            {
                logger.Error("Could not move " + file.FullName + " to " + destFileName);
            }
        }

        private void SetProperties()
        {
            if (UploadDirectory.Equals(ParseFailureDirectory))
            {
                throw new Exception("Upload and parse failed directories must not be the same directory: "
                                    + UploadDirectory);
            }

            if (!UploadDirectory.Exists)
            {
                UploadDirectory.Create();
            }
            if (!ParseFailureDirectory.Exists)
            {
                ParseFailureDirectory.Create();
            }
        }
    }
}