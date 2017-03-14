using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Data.SQLite {

    /// <summary>
    /// A very cut-down version of my usual DB helper class
    /// </summary>

    internal static class DBHelper {

        public static T GetDBValue<T>(object dbValue, T typeDefault) {

            if (dbValue == null || dbValue == DBNull.Value) {
                return typeDefault;
            }

            // Special case for Guids that are held on the DB as a string
            if (typeof(T).Name == "Guid" && dbValue is string) {
                Guid guid = new Guid(dbValue as string);
                return (T)(guid as object);
            }

            // This case covers enumerations that are stored as strings
            if (typeof(T).BaseType.Name == "Enum" && dbValue is string) {
                try {
                    // Can't do TryParse ...
                    T enumValue = (T)Enum.Parse(typeof(T), dbValue as string);
                    return enumValue;
                } catch {
                    return typeDefault;
                }
            }

            // This handles the case where a bool is represented either by an integer (0 = false)
            // or a parse-able string 
            if (typeof(T) == typeof(bool)) {
                if (dbValue.GetType() == typeof(int)) {
                    return (T)(((int)dbValue != 0) as object);
                } else if (dbValue.GetType() == typeof(double)) {
                    return (T)(((double)dbValue != 0) as object);
                } else if (dbValue.GetType() == typeof(decimal)) {
                    return (T)(((decimal)dbValue != 0) as object);
                } else {
                    bool bTrue = false;
                    if (Boolean.TryParse(dbValue.ToString(), out bTrue)) {
                        return (T)(bTrue as object);
                    } else {
                        switch (dbValue.ToString().ToLower()) {
                            case "yes":
                            case "ok":
                            case "1":
                                bTrue = true;
                                break;
                            case "no":
                            case "0":
                                bTrue = false;
                                break;
                            default:
                                return typeDefault;
                        }

                        return (T)(bTrue as object);
                    }
                }
            }



            if (dbValue.GetType() != typeof(T)) {
                if (typeof(T) == typeof(int)) {
                    return (T)(int.Parse(dbValue.ToString()) as object);
                } else if (typeof(T) == typeof(Int16)) {
                    return (T)(Int16.Parse(dbValue.ToString()) as object);
                } else if (typeof(T) == typeof(Int32)) {
                    return (T)(Int32.Parse(dbValue.ToString()) as object);
                } else if (typeof(T) == typeof(Int64)) {
                    return (T)(Int64.Parse(dbValue.ToString()) as object);
                } else if (typeof(T) == typeof(double)) {
                    return (T)(double.Parse(dbValue.ToString()) as object);
                } else if (typeof(T) == typeof(float)) {
                    return (T)(float.Parse(dbValue.ToString()) as object);
                } else if (typeof(T) == typeof(decimal)) {
                    return (T)(decimal.Parse(dbValue.ToString(), System.Globalization.NumberStyles.Float) as object);
                } else if (typeof(T) == typeof(long)) {
                    return (T)(long.Parse(dbValue.ToString()) as object);
                } else if (typeof(T) == typeof(DateTime?)) {
                    if (dbValue == null) return typeDefault;
                    DateTime dt;
                    if (!DateTime.TryParse(dbValue.ToString(), out dt)) {
                        return typeDefault;
                    }
                    return (T)(dt as object);
                } else if (typeof(T) == typeof(DateTime)) {
                    DateTime dt;
                    if (!DateTime.TryParse(dbValue.ToString(), out dt)) {
                        decimal d;
                        if (!decimal.TryParse(dbValue.ToString(), out d)) {
                            double n;
                            if (!double.TryParse(dbValue.ToString(), out n)) {
                                return typeDefault;
                            }
                            dt = new DateTime((long)n);
                        }
                        dt = new DateTime((long)d);
                    }
                    return (T)(dt as object);
                } else if (typeof(T) == typeof(string)) {
                    if (dbValue.GetType() == typeof(DateTime)) {
                        return (T)(((DateTime)dbValue).Ticks.ToString() as object);
                    } else if (dbValue.GetType() == typeof(byte[])) {
                        return (T)(BitConverter.ToString((byte[])dbValue) as object);
                    } else {
                        return (T)(dbValue.ToString() as object);
                    }

                }
            }

            return (T)dbValue;


        }

    }
}
