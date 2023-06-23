using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinCRH
{
    public class NumericParameterChange : IParameterChange
    {
        public NumericParameterChange(Element element, BuiltInParameter paramterName, string measurementType, double originalParameterValue, double changeInValue)
        {
            Element = element;
            BuiltInParameter = paramterName;
            IsBuiltInParameter = true;
            MeasurementType = measurementType;
            OriginalParameterValue = originalParameterValue;
            ChangeInParameterValue = changeInValue;
        }

        public NumericParameterChange(Element element, string paramterName, string measurementType, double originalParameterValue, double changeInValue)
        {
            Element = element;
            ParameterName = paramterName;
            IsBuiltInParameter = false;
            MeasurementType = measurementType;
            OriginalParameterValue = originalParameterValue;
            ChangeInParameterValue = changeInValue;
        }


        //No Setters to avoid changes to data after creation
        public Element Element { get; }
        private bool IsBuiltInParameter { get; }
        public BuiltInParameter BuiltInParameter { get; }
        public string ParameterName { get; }
        public double OriginalParameterValue { get; }
        public double ChangeInParameterValue { get; }
        public string MeasurementType { get; }
        public bool TryApplyChange()
        {
            var parameter = GetParameter();
            if (parameter != null)
            {
                return parameter.SetValueString(ChangeInParameterValue.ToString());
            }
            return false;
        }

        private  Parameter GetParameter()
        {
            if (IsBuiltInParameter) 
            {
                return this.Element.get_Parameter(this.BuiltInParameter);
            }
            return this.Element.GetParameters(this.ParameterName).FirstOrDefault();
        }
        private string GetParameterName() 
        {
            if (IsBuiltInParameter)
            {
                return this.BuiltInParameter.ToString();
            }
            return this.ParameterName;
        }

        /// <summary>
        /// A specialized message reporting the specific change and to which element
        /// </summary>
        public string GetSuccessReport()
        {
            
            string message = 
                $"Element \"{Element.Name}\" of type: \"{Element.Category.Name}\" Changed\n" +
                $"Parameter: \"{GetParameterName()}\".\n" +
                $"OriginalValue: {OriginalParameterValue} {MeasurementType}\n" +
                $"Change: {ChangeInParameterValue} {MeasurementType}\n" +
                $"NewValue: {OriginalParameterValue + ChangeInParameterValue} {MeasurementType}\n";
            return message;
        }

        public string GetFailureReport()
        {
            string message = 
                $"Failed To Affect Change: \n" +
                $"Element \"{Element.Name}\" of type: \"{Element.Category.Name}\" Changed\n" +
                $"Parameter: \"{GetParameterName()}\".\n" +
                $"OriginalValue: {OriginalParameterValue} {MeasurementType}\n" +
                $"Change: {ChangeInParameterValue} {MeasurementType}\n" +
                $"NewValue: {OriginalParameterValue + ChangeInParameterValue} {MeasurementType}\n";
            return message;
        }
    }
}
