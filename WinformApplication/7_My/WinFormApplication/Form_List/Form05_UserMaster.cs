using Assamble;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form_List
{
    public partial class Form05_UserMaster : BaseChildForm
    {
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

            //ComboBox 세팅

            Common.setComboBox("DEPTCODE", cboDeptCode);
        }

        public override void DoInquire()
        {
            //조회

           string sUserId = txtUserId.Text;
           string sUserName = txtUserName.Text;
            string sDeptCode = cboDeptCode.SelectedValue.ToString();

            //DBHelper를 이용한 DB접근
            DBHelper helper = new DBHelper(); //생성자에서 db open

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("
                    ", helper.sCon); //저장 프로시져명과 db 접속정보를던져준다.

                //adapter에 저장 프로시져 형식의 sql을 실행할것을 등록..
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //저장프로시져로 던질 변수 설정
                adapter.SelectCommand.Parameters.AddWithValue("@USERID", sUserId);
                adapter.SelectCommand.Parameters.AddWithValue("@USERNAME", sUserId);
                adapter.SelectCommand.Parameters.AddWithValue("@DEPTCODE", sUserId);

                //기본적으로 모든 프로시져에 적용될 내용.
                adapter.SelectCommand.Parameters.AddWithValue("@LANG", "KO");
                adapter.SelectCommand.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
                adapter.SelectCommand.Parameters.AddWithValue("@RS_MSG", "").Direction = ParameterDirection.Output;

                //데이터 받을 데이터테이블 변수
                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);
                
                dgvUser.DataSource = dtTemp;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                helper.Close();
            }
          
        }
    }
}
