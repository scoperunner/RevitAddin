#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;
#endregion

namespace RevitAddinCRH
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            var panel = CreateRibbonPanel(app);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("AutoTopOffsetButton", "Auto Top-Offset", thisAssemblyPath, "RevitAddinCRH.Command")) is PushButton button)
            {
                button.ToolTip = "This Button excutes the task given by CRH Concrete. "; //Tooltip is a bit of a mouthfull since I can't see the function have a practical proffesional purpose. Since it is too unspecific.

                Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "icon.ico"));
                BitmapImage iconBitmap = new BitmapImage(uri);
                button.LargeImage = iconBitmap;
            }
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        public RibbonPanel CreateRibbonPanel(UIControlledApplication app) 
        {
            string tabName = "AutoOffsetterTab";
            RibbonPanel ribbonPanel = null;

            try { app.CreateRibbonTab(tabName); } catch(Exception ex) { Debug.WriteLine(ex.Message); }

            try { app.CreateRibbonPanel(tabName, "OffsetterPanel"); } catch (Exception ex) { Debug.WriteLine(ex.Message); }

            List<RibbonPanel> panels = app.GetRibbonPanels(tabName);
            foreach (RibbonPanel panel in panels.Where(p => p.Name == "OffsetterPanel")) 
            {
                ribbonPanel = panel;
            }

            return ribbonPanel;
        }
    }
}
