using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagTool
{
    public static bool PrintFailedData<T, U>(T t, U flag)
    where T : IDictionary<string, byte>
    where U : struct, IConvertible
    {
        List<string> failedData = new List<string>();

        // 플래그를 비트 연산하여 실패한 데이터를 리스트에 추가합니다.
        foreach (var pair in t)
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

    public static bool SetFlag(ref byte flag, string Indexname, bool value)
    {
        if (!LoadData.loadDataFlag.TryGetValue(Indexname, out byte bitIndex))
        {
            Debug.LogError($"{Indexname} 데이터에 대한 플래그 비트를 찾을 수 없습니다.");
            return false;
        }

        if (value)
        {
            flag |= (byte)(1 << bitIndex); // 비트를 1로 설정
        }
        else
        {
            flag &= (byte)~(1 << bitIndex); // 비트를 0으로 설정
        }

        return true;
    }

    public static bool SetFlag(ref ushort flag, string Indexname, bool value)
    {
        if (!LoadData.loadDataFlag.TryGetValue(Indexname, out byte bitIndex))
        {
            Debug.LogError($"{Indexname} 데이터에 대한 플래그 비트를 찾을 수 없습니다.");
            return false;
        }

        if (value)
        {
            flag |= (ushort)(1 << bitIndex); // 비트를 1로 설정
        }
        else
        {
            flag &= (ushort)~(1 << bitIndex); // 비트를 0으로 설정
        }

        return true;
    }

    public static bool SetFlag(ref uint flag, string Indexname, bool value)
    {
        if (!LoadData.loadDataFlag.TryGetValue(Indexname, out byte bitIndex))
        {
            Debug.LogError($"{Indexname} 데이터에 대한 플래그 비트를 찾을 수 없습니다.");
            return false;
        }

        if (value)
        {
            flag |= (uint)(1 << bitIndex); // 비트를 1로 설정
        }
        else
        {
            flag &= (uint)~(1 << bitIndex); // 비트를 0으로 설정
        }

        return true;
    }
}
