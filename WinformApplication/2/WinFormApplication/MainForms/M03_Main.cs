using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assamble;

namespace MainForms
{
    public partial class M03_Main : Form
    {
        public M03_Main()
        {
            //로그인 창 실행
            M01_Login M01 = new M01_Login();
            M01.ShowDialog();

            //로그인 성공 여부를 받아서 MAIN 화면 띄우는거 결정
            if (!Convert.ToBoolean(M01.Tag))
            {
                //로그인 실패시 메인창 종료
                
                //현재 클래스 종료
                //객체의 생성자 멤버에서 Close()실행 시 정상 종료 할 수 없음.
                //this.Close();

                //Application 클래스를 사용하여 프로세스의 강제 종료 처리.
                //현 시점에서 어플리케이션 강제 종료.
                Environment.Exit(0);
            }
            // 폼에 있는 컨트롤 디자인을 초기화 하여 구성함.
            InitializeComponent();

           




        }
    }
}
