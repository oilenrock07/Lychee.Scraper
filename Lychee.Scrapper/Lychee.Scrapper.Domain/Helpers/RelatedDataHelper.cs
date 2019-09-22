using System.Collections.Generic;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Domain.Helpers
{
    public class RelatedDataHelper
    {
        /// <summary>
        /// Maps the value to it's column in Related data
        /// so if String1 = Image, String2 = Image. Up to String10.
        /// If item.Name == Image, we check if String1 is empty, if yes we put the data there. if not, check String2 and so on until it got a place to store
        /// </summary>
        /// <param name="data">The object that we want to populate</param>
        /// <param name="definition">Example Image</param>
        /// <param name="columnDictionary">Holds the column definition value.</param>
        /// <param name="itemValue">the value that we want to store in the related data.</param>
        public static void SetValue(RelatedData data, string definition, Dictionary<(string, string), string> columnDictionary, object itemValue)
        {
            //using the passed definition, we get the RelatedData definition from the dictionary.
            //using this definition we can compare to each property and set the value accordingly

            //Strings
            string value;
            if (string.IsNullOrEmpty(data.String1) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String1"), out value) && value == definition)
            {
                data.String1 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String2) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String2"), out value) && value == definition)
            {
                data.String2 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String3) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String3"), out value) && value == definition)
            {
                data.String3 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String4) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String4"), out value) && value == definition)
            {
                data.String4 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String5) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String5"), out value) && value == definition)
            {
                data.String5 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String6) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String6"), out value) && value == definition)
            {
                data.String6 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String7) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String7"), out value) && value == definition)
            {
                data.String7 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String8) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String8"), out value) && value == definition)
            {
                data.String8 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String9) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String9"), out value) && value == definition)
            {
                data.String9 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String10) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String10"), out value) && value == definition)
            {
                data.String10 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String11) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String11"), out value) && value == definition)
            {
                data.String11 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String12) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String12"), out value) && value == definition)
            {
                data.String12 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String13) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String13"), out value) && value == definition)
            {
                data.String13 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String14) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String14"), out value) && value == definition)
            {
                data.String14 = itemValue.ToString();
                return;
            }
            
            if (string.IsNullOrEmpty(data.String15) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String15"), out value) && value == definition)
            {
                data.String15 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String16) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String16"), out value) && value == definition)
            {
                data.String16 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String17) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String17"), out value) && value == definition)
            {
                data.String17 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String18) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String18"), out value) && value == definition)
            {
                data.String18 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String19) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String19"), out value) && value == definition)
            {
                data.String19 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String20) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String20"), out value) && value == definition)
            {
                data.String20 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String21) &&
            columnDictionary.TryGetValue((nameof(RelatedData), "String21"), out value) && value == definition)
            {
                data.String21 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String22) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String22"), out value) && value == definition)
            {
                data.String22 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String23) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String23"), out value) && value == definition)
            {
                data.String23 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String24) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String24"), out value) && value == definition)
            {
                data.String24 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String25) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String25"), out value) && value == definition)
            {
                data.String25 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String26) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String26"), out value) && value == definition)
            {
                data.String26 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String27) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String27"), out value) && value == definition)
            {
                data.String27 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String28) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String28"), out value) && value == definition)
            {
                data.String28 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String29) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String29"), out value) && value == definition)
            {
                data.String29 = itemValue.ToString();
                return;
            }

            if (string.IsNullOrEmpty(data.String30) &&
                columnDictionary.TryGetValue((nameof(RelatedData), "String30"), out value) && value == definition)
            {
                data.String30 = itemValue.ToString();
                return;
            }

            //Decimals
            if (data.Decimal1 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Decimal1"), out value) && value == definition)
            {
                data.Decimal1 = itemValue.ToDecimal();
                return;
            }

            if (data.Decimal2 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Decimal2"), out value) && value == definition)
            {
                data.Decimal2 = itemValue.ToDecimal();
                return;
            }

            if (data.Decimal3 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Decimal3"), out value) && value == definition)
            {
                data.Decimal3 = itemValue.ToDecimal();
                return;
            }

            if (data.Decimal4 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Decimal4"), out value) && value == definition)
            {
                data.Decimal4 = itemValue.ToDecimal();
                return;
            }

            if (data.Decimal5 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Decimal5"), out value) && value == definition)
            {
                data.Decimal5 = itemValue.ToDecimal();
                return;
            }


            //Ints
            if (data.Int1 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Int1"), out value) && value == definition)
            {
                data.Int1 = itemValue.ToInt();
                return;
            }

            if (data.Int2 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Int2"), out value) && value == definition)
            {
                data.Int2 = itemValue.ToInt();
                return;
            }

            if (data.Int3 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Int3"), out value) && value == definition)
            {
                data.Int3 = itemValue.ToInt();
                return;
            }

            if (data.Int4 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Int4"), out value) && value == definition)
            {
                data.Int4 = itemValue.ToInt();
                return;
            }

            if (data.Int5 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Int5"), out value) && value == definition)
            {
                data.Int5 = itemValue.ToInt();
                return;
            }


            //Bools
            if (data.Bool1 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Bool1"), out value) && value == definition)
            {
                data.Bool1 = itemValue.ToBool();
                return;
            }

            if (data.Bool2 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Bool2"), out value) && value == definition)
            {
                data.Bool2 = itemValue.ToBool();
                return;
            }

            if (data.Bool3 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Bool3"), out value) && value == definition)
            {
                data.Bool3 = itemValue.ToBool();
                return;
            }

            if (data.Bool4 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Bool4"), out value) && value == definition)
            {
                data.Bool4 = itemValue.ToBool();
                return;
            }

            if (data.Bool5 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Bool5"), out value) && value == definition)
            {
                data.Bool5 = itemValue.ToBool();
                return;
            }


            //Dates
            if (data.Date1 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Date1"), out value) && value == definition)
            {
                data.Date1 = itemValue.ToDateTime();
                return;
            }

            if (data.Date2 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Date2"), out value) && value == definition)
            {
                data.Date2 = itemValue.ToDateTime();
                return;
            }

            if (data.Date3 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Date3"), out value) && value == definition)
            {
                data.Date3 = itemValue.ToDateTime();
                return;
            }

            if (data.Date4 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Date4"), out value) && value == definition)
            {
                data.Date4 = itemValue.ToDateTime();
                return;
            }

            if (data.Date5 == null &&
                columnDictionary.TryGetValue((nameof(RelatedData), "Date5"), out value) && value == definition)
            {
                data.Date5 = itemValue.ToDateTime();
                return;
            }
        }
    }
}
