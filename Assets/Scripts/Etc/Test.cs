using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터 툴 
/// 1) 엑셀 읽어서 Json 파일로 변환
/// 2) 엑셀 읽어서 코드 자동생성
/// </summary>

namespace Framework
{
    //delegate : 함수를 담을 수 있는 데이터 타입을 만들 수 있는 키워드
    public class Test : MonoBehaviour
    {
        //반환형 void 매개변수 void인 함수를 담을 수 있는 데이터 타입을 선언
        delegate void VoidDelegate();
        delegate void VoidDelegate<T>(T t);
        delegate bool BoolDelegate();

        //VoidDelegate 데이터 타입의 변수 선언
        VoidDelegate _voidDelegate;
        VoidDelegate<int> _intParamDelegate;

        private void Update()
        {
            //TestFunc이라는 함수의 메모리 주소를 _voidDelegate변수에 넣어준다
            _voidDelegate += TestFunc;
            _voidDelegate += TestFunc2;

            //담아 두었던 함수 실행
            if (_voidDelegate != null)
                _voidDelegate();

            _voidDelegate -= TestFunc;
            _voidDelegate -= TestFunc2;

            _voidDelegate = null;

            _intParamDelegate = TestFuncIntParam;
        }

        void TestFunc()
        {

        }

        void TestFunc2()
        {

        }

        void TestFuncIntParam(int i)
        {

        }
    }
}
