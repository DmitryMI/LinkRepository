using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LinkRepository.Repository.Sqlite
{
    static class SqliteUtils
    {
        public static int BoolToInt(bool value)
        {
            if (value) return 1;
            else return 0;
        }

        public static bool IntToBool(int value)
        {
            return value != 0;
        }
    }
}
