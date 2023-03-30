using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagTool
{
    /// <summary>
    /// dictionary�� flag�� �޾Ƽ�, flag ���� Ȱ��ȭ�� ��Ʈ�� �����Ǵ� dictionary value������ ����� �α׷� ����մϴ�.
    /// </summary>
    /// 
    public static bool PrintFailedData<T, U>(T dictionary, U flag)
    where T : IDictionary<string, byte>
    where U : struct, IConvertible
    {
        List<string> failedData = new List<string>();

        // �÷��׸� ��Ʈ �����Ͽ� ������ �����͸� ����Ʈ�� �߰��մϴ�.
        foreach (var pair in dictionary)
        {
            if ((flag.ToInt64(null) & (1L << pair.Value)) != 0)
                failedData.Add(pair.Key);
        }

        // ������ ������ �̸��� �̿��Ͽ� �α׸� ����մϴ�.
        if (failedData.Count > 0)
        {
            string dataNames = string.Join(", ", failedData.ToArray());
            string message = $"{dataNames} �������� �ε忡 �����߽��ϴ�.";
            Debug.LogError(message);
            return false;
        }
        else return true;
    }

    public static bool SetBit(ref byte flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(byte))
        {
            Debug.LogError($"�߸��� ��Ʈ �ε����Դϴ�: {bitIndex}");
            return false;
        }

        flag |= (byte)(1 << bitIndex);
        return true;
    }

    public static bool SetBit(ref short flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(short))
        {
            Debug.LogError($"�߸��� ��Ʈ �ε����Դϴ�: {bitIndex}");
            return false;
        }

        flag |= (short)(1 << bitIndex);
        return true;
    }

    public static bool SetBit(ref int flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(int))
        {
            Debug.LogError($"�߸��� ��Ʈ �ε����Դϴ�: {bitIndex}");
            return false;
        }

        flag |= (1 << bitIndex);
        return true;
    }

    public static bool SetBit(ref long flag, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex >= 8 * sizeof(long))
        {
            Debug.LogError($"�߸��� ��Ʈ �ε����Դϴ�: {bitIndex}");
            return false;
        }

        flag |= (1L << bitIndex);
        return true;
    }
}
