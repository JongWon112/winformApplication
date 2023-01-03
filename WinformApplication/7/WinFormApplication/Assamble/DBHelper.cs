using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SQL SERVER에 접속할 수 있게 도와주는 API
using System.Data.SqlClient;

namespace Assamble
{
    /*----------------------------------------------------------------------------
     * 데이터 베이스 접속 및 트랜잭션을 관리 할 수 있게 도와주는 DBHelper Class
     -----------------------------------------------------------------------------*/
    public  class DBHelper
    {
        public SqlConnection sCon = new SqlConnection(Common.sConn);
        public DBHelper()
        {
            // 데이터베이스 오픈.
            sCon.Open();
        }

        public void Close()
        {
            //데이터 베이스 종료
            sCon.Close();
        }
    }
}
