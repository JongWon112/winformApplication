using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assamble;

namespace Form_List
{
    public partial class Form03_ItemMaster : Form
    {
        SqlConnection sCon;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        SqlTransaction sTran;

        //조회 조건
        string sItemCode;
        string sItemName;
        string sStartDate;
        string sEndDate;
        string sItemType;
        string sEndFlag = "Y";
        public Form03_ItemMaster()
        {
            InitializeComponent();
        }

        
        //Load Event -> Form이 열릴 때 실행되는 이벤트
        private void Form03_ItemMaster_Load(object sender, EventArgs e)
        {
            // 아이템 마스터 화면이 오픈 될때 처리되는 로직

            #region < 1. 그리드 세팅 >  
            //  DataGridView 세팅  -> DataTable과 Grid는 거의 비슷함. DataTable을 그리드뷰로 바로 연결이 가능하다.

            // 1. 데이터 테이블 생성 (그리드에 표현될 컬럼을 세팅)
            DataTable dtGrid = new DataTable();
            dtGrid.Columns.Add("ITEMCODE", typeof(string)); // 종목코드
            dtGrid.Columns.Add("ITEMNAME", typeof(string)); // 품목 명
            dtGrid.Columns.Add("ITEMTYPE", typeof(string)); // 품목 유형
            dtGrid.Columns.Add("ITEMDESC", typeof(string)); // 품목 상세정보
            dtGrid.Columns.Add("ENDFLAG",  typeof(string)); // 단종 여부
            dtGrid.Columns.Add("PRODDATE", typeof(string)); // 출시 일자
            dtGrid.Columns.Add("MAKEDATE", typeof(string)); // 데이터 생성 일시
            dtGrid.Columns.Add("MAKER",    typeof(string)); // 생성자
            dtGrid.Columns.Add("EDITDATE", typeof(string)); // 수정일자
            dtGrid.Columns.Add("EDITOR",   typeof(string)); // 수정자

            // 2. 세팅 된 컬럼을 그리드에 매핑.
            // DataSource : 데이터베이스에서 가져온 테이블 형식의 데이터를 등록 할때 사용
            dgvGrid.DataSource = dtGrid;

            // 3. 그리드 컬럼에 한글 명칭으로 컬럼 Text 보여주기
            dgvGrid.Columns["ITEMCODE"].HeaderText = "품목코드";
            //dgvGrid.Columns[1].HeaderText = "품목코드";
            dgvGrid.Columns["ITEMNAME"].HeaderText = "품목명";
            dgvGrid.Columns["ITEMTYPE"].HeaderText = "품목유형";
            dgvGrid.Columns["ITEMDESC"].HeaderText = "품목상세";
            dgvGrid.Columns["ENDFLAG"].HeaderText  = "단종여부";
            dgvGrid.Columns["PRODDATE"].HeaderText = "출시일자";
            dgvGrid.Columns["MAKEDATE"].HeaderText = "등록일시";
            dgvGrid.Columns["MAKER"].HeaderText    = "등록자";
            dgvGrid.Columns["EDITDATE"].HeaderText = "수정일시";
            dgvGrid.Columns["EDITOR"].HeaderText   = "수정자";

            // 4. Column의 폭 지정
            dgvGrid.Columns[0].Width = 100; //품목코드
            dgvGrid.Columns[1].Width = 200; //품목명
            dgvGrid.Columns[2].Width = 300; //품목상세
            dgvGrid.Columns[6].Width = 250; //등록일시
            dgvGrid.Columns[8].Width = 250; //수정일시

            // 5. 컬럼의 수정 여부
            dgvGrid.Columns["ITEMCODE"].ReadOnly = true;
            dgvGrid.Columns["MAKEDATE"].ReadOnly = true;
            dgvGrid.Columns["MAKER"].ReadOnly    = true;
            dgvGrid.Columns["EDITDATE"].ReadOnly = true;
            dgvGrid.Columns["EDITOR"].ReadOnly   = true;

            #endregion

            #region < 2. 품목 유형 콤보박스 셋팅 >
            try
            {
                //데이터베이스에 공통기준정보(TB_Standard) 중 품목 유형(ITEMTYPE)의 정보를 받아와서 콤보박스에 등록하기.

                // 1. 데이터베이스 접속클래스 설정.
                sCon = new SqlConnection(Common.sConn); //db 오픈
                string sSqlSelect = string.Empty;
                sSqlSelect +=    " SELECT '' AS CODE_ID\r\n\t  ,'ALL' AS CODE_NAME";
                sSqlSelect += " UNION ALL";
                sSqlSelect +=    " SELECT MINORCODE                        AS CODE_ID      " +
                                 "         ,'[' + MINORCODE + ']' + CODENAME AS CODE_NAME  " +
                                 "     FROM TB_Standard                                    " +
                                 "    WHERE MAJORCODE = 'ITEMTYPE'                         " +
                                 "      AND MINORCODE <> '$'                               ";

                adapter = new SqlDataAdapter(sSqlSelect, sCon);
                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                //콤보박스에 품목유형 리스트 등록 
                cboItemType.DataSource = dtTemp;
                cboItemType.ValueMember = "CODE_ID"; // 로직 상 처리될 코드가 있는 컬럼.
                cboItemType.DisplayMember = "CODE_NAME"; // 사용자에게 보여줄 컬럼.

                //for (int i = 0; i < dtTemp.Rows.Count; i++)
                //{
                //    cboItemType.Items.Add(dtTemp.Rows[i]["CODE_NAME"]);
           
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sCon.Close();
            }
           


            #endregion
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // 조회 버튼 클릭 이벤트.

            //조회버튼을 클릭한 조회일 경우 검색조건을 설정 아니면 최근 검색 조건을 그대로 가져가자
            if(((Button)sender).Text == "조회")
            {
                // 조회조건 받아올 변수 파라매터 선언 및 값 등록.
                sItemCode = txtItemCode.Text;  // 품목코드 입력정보
                sItemName = txtItemName.Text;  // 품명 입력정보
                sStartDate = dtpStart.Text;     // 출시 일자 시작 일자.
                sEndDate = dtpEnd.Text;         // 출시 일자 종료 일자.
                                                //string sItemType = cboItemType.Text;   // Text : DisplayMember를 가져온다
                sItemType = Convert.ToString(cboItemType.SelectedValue); // SelectedValue : ValueMember에 들어가 있는 값 (실제값, 보이지 않지만 실제갑)

                if (chkOnlyName.Checked) sItemCode = ""; // 품목코드 와는 관계없는 나머지 조회 조건으로 검색.

                sEndFlag = "Y";
                if (rdoProd.Checked) sEndFlag = "N";  //생산이 선택되었을때.
            }
            

            // 품목 마스터 테이블에서 데이터를 조회 할 SQL 구문 작성.
            string sSelectSql = string.Empty;

            sSelectSql = "SELECT ITEMCODE" +
                         "      ,ITEMNAME  " +
                         "      ,ITEMTYPE  " +
                         "      ,ITEMDESC " +
                         "      ,ENDFLAG  " +
                         "      ,PRODDATE  " +
                         "      ,MAKEDATE  " +
                         "      ,MAKER" +
                         "      ,EDITDATE  " +
                         "      ,EDITOR " +
                         "  FROM TB_ItemMaster " +
                        $" WHERE ITEMCODE LIKE '%{sItemCode}%'" +
                        $"   AND ITEMNAME LIKE '%{sItemName}%'" +
                        $"   AND ITEMTYPE LIKE '%{sItemType}%' " +
                        $"   AND PRODDATE BETWEEN '{sStartDate}' AND '{sEndDate}'" +
                        $"   AND ENDFLAG = '{sEndFlag}'";

            sCon = new SqlConnection(Common.sConn);
            try
            {
                sCon.Open();
                
                

                adapter = new SqlDataAdapter(sSelectSql, sCon);
                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                dgvGrid.DataSource = dtTemp;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //추가 버튼 클릭 시 새로운 행 생성
            DataRow dr = ((DataTable)dgvGrid.DataSource).NewRow();
            ((DataTable)dgvGrid.DataSource).Rows.Add(dr);

            dgvGrid.Columns["ITEMCODE"].ReadOnly = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //선택한 행의 ITEMCODE를 기준으로 삭제
            string sItemCode = dgvGrid.CurrentRow.Cells["ITEMCODE"].Value.ToString(); // 현재 선택한 행의 ITEMCODE

            //델리게이션 체크
            if (dgvGrid.Rows.Count == 0) return;
            if (MessageBox.Show("선택한 품목을 삭제 하시겟습니까?", "품목삭제", MessageBoxButtons.YesNo) == DialogResult.No) return;

            sCon = new SqlConnection(Common.sConn);

            try
            {
                string sql = "Delete TB_ItemMaster " +
                              $" where itemcode = '{sItemCode}'";

                sCon.Open();
                cmd = new SqlCommand();
                cmd.Connection = sCon;
                sTran = sCon.BeginTransaction();
                cmd.Transaction = sTran;
                cmd.CommandText= sql;

                cmd.ExecuteNonQuery();

                sTran.Commit();

                MessageBox.Show("선택 품목을 삭제하엿습니다.");

                btnSearch_Click(sender, e);


            }
            catch(Exception ex)
            {
                sTran.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sCon.Close();
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 선택 된 행의 데이터를 저장하는 기능 구현.
            if(dgvGrid.Rows.Count == 0) return;

            // 선택 된 행에 잇는 컬럼별 데이터를 변수에 등록.
            string sItemCode = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMCODE"].Value); // 품목코드
            string sItemName = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMNAME"].Value); // 품목명
            string sItemType = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMTYPE"].Value); // 품목타입
            string sItemDesc = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMDESC"].Value); // 품목상세
            string sEndFlag  = Convert.ToString(dgvGrid.CurrentRow.Cells["ENDFLAG"].Value);  // 단종여부
            string sProdDate = Convert.ToString(dgvGrid.CurrentRow.Cells["PRODDATE"].Value); // 출시일


            // 필수 입력 정보 데이터 기입 여부 확인 (품목코드, 출시일자)
            string sMessage = string.Empty;
            if (sItemCode == "") sMessage = "품목코드";
            else if (sProdDate == "") sMessage = "출시일자";

            if(sMessage != "")
            {
                MessageBox.Show(sMessage + "를 입력하지 않았습니다.");
                return;
            }

            //품목 정보 갱신 등록 로직이 시작 되는 곳.
            if (MessageBox.Show("선택한 품목정보를 등록하시겠습니까?", "품목등록", MessageBoxButtons.YesNo) == DialogResult.No) return;

            sCon = new SqlConnection(Common.sConn);
            sCon.Open();
            cmd = new SqlCommand();



            // 품목 마스터 테이블에 이미 등록 되어 있는 품목 코드 인지 확인.
            // 있다면 update, 없다면 insert

            string sSelectSql = string.Empty;
            sSelectSql += "SELECT ITEMCODE FROM TB_ITEMMASTER";
            adapter = new SqlDataAdapter(sSelectSql, sCon);
            DataTable dtTemp = new DataTable();
            adapter.Fill(dtTemp);

            int iCuRowIndex = dgvGrid.CurrentRow.Index;
            for(int i = 0; i < dtTemp.Rows.Count; i++)
            {
                if (dtTemp.Rows[i]["ITEMCODE"].ToString() == sItemCode && dgvGrid.CurrentRow.Cells["MAKEDATE"].Value.ToString() == "")
                {
                    MessageBox.Show("중복되는 품목코드입니다.");
                    return;
                }
            }

          

            sSelectSql = string.Empty;
            sSelectSql += $"SELECT * FROM TB_ITEMMASTER WHERE ITEMCODE='{sItemCode}'";
            adapter =new SqlDataAdapter(sSelectSql, sCon);
            dtTemp = new DataTable();
            adapter.Fill(dtTemp);

            

            try
            {
              
                cmd.Connection = sCon; // 
                sTran = sCon.BeginTransaction();
                cmd.Transaction = sTran; // cmd에 트랜잭션 연결

                if (sEndFlag == "") sEndFlag = "N";

                string sUpInSql = string.Empty;
                string msg = string.Empty;
                if(dtTemp.Rows.Count == 0)
                {
                    //insert
                    sUpInSql += " INSERT INTO TB_ITEMMASTER (ITEMCODE, ITEMNAME, ITEMTYPE, ITEMDESC, ENDFLAG, PRODDATE, MAKEDATE, MAKER)" +
                                $"                      VALUES ('{sItemCode}','{sItemName}','{sItemType}','{sItemDesc}','{sEndFlag}', '{sProdDate}', GETDATE(), '{Common.sUserID}')";
                    msg = "추가";
                }
                else
                {
                    //update
                    sUpInSql += " UPDATE TB_ITEMMASTER SET                 " +
                                $"            ITEMNAME = '{sItemName}'      "+
                                $"           ,ITEMTYPE = '{sItemType}'      "+
                                $"           ,ITEMDESC = '{sItemDesc}'      "+
                                $"           ,ENDFLAG  = '{sEndFlag}'       "+
                                $"           ,PRODDATE = '{sProdDate}'      "+
                                $"           ,EDITDATE = GETDATE()          "+
                                $"           ,EDITOR    = '{Common.sUserID}' "+
                                $"      WHERE ITEMCODE = '{sItemCode}'      ";
                    msg = "수정";
                }

                cmd.CommandText= sUpInSql;
                cmd.ExecuteNonQuery();
                sTran.Commit();
                MessageBox.Show(msg + "(이)가 완료되었습니다.");

                //ITEMCODE READONLY
                dgvGrid.Columns["ITEMCODE"].ReadOnly = true;
                


            }
            catch (Exception ex)
            {
                sTran.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sCon.Close();
            }
            cmd = new SqlCommand();
            

         
        }
    }
}
