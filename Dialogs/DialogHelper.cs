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

            var response = TaskDialog.Show(title, message);
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
