using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace eToolsWebsite.Helpers
{
    static class ControlHelper
    {
        // Find the control with the ID within the container and return its value as integer.
        // Return the defaultValue in case of any error (container null, control not found, value not integer)
        public static int GetControlValue(Control container, string ID, int defaultValue)
        {
            var stringValue = GetControlValue(container, ID);
            return GetNumericValue(stringValue, defaultValue);
        }
        public static double GetControlValue(Control container, string ID, double defaultValue)
        {
            var stringValue = GetControlValue(container, ID);
            return GetNumericValue(stringValue, defaultValue);
        }
        public static decimal GetControlValue(Control container, string ID, decimal defaultValue)
        {
            var stringValue = GetControlValue(container, ID);
            return GetNumericValue(stringValue, defaultValue);
        }
        // Find the control with the ID within the container and return its value as string or null if not found.
        public static string GetControlValue(Control container, string ID)
        {

            if (container != null && container.FindControl(ID) != null)
            {
                var input = container.FindControl(ID);
                if (input is Label)
                {
                    return (input as Label).Text;
                }
                else if (input is TextBox)
                {
                    return (input as TextBox).Text;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // Returns the parent control of the object. Returns null if the object is null, not a control or has no parent.
        // You can specify a level (optional parameter, default 1 meaning parent) and the method will traverse through
        // the parents by number of levels: Returns self if level = 0, parent if level = 1, grandparent if level = 2, etc.
        public static Control GetParent(object child, int level = 1)
        {
            var currentObject = child;
            // this for loop exists (does not execute body of loop) if the current object is null or 
            // it is not of type Control (or a descendant of Control). Labels, textboxes, etc are all Controls.
            for (int i = 0; i < level && currentObject != null && (currentObject as Control) != null; i++)
            {
                currentObject = (child as Control).Parent;
            }
            // returns the current object if it is of type Control (or descendants thereof), otherwise returns null.
            return currentObject as Control;
        }

        // convert string to integer, return the defaultValue if the string is not a valid integer
        private static int GetNumericValue(string stringToConvert, int defaultValue)
        {
            int i;
            return (!string.IsNullOrWhiteSpace(stringToConvert) && int.TryParse(stringToConvert, out i)) ? i : defaultValue;
        }
        // convert string to double, return the defaultValue if the string is not a valid double
        private static double GetNumericValue(string stringToConvert, double defaultValue)
        {
            double i;
            return (!string.IsNullOrWhiteSpace(stringToConvert) && double.TryParse(stringToConvert, out i)) ? i : defaultValue;
        }
        // convert string to decimal, return the defaultValue if the string is not a valid decimal
        private static decimal GetNumericValue(string stringToConvert, decimal defaultValue)
        {
            decimal i;
            return (!string.IsNullOrWhiteSpace(stringToConvert) && decimal.TryParse(stringToConvert, out i)) ? i : defaultValue;
        }
    }
}
