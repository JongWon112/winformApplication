namespace Form_List
{
    partial class Form08_LineChart
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chtLine = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtLine)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.chtLine);
            // 
            // chtLine
            // 
            chartArea1.Name = "ChartArea1";
            this.chtLine.ChartAreas.Add(chartArea1);
            this.chtLine.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chtLine.Legends.Add(legend1);
            this.chtLine.Location = new System.Drawing.Point(3, 17);
            this.chtLine.Name = "chtLine";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtLine.Series.Add(series1);
            this.chtLine.Size = new System.Drawing.Size(794, 330);
            this.chtLine.TabIndex = 0;
            this.chtLine.Text = "chart1";
            // 
            // Form08_LineChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form08_LineChart";
            this.Text = "생산실적현황(라인차트)";
            this.Load += new System.EventHandler(this.Form08_LineChart_Load);
            this.groupBox22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtLine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chtLine;
    }
}
