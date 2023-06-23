#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAddinCRH.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion


/*
 * Quick note to readers.
 * Since this is an example to showcase my code, then there will be comment Around the code explaining thoughts and intent with code.
 * This is not something that would be done in production-code. But this is a showcase.
 */

namespace RevitAddinCRH
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        private static BuiltInCategory beamCatagory = BuiltInCategory.OST_StructuralFraming;
        private static BuiltInCategory collumsCatagory = BuiltInCategory.OST_StructuralColumns;
        private static BuiltInParameter BeamBotttomHeightParameterName = BuiltInParameter.STRUCTURAL_ELEVATION_AT_BOTTOM;
        private static string collumHeightParameterName = "Height"; //Height is not part of "BuiltInParameters"
        private static BuiltInParameter TopOffsetScheduleParameterName = BuiltInParameter.SCHEDULE_TOP_LEVEL_OFFSET_PARAM;
        private static BuiltInParameter TopOffsetFamilyParameterName = BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM;

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            //setup
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Getting all beams, since we don't have a selection according to the task set for us then we have to retrieve them all and act upon them all.  
            //Applicable for this specific case, but should probalbly be changed to handle selected elements instead. This current setup will find a random beam in the drawing.
            var beams = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(beamCatagory)
                .ToElements();
            var beam = (FamilyInstance)beams.FirstOrDefault();
          
            var beamBottomHeight = beam.get_Parameter(BeamBotttomHeightParameterName).AsValueString();
            double beamBottomHeightAsDouble;
            if (!Double.TryParse(beamBottomHeight, out beamBottomHeightAsDouble)) 
            {
                Debug.WriteLine("beam.STRUCTURAL_ELEVATION_AT_BOTTOM is not a numeric value. This constitutes an invalidParameterException");
                return Result.Failed;
            }

            var collums = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(collumsCatagory)
                .ToElements();

            var changeList = new List<IParameterChange>();
            

            // Modify document within a transaction
            List<string> dialogMessages = new List<string>();
            using (Transaction tx = new Transaction(doc))
            {
                //Lock Drawing, to prevent external changes in values.
                tx.Start("Transaction Name");

                foreach (Element collum in collums)
                {
                    string collumTopHeight = GetCollumHeightParameter(collum);
                    double collumTopHeightAsDouble;
                    if (!Double.TryParse(collumTopHeight, out collumTopHeightAsDouble))
                    {
                        Debug.WriteLine("beam.STRUCTURAL_ELEVATION_AT_BOTTOM is not a numeric value. This constitutes an invalidParameterException");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    var topoffsetValue = beamBottomHeightAsDouble - collumTopHeightAsDouble;
                    var change = new NumericParameterChange(collum, TopOffsetFamilyParameterName, "mm", collumTopHeightAsDouble, topoffsetValue);
                    changeList.Add(change);
                }

                foreach (var change in changeList) 
                {
                    if (change.TryApplyChange())
                    {
                        dialogMessages.Add(change.GetSuccessReport());
                    }
                    else {
                        Debug.WriteLine(change.GetFailureReport());
                    }
                }

                if (DialogHelper.DisplayConfirmDialog("Confirm Changes", dialogMessages))
                {
                    tx.Commit();
                    return Result.Succeeded;
                }
            }

            return Result.Failed;
        }

      

        private static string GetCollumHeightParameter(Element collum)
        {
            string result;

            var heightParam = collum.GetParameters(collumHeightParameterName).FirstOrDefault();
            result = heightParam.AsValueString();

            return result;
        }


        #region Legacy-code
        //I've left these to show that I did try other approaches, but settled on IParameterChange as my approach

        /// <summary>
        /// Tries to set parameter. Allows for specific parameter searches. But it is suggested to use "BuiltInParameters".
        /// </summary>
        /// <param name="element">Element to edit parameter on</param>
        /// <param name="value"></param>
        /// <returns>returns true if action were successfull. Returns false if the parameter doesn't exist or if action failed</returns>
        private static bool TrySetParameter(Element element, BuiltInParameter parameter, string value)
        {
            var heightParam = element.get_Parameter(parameter);
            if (heightParam != null)
            {
                heightParam.SetValueString(value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tries to set parameter. Allows for specific parameter searches. But it is suggested to use "BuiltInParameters".
        /// </summary>
        /// <param name="element">Element to edit parameter on</param>
        /// <param name="value"></param>
        /// <returns>returns true if action were successfull. Returns false if the parameter doesn't exist or if action failed</returns>
        private static bool TrySetParameter(Element element, string parameterName, string value)
        {
            var heightParam = element.GetParameters(parameterName).FirstOrDefault();
            if (heightParam != null)
            {
                heightParam.SetValueString(value);
                return true;
            }
            return false;
        }
        #endregion
    }
}
