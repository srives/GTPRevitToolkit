using System;
using System.IO;
using Gtpx.ModelSync.DataModel.Enums;

namespace Gtpx.ModelSync.Services.Models
{
    public class LocalFileContext
    {
        public bool IsSilentMode { get; set; }

        public string LocalImportLogPath { get; set; }

        public string ModelName { get; set; }

        public ModelSyncMode ModelSyncMode { get; set; }

        public PlatformType PlatformType { get; set; }

        public string GetLocalExtractTesterInfoPath()
        {
            var subFolder = string.Empty;
            switch (PlatformType)
            {
                case PlatformType.AutoCAD:
                    subFolder = "AutoCAD";
                    break;

                case PlatformType.Revit:
                    subFolder = "Revit";
                    break;
            }

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "GTP Software Inc",
                                "STRATUS Logs",
                                subFolder,
                                $"{ModelName}.json");
        }

        public string GetLocalLogPath()
        {
            var subFolder = string.Empty;
            switch (PlatformType)
            {
                case PlatformType.AutoCAD:
                    subFolder = "AutoCAD";
                    break;

                case PlatformType.Revit:
                    subFolder = "Revit";
                    break;
            }

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "GTP Software Inc",
                                "STRATUS Logs",
                                subFolder,
                                $"Log - {ModelName} - {ModelSyncMode}.txt");
        }

        public string GetLocalStorageDirectory(string localStorageRootDirectory)
        {
            var localStorageDirectory = string.Empty;

            if (!string.IsNullOrEmpty(localStorageRootDirectory))
            {
                localStorageDirectory = Path.Combine(localStorageRootDirectory, ModelName);
            }

            return localStorageDirectory;
        }
    }
}
