using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assamble
{
    public class DBHelper
    {
        public SqlConnection sCon;
        public DBHelper() 
        { 
            sCon= new SqlConnection();
            sCon.Open();
        }

        public void Close()
        {
            //데이터 베이스 종료
            sCon.Close();
        }

    }
}
