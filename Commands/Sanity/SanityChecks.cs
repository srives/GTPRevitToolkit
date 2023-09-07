using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Windows.Forms;
using GTP.Extractors;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.Services.Models;
using GTP.UI;
using System.Diagnostics;
using GTP.Utilities;
using System.Threading;

namespace GTP.Commands.Sanity
{
	[Transaction(TransactionMode.Manual)]
    public class SanityChecks : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			try
			{
                UIApplication uiApp = commandData?.Application;
                if (uiApp != null)
				{
                    var ver = string.Empty;
                    var dt = File.GetLastWriteTime(GetType().Assembly.Location);
                    if (dt != null && dt != DateTime.MinValue)
                    {
                        ver = $" v{dt.Year}.{dt.Month}.{dt.Day}";
                    }

                    Document doc = uiApp.ActiveUIDocument.Document;
					using (GTPDashboard ui = new GTPDashboard(doc, ver))
					{
						var win32 = new IntPtrToIWin32Window(uiApp.MainWindowHandle);
						ui.ShowDialog(win32);
					}
                }
				return Result.Succeeded;
			}
			catch (Autodesk.Revit.Exceptions.OperationCanceledException)
			{
				return Result.Cancelled;
			}
			catch (OperationCanceledException)
			{
				return Result.Cancelled;
			}
			catch (ArgumentNullException e)
			{
				return Result.Failed;
			}
			catch (Exception e)
			{
				return Result.Failed;
			}
		}

	}
}
