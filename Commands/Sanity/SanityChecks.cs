﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

using Autodesk.Revit.DB.Fabrication;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Documents;
using System.Linq;
using System.CodeDom;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using GTP.Extractors;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.Services.Models;
using Gtpx.ModelSync.CAD.Cache;
using Gtpx.ModelSync.Export.Revit.Providers;
using Newtonsoft.Json;

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
			var ver = string.Empty;
			try
			{
                UIApplication uiApp = commandData?.Application;
				if (uiApp != null)
				{

                    Document doc = uiApp.ActiveUIDocument.Document;
					LocalFileContext lfc = new LocalFileContext();
					Notifier logger = new Notifier(lfc, Serilog.Log.Logger); // TO DO: Replace with my own logger
                    ElementExtractor.Execute(doc, logger);


                    var dt = File.GetCreationTime(GetType().Assembly.Location);
					if (dt != null && dt != DateTime.MinValue)
					{
						ver += $" v{dt.Year}.{dt.Month}.{dt.Day}";
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
