using Assamble;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace Form_List
{
    public partial class Form07_ColumnChart : BaseChildForm
    {
        public Form07_ColumnChart()
        {
            InitializeComponent();
        }

        private void Form07_ColumnChart_Load(object sender, EventArgs e)
        {
            // 1. 콤보박스 세팅. (품목 마스터에 있는 품목을 콤보박스로 셋팅.)
            Common.SetComboContol(cboItem);

            // 2. 그리드 세팅. 
            GridUtil _Gridutril = new GridUtil();
            _Gridutril.InitColumnGrid(dgvGrid, "PRODDATE", "생산일자", typeof(string), 150, DataGridViewContentAlignment.MiddleLeft,  false);
            _Gridutril.InitColumnGrid(dgvGrid, "SEQ",      "순번",     typeof(int),    80,  DataGridViewContentAlignment.MiddleRight, false);
            _Gridutril.InitColumnGrid(dgvGrid, "ITEMCODE", "생산품목", typeof(string), 150, DataGridViewContentAlignment.MiddleLeft,  false);
            _Gridutril.InitColumnGrid(dgvGrid, "ITEMNAME", "품명",     typeof(string), 150, DataGridViewContentAlignment.MiddleLeft,  false);
            _Gridutril.InitColumnGrid(dgvGrid, "PRODQTY",  "생산수량", typeof(int),    100, DataGridViewContentAlignment.MiddleRight, false);
        }

        public override void DoInquire()
        {
            // 그리드 뷰 에 품목 별 생산 데이터 표현.
            DBHelper helper = new DBHelper();
            try
            {
                SqlDataAdapter Adapter = new SqlDataAdapter("PP_ItemPerProd_S1", helper.sCon);

                string sItemcode = Convert.ToString(cboItem.SelectedValue);


                // Adapter 에게 저장 프로시져 형식의 SQL 을 실행할것을 등록함.
                Adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                // 저장 프로시저가 받을 파라매터(인자) 값 설정.
                Adapter.SelectCommand.Parameters.AddWithValue("@ITEMCODE", sItemcode);

                // 기본적으로 모든 프로시져에 적용될 내용.
                Adapter.SelectCommand.Parameters.AddWithValue("@LANG", "KO");
                Adapter.SelectCommand.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
                Adapter.SelectCommand.Parameters.AddWithValue("@RS_MSG", "").Direction = ParameterDirection.Output;

                DataTable dtTemp = new DataTable();
                Adapter.Fill(dtTemp);

                dgvGrid.DataSource = dtTemp;
                SetCulomnChart(sItemcode, helper);
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

        void SetCulomnChart(string sItemCode, DBHelper helper)
        {
            // 데이터베이스에 접속 해서 일자별 품목 합한 수량을 차트로 나타내는 로직. 
            SqlDataAdapter Adapter = new SqlDataAdapter("PP_ItemPerProd_S2", helper.sCon);

            // Adapter 에게 저장 프로시져 형식의 SQL 을 실행할것을 등록함.
            Adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            // 저장 프로시저가 받을 파라매터(인자) 값 설정.
            Adapter.SelectCommand.Parameters.AddWithValue("@ITEMCODE", sItemCode);

            // 기본적으로 모든 프로시져에 적용될 내용.
            Adapter.SelectCommand.Parameters.AddWithValue("@LANG", "KO");
            Adapter.SelectCommand.Parameters.AddWithValue("@RS_CODE", "").Direction = ParameterDirection.Output;
            Adapter.SelectCommand.Parameters.AddWithValue("@RS_MSG", "").Direction = ParameterDirection.Output;

            DataTable dtTemp = new DataTable();
            Adapter.Fill(dtTemp);


            // 차트에 표현

            // 1. 차트의 초기화
            // Series : 차트를 표현 하는 연속된 데이터의 모음.
            chartItem.Series.Clear();

            // 2. 조회된 DataTable 의 가장 큰 생산수량 을 Y 축 에 셋팅.
            //int iMaxQty = 0;
            //for (int i = 0; i < dtTemp.Rows.Count; i++)
            //{
            //    if (Convert.ToInt32(dtTemp.Rows[i]["PRODQTY"]) > iMaxQty)
            //    {
            //        iMaxQty = Convert.ToInt32(dtTemp.Rows[i]["PRODQTY"]);
            //    }
            //}
            //iMaxQty: 최대수량

            DataRow[] dr = dtTemp.Select("PRODQTY = MAX(PRODQTY)");

            chartItem.ChartAreas[0].AxisY.Minimum = 0;
            chartItem.ChartAreas[0].AxisY.Maximum = Convert.ToInt32(dr[0]["PRODQTY"]) + 20;


            // 3. 데이터 테이블을 차트에 바인딩 (매핑)
            chartItem.DataBindTable(dtTemp.DefaultView, "PRODDATE");

            // 4.  막대 차트로 표현해야 하는 데이터 의 이름 과 설정 정보 등록.
            chartItem.Series[0].Name = Convert.ToString(dtTemp.Rows[0]["ITEMNAME"]); // 표현해야 할 데이터의 이름.
            chartItem.Series[0].Color = Color.Green;         // 표현될 차트의 색상. 
            chartItem.Series[0].IsValueShownAsLabel = true;  // 컬럼 차트 위에 수량을 숫자료 표기.
        }
    }
}
