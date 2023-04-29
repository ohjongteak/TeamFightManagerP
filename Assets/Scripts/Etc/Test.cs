using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� 
/// 1) ���� �о Json ���Ϸ� ��ȯ
/// 2) ���� �о �ڵ� �ڵ�����
/// </summary>

namespace Framework
{
    //delegate : �Լ��� ���� �� �ִ� ������ Ÿ���� ���� �� �ִ� Ű����
    public class Test : MonoBehaviour
    {
        //��ȯ�� void �Ű����� void�� �Լ��� ���� �� �ִ� ������ Ÿ���� ����
        delegate void VoidDelegate();
        delegate void VoidDelegate<T>(T t);
        delegate bool BoolDelegate();

        //VoidDelegate ������ Ÿ���� ���� ����
        VoidDelegate _voidDelegate;
        VoidDelegate<int> _intParamDelegate;

        private void Update()
        {
            //TestFunc�̶�� �Լ��� �޸� �ּҸ� _voidDelegate������ �־��ش�
            _voidDelegate += TestFunc;
            _voidDelegate += TestFunc2;

            //��� �ξ��� �Լ� ����
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
