using Assamble;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Form_List
{
    public partial class Form04_userMaster : Form
    {
        SqlConnection sCon;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        SqlTransaction sTran;
        public Form04_userMaster()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sUserId = txtUserId.Text;
            string sUserName = txtUserName.Text;
            string sDeptCode = cboDeptCode.SelectedValue.ToString();

            string selectSql = string.Empty;
            selectSql +=  "SELECT USERID";
            selectSql +=  "     , USERNAME";
            selectSql +=  "     , PW";
            selectSql +=  "     , PW_FCnt";
            selectSql +=  "     , DEPTCODE";
            selectSql +=  "     , MAKEDATE";
            selectSql +=  "     , MAKER ";
            selectSql +=  "     , EDITDATE ";
            selectSql +=  "     , EDITOR  ";
            selectSql +=  " FROM TB_User  ";
            selectSql += $"WHERE USERID LIKE '%{sUserId}%' " +
                         $"  AND USERNAME LIKE '%{sUserName}%'" +
                         $"  AND DEPTCODE LIKE '%{sDeptCode}%'";

            try
            {
                sCon = new SqlConnection(Common.sConn);
                adapter = new SqlDataAdapter(selectSql, sCon);
                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);
                dgvUser.DataSource = dtTemp;

                //현재 커서가 있는 행의 사진 로드
                dgvUser_CellClick(null,null);
                
                
                
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sCon.Close();
            }

        }

        private void Form04_userMaster_Load(object sender, EventArgs e)
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

            //로드될때 콤보박스 세팅
            try
            {
                //데이터베이스에 공통기준정보(TB_Standard) 중 품목 유형(ITEMTYPE)의 정보를 받아와서 콤보박스에 등록하기.

                // 1. 데이터베이스 접속클래스 설정.
                sCon = new SqlConnection(Common.sConn); //db 오픈
                string sSqlSelect = string.Empty;
                sSqlSelect += " SELECT '' AS DEPTCODE                              ";
                sSqlSelect += " 	  ,'ALL' AS DEPTNAME                           ";
                sSqlSelect += " UNION ALL                                          ";
                sSqlSelect += " SELECT MINORCODE DEPTCODE                          ";
                sSqlSelect += " 	  ,CODENAME + '[' + MINORCODE +']' AS DEPTNAME ";
                sSqlSelect += "       FROM TB_Standard                             ";
                sSqlSelect += " 	  WHERE MAJORCODE = 'DEPTCODE'                 ";
                sSqlSelect += " 	    AND MINORCODE <> '$';                      ";

                adapter = new SqlDataAdapter(sSqlSelect, sCon);
                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                //콤보박스에 품목유형 리스트 등록 
                cboDeptCode.DataSource = dtTemp;
                cboDeptCode.ValueMember = "DEPTCODE"; // 로직 상 처리될 코드가 있는 컬럼.
                cboDeptCode.DisplayMember = "DEPTNAME"; // 사용자에게 보여줄 컬럼.
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
            DataRow dr = ((DataTable)dgvUser.DataSource).NewRow();
            ((DataTable)dgvUser.DataSource).Rows.Add(dr);

            //ID 수정 허용
            dgvUser.Columns["USERID"].ReadOnly = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //ID, 이름, 비밀번호 항상 필요함
            string sUserId = dgvUser.CurrentRow.Cells["USERID"].Value.ToString();
            string sUserName = dgvUser.CurrentRow.Cells["USERNAME"].Value.ToString();
            string sUserPw = dgvUser.CurrentRow.Cells["PW"].Value.ToString();
            string sPW_FCNT = dgvUser.CurrentRow.Cells["PW_FCNT"].Value.ToString();
            string sDeptCode = dgvUser.CurrentRow.Cells["DEPTCODE"].Value.ToString();


            if (sUserId == "" || sUserName == "" || sUserPw == "")
            {
                MessageBox.Show("id, 이름, 비밀번호는 공백이 안됩니다.");
                return;
            }

            //UPDATE, INSERT 구분
            //선택 행의 USERID가 DB 조회시 존재하지 않으면 INSERT
            //선택 행의 USERID가 존재하고, 선택행의 MAKEDATE가 NULL이면 이미 존재하는 ID로 추가 하려고 하는 것

            //선택 행의 USERID로 DB 조회

            string sSelectSql = "SELECT USERID, MAKEDATE" +
                "                 FROM TB_USER" +
               $"                WHERE USERID = '{sUserId}'";
            try
            {
                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                adapter = new SqlDataAdapter(sSelectSql, sCon);
                DataTable dtTemp = new DataTable();
                adapter.Fill(dtTemp);

                
                if(dtTemp.Rows.Count == 0) //INSERT
                {
                    //insert
                    string sInsertSql = " INSERT INTO TB_User (USERID, USERNAME, PW, PW_FCnt, DEPTCODE, MAKEDATE, MAKER) " +
                                       $" VALUES ('{sUserId}', '{sUserName}', '{sUserPw}', '{sPW_FCNT}', '{sDeptCode}', GETDATE(), '{Common.sUserID}') ";

                    try
                    {
                        cmd = new SqlCommand(sInsertSql);
                        sTran = sCon.BeginTransaction();
                        cmd.Connection = sCon;
                        cmd.Transaction = sTran;
                        cmd.CommandText = sInsertSql;
                        cmd.ExecuteNonQuery();
                        sTran.Commit();
                        MessageBox.Show("추가되었습니다.");
                    }
                    catch(Exception ex)
                    {
                        sTran.Rollback();
                        MessageBox.Show("추가에 실패하였습니다." + ex.ToString());
                    }

                }
                else if (dgvUser.CurrentRow.Cells["MAKEDATE"].Value.ToString() == "") //중복 ID INSERT 시도
                {
                    MessageBox.Show("이미 존재하는 아이디입니다.");
                    return;
                }
                else // 수정
                {
                    //update
                    string sUpdateSql = "UPDATE TB_USER " +
                        $"                  SET USERNAME = '{sUserName}'" +
                        $"                      ,PW = '{sUserPw}'" +
                        $"                      ,PW_FCNT = '{sPW_FCNT}'" +
                        $"                      ,DEPTCODE = '{sDeptCode}'" +
                        $"                      ,EDITDATE = GETDATE()" +
                        $"                      ,EDITOR = '{Common.sUserID}'" +
                        $"                WHERE USERID = '{sUserId}'";

                    try
                    {
                        cmd = new SqlCommand(sUpdateSql);
                        sTran = sCon.BeginTransaction();
                        cmd.Connection = sCon;
                        cmd.Transaction = sTran;
                        cmd.CommandText = sUpdateSql;
                        cmd.ExecuteNonQuery();
                        sTran.Commit();
                        MessageBox.Show("수정되었습니다..");
                    }
                    catch (Exception ex)
                    {
                        sTran.Rollback();
                        MessageBox.Show("수정에 실패하였습니다." + ex.ToString());
                    }


                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally 
            {
                sCon.Close(); 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUser.Rows.Count == 0) return;
            DialogResult dirResult = MessageBox.Show("선택한 품목을 삭제 하시겠습니까?", "데이터 삭제", MessageBoxButtons.YesNo);
            if (dirResult == DialogResult.No) return;
            //삭제할 userid
            string sUserId = dgvUser.CurrentRow.Cells["USERID"].Value.ToString();

            string sDeleteSql = $"DELETE FROM TB_USER WHERE USERID = '{sUserId}'";

            try
            {
                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                cmd = new SqlCommand();
                sTran = sCon.BeginTransaction() ;
                cmd.Connection = sCon;
                cmd.Transaction = sTran;
                cmd.CommandText = sDeleteSql;
                cmd.ExecuteNonQuery();

                sTran.Commit();
                MessageBox.Show("삭제되었습니다.");

                btnSearch_Click(null, null);
            }
            catch(Exception ex)
            {
                sTran.Rollback();
                MessageBox.Show("삭제 실패하였습니다. " + ex.ToString());
            }
            finally { sCon.Close(); }
        }

        private void btnImgLoad_Click(object sender, EventArgs e)
        {
            // 이미지 불러오기
            if (dgvUser.RowCount == 0) return;

            // 파일 탐색기 호출 (OpenFileDialog : 파일 탐색기 클래스, Window 제공 API)
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult dirResult = dialog.ShowDialog();

            if (dirResult != DialogResult.OK) return;

            //사진을 선택 하였을 경우 처리되는 로직
            string sImageFilePath = dialog.FileName; // 사진 파일이 저장되어 있는 폴더의 경로와 사진 파일의 정보.
            picboxUser.Image = Bitmap.FromFile(sImageFilePath); // 사진 파일의 경로를 찾아가 Byte[] 배열 형식으로 반환하여 이미지뷰어(picItemImage)에 표현한다.
            picboxUser.Tag = sImageFilePath;
        }

        private void btnImgSave_Click(object sender, EventArgs e)
        {

            // 1. 벨리데이션 체크
            if (dgvUser.RowCount == 0) return;      //품목 정보 미조회
            if (picboxUser.Image == null) return; // 저장 대상 이미지 미오픈

            if (MessageBox.Show("현재 이미지를 사용자이미지로 등록하시겠습니까?", "이미지 저장", MessageBoxButtons.YesNo) == DialogResult.No) return;

            Byte[] bImage = null; // 이미지 파일이 등록 될 Byte 배열.

            try
            {
                

                #region < 사진 파일을 APP으로 전달 >

                FileStream stream = new FileStream(Convert.ToString(picboxUser.Tag), FileMode.Open, FileAccess.Read);

                // 3. 스트림을 통해 읽어온 Binary 코드를 Byte코드로 변환.
                BinaryReader reader = new BinaryReader(stream);

                // 4. 만들어진 Binary 코드의 이미지를  Byte화 하여 App의 데이터 자료형 구조에 담는다.
                bImage = reader.ReadBytes(Convert.ToInt32(stream.Length));

                // 5. 바이너리 리더 종료
                reader.Close();
                // 6. 파일 스트림 종료
                stream.Close();

                #endregion

                #region < 유저 이미지 저장 (UPDATE) >

                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                cmd = new SqlCommand();
                cmd.Connection = sCon;

                string sUpdateSql = "UPDATE TB_USER SET" +
                    $"       USERIMAGE = @USERIMAGE          " + // 유저 이미지 변수 생성.
                    $" WHERE USERID = '{dgvUser.CurrentRow.Cells["USERID"].Value}'";

                cmd.Parameters.AddWithValue("@USERIMAGE", bImage);

                cmd.CommandText = sUpdateSql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("이미지가 정상적으로 등록 되었습니다.");

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("이미지등록에 실패하였습니다.\r\n" + ex.ToString());
            }
            finally
            {
                sCon.Close();
            }
        }

        private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            picboxUser.Image = null;

            if (dgvUser.Rows.Count == 0) return;
            string sUserId = dgvUser.CurrentRow.Cells["USERID"].Value.ToString();

            try
            {
                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                string sSelectSql = $"SELECT USERIMAGE FROM TB_USER WHERE USERID = '{sUserId}'";
                adapter = new SqlDataAdapter(sSelectSql, sCon);
                DataTable dtTemp = new DataTable();

                adapter.Fill(dtTemp);

                if (dtTemp.Rows.Count == 0) return;

                // 품목 별 이미지 BYTE 코드가 있는지 체크
                if (Convert.ToString(dtTemp.Rows[0]["USERIMAGE"]) == "") return;


                // byte[] 배열 형식으로 받아올 변수 생성
                Byte[] bImage = null;

                // byte 배열 형식으로 byte코드 형 변환
                bImage = (byte[])dtTemp.Rows[0]["USERIMAGE"];

                //byte[] 배열인 bImage를 Bitmap(픽셀 이미지로 변경해주는 클래스)로 변환.
                picboxUser.Image = new Bitmap(new MemoryStream(bImage));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { sCon.Close(); }
           
        }

        private void btnImgDelete_Click(object sender, EventArgs e)
        {
            // 이미지를 삭제할 대상 품목이 있는지 확인
            if (dgvUser.Rows.Count == 0) return;
            if (picboxUser.Image == null) return;

            string sUserId = dgvUser.CurrentRow.Cells["USERID"].Value.ToString();

            // 품목 별 이미지를 삭제(null)로 업데이트
            try
            {
                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                cmd = new SqlCommand();
                cmd.Connection = sCon;
                sTran = sCon.BeginTransaction();
                cmd.Transaction = sTran;

                string sUpdateSql = $"UPDATE TB_USER SET USERIMAGE = NULL WHERE USERID = '{sUserId}'";
                cmd.CommandText = sUpdateSql;
                cmd.ExecuteNonQuery();
                sTran.Commit();

                MessageBox.Show("이미지를 삭제하였습니다.");

                picboxUser.Image = null;

            }
            catch (Exception ex)
            {
                sTran.Rollback();
                MessageBox.Show("이미지 삭제를 실패하였습니다." + ex.ToString());
            }
            finally
            {
                sCon.Close();
            }
        }
    }
}
