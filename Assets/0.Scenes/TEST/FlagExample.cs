using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace TEST
{
    public class FlagExample : MonoBehaviour
    {

        private Dictionary<string, byte> loadDataFlag = new Dictionary<string, byte>()
    {
        {"buildObject Data", 1},
        {"enemy Data", 2},
        {"player Data", 3},
        {"time Data", 4},
        {"none1", 5},
        {"none2", 6},
        {"none3", 7}
    };

        [SerializeField]
        long flag;
        [SerializeField]
        int newflag = 0;
        [SerializeField]
        int newflag2 = 0;
        private void Start()
        {

            FlagTool.SetBit(ref newflag2, 1, true);
            FlagTool.SetBit(ref newflag2, 3, true);
            Debug.Log(FlagTool.PrintFailedData(loadDataFlag, newflag2));

            FlagTool.SetBit(ref flag, 1, true);
            FlagTool.SetBit(ref flag, 3, true);
            Debug.Log(FlagTool.PrintFailedData(loadDataFlag, flag));

            FlagTool.SetBit(ref newflag, 1, true);
            FlagTool.SetBit(ref newflag, 3, true);
            Debug.Log(FlagTool.PrintFailedData(loadDataFlag, newflag));
        }
    }
}
