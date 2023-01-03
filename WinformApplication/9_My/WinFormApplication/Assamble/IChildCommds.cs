using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assamble
{
    public interface IChildCommds
    {
        /*
         
         인터페이스(Interface)
            - 클래스와 비슷한 구조로 메서드 , 필드 맴버를 갖지만, 코딩상으로 로직을 직접 구현하지 않고 단지 정의만 적용한다.
            - 다른 클래스가 해당 인터페이스를 상속 하는 경우 인터페이스의 모든 멤버에 대한 구현이 적용 되어야 한다.

         * 상속 받는 클래스가 구현해야 할 기준 메서드와 필드 멤버를 미리 규정해두고 해당 이름으로 로직을 구현 할 수 잇게 하는 속성.
            - 여러 작업자가 동시에 개발을 진행 할 때 공통적으로 사용되는 기능에 대해서는 정해진 메서드 이름으로 통일하여 사용 하도록 하는 개발 방법.

         */

        //인터페이스는 접근제한자를 지정할수 없고, 모두 public으로 만들어진다.
        void DoInquire(); // 조회 버튼 기능
        void DoNew();     // 추가 버튼
        void DoDelete();  // 삭제 버튼
        void DoSave();    // 저장 버튼
    }
}
