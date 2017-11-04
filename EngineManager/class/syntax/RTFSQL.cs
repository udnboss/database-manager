using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EngineManager
{
    class RTFSQL
    {
        protected static string wrap = @"{{\rtf\ansi\deff0{{\colortbl;{0}}}{1}}}";

        protected enum DataTypes
        {
            INT, BIGINT, TINYINT, CHAR, NCHAR, NVARCHAR, VARCHAR, LONG, FLOAT, TEXT, BIT, DATETIME, DATE, TIME, BINARY, IMAGE
        }

        protected enum Functions
        {
            GETDATE, YEAR, MONTH, DAY, STR, CAST, OBJECT_ID, SCHEMA_ID
        }

        protected enum Keywords
        {
            SELECT, CREATE, DELETE, INSERT, UPDATE, FROM, WHERE, ORDER, BY, GROUP, ALTER, ADD, DROP, PRIMARY, FOREIGN, KEY, DECLARE, EXEC,
            IF, IN, ON, AFTER, TABLE, TRIGGER, FUNCTION, VIEW, PROCEDURE, COLUMN, CONSTRAINT, AS, DEFAULT, IDENTITY, TYPE, BEGIN, END, ROLLBACK, TRANSACTION, GO, INNER, LEFT, RIGHT, JOIN, RETURN, RETURNS
        }
        protected enum Operators
        {
            NOT, NULL, AND, OR
        }
        //public static string Format(string sql)
        //{
        //    //get colors
        //    var colors = new List<Color> { Color.Black, Color.Blue, Color.Blue, Color.Gray, Color.Magenta, Color.Teal, Color.Maroon, Color.Green };
        //    var colorset = string.Concat(colors.Select(x => GetRtfColor(x)).ToList().ToArray());

        //    sql = Regex.Replace(sql, @"^(\s|\t)*(\r\n)", "", RegexOptions.Multiline);
        //    sql = sql.Replace("\r\n", "\r\n\\line\r\n");
        //    sql = sql.Replace("\t", "\r\n\\tab\r\n");

        //    //colorize object names
        //    sql = Regex.Replace(sql, @"(^|\s)(\[.*?\])($|\s)", string.Format("$1\r\n\\cf{0}\r\n$2\r\n\\cf1\r\n$3", 6));

        //    //colorize strings
        //    sql = Regex.Replace(sql, @"'(.*?)'", string.Format("\r\n\\cf{0}\r\n'$1'\r\n\\cf1\r\n", 7));

        //    //colorize comments
        //    sql = Regex.Replace(sql, @"--(.*?)($|\r\n)", string.Format("\r\n\\cf{0}\r\n--$1\r\n\\cf1\r\n", 8));

        //    foreach (var word in Enum.GetNames(typeof(Keywords)))
        //    {
        //        sql = DoReplace(sql, word, 1);
        //    }
        //    foreach (var word in Enum.GetNames(typeof(DataTypes)))
        //    {
        //        sql = DoReplace(sql, word, 2);
        //    }
        //    foreach (var word in Enum.GetNames(typeof(Operators)))
        //    {
        //        sql = DoReplace(sql, word, 3);
        //    }
        //    foreach (var word in Enum.GetNames(typeof(Functions)))
        //    {
        //        sql = DoReplace(sql, word, 4);
        //    }



        //    //sql = Regex.Replace(sql, @"(\\cf1\s+)(\\cf)", @"$2");
        //    //sql = Regex.Replace(sql, @"\ {2,}", @" ");

        //    return string.Format(wrap, colorset, sql);
        //}

        protected static string DoReplace(string sql, string word, int colorIndex)
        {
            return Regex.Replace(sql, string.Format(@"(^|\s|\(|\,)({0})([$\s\)\(\,])", word), string.Format("$1\r\n\\cf{0}\r\n{1}\r\n\\cf1\r\n$3", colorIndex + 1, word), RegexOptions.IgnoreCase);
        }

        protected static string GetRtfColor(System.Drawing.Color c)
        {
            var color = @"\red{0}\green{1}\blue{2};";
            return string.Format(color, c.R, c.G, c.B);
        }
    }
}
