using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MatchPacket : Packet
{
    public MatchPacket() : base() { }

    public override byte[] DeSerialzed(byte[] buffer)
    {
        byte[] dataValue = new byte[256];
        int Length = 0;
        byte[] dataStateType = new byte[sizeof(int)];

        Length = UnpackingHeader(buffer);

        Buffer.BlockCopy(buffer, Length, dataStateType, 0, dataStateType.Length);
        Length += dataStateType.Length;

        ServerConnect.Instance.currentState = (ServerUtil.Header.ConnectionState)BitConverter.ToInt32(dataStateType);

        Buffer.BlockCopy(buffer, Length, dataValue, 0, _packetHeader.Length - Length);

        return dataValue;
    }

    public override byte[] Serialzed(string buffer)
    {
        byte[] byteState = BitConverter.GetBytes((int)ServerConnect.Instance.currentState);

        byte[] header = PackingHeader(ServerUtil.Header.HeaderType.MATCH, buffer.Length + byteState.Length);

        byte[] data = BitConverter.GetBytes(int.Parse(buffer));

        byte[] result = new byte[header.Length + data.Length];

        Buffer.BlockCopy(header, 0, result, 0, header.Length);                      // PK_Data

        Buffer.BlockCopy(data, 0, result, header.Length, data.Length);

        return result;
    }
}