using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class Timer : MonoBahaviourSingleton<Timer>
    {
        //public delegate void VoidFunc();

        //Action,Func : 일일이 delegate로 데이터 타입 만들기 귀찮으니 닷넷프레임워크에서 미리 템플릿 형태로 정의해 놓았다
        //Action : 반환형 void 인 애들 담는 데이터 타입
        //Func : 반환형이 void가 아닌 애들 담는 데이터 타입

        Action<int> _test;
        Action<int, float> _test2;
        Action<int, int, int> _test3;

        Func<bool> _func;
        Func<int, bool> _func2;
        Func<int, float, bool> _func3;

        public void ExecuteTimer(float waitTime, Action func)
        {
            StartCoroutine(CoroutineTimer(waitTime, func));
        }

        IEnumerator CoroutineTimer(float waitTime, Action func)
        {
            yield return new WaitForSeconds(waitTime);

            func();
        }
    }
}