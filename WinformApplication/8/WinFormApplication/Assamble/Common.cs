using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// 클래스 라이브러리 = namespace = 프로젝트 = DLL 파일.
namespace Assamble
{
    // 클래스 라이브러리
    // 하나 이상의 APP 또는 프로젝트에서 호출되는 변수 및 메서드(로직) 등을 작성하여 DLL 파일로 제공 할 수 있게 만든 프로젝트 형식
    // 단독으로 실행 되지 않고 외부 프로그램에서 참조해서 호출하는 구조.
    // 배포 및 재사용이 용이, 보안성 향상, 의 장점이 있다.

    public class Common
    {
        //const 상수는 기본적으로 static 속성을 가지고  있기 때문에 외부에서 사용 시 객체 생성 없이 사용 할 수있다.
        public const string sConn = "Data Source = (local); Initial Catalog  = AppDev; Integrated Security = SSPI;";
        public static string sUserID = "";
        public static string sUserName = "";

        static SqlConnection sCon;
        static DataTable dtTemp;

        public static void SetComboControl(string sMajorCode, ComboBox comboBox)
        {
            
            //로드될때 콤보박스 세팅
            try
            {
                //데이터베이스에 공통기준정보(TB_Standard) 중 품목 유형(ITEMTYPE)의 정보를 받아와서 콤보박스에 등록하기.

                // 1. 데이터베이스 접속클래스 설정.
                sCon = new SqlConnection(Common.sConn); //db 오픈
                string sSqlSelect = string.Empty;
                sSqlSelect += " SELECT '' AS CODE_ID                              ";
                sSqlSelect += " 	  ,'ALL' AS CODENAME                           ";
                sSqlSelect += " UNION ALL                                          ";
                sSqlSelect += " SELECT MINORCODE AS CODE_ID                          ";
                sSqlSelect += " 	  ,CODENAME + '[' + MINORCODE +']' AS CODENAME ";
                sSqlSelect += "       FROM TB_Standard                             ";
                sSqlSelect += $" 	  WHERE MAJORCODE = '{sMajorCode}'                 ";
                sSqlSelect += " 	    AND MINORCODE <> '$';                      ";

                SqlDataAdapter adapter = new SqlDataAdapter(sSqlSelect, sCon);
                dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                comboBox.DataSource = dtTemp;
                comboBox.ValueMember = "CODE_ID"; // 로직 상 처리될 코드가 있는 컬럼.
                comboBox.DisplayMember = "CODENAME"; // 사용자에게 보여줄 컬럼.

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
      
            }
            finally
            {
                sCon.Close();
            }
            
           
        }
    }

  
}
