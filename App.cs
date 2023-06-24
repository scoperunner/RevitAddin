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
using Adwin = Autodesk.Windows;
#endregion

namespace RevitAddinCRH
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            var AddinPanel = CreateRibbonPanel(Tab.AddIns, app);
            
            AttachOffSetButton(AddinPanel);
            return Result.Succeeded;
        }

        private static void AttachOffSetButton(RibbonPanel panel)
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            if (
                panel.AddItem(
                    new PushButtonData(
                        "AutoTopOffsetButton",
                        "Auto Top-Offset",
                        thisAssemblyPath,
                        "RevitAddinCRH.Command"
                        )
                    )
                is PushButton button)
            {
                //Tooltip is a bit of a mouthfull since I can't see the function have a practical proffesional purpose. Since it is too unspecific.
                button.ToolTip = "This Button excutes the task given by CRH Concrete. ";

                //Add Icon, The icon might not be the best size. I'm not the biggest icon-specialist.
                BitmapImage iconBitmap = GetIcon(thisAssemblyPath);
                button.Image = iconBitmap;
            }
        }

        private static BitmapImage GetIcon(string thisAssemblyPath)
        {
            Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "RevitAddinCRH", "Resouces", "icon.ico"));
            BitmapImage iconBitmap = new BitmapImage(uri);
            return iconBitmap;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Creates a RibbonPanel in given Tab.
        /// This method Does not create a tab. it purely create and adds a panel to a existing tab.
        /// </summary>
        /// <returns>The RibbonPanel Created</returns>
        private RibbonPanel CreateRibbonPanel(string TabName, UIControlledApplication app) 
        {
            try { return  app.CreateRibbonPanel(TabName, "OffsetterPanel"); } catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null;
        }
        /// <summary>
        /// Creates a RibbonPanel in given Tab.
        /// This method Does not create a tab. it purely create and adds a panel to a existing tab.
        /// </summary>
        /// <returns>The RibbonPanel Created</returns>
        private RibbonPanel CreateRibbonPanel(Tab TabName, UIControlledApplication app)
        {
            try { return app.CreateRibbonPanel(TabName, "OffsetterPanel"); } catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null;
        }
    }
}
