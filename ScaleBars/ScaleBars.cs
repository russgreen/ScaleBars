using Autodesk.Revit.DB;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScaleBars;
internal static class ScaleBars
{
    public static void SetScaleBars(ViewSheet sheet, Document doc)
    {
        FilteredElementCollector title_block_instances = new FilteredElementCollector(doc, sheet.Id)
            .OfCategory(BuiltInCategory.OST_TitleBlocks)
            .OfClass(typeof(FamilyInstance));

        foreach (FamilyInstance tb in title_block_instances)
        {
            var scale = ParseScale(sheet.get_Parameter(BuiltInParameter.SHEET_SCALE).AsString());

            if(scale == "As indicated")
            {
                return;
            }

            var paramList = new List<Parameter>();
            ParameterSet paramSet = tb.Parameters;
            IEnumerator enumerator = paramSet.GetEnumerator();
            enumerator.Reset();

            while (enumerator.MoveNext())
                paramList.Add(enumerator.Current as Parameter);

            foreach (Parameter param in paramList)
            {
                if (param.Definition.Name.StartsWith("1-") == true | param.Definition.Name.StartsWith("NTS") == true)
                {
                    // check to see if the scale bar needs to be changed.
                    if (param.Definition.Name == scale)
                    {
                        if (param.AsInteger() == 1)
                        {
                            return;
                        }
                    }
                }
            }

            foreach (Parameter param in paramList)
            {
                if (param.Definition.Name.StartsWith("1-") == true | param.Definition.Name.StartsWith("NTS") == true)
                {
                    using (var t = new Transaction(doc, "Modify Scale Parameter"))
                    {
                        t.Start();
                        param.Set(0);
                        if (param.Definition.Name == scale)
                        {
                            param.Set(1);
                        }
                        t.Commit();
                    }
                }
            }
        }
    }

    private static string ParseScale(string scale)
    {
        var regWhitespace = new Regex(@"\s");
        scale = regWhitespace.Replace(scale, string.Empty);

        string returnValue = string.Empty;
        switch (scale ?? "")
        {
            case "Asindicated":
                returnValue = "As indicated";
                break;

            case "1:5000":
                returnValue = "1-5000";
                break;

            case "1:2500":
                returnValue = "1-2500";
                break;

            case "1:2000":
                returnValue = "1-2000";
                break;

            case "1:1250":
                returnValue = "1-1250";
                break;

            case "1:1000":
                returnValue = "1-1000";
                break;

            case "1:500":
                returnValue = "1-0500";
                break;

            case "1:250":
                returnValue = "1-0250";
                break;

            case "1:200":
                returnValue = "1-0200";
                break;

            case "1:100":
                returnValue = "1-0100";
                break;

            case "1:50":
                returnValue = "1-0050";
                break;

            case "1:20":
                returnValue = "1-0020";
                break;

            case "1:10":
                returnValue = "1-0010";
                break;

            case "1:5":
                returnValue = "1-0005";
                break;

            case "1:2":
                returnValue = "1-0002";
                break;

            case "1:1":
                returnValue = "1-0001";
                break;

            default:
                returnValue = "NTS";
                break;
        }

        return returnValue;
    }
}
