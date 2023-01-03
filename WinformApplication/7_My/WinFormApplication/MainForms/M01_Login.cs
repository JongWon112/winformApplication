using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

//SQL SERVER 접속 클래스 라이브러리
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



// 공통 로직 및 변수 등을 관리하는 우리가 만든 클래스 라이브러리
using Assamble;

/* WinFormApplication 강의의 목표
   C# .NetFrameWork WinForm의 기본 도구와 프로그래밍 문법을 사용하여 데이터 베이스와 유기적으로 연결 되는 개발 솔루션의 프레임을 만들어 보고
   시스템 개발 프레임 코어 소스의 구성 원리를 이해 및 기능을 습득한다.*/



/*------------------------------------------------------------------------------------------------------------------------------------------------
 * NAME   : M01_Login
 * DESC   : 시스템 로그인
 * ------------------------------------------------------------------------------------------------------------------------------------------------
 * DATE   : 2022-12-08
 * AUTHOR : 이종원
 * DESC   : 최초 프로그램 작성
 */

namespace MainForms
{
    public partial class M01_Login : Form
    {

        #region < 필드 멤버 >
        private int iLoginFCnt = 0;
        private SqlConnection Connect;
        private SqlCommand sCmd;
        private SqlTransaction sTran;
        private SqlDataAdapter sAdapter; 
        #endregion

        public M01_Login()
        {
            InitializeComponent();
            this.Tag = false;
        }
      

        #region <Method>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }


