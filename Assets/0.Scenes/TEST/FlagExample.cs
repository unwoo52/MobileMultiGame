using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace TEST
{
    public class FlagExample : MonoBehaviour
    {
        [SerializeField]
        long flag;
        [SerializeField]
        int newflag = 0;
        private void Start()
        {
            /*
            FlagTool.SetBit(ref newflag, 1);
            FlagTool.SetBit(ref newflag, 3);
            Debug.Log(FlagTool.PrintFailedData(LoadDataFlag.loadDataFlag, newflag));*/
        }

    }
}
