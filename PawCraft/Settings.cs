namespace PawCraft
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Application global settings
    /// </summary>
    internal static class Settings
    {
        /// <summary>
        /// Settings file path
        /// </summary>
        private static readonly string settingFile = Path.Combine(Path.GetDirectoryName(typeof(Settings).Assembly.Location), "settings.conf");

        /// <summary>
        /// Setting file values
        /// </summary>
        private static readonly Dictionary<string, object> values = new Dictionary<string, object>();

        /// <summary>
        /// Get integer value
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">Saved value</param>
        /// <returns><see langword="true"/> on success</returns>
        internal static bool GetValue(string key, out int value)
        {
            if (Settings.values.TryGetValue(key, out object gotValue) && gotValue is int integerValue)
            {
                value = integerValue;
                return true;
            }

            value = int.MinValue;
            return false;
        }

        /// <summary>
        /// Get double value
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">Saved value</param>
        /// <returns><see langword="true"/> on success</returns>
        internal static bool GetValue(string key, out double value)
        {
            if (Settings.values.TryGetValue(key, out object gotValue) && gotValue is double doubleValue)
            {
                value = doubleValue;
                return true;
            }

            value = double.NaN;
            return false;
        }

        /// <summary>
        /// Get string value
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">Saved value</param>
        /// <returns><see langword="true"/> on success</returns>
        internal static bool GetValue(string key, out string value)
        {
            if (Settings.values.TryGetValue(key, out object gotValue) && gotValue is string stringValue)
            {
                value = stringValue;
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Load application settings
        /// </summary>
        internal static void LoadSettings()
        {
            try
            {
                if (File.Exists(Settings.settingFile))
                {
                    foreach (string line in File.ReadLines(Settings.settingFile))
                    {
                        string key = line.Substring(0, line.IndexOf('[')).Trim();
                        string type = line.Substring(0, line.IndexOf(']')).Substring(line.IndexOf('[') + 1).Trim().ToLower();
                        string value = line.Substring(line.IndexOf('=') + 1).Trim();

                        int tempInt = 0;
                        double tempDouble = 0.0;

                        switch (type)
                        {
                            case "int32":

                                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt))
                                {
                                    Settings.SetValue(key, tempInt);
                                }

                                break;

                            case "string":
                                Settings.SetValue(key, value);
                                break;

                            case "double":

                                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out tempDouble))
                                {
                                    Settings.SetValue(key, tempDouble);
                                }

                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// Save application settings
        /// </summary>
        internal static void SaveSettings()
        {
            try
            {
                using (FileStream stream = File.Open(Settings.settingFile, FileMode.Create))
                {
                    using (TextWriter writer = new StreamWriter(stream))
                    {
                        foreach (KeyValuePair<string, object> value in Settings.values)
                        {
                            if (value.Value != null && !string.IsNullOrWhiteSpace(value.Key))
                            {
                                TypeConverter converter = new TypeConverter();

                                writer.WriteLine(
                                    string.Format(
                                        "{0}[{1}] = {2}",
                                        value.Key,
                                        value.Value.GetType().Name,
                                        converter.ConvertToInvariantString(value.Value)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// Set integer value
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">The value</param>
        internal static void SetValue(string key, int value)
        {
            Settings.SetValue(key, (object)value);
        }

        /// <summary>
        /// Set double value
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">The value</param>
        internal static void SetValue(string key, double value)
        {
            Settings.SetValue(key, (object)value);
        }

        /// <summary>
        /// Set string value
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">The value</param>
        internal static void SetValue(string key, string value)
        {
            Settings.SetValue(key, (object)value);
        }

        /// <summary>
        /// Set value
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">The value</param>
        private static void SetValue(string key, object value)
        {
            if (Settings.values.ContainsKey(key))
            {
                Settings.values[key] = value;
            }
            else
            {
                Settings.values.Add(key, value);
            }
        }
    }
}