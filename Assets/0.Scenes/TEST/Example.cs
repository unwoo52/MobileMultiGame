using UnityEngine;
using System.Collections.Generic;
using System;

namespace TEST
{
    public class Example : MonoBehaviour
    {
        [SerializeField]
        long flag;
        [SerializeField]
        uint newflag = 0;
        private void Start()
        {
            FlagTool.SetFlag(ref newflag, "buildObject", false);
            FlagTool.SetFlag(ref newflag, "player Data", false);
            Debug.Log(FlagTool.PrintFailedData(LoadData.loadDataFlag, newflag));
        }
    }
}
