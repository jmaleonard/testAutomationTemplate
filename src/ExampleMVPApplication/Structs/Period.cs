#region Directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#endregion

namespace ExampleMVPApplication.Structs
{
    /// <summary>
    /// Represents an accounting period, expressed as a combination of year and month
    /// </summary>
    [Serializable]
    public struct Period : IComparable<Period>
    {
        #region Fields

        private int _period;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Period" /> struct.
        /// </summary>
        /// <param name="date">The date.</param>
        public Period(DateTime date)
        {
            this._period = (date.Year * 100) + date.Month;

            if (!Period.IsValidPeriod(this._period))
            {
                throw new ArgumentException("period value is not valid");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Period"/> struct.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <exception cref="System.ArgumentException">Period value is not valid</exception>
        public Period(int year, int month)
        {
            this._period = (year * 100) + month;

            if (this._period < Period.MinValue)
            {
                this._period = Period.MinValue;
            }

            if (this._period > Period.MaxValue)
            {
                this._period = Period.MaxValue;
            }

            if (!Period.IsValidPeriod(this._period))
            {
                throw new ArgumentException("period value is not valid");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Period" /> struct.
        /// </summary>
        /// <param name="period">The period.</param>
        public Period(int period)
        {
            this._period = period;

            if (!Period.IsValidPeriod(this._period))
            {
                throw new ArgumentException("period value is not valid");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Period" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Period(string value)
        {
            int result;
            string pattern = @"^\d{4} Q[1234]$";
            if (Regex.IsMatch(value, pattern))
            {
                this._period = ConvertQuarterStringToInt(value);

                if (!Period.IsValidPeriod(this._period))
                {
                    throw new ArgumentException("Period value is not valid");
                }
            }
            else if (int.TryParse(value, out result))
            {
                this._period = result;

                if (!Period.IsValidPeriod(this._period))
                {
                    throw new ArgumentException("Period value is not valid");
                }
            }
            else
            {
                throw new ArgumentException("String is not a period");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default date.
        /// </summary>
        /// <value>
        /// The default date.
        /// </value>
        public static DateTime DefaultDate
        {
            get
            {
                return new Period(Period.MinValue).ToPeriodStartDate();
            }
        }

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <value>
        /// The min value.
        /// </value>
        public static int MinValue
        {
            get
            {
                return 190001;
            }
        }

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <value>
        /// The max value.
        /// </value>
        public static int MaxValue
        {
            get
            {
                return 999912;
            }
        }

        /// <summary>
        /// Gets the months.
        /// </summary>
        /// <value>
        /// The months.
        /// </value>
        public int Months
        {
            get
            {
                return this.Value % 100;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value
        {
            get
            {
                if (this._period == default(int))
                {
                    this._period = MinValue;
                }

                return this._period;
            }
        }

        /// <summary>
        /// Gets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year
        {
            get
            {
                return this.Value / 100;
            }
        }

        /// <summary>
        /// Gets the quarter.
        /// </summary>
        /// <value>
        /// The quarter.
        /// </value>
        public string Quarter
        {
            get
            {
                string quarter = string.Empty;
                switch (this.Months)
                {
                    case 1:
                        quarter = "Q1";
                        break;
                    case 4:
                        quarter = "Q2";
                        break;
                    case 7:
                        quarter = "Q3";
                        break;
                    case 10:
                        quarter = "Q4";
                        break;
                }

                return quarter;
            }
        }

        /// <summary>
        /// Gets the display string.
        /// </summary>
        /// <value>
        /// The display string.
        /// </value>
        public string DisplayString
        {
            get
            {
                return this.ToDisplayString();
            }
        }

        #endregion

        #region Methods

        #region Operator Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="ITRS.Common.Structs.Period"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Period(int value)
        {
            return new Period(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ITRS.Common.Structs.Period"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Period(string value)
        {
            return new Period(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DataTypeTest.Period"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator int(Period period)
        {
            return period.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DataTypeTest.Period"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator string(Period period)
        {
            return period.ToDisplayString();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Period period1, Period period2)
        {
            return Period.Equals(period1, period2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Period period1, Period period2)
        {
            return !Period.Equals(period1, period2);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Period period1, Period period2)
        {
            return period1.Value < period2.Value;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Period period1, Period period2)
        {
            return period1.Value > period2.Value;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Period period1, Period period2)
        {
            return period1.Value <= period2.Value;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Period period1, Period period2)
        {
            return period1.Value >= period2.Value;
        }

        #endregion

        /// <summary>
        /// Compares the two periods.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns></returns>
        public static bool Equals(Period period1, Period period2)
        {
            return period1.Equals(period2);
        }

        /// <summary>
        /// Determines whether [is valid period] [the specified period].
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns>
        ///   <c>true</c> if [is valid period] [the specified period]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidPeriod(int period)
        {
            // Default value for an empty period
            if (period == 0)
            {
                return true;
            }

            // check length
            if (period.ToString().Length != 6)
            {
                return false;
            }

            // check months are in quarterly format (1, 4, 7, 10)
            if ((period % 100) == 0 || (period % 100) % 3 != 1)
            {
                return false;
            }

            // check year range
            if (period < Period.MinValue || period > Period.MaxValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether [is valid period] [the specified period].
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns>
        ///   <c>true</c> if [is valid period] [the specified period]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidPeriod(string period)
        {
            // null value
            if (string.IsNullOrEmpty(period))
            {
                return true;
            }

            period = period.Replace(@"/", string.Empty);

            // check length
            if (period.ToString().Length != 6)
            {
                return false;
            }

            int intPeriod;

            if (!int.TryParse(period, out intPeriod))
            {
                return false;
            }

            if (!Period.IsValidPeriod(intPeriod))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calculate the difference in months.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns></returns>
        public static int MonthDifference(Period period1, Period period2)
        {
            return period1.MonthDifference(period2);
        }

        /// <summary>
        /// Calculate the difference in months.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public int MonthDifference(Period target)
        {
            int currentMonths = (this.Value / 100 * 12) + (this.Value % 100);
            int targetMonths = (target.Value / 100 * 12) + (target.Value % 100);
            return targetMonths - currentMonths;
        }

        /// <summary>
        /// Years the difference.
        /// </summary>
        /// <param name="period1">The period1.</param>
        /// <param name="period2">The period2.</param>
        /// <returns></returns>
        public static int YearDifference(Period period1, Period period2)
        {
            return period1.YearDifference(period2);
        }

        /// <summary>
        /// Years the difference.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public int YearDifference(Period target)
        {
            int monthDifference = this.MonthDifference(target);
            return (int)Math.Floor((double)monthDifference / 12.0);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Period AddQuarters(int value)
        {
            DateTime currentPeriodDate = new DateTime(this.Value / 100, this.Value % 100, 1);
            DateTime newPeriodDate = currentPeriodDate.AddMonths(3 * value);
            return new Period((newPeriodDate.Year * 100) + newPeriodDate.Month);
        }

        /// <summary>
        /// Adds the years.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Period AddYears(int value)
        {
            if (value > int.MaxValue / 100)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return new Period(this.Value + (value * 100));
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
        /// </returns>
        public int CompareTo(Period other)
        {
            return this.Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Compares the period to the current period.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Equals(Period value)
        {
            return this.Value == value.Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// To the period start date.
        /// </summary>
        /// <returns></returns>
        public DateTime ToPeriodStartDate()
        {
            return new DateTime(this.Value / 100, this.Value % 100, 1);
        }

        /// <summary>
        /// To the period end date.
        /// </summary>
        /// <returns></returns>
        public DateTime ToPeriodEndDate()
        {
            return this.ToPeriodStartDate().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// To the quarter end date.
        /// </summary>
        /// <returns></returns>
        public DateTime ToQuarterEndDate()
        {
            return this.ToPeriodStartDate().AddMonths(3).AddDays(-1);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return this.ToPeriodStartDate().ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates the period list.
        /// </summary>
        /// <param name="startPeriod">The start period.</param>
        /// <param name="endPeriod">The end period.</param>
        /// <returns></returns>
        public static IList<Period> CreatePeriodList(Period startPeriod, Period endPeriod)
        {
            if (startPeriod > endPeriod)
            {
                var temp = startPeriod;
                startPeriod = endPeriod;
                endPeriod = temp;
            }
            else if (startPeriod == endPeriod)
            {
                return new List<Period>() { startPeriod };
            }

            IList<Period> periodList = new List<Period>();
            for (Period period = startPeriod; period <= endPeriod; period = period.AddQuarters(1))
            {
                periodList.Add(period);
            }

            return periodList;
        }

        /// <summary>
        /// To the display string.
        /// </summary>
        /// <returns></returns>
        public string ToDisplayString()
        {
            return string.Format("{0} {1}", this.Year, this.Quarter);
        }

        /// <summary>
        /// Converts the quarter string to integer.
        /// </summary>
        /// <param name="value">The value.</param>
        private static int ConvertQuarterStringToInt(string value)
        {
            int year;
            int month = 0;
            switch (value.Substring(6, 1))
            {
                case "1":
                    month = 1;
                    break;
                case "2":
                    month = 4;
                    break;
                case "3":
                    month = 7;
                    break;
                case "4":
                    month = 10;
                    break;
                default:
                    throw new ArgumentException("Period value is not valid");
            }

            if (!int.TryParse(value.Substring(0, 4), out year))
            {
                throw new ArgumentException("Period value is not valid");
            }
            else
            {
                return (year * 100) + month;
            }
        }

        /// <summary>
        /// Gets the quarter period corresponding to the date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static Period GetQuarterPeriod(DateTime date)
        {
            return new Period(date.Year, Period.GetQuarterMonth(date));
        }

        /// <summary>
        /// Gets the quarter month closest to the date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static int GetQuarterMonth(DateTime date)
        {
            if (date.Month <= 3)
            {
                return 1; // Start Month of the first Quarter
            }
            else if (date.Month <= 6)
            {
                return 4; // Start Month of the second Quarter
            }
            else if (date.Month <= 9)
            {
                return 7; // Start Month of the third Quarter
            }
            else
            {
                return 10; // Start Month of the fourth Quarter
            }
        }

        #endregion
    }
}
