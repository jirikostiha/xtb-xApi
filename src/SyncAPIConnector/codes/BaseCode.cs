using System;

namespace xAPI.Codes
{
    /// <summary>
    /// Base class for all xAPI codes.
    /// </summary>
    public class BaseCode
    {
        private long code;

        /// <summary>
        /// Creates new base code object.
        /// </summary>
        /// <param name="code">Code represented as long value.</param>
        public BaseCode(long code)
        {
            this.code = code;
        }

        /// <summary>
        /// Raw code received from the API.
        /// </summary>
        public long Code
        {
            get { return code; }
            set { this.code = value; }
        }

        public static bool operator ==(BaseCode baseCode1, BaseCode baseCode2)
        {
            if(ReferenceEquals(baseCode1, baseCode2))
                return true;

            if((object)baseCode1 == null || (object)baseCode2 == null)
                return false;

            return (baseCode1.Code == baseCode2.Code);
        }

        public static bool operator !=(BaseCode baseCode1, BaseCode baseCode2)
        {
            return !(baseCode1 == baseCode2);
        }

        public override bool Equals(object target)
        {
            if (target == null)
                return false;

            BaseCode baseCode = target as BaseCode;
            if ((object)baseCode == null)
                return false;

            return (this.Code == baseCode.Code);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [Obsolete("Use Code instead")]
        public long LongValue
        {
            get { return code; }
            set { this.code = value; }
        }

        [Obsolete("Use Code instead")]
        public long longValue()
        {
            return code;
        }
    }
}
