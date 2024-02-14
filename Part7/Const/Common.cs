using System.Collections.Generic;

namespace Part78.Const
{
    internal class Common
    {
        public const string YearLabel = "Год выпуска";
        public const string NameLabel = "Название";
        public const string DurationLabel = "Продолжительность";

        public static List<string> SearchLabelsList => new List<string>
        {
            YearLabel, NameLabel, DurationLabel
        };
    }
}
