using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace GTP
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {

        /// Handles any necessary cleanup tasks for the toolkit on Revit shutdown.
        /// </summary>
        /// <param name="application">The UIControlledApplication for the currently running Revit instance.</param>
        /// <returns>The Result status of the function.</returns>

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


        /// <summary>
        /// Handles any startup tasks for the toolkit on Revit startup.
        /// </summary>
        /// <param name="application">The UIControlledApplication for the currently running Revit instance.</param>
        /// <returns>The Result status of the function.</returns>
        public Result OnStartup(UIControlledApplication application)
        {
            var ribbonTabName  = CreateRibbonTab(application, "GTP Toolkit");
            var modelCheck     = CreateRibbonPanel(application, ribbonTabName, "Model Check");
            ModelHealthButtons(modelCheck, "1");

            return Result.Succeeded;
        }

        public void ModelHealthButtons(RibbonPanel healthPanel, string ver)
        {
            var sanityChecks = AddPulldownButtonToPanel(healthPanel,
                "Model Check",
                "Model Check",
                "",
                ver,
                Properties.Resources.doc);

            AddPushButtoToPulldownButton(sanityChecks,
                "ElementExtractorSanity",       // btn Name
                "STRATUS Publish Sanity Check", // btn Text
                Assembly.GetExecutingAssembly().Location, // DLL
                "GTP.Commands.Sanity.SanityChecks", // Class
                "Get timing on how long to load all elments and process them in a publish",
                ver);

            /*
            AddPushButtonToPanel(healthPanel,
                "Button Name",
                "Button Text",
                Assembly.GetExecutingAssembly().Location,
                "GTP.Commands.CLASS NAME",
                "",
                "", 
                Properties.Resources.heart 
                );
            */
        }

        /// <summary>
        /// Creates a new pulldown button and attaches it to a panel.
        /// </summary>
        /// <param name="panel"> The panel to attach the button to. </param>
        /// <param name="btnName"> The internal name of the button. </param>
        /// <param name="btnText"> The text displayed on the button. </param>
        /// <param name="toolTip"> The text displayed on button hover. </param>
        /// <param name="longDescription"> The text displayed on extended button hover. </param>
        /// <param name="image"> The image displayed on the button. </param>
        /// <returns> Returns the newly created button to attach push buttons to. </returns>
        private static PulldownButton AddPulldownButtonToPanel(
            RibbonPanel panel,
            string btnName,
            string btnText,
            string toolTip,
            string longDescription,
            Image image)
        {
            if (panel == null)
            {
                throw new ArgumentNullException(paramName: nameof(panel));
            }

            var btn = new PulldownButtonData(btnName, btnText)
            {
                ToolTip = toolTip,
                LongDescription = longDescription
            };

            if (image != null)
            {
                var imageSource = GetImageSource(image);
                btn.Image = imageSource;
                btn.LargeImage = imageSource;
            }

            return panel.AddItem(btn) as PulldownButton;
        }

        /// <summary>
        /// Crates a new push button and attaches it to a panel.
        /// </summary>
        /// <param name="panel"> The panel to attach the button to. </param>
        /// <param name="btnName"> The internal name of the button. </param>
        /// <param name="btnText"> The text displayed on the button. </param>
        /// <param name="assemblyName"> The name of the assembly where the targeted command exists. </param>
        /// <param name="className"> The fully qualifed name of the targeted command. </param>
        /// <param name="toolTip"> The text displayed on button hover. </param>
        /// <param name="longDescription"> The text displayed on extended button hover. </param>
        /// <param name="image"> The image to display on the button. </param>
        private static void AddPushButtonToPanel(
            RibbonPanel panel,
            string btnName,
            string btnText,
            string assemblyName,
            string className,
            string toolTip,
            string longDescription,
            Image image)
        {
            if (panel == null)
            {
                throw new ArgumentNullException(paramName: nameof(panel));
            }

            var btn = new PushButtonData(btnName, btnText, assemblyName, className)
            {
                ToolTip = toolTip,
                LongDescription = longDescription
            };

            if (image != null)
            {
                var imageSource = GetImageSource(image);
                btn.Image = imageSource;
                btn.LargeImage = imageSource;
            }

            panel.AddItem(btn);
        }


        /// <summary>
        /// Creates a new push button and attaches it to a pulldown button.
        /// </summary>
        /// <param name="pulldown"> The pulldown button to attach the push buttons to. </param>
        /// <param name="btnName"> The internal name of the button. </param>
        /// <param name="btnText"> The text displayed on the button. </param>
        /// <param name="assemblyName"> The name of the assembly where the targeted command exists. </param>
        /// <param name="className"> The fully qualifed name of the targeted command. </param>
        /// <param name="toolTip"> The text displayed on button hover. </param>
        /// <param name="longDescription"> The text displayed on extended button hover. </param>
        private static void AddPushButtoToPulldownButton(
            PulldownButton pulldown,
            string btnName,
            string btnText,
            string assemblyName,
            string className,
            string toolTip,
            string longDescription)
        {
            if (pulldown == null)
            {
                throw new ArgumentNullException(paramName: nameof(pulldown));
            }

            var btn = new PushButtonData(btnName, btnText, assemblyName, className)
            {
                ToolTip = toolTip,
                LongDescription = longDescription
            };

            pulldown.AddPushButton(btn);
        }


        /// <summary>
        /// Creates a new panel and attaches it to a ribbon tab.
        /// </summary>
        /// <param name="application"> The Revit application instantce. </param>
        /// <param name="tabName"> The tab name to which to attach the panel to. </param>
        /// <param name="panelName">The name of the panel that is displayed in the ribbon tab. </param>
        /// <returns> Returns the panel to attach UI elements to. </returns>
        private static RibbonPanel CreateRibbonPanel(UIControlledApplication application, string tabName, string panelName)
        {
            if (application == null)
            {
                throw new ArgumentNullException(paramName: nameof(application));
            }

            RibbonPanel panel = null;
            var panels = application.GetRibbonPanels(tabName);

            foreach (var p in panels)
            {
                if (p.Name == panelName)
                {
                    panel = p;
                    break;
                }
            }

            if (panel == null)
            {
                panel = application.CreateRibbonPanel(tabName, panelName);
            }

            return panel;
        }


        /// <summary>
        /// Creates a new ribbon tab and attaches it to the client.
        /// </summary>
        /// <param name="application"> The Revit application instance. </param>
        /// <param name="tabName"> The tab name that is displayed in the revit client. </param>
        /// <returns> Returns the tab name to attach UI elements to. </returns>
        private static string CreateRibbonTab(UIControlledApplication application, string tabName)
        {
            if (application == null)
            {
                throw new ArgumentNullException(paramName: nameof(application));
            }

            application.CreateRibbonTab(tabName);

            return tabName;
        }



        /// <summary>
        /// Creates a new bitmap of a given image.
        /// </summary>
        /// <param name="img"> The image to create a bitmap of. </param>
        /// <returns> Returns the generated bitmap. </returns>
        private static BitmapSource GetImageSource(Image img)
        {
            var bmp = new BitmapImage();

            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;
                bmp.EndInit();
            }

            return bmp;
        }
    }
}
