using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class Timer : MonoBahaviourSingleton<Timer>
    {
        //public delegate void VoidFunc();

        //Action,Func : ������ delegate�� ������ Ÿ�� ����� �������� ��������ӿ�ũ���� �̸� ���ø� ���·� ������ ���Ҵ�
        //Action : ��ȯ�� void �� �ֵ� ��� ������ Ÿ��
        //Func : ��ȯ���� void�� �ƴ� �ֵ� ��� ������ Ÿ��

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