        private void DoLogin()
        {
            
            try
            {
                // 데이터 베이스에 접속 할 경로.
                string sConn = Common.sConn;

                // 데이터 베이스 접속 객체 생성
                Connect = new SqlConnection(sConn);

                //데이터 베이스 오픈 명령.
                Connect.Open();

                // ID / PW 를 데이터 베이스에서 가자와서 비교 로직. 
                string sUserId = txtUserId.Text;
                string sPassWord = txtPassWord.Text;

                #region < ID 와 PW 가 동시에 일치 하는지 를 비교하는 경우 >
                //// ID 와 PW 를 정확하게 입력 하였는지 확인.
                //string sFindUserImfo = $"SELECT USERID,PW FROM TB_USER WHERE USERID = '{sUserId}' AND PW = '{sPassWord}';  ";

                //// SqlDataAdapter : 데이터베이스 연결 후 SELECT SQL 구문 전달 및 결과를 
                ////                  응용프로그램에 받아오는 기능 클래스.
                //SqlDataAdapter adapter= new SqlDataAdapter(sFindUserImfo, Connect);

                //// DataTable : 프로그래밍 언어에서 데이터를 테이블 형태로 관리하는 클래스.
                //DataTable dtTemp = new DataTable();

                //adapter.Fill(dtTemp);


                //// ID 와 PW 를 정확히 입력하지 않은 경우. 
                //if (dtTemp.Rows.Count == 0)
                //{
                //    MessageBox.Show("로그인 ID 또는 PW 가 잘못 되었습니다.");
                //    return;
                //}
                //// 로그인을 성공하였을 경우 로직.
                #endregion


                #region < ID 의 존재 여부에 따라 PW 의 일치 여부를 비교하는 경우 >

                //string sFindUserImfo = $"SELECT USERNAME, PW FROM TB_USER WHERE USERID = '{sUserId}'";
                //SqlDataAdapter adapter = new SqlDataAdapter(sFindUserImfo, Connect);

                //DataTable dtTemp = new DataTable();
                //adapter.Fill(dtTemp);

                ////ID를 잘못 입력한 경우 받아온 결과의 행이 없다.


                //if (dtTemp.Rows.Count == 0)
                //{
                //    MessageBox.Show("존재하지 않는 ID 입니다.");
                //    return;
                //}

                ////존재하는 ID를 입력하여 데이터베이스에서 사용자 정보를 받아 왔을 경우
                //else if (sPassWord != dtTemp.Rows[0]["PW"].ToString())
                //{
                //    MessageBox.Show("비밀번호를 잘못 입력하였습니다.");
                //    return;
                //}

                #endregion

                #region < 비밀번호 3회 이상 실패 시 프로그램 종료 >
                ////UPDATE 구문을 사용하지 않고 프로그램에서만 3회 실패 시 종료 하는 로직
                //string sFindUserImfo = $"SELECT USERNAME, PW FROM TB_USER WHERE USERID = '{sUserId}'";
                //SqlDataAdapter adapter = new SqlDataAdapter(sFindUserImfo, Connect);
                //DataTable dtTemp = new DataTable();
                //adapter.Fill(dtTemp);

                ////ID를 잘못 입력한 경우 받아온 결과의 행이 없다.
                //if (dtTemp.Rows.Count == 0)
                //{
                //    MessageBox.Show("존재하지 않는 ID 입니다.");
                //    return;
                //}
                //else if (sPassWord != dtTemp.Rows[0]["PW"].ToString())
                //{
                //    //비밀번호 틀린 횟수를 누적시켜야함
                //    iLoginFCnt++;

                //    if (iLoginFCnt == 3)
                //    {
                //        MessageBox.Show("비밀번호를 3회이상 잘못 입력하여 프로그램을 종료 합니다.ㅋ");

                //        // 종료
                //        this.Close(); //클래스를 닫는 기능.. 프로그램 종료가 아님.
                //    }

                //    MessageBox.Show($"비밀번호를 잘못 입력하였습니다. 남은횟수{ 3 - iLoginFCnt }");
                //    return;

                //}

                ////로그인 성공임
                //iLoginFCnt = 0;
                //MessageBox.Show($"{dtTemp.Rows[0]["USERNAME"].ToString()}님 반갑습니다.");

                #endregion

                #region < 비밀번호 실패 횟수를 DB에저장하고 프로그램이 종료 된 후에도 다시 실행 시켜도 로그인이 되지 않도록 설정

                // 아이디 비밀번호 체크

                //select query
                string selectSQL = $"SELECT USERNAME, PW, PW_FCNT FROM TB_USER WHERE USERID = '{sUserId}'";

                //쿼리를 보낼 객체
                sAdapter =  new SqlDataAdapter(selectSQL, Connect);

                //쿼리 결과를 담을 Table
                DataTable dtTemp = new DataTable();

                //쿼리 전송 및 결과 받기
                sAdapter.Fill(dtTemp);

                // 쿼리 결과에 따라 처리

                string updateSQL;
                
                //if (dtTemp.Rows.Count == 0)
                //{
                //    MessageBox.Show("존재하지 않는 ID 입니다.");
                //    return;
                //}
                //else if (Convert.ToInt32(dtTemp.Rows[0]["PW_FCNT"]) == 3)
                //{
                //    MessageBox.Show("비밀번호 3회 오기입. 관리자에게 문의하세요");
                //    return;
                //}
                //else if(sPassWord != dtTemp.Rows[0]["PW"].ToString())
                //{
                //    int iPwFcnt = Convert.ToInt32(dtTemp.Rows[0]["PW_FCNT"]);
                //    iPwFcnt++;

                //    //트랜잭션 클래스
                //    sTran = Connect.BeginTransaction();
                //    sCmd = new SqlCommand();

                //    try
                //    {
                //        //Command에 접속 주소 등록
                //        sCmd.Connection = Connect;
                //        //Command에 트랜잭셔 ㄴ등록
                //        sCmd.Transaction = sTran;
                //        //Command에 명령문 등록
                //        updateSQL = "UPDATE TB_USER " +
                //                            $"SET PW_FCNT ={iPwFcnt} " +
                //                            $"WHERE USERID = '{sUserId}'";

                //        sCmd.CommandText = updateSQL;

                //        //UPDATE문 데이터 베이스에 실행
                //        sCmd.ExecuteNonQuery();
                //        sTran.Commit();
                //    }
                //    catch(Exception ex)
                //    {
                //        sTran.Rollback();
                //        MessageBox.Show(ex.ToString());
                //        return;
                //    }
                //    finally
                //    {

                //    }
                //}

                //비밀번호를 잘못입력했거나, 이미 3회 이상 실패했을때 
                if (sPassWord != dtTemp.Rows[0]["PW"].ToString() || dtTemp.Rows[0]["PW_FCNT"].ToString() == "2")
                {
                    //비밀번호 잘못 입력 횟수가 3이면 로그인 실패, 프로그램 종료 
                    if (dtTemp.Rows[0]["PW_FCNT"].ToString() == "2" )
                    {
                        MessageBox.Show("비밀번호를 3회 이상 잘못 입력하였습니다. 관리자와 문의하세요");
                        this.Close();
                        return;
                    }

                    //비밀번호 잘못 입력 시 DB에 잘못 입력  횟수를 누적

                    int iPwFCnt = Convert.ToInt32(dtTemp.Rows[0]["PW_FCNT"]);
                    iPwFCnt++;
                    MessageBox.Show($"비밀번호를 잘못 입력하였습니다. 남은 횟수 : {3 - iPwFCnt}");

                    updateSQL = "UPDATE TB_USER " +
                                        "SET PW_FCNT = PW_FCNT + 1 " +
                                        $"WHERE USERID = '{sUserId}'";

                    sCmd = new SqlCommand();

                    // 2. 트랜잭션 사용 선언(Commit, Rollback)
                    sTran = Connect.BeginTransaction();  //SqlConnection 클래스의 BeginTransaction메서드에서 내부적으로 객체를 생성해서 돌려준다.

                    // 3. 데이터 베이스에 데이터 갱신 명령 전달 클래스 객체에 트랜잭션 등록
                    sCmd.Transaction = sTran;
                    // 4. 접속 정보 등록
                    sCmd.Connection = Connect;

                    sCmd.CommandText = updateSQL;

                    // 6.command 명령 실행
                    sCmd.ExecuteNonQuery();

                    //7. 변경내역 승인 명령
                    sTran.Commit();

                    return;

                }

                //정상 로그인 성공시 비밀번호 실패 횟수 초기화
                
                updateSQL = "UPDATE TB_USER " +
                                       "SET PW_FCNT = 0 " +
                                       $"WHERE USERID = '{sUserId}'";

                sCmd = new SqlCommand();

                // 2. 트랜잭션 사용 선언(Commit, Rollback)
                sTran = Connect.BeginTransaction();

                // 3. 데이터 베이스에 데이터 갱신 명령 전달 클래스 객체에 트랜잭션 등록
                sCmd.Transaction = sTran;
                // 4. 접속 정보 등록
                sCmd.Connection = Connect;

                sCmd.CommandText = updateSQL;

                // 6.command 명령 실행
                sCmd.ExecuteNonQuery();

                //7. 변경내역 승인 명령
                sTran.Commit();

                MessageBox.Show("관리자님 반갑습니다.");

                //로그인 성공여부 저장
                //Class 속성 중 Tag속성에 성공여부를 저장
                //Tag 속성에는 무슨값이든 다 들어갈 수 있다.
                this.Tag = true;

                this.Close();
              
                #endregion

            }
            catch (Exception ex)
            {
                sTran.Rollback();
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                Connect.Close();
            }
        }

        //txtPassWord 텍스트 박스에서 Enter키 누를 경우 로그인 메서드 실행
     


        #endregion

        private void btnPWChange_Click(object sender, EventArgs e)
        {
            //비밀번호 변경 창 호출.
            M02_PasswordChange m02 = new M02_PasswordChange();

            //로그인창 숨기기
            //this.Visible = false;
            this.SetVisibleCore(false);
            

            //m02.Show(); // 새롭게 열린 form과 기존 form이 개별적으로 동작한다. - 병렬적으로 수행 (비동기)

            m02.ShowDialog(); // 새롭게 열린 form이 닫힐 때까지 기존 form에 접근 x - 직렬적으로 수행 (동기)

            //로그인창 표시하기
           // this.Visible = true;
            this.SetVisibleCore(true);
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                DoLogin();
            }
        }
    }
}
