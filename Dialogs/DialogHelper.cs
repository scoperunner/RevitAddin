using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinCRH.Dialogs
{
    public static class DialogHelper
    {
        public static bool DisplayConfirmDialog(string title, string message) 
        {
            //I'm Unsure of how to implement multiple buttons. But right now there is no-demand for a cancel-mechanic
            var response = TaskDialog.Show(title, message, TaskDialogCommonButtons.Ok);
            if (response == TaskDialogResult.Ok) 
            {
                return true;
            }
            return false;
        }

        public static bool DisplayConfirmDialog(string title ,List<string> messages) 
        {
            var message = AggregateMessages(messages);
            return DisplayConfirmDialog(title, message);
        }

        private static string AggregateMessages(List<string> messages)
        {
            var message = String.Join("\n\n", messages);
            return message;
        }
    }
}
