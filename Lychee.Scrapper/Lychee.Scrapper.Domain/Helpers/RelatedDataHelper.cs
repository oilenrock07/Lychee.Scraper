using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Domain.Helpers
{
    public class RelatedDataHelper
    {
        public static void SetValue(RelatedData data, string columnName, object itemValue)
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
            }
        }
    }
}
