using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IngamePacket : Packet
{
    public IngamePacket() : base() { }

    public override byte[] DeSerialzed(byte[] buffer)
    {
        byte[] dataValue = new byte[4];
        int Length = 0;
        byte[] dataStateType = new byte[sizeof(int)];

        Length = UnpackingHeader(buffer);

        Buffer.BlockCopy(buffer, Length, dataStateType, 0, dataStateType.Length);
        Length += dataStateType.Length;

        ServerConnect.Instance.currentState = (ServerUtil.Header.ConnectionState)BitConverter.ToInt32(dataStateType);

        //어떤 플레이어의 정보인지 받기
        int currentClietIndex = BitConverter.ToInt32(buffer, Length);
        if (currentClietIndex == 100)
        {
            ServerConnect.Instance.myClientIndex = currentClietIndex;
            Length += sizeof(Int32);
        }


        //해당 플레이어의 좌표 받기
        float posX = BitConverter.ToSingle(buffer, Length);
        Length += sizeof(float);

        float posY = BitConverter.ToSingle(buffer, Length);
        Length += sizeof(float);

        float rotation = BitConverter.ToSingle(buffer, Length);
        Length += sizeof(float);

        float hp = BitConverter.ToSingle(buffer, Length);
        Length += sizeof(float);

        ServerConnect.Instance.clientInfo[currentClietIndex].mTransform = new Vector3(posX, posY, 0);

        ServerConnect.Instance.clientInfo[currentClietIndex].rotation = rotation;

        ServerConnect.Instance.clientInfo[currentClietIndex].hp = hp;

        dataValue = BitConverter.GetBytes(currentClietIndex);

        return dataValue;
    }

    public override byte[] Serialzed(string buffer)
    {
        int Length = 0;

        byte[] byteState = BitConverter.GetBytes((int)ServerConnect.Instance.currentState);

        //------------------------------------------
        int clientIndex = ServerConnect.Instance.clientInfo[ServerConnect.Instance.myClientIndex].sessionIndex;

        UnityEngine.Vector3 clientPos = ServerConnect.Instance.clientInfo[ServerConnect.Instance.myClientIndex].mTransform;

        float clientRotation = ServerConnect.Instance.clientInfo[ServerConnect.Instance.myClientIndex].rotation;

        float clientHp = ServerConnect.Instance.clientInfo[ServerConnect.Instance.myClientIndex].hp;

        byte[] dataClientIndex = BitConverter.GetBytes(clientIndex);
        byte[] dataPosX = BitConverter.GetBytes(clientPos.x);
        byte[] dataPosY = BitConverter.GetBytes(clientPos.y);
        byte[] dataRotation = BitConverter.GetBytes(clientRotation);
        byte[] dataHp = BitConverter.GetBytes(clientHp);
        //------------------------------------------

        byte[] header = PackingHeader(ServerUtil.Header.HeaderType.INGAME, byteState.Length + dataClientIndex.Length + dataPosX.Length + dataPosY.Length + dataRotation.Length + dataHp.Length);

        //공사중

        byte[] data = BitConverter.GetBytes(int.Parse(buffer));

        

        byte[] result = new byte[header.Length + byteState.Length + dataClientIndex.Length + dataPosX.Length + dataPosY.Length + dataRotation.Length + dataHp.Length];

        

        Buffer.BlockCopy(header, 0, result, 0, header.Length);                      // PK_Data
        Length += header.Length;

        Buffer.BlockCopy(data, 0, result, Length, data.Length);
        Length += data.Length;

        Buffer.BlockCopy(dataClientIndex, 0, result, Length, dataClientIndex.Length);
        Length += dataClientIndex.Length;

        Buffer.BlockCopy(dataPosX, 0, result, Length, dataPosX.Length);
        Length += dataPosX.Length;

        Buffer.BlockCopy(dataPosY, 0, result, Length, dataPosY.Length);
        Length += dataPosY.Length;

        Buffer.BlockCopy(dataRotation, 0, result, Length, dataRotation.Length);
        Length += dataRotation.Length;

        Buffer.BlockCopy(dataHp, 0, result, Length, dataHp.Length);
        Length += dataHp.Length;

        return result;
    }
}
