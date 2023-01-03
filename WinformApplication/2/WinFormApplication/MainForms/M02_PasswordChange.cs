using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assamble;


namespace MainForms
{
    /*------------------------------------------------------------------------------------------------------------------------------------------------
     * NAME   : M02_PasswordChange
     * DESC   : 비밀번호 변경
     * ------------------------------------------------------------------------------------------------------------------------------------------------
     * DATE   : 2022-12-08
     * AUTHOR : 이종원
     * DESC   : 최초 프로그램 작성
     */
    public partial class M02_PasswordChange : Form
    {
        // **********************************************************************************************
        //  SQL SERVER와 연동하기 위해 필요한 클래스를 필드멤버로 지정함.
        // **********************************************************************************************
        // 1. 공통 클래스 (데이터베이스 접속 클래스)
        private SqlConnection sCon;

        // 2. 데이터베이스에 SELECT를 실행하여 데이터를 받아오는 클래스
        private SqlDataAdapter sAdapter;

        // 3. INSNERT, UPDATE, DELETE 의 명령을 전달 할 클래스
        private SqlTransaction sTran; // 데이터베이스 데이터 관리 권한 클래스 -> Commit, Rollback 을 위한 클래스
        private SqlCommand     sCmd; //  데이터베이스 갱신 명령을 전달하는 클래스
        public M02_PasswordChange()
        {
            InitializeComponent();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            //비밀번호 변경 버튼 클릭
            string sMessage = string.Empty;
            try
            {
                /* 벨리데이션 체크
                    - 응용 프로그램 실행 시 발생 할 수 있는 예외 상황을 미리 인지하여 예외 상황 발생 경우를 사용자에게 전달하는
                      로직을 구현해 둠으로써 시스템 오류를 막고 프로그램의 신뢰도를 높여주는 프로그래밍 구현 개발 방법.
                 */

                //텍스트 박스에 정보 입력 여부 확인
                if (txtUserId.Text == "") sMessage = "사용자 ID";
                else if (txtPerPW.Text == "") sMessage = "이전 비밀번호";
                else if (txtChangePW.Text == "") sMessage = "변경 비밀번호";

                if (sMessage != "")
                {
                    MessageBox.Show(sMessage + "가 입력되지 않았습니다.");
                    return;
                }

                // 0. 텍스트박스에 입력한 사용자 정보와 변경 비밀번호 변수에 담기.
                string sUserId = txtUserId.Text;  // 사용자 ID
                string sPerPW = txtPerPW.Text;    // 기존 비밀번호
                string sNewPW = txtChangePW.Text; // 변경 할 비밀번호

                // 1. 데이터 베이스 오픈
                //데이터 베이스 주소 변수에 담기
                string strCon = Common.sConn;
                
                sCon = new SqlConnection(strCon);//접속 경로를 데이터베이스 커넥터 객체에 전달
                sCon.Open(); // DB OPEN

                #region < 기존 비밀번호와 비교하여 변경 가능한 상태인지체크 >

                // 2. 사용자 ID와 PW 가 일치하는지 확인
                string sSelectSQL = "SELECT PW " +
                                     " FROM TB_USER " +
                                   $" WHERE USERID = '{sUserId}'" +
                                   $"   AND PW = '{sPerPW}'";

                sAdapter = new SqlDataAdapter(sSelectSQL, sCon); // Adapter에 SQL 구문과 접속 정보 등록
                DataTable dtTemp = new DataTable(); //DB로 부터 결과를 담는 DataTable 변수
                sAdapter.Fill(dtTemp); //SQL 실행 및 

                if (dtTemp.Rows.Count == 0) { MessageBox.Show("일치하는 id가 없습니다."); return; }
                else if (dtTemp.Rows[0]["PW"].ToString() != sPerPW) { MessageBox.Show("비밀번호가 틀립니다."); return; }

                #endregion

                #region < 비밀번호 변경 로직 >


                // 3. 일치 한다면 비밀번호 변경

                if (MessageBox.Show("비밀번호를 변경하시겠습니까?", "비밀번호 변경", MessageBoxButtons.YesNo) == DialogResult.No) return;


                // 1. SQL 구문 UPDATE, INSERT, DELETE (갱신) 명령 전달 클래스 객체 생성
                sCmd = new SqlCommand();
                
                // 2. 트랜잭션 사용 선언(Commit, Rollback)
                sTran = sCon.BeginTransaction(); //

                // 3. 데이터 베이스에 데이터 갱신 명령 전달 클래스 객체에 트랜잭션 등록
                sCmd.Transaction = sTran;

                // 4. 접속 정보 등록
                sCmd.Connection = sCon;

                // 5. SQL문을 등록
                string sUpdateSql = "UPDATE TB_USER " +
                                      $"SET PW     = '{sNewPW}' " +
                                    $"WHERE USERID = '{sUserId}'";
                
                sCmd.CommandText = sUpdateSql;
                
                // 6.command 명령 실행
                sCmd.ExecuteNonQuery();
                
                //7. 변경내역 승인 명령
                sTran.Commit();

                MessageBox.Show("비밀번호가 변경되었습니다.");

                this.Close();
                #endregion




            }
            catch (Exception ex)
            {
                //트랜잭션이 설정 되어있을 경우만 rollbaock 하도록 분기문 적용
                if(sTran != null) sTran.Rollback();
                
                MessageBox.Show(ex.ToString());
                
            }
            finally
            {
                if (sCon != null) sCon.Close();
                
              
            }

        }
    }
}
