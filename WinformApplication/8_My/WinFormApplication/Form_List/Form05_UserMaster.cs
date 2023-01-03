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
            dt.Columns.Add("USERID", typeof(string));
            dt.Columns.Add("USERNAME", typeof(string));
            dt.Columns.Add("PW", typeof(string));
            dt.Columns.Add("PW_FCNT", typeof(string));
            dt.Columns.Add("DEPTCODE", typeof(string));
            dt.Columns.Add("MAKEDATE", typeof(string));
            dt.Columns.Add("MAKER", typeof(string));
            dt.Columns.Add("EDITDATE", typeof(string));
            dt.Columns.Add("EDITOR", typeof(string));

            dgvUser.DataSource = dt;
            dgvUser.Columns["USERID"].HeaderText = "사용자ID";
            dgvUser.Columns["USERNAME"].HeaderText = "사용자명";
            dgvUser.Columns["PW"].HeaderText = "비밀번호";
            dgvUser.Columns["PW_FCNT"].HeaderText = "실패횟수";
            dgvUser.Columns["DEPTCODE"].HeaderText = "부서";
            dgvUser.Columns["MAKEDATE"].HeaderText = "등록일시";
            dgvUser.Columns["MAKER"].HeaderText = "등록자";
            dgvUser.Columns["EDITDATE"].HeaderText = "수정일자";
            dgvUser.Columns["EDITOR"].HeaderText = "수정자";

            //readonly 세팅
            dgvUser.Columns["USERID"].ReadOnly = true;
            dgvUser.Columns["MAKEDATE"].ReadOnly = true;
            dgvUser.Columns["MAKER"].ReadOnly = true;
            dgvUser.Columns["EDITDATE"].ReadOnly = true;
            dgvUser.Columns["EDITOR"].ReadOnly = true;


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
                //adapter.SelectCommand.Parameters.AddWithValue("@LANG", "KO");
                //adapter.SelectCommand.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
                //adapter.SelectCommand.Parameters.AddWithValue("@RS_MSG",  "").Direction = ParameterDirection.Output;

                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                dgvUser.DataSource = dtTemp;

                
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

    }
}
