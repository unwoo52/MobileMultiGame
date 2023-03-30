using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagTool
{
    /// <summary>
    /// dictionary와 flag를 받아서, flag 내에 활성화된 비트에 대응되는 dictionary value값들을 디버그 로그로 출력합니다.
    /// </summary>
    /// 
    public static bool PrintFailedData<T, U>(T dictionary, U flag)
    where T : IDictionary<string, byte>
    where U : struct, IConvertible
    {
        List<string> failedData = new List<string>();

        // 플래그를 비트 연산하여 실패한 데이터를 리스트에 추가합니다.
        foreach (var pair in dictionary)
        {
            if ((flag.ToInt64(null) & (1L << pair.Value)) != 0)
                failedData.Add(pair.Key);
        }

        // 실패한 데이터 이름을 이용하여 로그를 출력합니다.
        if (failedData.Count > 0)
        {
            string dataNames = string.Join(", ", failedData.ToArray());
            string message = $"{dataNames} 데이터의 로드에 실패했습니다.";
            Debug.LogError(message);
            return false;
        }
        else return true;
    }

    public static bool SetBit(ref byte flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(byte))
        {
            Debug.LogError($"잘못된 비트 인덱스입니다: {bitIndex}");
            return false;
        }

        flag |= (byte)(1 << bitIndex);
        return true;
    }

    public static bool SetBit(ref short flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(short))
        {
            Debug.LogError($"잘못된 비트 인덱스입니다: {bitIndex}");
            return false;
        }

        flag |= (short)(1 << bitIndex);
        return true;
    }

    public static bool SetBit(ref int flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(int))
        {
            Debug.LogError($"잘못된 비트 인덱스입니다: {bitIndex}");
            return false;
        }

        flag |= (1 << bitIndex);
        return true;
    }

    public static bool SetBit(ref long flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(long))
        {
            Debug.LogError($"잘못된 비트 인덱스입니다: {bitIndex}");
            return false;
        }

        flag |= (1L << bitIndex);
        return true;
    }
}
