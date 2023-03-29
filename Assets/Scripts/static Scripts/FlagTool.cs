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

        // �÷��׸� ��Ʈ �����Ͽ� ������ �����͸� ����Ʈ�� �߰��մϴ�.
        foreach (var pair in t)
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

    public static bool SetFlag(ref byte flag, string Indexname, bool value)
    {
        if (!LoadData.loadDataFlag.TryGetValue(Indexname, out byte bitIndex))
        {
            Debug.LogError($"{Indexname} �����Ϳ� ���� �÷��� ��Ʈ�� ã�� �� �����ϴ�.");
            return false;
        }

        if (value)
        {
            flag |= (byte)(1 << bitIndex); // ��Ʈ�� 1�� ����
        }
        else
        {
            flag &= (byte)~(1 << bitIndex); // ��Ʈ�� 0���� ����
        }

        return true;
    }

    public static bool SetFlag(ref ushort flag, string Indexname, bool value)
    {
        if (!LoadData.loadDataFlag.TryGetValue(Indexname, out byte bitIndex))
        {
            Debug.LogError($"{Indexname} �����Ϳ� ���� �÷��� ��Ʈ�� ã�� �� �����ϴ�.");
            return false;
        }

        if (value)
        {
            flag |= (ushort)(1 << bitIndex); // ��Ʈ�� 1�� ����
        }
        else
        {
            flag &= (ushort)~(1 << bitIndex); // ��Ʈ�� 0���� ����
        }

        return true;
    }

    public static bool SetFlag(ref uint flag, string Indexname, bool value)
    {
        if (!LoadData.loadDataFlag.TryGetValue(Indexname, out byte bitIndex))
        {
            Debug.LogError($"{Indexname} �����Ϳ� ���� �÷��� ��Ʈ�� ã�� �� �����ϴ�.");
            return false;
        }

        if (value)
        {
            flag |= (uint)(1 << bitIndex); // ��Ʈ�� 1�� ����
        }
        else
        {
            flag &= (uint)~(1 << bitIndex); // ��Ʈ�� 0���� ����
        }

        return true;
    }
}
