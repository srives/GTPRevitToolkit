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

namespace GTP.Commands.Sanity
{
	[Transaction(TransactionMode.Manual)]
    public class SanityChecks : IExternalCommand
    {
		/// <summary>
		/// The execution context for the addin.
		/// </summary>
		private protected ApplicationContext context;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			try
			{
                UIApplication uiApp = commandData?.Application;
				if (uiApp != null)
				{
                    var ver = string.Empty;
                    var dt = File.GetCreationTime(GetType().Assembly.Location);
                    if (dt != null && dt != DateTime.MinValue)
                    {
                        ver = $" v{dt.Year}.{dt.Month}.{dt.Day}";
                    }
                    Document doc = uiApp.ActiveUIDocument.Document;
					using (GTPDashboard ui = new GTPDashboard(doc, ver))
					{
						ui.ShowDialog();
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
