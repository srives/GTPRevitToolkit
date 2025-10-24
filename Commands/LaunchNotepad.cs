using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.IO;

namespace GTP.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class LaunchNotepad : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string settingsPath = Path.Combine(appDataPath, @"GTP Software Inc\STRATUS\settings.json");

                if (File.Exists(settingsPath))
                {
                    Process.Start("notepad.exe", $"\"{settingsPath}\"");
                }
                else
                {
                    TaskDialog.Show("File Not Found", $"The file was not found:\n{settingsPath}");
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}