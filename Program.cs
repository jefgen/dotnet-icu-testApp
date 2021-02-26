using System;
using System.Reflection;
using System.Globalization;

namespace testApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool usingNls = true;
            Type globalizationMode = Type.GetType("System.Globalization.GlobalizationMode");
            if (globalizationMode != null)
            {
                MethodInfo methodInfo = globalizationMode.GetProperty("UseNls", BindingFlags.NonPublic | BindingFlags.Static)?.GetMethod;
                if (methodInfo != null)
                {
                    usingNls = (bool)methodInfo.Invoke(null, null);
                }
                else
                {
                    Console.WriteLine("Failed to get methodInfo.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Failed to get globalizationMode.");
                return;
            }

            Console.WriteLine("Using NLS: " + usingNls);

            if (!usingNls)
            {
                int icuVersion = 0;
                try
                {
                    Type interopGlobalization = Type.GetType("Interop+Globalization", throwOnError: true);
                    if (interopGlobalization != null)
                    {
                        MethodInfo methodInfo = interopGlobalization.GetMethod("GetICUVersion", BindingFlags.NonPublic | BindingFlags.Static);
                        if (methodInfo != null)
                        {
                            icuVersion = (int)methodInfo.Invoke(null, null);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to get ICU Version.");
                    return;
                }

                Version ver = new Version(icuVersion >> 24, (icuVersion >> 16) & 0xFF, (icuVersion >> 8) & 0xFF, icuVersion & 0xFF);
                Console.WriteLine("ICU Version: " + ver);
            }

            Console.WriteLine("CultureInfo.CurrentCulture: " + CultureInfo.CurrentCulture.ToString());

            string queryLocale = CultureInfo.CurrentCulture.ToString();

            if (args.Length == 1)
            {
                queryLocale = args[0];
            }

            Console.WriteLine();
            Console.WriteLine("Outputting info on: " + queryLocale);
            var testCulture = new CultureInfo(queryLocale);

            Console.WriteLine();
            Console.WriteLine("{0,-31}{1,-47}", "PROPERTY", "LOCALE");
            Console.WriteLine("{0,-31}{1,-47}", "Name", testCulture.Name);
            Console.WriteLine("{0,-31}{1,-47}", "Parent", testCulture.Parent);
            Console.WriteLine("{0,-31}{1,-47}", "IetfLanguageTag ", testCulture.IetfLanguageTag);

            Console.WriteLine("{0,-31}{1,-47}", "DisplayName", testCulture.DisplayName);
            Console.WriteLine("{0,-31}{1,-47}", "EnglishName", testCulture.EnglishName);
            Console.WriteLine("{0,-31}{1,-47}", "NativeName", testCulture.NativeName);

            Console.WriteLine("{0,-31}{1,-47}", "Calendar", testCulture.Calendar);
            Console.WriteLine("{0,-31}{1,-47}", "CompareInfo", testCulture.CompareInfo);
            Console.WriteLine("{0,-31}{1,-47}", "CultureType", testCulture.CultureTypes);
            Console.WriteLine("{0,-31}{1,-47}", "IsNeutralCulture", testCulture.IsNeutralCulture);
            Console.WriteLine("{0,-31}{1,-47}", "IsReadOnly", testCulture.IsReadOnly);
            Console.WriteLine("{0,-31}0x{1,-47:X4}", "LCID", testCulture.LCID);
            Console.WriteLine("{0,-31}{1,-47}", "TextInfo", testCulture.TextInfo);
            Console.WriteLine("{0,-31}{1,-47}", "ThreeLetterISOLanguageName", testCulture.ThreeLetterISOLanguageName);
            Console.WriteLine("{0,-31}{1,-47}", "ThreeLetterWindowsLanguageName", testCulture.ThreeLetterWindowsLanguageName);
            Console.WriteLine("{0,-31}{1,-47}", "TwoLetterISOLanguageName", testCulture.TwoLetterISOLanguageName);
            Console.WriteLine();

            // Output DateTimeFormat.
            Console.WriteLine("DateTimeFormatInfo:");
            Console.WriteLine();
            PropertyInfo[] props = testCulture.DateTimeFormat.GetType().GetProperties();
            DateTime value = new DateTime(2012, 5, 28, 11, 35, 0);

            foreach (var prop in props)
            {
                // Is this a format pattern-related property?
                if (prop.Name.Contains("Pattern"))
                {
                    string fmt = prop.GetValue(testCulture.DateTimeFormat, null).ToString();
                    Console.WriteLine("{0,-33} {1,-36} Example: {3}", prop.Name, fmt, "", value.ToString(fmt));
                }
            }
            Console.WriteLine();

            NumberFormatInfo nfi = testCulture.NumberFormat;
            Console.WriteLine("NumberFormatInfo:");
            Console.WriteLine();
            Console.WriteLine("{0,-31}{1,-47}", "CurrencyDecimalDigits", nfi.CurrencyDecimalDigits);
            Console.WriteLine("{0,-31}{1,-47}", "CurrencyDecimalSeparator", nfi.CurrencyDecimalSeparator);
            Console.WriteLine("{0,-31}{1,-47}", "CurrencyGroupSeparator", nfi.CurrencyGroupSeparator);
            Console.WriteLine("{0,-31}{1,-47}", "CurrencyGroupSizes", string.Join(";", nfi.CurrencyGroupSizes));
            Console.WriteLine("{0,-31}{1,-47}", "CurrencyNegativePattern", nfi.CurrencyNegativePattern);
            Console.WriteLine("{0,-31}{1,-47}", "CurrencyPositivePattern", nfi.CurrencyPositivePattern);
            Console.WriteLine("{0,-31}{1,-47}", "CurrencySymbol", nfi.CurrencySymbol);
            Console.WriteLine("{0,-31}{1,-47}", "DigitSubstitution", nfi.DigitSubstitution);
            Console.WriteLine("{0,-31}{1,-47}", "NaNSymbol", nfi.NaNSymbol);
            Console.WriteLine("{0,-31}{1,-47}", "NativeDigits", string.Join(",", nfi.NativeDigits));
            Console.WriteLine("{0,-31}{1,-47}", "NegativeInfinitySymbol", nfi.NegativeInfinitySymbol);
            Console.WriteLine("{0,-31}{1,-47}", "NegativeSign", nfi.NegativeSign);
            Console.WriteLine("{0,-31}{1,-47}", "NumberDecimalDigits", nfi.NumberDecimalDigits);
            Console.WriteLine("{0,-31}{1,-47}", "NumberDecimalSeparator", nfi.NumberDecimalSeparator);
            Console.WriteLine("{0,-31}{1,-47}", "NumberGroupSeparator", nfi.NumberGroupSeparator);
            Console.WriteLine("{0,-31}{1,-47}", "NumberGroupSizes", string.Join(";", nfi.NumberGroupSizes));
            Console.WriteLine("{0,-31}{1,-47}", "NumberNegativePattern", nfi.NumberNegativePattern);
            Console.WriteLine("{0,-31}{1,-47}", "PercentDecimalDigits", nfi.PercentDecimalDigits);
            Console.WriteLine("{0,-31}{1,-47}", "PercentDecimalSeparator", nfi.PercentDecimalSeparator);
            Console.WriteLine("{0,-31}{1,-47}", "PercentGroupSeparator", nfi.PercentGroupSeparator);
            Console.WriteLine("{0,-31}{1,-47}", "PercentGroupSizes", string.Join(";", nfi.PercentGroupSizes));
            Console.WriteLine("{0,-31}{1,-47}", "PercentNegativePattern", nfi.PercentNegativePattern);
            Console.WriteLine("{0,-31}{1,-47}", "PercentPositivePattern", nfi.PercentPositivePattern);
            Console.WriteLine("{0,-31}{1,-47}", "PercentSymbol", nfi.PercentSymbol);
            Console.WriteLine("{0,-31}{1,-47}", "PerMilleSymbol", nfi.PerMilleSymbol);
            Console.WriteLine("{0,-31}{1,-47}", "PositiveInfinitySymbol", nfi.PositiveInfinitySymbol);
            Console.WriteLine("{0,-31}{1,-47}", "PositiveSign", nfi.PositiveSign);

            Console.WriteLine();
        }
    }
}
