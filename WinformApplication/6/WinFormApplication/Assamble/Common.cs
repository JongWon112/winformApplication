using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 클래스 라이브러리 = namespace = 프로젝트 = DLL 파일.
namespace Assamble
{
    // 클래스 라이브러리
    // 하나 이상의 APP 또는 프로젝트에서 호출되는 변수 및 메서드(로직) 등을 작성하여 DLL 파일로 제공 할 수 있게 만든 프로젝트 형식
    // 단독으로 실행 되지 않고 외부 프로그램에서 참조해서 호출하는 구조.
    // 배포 및 재사용이 용이, 보안성 향상, 의 장점이 있다.

    public class Common
    {
        //const 상수는 기본적으로 static 속성을 가지고  있기 때문에 외부에서 사용 시 객체 생성 없이 사용 할 수있다.
        public const string sConn = "Data Source = (local); Initial Catalog  = AppDev; Integrated Security = SSPI;";
        public static string sUserID = "";
        public static string sUserName = "";
    }

    public class SelectVar
    {
        
    }
}
