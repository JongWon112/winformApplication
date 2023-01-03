using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

           //테스터


            #endregion

            #region < 2. 품목 유형 콤보박스 셋팅 >
            try
            {
                //데이터베이스에 공통기준정보(TB_Standard) 중 품목 유형(ITEMTYPE)의 정보를 받아와서 콤보박스에 등록하기.

                // 1. 데이터베이스 접속클래스 설정.
                sCon = new SqlConnection(Common.sConn); //db 오픈

                string sSqlSelect = string.Empty;
                sSqlSelect =  " SELECT ''   AS CODE_ID    ";
                sSqlSelect += "       ,'ALL' as CODE_NAME ";
                sSqlSelect += "  UNION ALL                ";

                sSqlSelect += " SELECT MINORCODE                        AS CODE_ID     " +
                              "       ,'[' + MINORCODE + ']' + CODENAME AS CODE_NAME   " +
                              "   FROM TB_Standard                                     " +
                              "  WHERE MAJORCODE = 'ITEMTYPE'                          " +
                              "    AND MINORCODE <> '$'                                " ;

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

  
        private void btnSearch_Click(object sender = null, EventArgs e = null)
        {
            // 조회 버튼 클릭 이벤트.

            sCon = new SqlConnection(Common.sConn);
            try
            {
                sCon.Open();

                // 조회조건 받아올 변수 파라매터 선언 및 값 등록.
                string sItemCode  = txtItemCode.Text;  // 품목코드 입력정보
                string sItemName  = txtItemName.Text;  // 품명 입력정보
                string sStartDate = dtpStart.Text;     // 출시 일자 시작 일자.
                string sEndDate = dtpEnd.Text;         // 출시 일자 종료 일자.
                //string sItemType = cboItemType.Text;   // Text : DisplayMember를 가져온다
                string sItemType = Convert.ToString(cboItemType.SelectedValue); // SelectedValue : ValueMember에 들어가 있는 값 (실제값, 보이지 않지만 실제갑)

                if (chkOnlyName.Checked) sItemCode = ""; // 품목코드 와는 관계없는 나머지 조회 조건으로 검색.

                string sEndFlag = "Y";
                if (rdoProd.Checked) sEndFlag = "N";  //생산이 선택되었을때.

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
                            $"   AND ENDFLAG = '{sEndFlag}'" +
                            $" ORDER BY PRODDATE DESC";

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

            dgvGrid_CellClick(null, null);
        }

        private void btnAdd_Click(object sender, EventArgs e) // 선택적 인수로 변경하면 인수로 안넣어도 실행할 있다.
        {
            // 품목 정보 등록을 위한 그리드 행 추가.
            // 1. 그리드의 컬럼 포맷이 들어있는 빈 행 데이터 타입이 필요.(DataRow : 행 데이터 타입)
            //  (DataTable)dgvGrid.DataSource : 데이터 테이블 형식으로 형변화 한다.
            DataRow dr = ((DataTable)dgvGrid.DataSource).NewRow();
            
            // 2. 빈 깡통 행을 그리드에 추가
            ((DataTable)dgvGrid.DataSource).Rows.Add(dr);

            // 3. 품목 코드 입력 할 수 있도록 컬럼 수정 여부 풀기.
            dgvGrid.Columns["ITEMCODE"].ReadOnly = false;
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 품목 마스터 데이터 삭제 삭제.
            // 선택된 행에 있는 품목을 삭제.
            if (dgvGrid.Rows.Count == 0) return;
            DialogResult dirResult =  MessageBox.Show("선택한 품목을 삭제 하시겠습니까?", "데이터 삭제", MessageBoxButtons.YesNo);
            if (dirResult == DialogResult.No) return;

            //데이터베이스 접속 및 Command 클래스 설정.

            //1. 데이터베이스 오픈
            sCon = new SqlConnection(Common.sConn);

            try
            {
                sCon.Open();

                //2. 삭제(Delete) 명령어를 수행할 Command 객체
                cmd = new SqlCommand();
                
                //3. 갱신 데이터 승인 권한 가지고 오기
                sTran = sCon.BeginTransaction();

                //4. Command에 트랜잭션 등록.
                cmd.Transaction = sTran;

                //5.커맨드에 접속 경로 등록
                cmd.Connection = sCon;

                //6.커맨드에 쿼리 등록

                // 선택한 행에 있는 ITEMCODE 값 ㅊ출 후 변수에 등록하기
                string sItemCode = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMCODE"].Value);

                string sDeleteSql = string.Empty;
                sDeleteSql +=  "DELETE FROM TB_ItemMaster         ";
                sDeleteSql += $" WHERE ITEMCODE = '{sItemCode}'; ";
                cmd.CommandText = sDeleteSql;

                //7. Commend 실행
                cmd.ExecuteNonQuery();
                
                //8. Commit
                sTran.Commit();

                //9. 정상 삭제 메세지 표현
                MessageBox.Show("품목 정보를 삭제 하였습니다.");

                //10. 재 조회
                btnSearch_Click();
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
            if (dgvGrid.Rows.Count == 0) return;

            // 선택 된 행에 잇는 컬럼별 데이터를 변수에 등록.
            string sItemCode = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMCODE"].Value); // 품목코드
            string sItemName = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMNAME"].Value); // 품목명
            string sItemType = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMTYPE"].Value); // 품목타입
            string sItemDesc = Convert.ToString(dgvGrid.CurrentRow.Cells["ITEMDESC"].Value); // 품목상세
            string sEndFlag = Convert.ToString(dgvGrid.CurrentRow.Cells["ENDFLAG"].Value);  // 단종여부
            string sProdDate = Convert.ToString(dgvGrid.CurrentRow.Cells["PRODDATE"].Value); // 출시일


            // 필수 입력 정보 데이터 기입 여부 확인 (품목코드, 출시일자)
            string sMessage = string.Empty;
            if (sItemCode == "") sMessage = "품목코드";
            else if (sProdDate == "") sMessage = "출시일자";

            if (sMessage != "")
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

            int iCuRowIndex = dgvGrid.CurrentRow.Index;
            for (int i = 0; i < dgvGrid.Rows.Count - 1; i++)
            {
                if (dgvGrid.Rows[i].Cells["ITEMCODE"].Value.ToString() == sItemCode)
                {
                    if (iCuRowIndex == i) continue;
                    if (Convert.ToString(dgvGrid.Rows[i].Cells["ITEMCODE"].Value) == sItemCode)
                    {
                        MessageBox.Show($"중복되는 품목코드를 입력하였습니다. 코드 행 :{i + 1}");
                        return;
                    }

                }
            }



            string sSelectSql = string.Empty;
            sSelectSql += $"SELECT * FROM TB_ITEMMASTER WHERE ITEMCODE='{sItemCode}'";
            adapter = new SqlDataAdapter(sSelectSql, sCon);
            DataTable dtTemp = new DataTable();
            adapter.Fill(dtTemp);



            try
            {

                cmd.Connection = sCon; // 
                sTran = sCon.BeginTransaction();
                cmd.Transaction = sTran; // cmd에 트랜잭션 연결

                if (sEndFlag == "") sEndFlag = "N";

                string sUpInSql = string.Empty;
                string msg = string.Empty;
                if (dtTemp.Rows.Count == 0)
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
                                $"            ITEMNAME = '{sItemName}'      " +
                                $"           ,ITEMTYPE = '{sItemType}'      " +
                                $"           ,ITEMDESC = '{sItemDesc}'      " +
                                $"           ,ENDFLAG  = '{sEndFlag}'       " +
                                $"           ,PRODDATE = '{sProdDate}'      " +
                                $"           ,EDITDATE = GETDATE()          " +
                                $"           ,EDITOR    = '{Common.sUserID}' " +
                                $"      WHERE ITEMCODE = '{sItemCode}'      ";
                    msg = "수정";
                }

                cmd.CommandText = sUpInSql;
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

        private void btnImageLoad_Click(object sender, EventArgs e)
        {
            // 이미지 불러오기
            if (dgvGrid.RowCount == 0) return;

            // 파일 탐색기 호출 (OpenFileDialog : 파일 탐색기 클래스, Window 제공 API)
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult dirResult =  dialog.ShowDialog();

            if (dirResult != DialogResult.OK) return;

            //사진을 선택 하였을 경우 처리되는 로직
            string sImageFilePath = dialog.FileName; // 사진 파일이 저장되어 있는 폴더의 경로와 사진 파일의 정보.
            picItemImage.Image = Bitmap.FromFile(sImageFilePath); // 사진 파일의 경로를 찾아가 Byte[] 배열 형식으로 반환하여 이미지뷰어(picItemImage)에 표현한다.
            picItemImage.Tag= sImageFilePath;
            

        }

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            // 선행 되어야 하는 일 : TB_ItemMaster 테이블에 ITEMIMAGE 컬럼 생성 (image 데이터 타입)

            // 저장 버튼 클릭 시 품목별 사진 데이터베이스에 저장

            // 1. 벨리데이션 체크
            if(dgvGrid.RowCount == 0) return;      //품목 정보 미조회
            if(picItemImage.Image == null) return; // 저장 대상 이미지 미오픈

            if(MessageBox.Show("현재 이미지를 품목에 등록하시겠습니까?", "이미지 저장", MessageBoxButtons.YesNo) == DialogResult.No) return;

            Byte[] bImage = null; // 이미지 파일이 등록 될 Byte 배열.

            try
            {
                /*
                    ------------      BinaryReader      ------------      FileStream      ------------

                      APP(Byte)      <------------>      RAM(Binary)    <------------>     File(Byte)

                    ------------      BinaryReader      ------------      FileStream      ------------


                 Byte   : CPU가 아닌 가상머신(OS)에서 이해 할 수 있는 코드의 이진 파일.
                 Binary : 컴퓨터(CPU)가 인식 할 수 있는 0, 1로 이루어진 이진코드
                 */

                #region < 사진 파일을 APP으로 전달 >

                // 2. 파일 스트림을 통해 파일을 오픈하고 바이너리 형식으로 변환
                // FileMode.Open   : 경로에 있는 사진 파일에 접근
                // FileAccess.Read : 읽기 전용으로 읽어오겟다.
                FileStream stream = new FileStream(Convert.ToString(picItemImage.Tag), FileMode.Open, FileAccess.Read);
               
                // 3. 스트림을 통해 읽어온 Binary 코드를 Byte코드로 변환.
                BinaryReader reader = new BinaryReader(stream);
                
                // 4. 만들어진 Binary 코드의 이미지를  Byte화 하여 App의 데이터 자료형 구조에 담는다.
                bImage = reader.ReadBytes(Convert.ToInt32(stream.Length));

                // 5. 바이너리 리더 종료
                reader.Close();
                // 6. 파일 스트림 종료
                stream.Close();

                #endregion

                #region < 품목 마스터에 품목 별 사진 저장 (UPDATE) >
                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                cmd = new SqlCommand();
                cmd.Connection = sCon;

                //string sUpdateSql = "UPDATE TB_ITEMMASTER SET" +
                //                    $"       ITEMIMAGE = {bImage}" +
                //                    $" WHERE ITEMCODE = '{dgvGrid.CurrentRow.Cells["ITEMCODE"].Value}'";

                string sUpdateSql = "UPDATE TB_ITEMMASTER SET" +
                    $"       ITEMIMAGE = @ITEMIMAGE          " + // 품목 이미지 변수 생성.
                    $" WHERE ITEMCODE = '{dgvGrid.CurrentRow.Cells["ITEMCODE"].Value}'";

                cmd.Parameters.AddWithValue("@ITEMIMAGE", bImage);

                cmd.CommandText= sUpdateSql;
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

        private void dgvGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 품목을선택 했을 때 이미지 표현
            //선택 품목의 ITEMCODE
            string sItemCode = dgvGrid.CurrentRow.Cells["ITEMCODE"].Value.ToString();

            
            picItemImage.Image = null;

            try
            {
                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                string sSelectSql = $"SELECT ITEMIMAGE FROM TB_ITEMMASTER WHERE ITEMCODE = '{sItemCode}'";
                adapter = new SqlDataAdapter(sSelectSql, sCon);
                DataTable dtTemp = new DataTable();

                adapter.Fill(dtTemp);

                if (dtTemp.Rows.Count == 0) return;

                // 품목 별 이미지 BYTE 코드가 있는지 체크
                if (Convert.ToString(dtTemp.Rows[0]["ITEMIMAGE"]) == "") return;
     

                // byte[] 배열 형식으로 받아올 변수 생성
                Byte[] bImage = null;

                // byte 배열 형식으로 byte코드 형 변환
                bImage = (byte[])dtTemp.Rows[0]["ITEMIMAGE"];

                //byte[] 배열인 bImage를 Bitmap(픽셀 이미지로 변경해주는 클래스)로 변환.
                picItemImage.Image = new Bitmap(new MemoryStream(bImage));

                


            }
            catch(Exception ex)
            {
                MessageBox.Show("이미지 로드에 실패하였습니다." + ex.ToString() );
            }
            finally
            {
                sCon.Close(); 
            }
        }

        private void btnImageDelete_Click(object sender, EventArgs e)
        {
            // 이미지를 삭제할 대상 품목이 있는지 확인
            if(dgvGrid.Rows.Count == 0) return;
            if (picItemImage.Image == null) return;

            string sItemCode = dgvGrid.CurrentRow.Cells["ITEMCODE"].Value.ToString();

            // 품목 별 이미지를 삭제(null)로 업데이트
            try
            {
                sCon = new SqlConnection(Common.sConn);
                sCon.Open();
                cmd = new SqlCommand();
                cmd.Connection= sCon;
                sTran = sCon.BeginTransaction();
                cmd.Transaction = sTran;

                string sUpdateSql = $"UPDATE TB_ITEMMASTER SET ITEMIMAGE = NULL WHERE ITEMCODE = '{sItemCode}'";
                cmd.CommandText = sUpdateSql;
                cmd.ExecuteNonQuery();
                sTran.Commit();

                MessageBox.Show("이미지를 삭제하였습니다.");

                picItemImage.Image = null;

            }
            catch(Exception ex)
            {
                sTran.Rollback();
                MessageBox.Show("이미지 삭제를 실패하였습니다." + ex.ToString() );
            }
            finally
            {
                sCon.Close();
            }
        }
    }
}
