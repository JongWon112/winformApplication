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
        SqlConnection connect;
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

                // 1. 데이터 베이스 오픈
                string sConn = "Data Source = (local); Initial Catalog  = AppDev; Integrated Security = SSPI;";
                connect = new SqlConnection(sConn);
                connect.Open();

                // 2. 사용자 ID와 PW 가 일치하는지 확인
                string sUserId = txtUserId.Text;
                string sPrePW = txtPerPW.Text;

                string sFindUserImfo = $"SELECT USERNAME, PW FROM TB_USER WHERE USERID = '{sUserId}'";
                SqlDataAdapter adapter = new SqlDataAdapter(sFindUserImfo, connect);
                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                if (dtTemp.Rows.Count == 0) { MessageBox.Show("일치하는 id가 없습니다."); return; }
                else if (dtTemp.Rows[0]["PW"].ToString() != sPrePW) { MessageBox.Show("비밀번호가 틀립니다."); return; }

                // 3. 일치 한다면 비밀번호 변경


                // 4. 데이터베이스 닫기


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {

            }

        }
    }
}
