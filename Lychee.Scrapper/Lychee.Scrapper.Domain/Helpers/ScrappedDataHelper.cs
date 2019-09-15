using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Domain.Helpers
{
    public static class ScrappedDataHelper
    {
        public static void SetValue(ScrappedData data, string columnName, object itemValue)
        {
            switch (columnName.ToLower())
            {
                //Strings
                case "string1":
                    data.String1 = itemValue.ToString();
                    break;
                case "string2":
                    data.String2 = itemValue.ToString();
                    break;
                case "string3":
                    data.String3 = itemValue.ToString();
                    break;
                case "string4":
                    data.String4 = itemValue.ToString();
                    break;
                case "string5":
                    data.String5 = itemValue.ToString();
                    break;
                case "string6":
                    data.String6 = itemValue.ToString();
                    break;
                case "string7":
                    data.String7 = itemValue.ToString();
                    break;
                case "string8":
                    data.String8 = itemValue.ToString();
                    break;
                case "string9":
                    data.String9 = itemValue.ToString();
                    break;
                case "string10":
                    data.String10 = itemValue.ToString();
                    break;
                case "string11":
                    data.String11 = itemValue.ToString();
                    break;
                case "string12":
                    data.String12 = itemValue.ToString();
                    break;
                case "string13":
                    data.String13 = itemValue.ToString();
                    break;
                case "string14":
                    data.String14 = itemValue.ToString();
                    break;
                case "string15":
                    data.String15 = itemValue.ToString();
                    break;
                case "string16":
                    data.String16 = itemValue.ToString();
                    break;
                case "string17":
                    data.String17 = itemValue.ToString();
                    break;
                case "string18":
                    data.String18 = itemValue.ToString();
                    break;
                case "string19":
                    data.String19 = itemValue.ToString();
                    break;
                case "string20":
                    data.String20 = itemValue.ToString();
                    break;

                //Ints
                case "int1":
                    data.Int1 = itemValue.ToInt();
                    break;
                case "int2":
                    data.Int2 = itemValue.ToInt();
                    break;
                case "int3":
                    data.Int3 = itemValue.ToInt();
                    break;
                case "int4":
                    data.Int4 = itemValue.ToInt();
                    break;
                case "int5":
                    data.Int5 = itemValue.ToInt();
                    break;
                case "int6":
                    data.Int6 = itemValue.ToInt();
                    break;
                case "int7":
                    data.Int7 = itemValue.ToInt();
                    break;
                case "int8":
                    data.Int8 = itemValue.ToInt();
                    break;
                case "int9":
                    data.Int9 = itemValue.ToInt();
                    break;
                case "int10":
                    data.Int10 = itemValue.ToInt();
                    break;

                //Dates
                case "date1":
                    data.Date1 = itemValue.ToDateTime();
                    break;
                case "date2":
                    data.Date2 = itemValue.ToDateTime();
                    break;
                case "date3":
                    data.Date3 = itemValue.ToDateTime();
                    break;
                case "date4":
                    data.Date4 = itemValue.ToDateTime();
                    break;
                case "date5":
                    data.Date5 = itemValue.ToDateTime();
                    break;
                case "date6":
                    data.Date6 = itemValue.ToDateTime();
                    break;
                case "date7":
                    data.Date7 = itemValue.ToDateTime();
                    break;
                case "date8":
                    data.Date8 = itemValue.ToDateTime();
                    break;
                case "date9":
                    data.Date9 = itemValue.ToDateTime();
                    break;
                case "date10":
                    data.Date10 = itemValue.ToDateTime();
                    break;

                //Booleans
                case "bool1":
                    data.Bool1 = itemValue.ToBool();
                    break;
                case "bool2":
                    data.Bool2 = itemValue.ToBool();
                    break;
                case "bool3":
                    data.Bool3 = itemValue.ToBool();
                    break;
                case "bool4":
                    data.Bool4 = itemValue.ToBool();
                    break;
                case "bool5":
                    data.Bool5 = itemValue.ToBool();
                    break;
                case "bool6":
                    data.Bool6 = itemValue.ToBool();
                    break;
                case "bool7":
                    data.Bool7 = itemValue.ToBool();
                    break;
                case "bool8":
                    data.Bool8 = itemValue.ToBool();
                    break;
                case "bool9":
                    data.Bool9 = itemValue.ToBool();
                    break;
                case "bool10":
                    data.Bool10 = itemValue.ToBool();
                    break;

                //Decimals
                case "decimal1":
                    data.Decimal1 = itemValue.ToDecimal();
                    break;
                case "decimal2":
                    data.Decimal2 = itemValue.ToDecimal();
                    break;
                case "decimal3":
                    data.Decimal3 = itemValue.ToDecimal();
                    break;
                case "decimal4":
                    data.Decimal4 = itemValue.ToDecimal();
                    break;
                case "decimal5":
                    data.Decimal5 = itemValue.ToDecimal();
                    break;
                case "decimal6":
                    data.Decimal6 = itemValue.ToDecimal();
                    break;
                case "decimal7":
                    data.Decimal7 = itemValue.ToDecimal();
                    break;
                case "decimal8":
                    data.Decimal8 = itemValue.ToDecimal();
                    break;
                case "decimal9":
                    data.Decimal9 = itemValue.ToDecimal();
                    break;
                case "decimal10":
                    data.Decimal10 = itemValue.ToDecimal();
                    break;
            }
        }
    }
}
