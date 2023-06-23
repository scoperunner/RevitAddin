using Autodesk.Revit.DB;

namespace RevitAddinCRH
{
    public interface IParameterChange
    {
        Element Element { get; }
        string MeasurementType { get; }
        double ChangeInParameterValue { get; }
        double OriginalParameterValue { get; }
        string ParameterName { get; }
        string GetSuccessReport();
        string GetFailureReport();
        bool TryApplyChange();
    }
}