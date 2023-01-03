using Assamble;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Form_List
{

    /*------------------------------------------------------------------------------------------------------------------------------------------------
     * NAME   : Form05_UserMaster
     * DESC   : 저장 프로시져를 이용한 사용자 관리 화면
     * ------------------------------------------------------------------------------------------------------------------------------------------------
     * DATE   : 2022-12-16
     * AUTHOR : 이종원
     * DESC   : 최초 프로그램 작성
     */

    public partial class Form05_UserMaster : BaseChildForm
    {
        SqlConnection sCon;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        SqlTransaction sTran;
        public Form05_UserMaster()
        {
            InitializeComponent();
        }

      
        private void Form05_UserMaster_Load(object sender, EventArgs e)
        {
            //데이터그리드뷰 기본 세팅
            DataTable dt = new DataTable();
            dt.Columns.Add("USERCODE", typeof(string));
            dt.Columns.Add("USERNAME", typeof(string));
            dt.Columns.Add("PASSWORD", typeof(string));
            dt.Columns.Add("PW_F_CNT", typeof(string));
            dt.Columns.Add("DEPTCODE", typeof(string));
            dt.Columns.Add("MAKEDATE", typeof(string));
            dt.Columns.Add("MAKER", typeof(string));
            dt.Columns.Add("EDITDATE", typeof(string));
            dt.Columns.Add("EDITOR", typeof(string));

            dgvGrid.DataSource = dt;
            dgvGrid.Columns["USERCODE"].HeaderText = "사용자ID";
            dgvGrid.Columns["USERNAME"].HeaderText = "사용자명";
            dgvGrid.Columns["PASSWORD"].HeaderText = "비밀번호";
            dgvGrid.Columns["PW_F_CNT"].HeaderText = "실패횟수";
            dgvGrid.Columns["DEPTCODE"].HeaderText = "부서";
            dgvGrid.Columns["MAKEDATE"].HeaderText = "등록일시";
            dgvGrid.Columns["MAKER"].HeaderText = "등록자";
            dgvGrid.Columns["EDITDATE"].HeaderText = "수정일자";
            dgvGrid.Columns["EDITOR"].HeaderText = "수정자";

            //readonly 세팅
            dgvGrid.Columns["USERCODE"].ReadOnly = true;
            dgvGrid.Columns["MAKEDATE"].ReadOnly = true;
            dgvGrid.Columns["MAKER"].ReadOnly = true;
            dgvGrid.Columns["EDITDATE"].ReadOnly = true;
            dgvGrid.Columns["EDITOR"].ReadOnly = true;

            dgvGrid.Columns["MAKEDATE"].Width= 150;
            dgvGrid.Columns["EDITDATE"].Width= 150;

            //콤보박스에 품목유형 리스트 등록 
            
            Common.SetComboControl("DEPTCODE", cboDeptCode);
         
        }
       
        public override void DoInquire()
        {

            //조회 버튼 클릭 시 사용자 정보 조회
            string sUserId = txtUserId.Text;
            string sUserName = txtUserName.Text;
            string sDeptCode = cboDeptCode.SelectedValue.ToString();

            DBHelper helper = new DBHelper(); // 생성자에서 db open 한다.

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("dbo.BM_UserMaster_S1", helper.sCon);

                //Adapter에게 저장 프로시져 형식의 SQL을 실행할 것을 등록함.
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //저장 프로시져가 받을 파라메터(인자) 값 설정
                adapter.SelectCommand.Parameters.AddWithValue("@USERID", sUserId);
                adapter.SelectCommand.Parameters.AddWithValue("@USERNAME", sUserName);
                adapter.SelectCommand.Parameters.AddWithValue("@DEPTCODE", sDeptCode);

                //기본적으로 모든 프로시져에 적용될 내용.
                adapter.SelectCommand.Parameters.AddWithValue("@LANG", "KO");
                adapter.SelectCommand.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
                adapter.SelectCommand.Parameters.AddWithValue("@RS_MSG", "").Direction = ParameterDirection.Output;

                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                dgvGrid.DataSource = dtTemp;

                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                helper.Close();
            }

        }

        public override void DoNew()
        {
            //툴바에서 추가 버튼을 클릭 했을 때 실행 되는 로직.
            base.DoNew();

            //그리드에 셋팅되어 있는 컬럼 포맷(양식)을 가진 빈 깡통 행 생성
            //DataRow dr = ((DataTable)dgvUser.DataSource).NewRow();

            // Object 형식의 DataSource를 정렬하여 양식이 같은 행을 생성 할 필요가 있음.
            // 그리드의 DataSource를 DataTable 형식으로 정렬
            DataTable dtTemp = (DataTable)dgvGrid.DataSource;
            // 정렬된 데이터 테이블에 한 행을 신규 생성
            DataRow dr = dtTemp.NewRow();
            // 생성 된 신규행을 그리드 데이터 소스에 추가
            ((DataTable)dgvGrid.DataSource).Rows.Add(dr);

            dgvGrid.Columns["USERCODE"].ReadOnly = false;
        }

        public override void DoDelete()
        {
            // 툴바의 삭제 버튼을 클릭 하였을 경우 로직

            // 1. 삭제 할 내역이 있는지 확인
            if (dgvGrid.Rows.Count == 0) return;

            // RemoveAt()  -> 행 자체를삭제한다. 복구할 수 없으며 행의 상태를 확인 할 수 없다.
            //// 2. 삭제 할 행의 위치 index 찾기
            //int iRowIndex = dgvGrid.CurrentRow.Index;

            //// 3. Grid의 DataSource에 바인딩(매핑, 연결)된 DataTable의 행을 삭제
            //DataTable dtTemp = (DataTable )dgvGrid.DataSource;

            //// 4. 선택한 행의 위치 정보를 가진 DataTable 데이터 삭제.
            //dtTemp.Rows.RemoveAt(iRowIndex);

            // Delete()  -> 삭제 데이터를 복구 가능 하며, 행의 상태를 삭제 항태로 확인 할 수 있다.
            //              Grid에 표현되는 내용은 삭제가 된 것 처럼 표현 가능.

            DataTable dtTemp = (DataTable)dgvGrid.DataSource;
          
            foreach (DataGridViewRow dr in dgvGrid.SelectedRows)
            {
                dtTemp.Rows[dr.Index].Delete();
            }

            //int iRowIndex = dgvGrid.CurrentRow.Index;
            //DataTable dtTemp = (DataTable )dgvGrid.DataSource;
            //dtTemp.Rows[iRowIndex].Delete();
            
            

        }

        public override void DoSave()
        {
            foreach(DataRow dr in dgvGrid.SelectedRows)
            {
                MessageBox.Show(dr["USERCODE"].ToString());
            }
            // 표 형식의 데이터(DataGrideView)의 데이터를 등록하는 방법으로 주로 사용되는 방법.
            // 그리드에 표현된 데이터를 일괄 추가, 수정, 삭제, 로직 적용.

            // 1. 데이터 베이스 오픈
            DBHelper helper  = new DBHelper();

            try
            {
                // 2. 데이터베이스 갱신명령 전달 클래스 객체 생성.
                cmd = new SqlCommand();
                // 3. 일괄 승인 및 일괄 복구 트랜잭션 설정
                sTran = helper.sCon.BeginTransaction();
                // 4. 트랜잭션 커맨드에 연결
                cmd.Transaction = sTran;
                // 5. 커맨드에 접속 정보 연결
                cmd.Connection = helper.sCon;

                cmd.CommandType = CommandType.StoredProcedure;

                // 6. 그리드의 행 중에 갱신 이력이 있는 행만 추출하여 DataTable 에 담기
                dgvGrid.Update(); //그리드에 갱신 된 데이터 확정.

                DataTable dtTemp = ((DataTable)dgvGrid.DataSource).GetChanges();

                // 7. 갱신된 행만 추출한 데이터테이블의 행수 만큼 반복하여 update, insert, delete 분기 짓기 
                string sMessage = string.Empty;
                foreach (DataRow dr in dtTemp.Rows)
                {
                    // 데이터 테이블에서 추출한 행의 상태 비교
                    switch (dr.RowState)
                    {
                        case DataRowState.Deleted:
                            //Delete된 행의 데이터 복구
                            dr.RejectChanges();
                            cmd.CommandText = "BM_UserMaster_D1";
                            cmd.Parameters.AddWithValue("@USERID", Convert.ToString(dr["USERCODE"]));

                            cmd.Parameters.AddWithValue("@LANG", "KO");
                            cmd.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
                            cmd.Parameters.AddWithValue("@RS_MSG", "").Direction = ParameterDirection.Output;

                            cmd.ExecuteNonQuery();


                            break;
                        case DataRowState.Added:
                            //지금 추출한 행의 상태가 신규 추가 상태 라면
                            // 1. 필수 입력 데이터 확인
                            sMessage = string.Empty;
                            if (Convert.ToString(dr["USERCODE"]) == "") sMessage = "사용자ID";
                            else if (Convert.ToString(dr["USERNAME"]) == "") sMessage = "사용자 명";
                            else if (Convert.ToString(dr["PASSWORD"]) == "") sMessage = "비밀번호";
                            
                            if(sMessage != "")
                            {
                                throw new Exception(sMessage + "를 입력하지 않았습니다.");
                                return;
                            }

                            // 2. 사용자 등록 로직 구현.
                            cmd.CommandText = "BM_UserMaster_I1";
                            cmd.Parameters.AddWithValue("@USERID",   Convert.ToString(dr["USERCODE"]));
                            cmd.Parameters.AddWithValue("@USERNAME", Convert.ToString(dr["USERNAME"]));
                            cmd.Parameters.AddWithValue("@PW",       Convert.ToString(dr["PASSWORD"]));
                            cmd.Parameters.AddWithValue("@PW_FCNT",  Convert.ToString(dr["PW_F_CNT"]));
                            cmd.Parameters.AddWithValue("@DEPTCODE", Convert.ToString(dr["DEPTCODE"]));
                            cmd.Parameters.AddWithValue("@MAKER",    Common.sUserID);

                            cmd.Parameters.AddWithValue("@LANG", "KO");
                            cmd.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
                            cmd.Parameters.AddWithValue("@RS_MSG", "").Direction = ParameterDirection.Output;

                            // 3. 커맨드의 실행
                            cmd.ExecuteNonQuery();

                            break;
                        case DataRowState.Modified:
                            //지금추출한 행의 상태가 수정 상태라면
                            // 1. 필수 입력 데이터 확인
                            sMessage = string.Empty;
                            if (Convert.ToString(dr["USERCODE"]) == "") sMessage = "사용자ID";
                            else if (Convert.ToString(dr["USERNAME"]) == "") sMessage = "사용자 명";
                            else if (Convert.ToString(dr["PASSWORD"]) == "") sMessage = "비밀번호";
                            
                            if(sMessage != "")
                            {
                                throw new Exception(sMessage + "를 입력하지 않았습니다.");
                                return;
                            }

                            // 2. 사용자 정보수정 로직 구현.
                            cmd.CommandText = "BM_UserMaster_U1";
                            cmd.Parameters.AddWithValue("@USERID",   Convert.ToString(dr["USERCODE"]));
                            cmd.Parameters.AddWithValue("@USERNAME", Convert.ToString(dr["USERNAME"]));
                            cmd.Parameters.AddWithValue("@PW",       Convert.ToString(dr["PASSWORD"]));
                            cmd.Parameters.AddWithValue("@PW_FCNT",  Convert.ToString(dr["PW_F_CNT"]));
                            cmd.Parameters.AddWithValue("@DEPTCODE", Convert.ToString(dr["DEPTCODE"]));
                            cmd.Parameters.AddWithValue("@EDITOR",    Common.sUserID);

                            cmd.Parameters.AddWithValue("@LANG", "KO");
                            cmd.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
                            cmd.Parameters.AddWithValue("@RS_MSG", "").Direction = ParameterDirection.Output;

                            // 3. 커맨드의 실행
                            cmd.ExecuteNonQuery();
                            break;

                    }

                    string sRs_Code = Convert.ToString(cmd.Parameters["@RS_CODE"].Value);
                    string sRs_Msg = Convert.ToString(cmd.Parameters["@RS_MSG"].Value);
                    if (sRs_Code != "S")
                    {
                        sTran.Rollback();
                        MessageBox.Show(sRs_Msg);
                        return;
                    }
                     // 처리한 커맨드의 파라미터 정보 삭제
                     // 다음 커맨드에서 변수를 지정 할 수 있도록 초기화 함.
                    cmd.Parameters.Clear();
                }
   
                sTran.Commit();
                MessageBox.Show("정상적으로 데이터를 등록하였습니다.");
                DoInquire(); // 재조회
            }
            catch(Exception ex)
            {
                sTran.Rollback();
                MessageBox.Show("데이터 등록에 실패하였습니다." + ex.ToString());
            }
            finally
            {
                helper.Close();
            }

        }

    }
}
