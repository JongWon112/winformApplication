using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assamble
{
    public class GridUtil
    {
        private DataTable dtTemp = new DataTable();
        /// <summary>
        /// 데이터 그리드 뷰 컬럼 셋팅 메서드
        /// </summary>
        /// <param name="dgvTemp">세팅할 그리드</param>
        /// <param name="sColumnID">컬럼의 ID</param>
        /// <param name="sColumnText">컬럼의 텍스트</param>
        /// <param name="columnType">컬럼의 데이터 타입</param>
        /// <param name="columnWidth">컬럼의 넓이</param>
        /// <param name="Align">컬럼 데이터 정렬 방식</param>
        /// <param name="editable">컬럼 수정 여부</param>
        public void InitColumnGrid(DataGridView dgvTemp, string sColumnID, string sColumnText, Type columnType, int columnWidth, DataGridViewContentAlignment Align, bool editable)
        {
            // 그리드 셋팅하는 함수

            // 1. 데이터 테이이블에 컬럼과 타임 셋팅
            dtTemp.Columns.Add(sColumnID, columnType);
            // 2. 그리드뷰에 컬럼 셋팅
            dgvTemp.DataSource = dtTemp;
            // 3. 컬럼에 한글 명칭 Text 부여
            dgvTemp.Columns[sColumnID].HeaderText = sColumnText;
            // 4. 컬럼의 폭 부여
            dgvTemp.Columns[sColumnID].Width= columnWidth;
            // 5. 컬럼의 데이터 정렬
            dgvTemp.Columns[sColumnID].DefaultCellStyle.Alignment = Align;
            // 7. 데이터 수정 여부
            dgvTemp.Columns[sColumnID].ReadOnly = !editable;
        }
    }
}
