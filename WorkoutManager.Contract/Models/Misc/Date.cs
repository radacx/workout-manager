using System;
using System.Collections.Generic;

namespace WorkoutManager.Contract.Models.Misc
{
    public class Date : IEquatable<Date>
    {
        public int Year { get; }

        public int Month { get; }

        public int Day { get; }

        public static IEqualityComparer<Date> YearMonthDayComparer { get; } = new YearMonthDayEqualityComparer();

        public Date(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public bool Equals(Date other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Year == other.Year
                && Month== other.Month
                && Day == other.Day;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType()
                && Equals((Date) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Year;
                hashCode = (hashCode * 397) ^ Month;
                hashCode = (hashCode * 397) ^ Day;

                return hashCode;
            }
        }

        private sealed class YearMonthDayEqualityComparer : IEqualityComparer<Date>
        {
            public bool Equals(Date x, Date y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x.Year == y.Year
                    && x.Month == y.Month
                    && x.Day == y.Day;
            }

            public int GetHashCode(Date obj)
            {
                unchecked
                {
                    var hashCode = obj.Year;
                    hashCode = (hashCode * 397) ^ obj.Month;
                    hashCode = (hashCode * 397) ^ obj.Day;

                    return hashCode;
                }
            }
        }
    }
}