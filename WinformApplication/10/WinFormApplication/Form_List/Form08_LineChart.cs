using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Form_List
{
    public partial class Form08_LineChart : Assamble.BaseChildForm
    {
        public Form08_LineChart()
        {
            InitializeComponent();
        }

        private void Form08_LineChart_Load(object sender, EventArgs e)
        {
            // 폼이 활성화 될 때 라인 차트 구현
            
            chtLine.Series.Clear();

            #region < 시리즈 1 내역 등록 >
            // 1. 차트로 구성 할 시리즈 생성

            Series series1 = new Series(); // 시리즈 1 객체 생성.
            series1.Name = "Test_Series1";
            series1.ChartType = SeriesChartType.Line; // 라인 차트로 구성함.
            chtLine.Series.Add(series1); // 차트에 시리즈 추가.

            // 2. 시리즈에 등록 될 데이터 생성.
            int[] iValues = { 100, 110, 90, 80, 70, 90, 88, 105, 120, 95, 85, 77 }; // Y 축에 표현될 데이터
            string[] sXvalues = { "22-01", "22-02", "22-03", "22-04", "22-05", "22-06",
                                 "22-07", "22-08", "22-09", "22-10", "22-11", "22-12"};

            // 3. 시리즈에 데이터 매핑.
            // iValue 배열에서 
            int iXpoint = 0;
            foreach(int i in iValues)
            {
                chtLine.Series[0].Points.AddXY(sXvalues[iXpoint], i);
                iXpoint++;
            }

            series1.IsValueShownAsLabel = true; // 포인트에 데이터 표현
            #endregion


            #region < 시리즈 2 내역 등록 >
            // 1. 차트로 구성 할 시리즈 생성

            Series series2 = new Series(); // 시리즈 1 객체 생성.
            series2.Name = "Test_Series2";
            series2.ChartType = SeriesChartType.Line; // 라인 차트로 구성함.
            chtLine.Series.Add(series2); // 차트에 시리즈 추가.

            // 2. 시리즈에 등록 될 데이터 생성.
            int[] iValues2 = { 70, 140, 80, 60, 78, 88, 102, 98, 68, 105, 132, 124 }; // Y 축에 표현될 데이터
            string[] sXvalues2 = { "22-01", "22-02", "22-03", "22-04", "22-05", "22-06",
                                 "22-07", "22-08", "22-09", "22-10", "22-11", "22-12"};

            // 3. 시리즈에 데이터 매핑.
            // iValue 배열에서 
            iXpoint = 0;
            foreach (int i in iValues2)
            {
                chtLine.Series[1].Points.AddXY(sXvalues[iXpoint], i);
                iXpoint++;
            }

            series2.IsValueShownAsLabel = true; // 포인트에 데이터 표현
            #endregion
        }
    }
}
