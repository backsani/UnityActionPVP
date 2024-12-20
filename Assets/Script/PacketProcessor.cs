using ServerUtil.Header;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketProcessor
{
    //패킷 관련 변수
    //public List<Packet> packetData = new List<Packet>(10);
    private LoginPacket PK_login = new LoginPacket();
    private MatchPacket PK_match = new MatchPacket();
    private IngamePacket PK_ingame = new IngamePacket();


    private readonly Dictionary<HeaderType, Packet> packetData = new Dictionary<HeaderType, Packet>();

    Buffer_Converter bufferCon = new Buffer_Converter();

    //HeaderType : ServerUtil.Header.HeaderType
    private readonly Dictionary<HeaderType, Action<byte[], Packet>> handlers = new Dictionary<HeaderType, Action<byte[], Packet>>();
    

    public PacketProcessor()
    {
        packetData[HeaderType.ACCEPT] = PK_login;
        packetData[HeaderType.MATCH] = PK_match;
        packetData[HeaderType.INGAME] = PK_ingame;

        handlers[HeaderType.ACCEPT] = HandleLoginProcesse;
        handlers[HeaderType.MATCH] = HandleMatchProcesse;
        handlers[HeaderType.INGAME] = HandleIngameProcesse;
    }

    public void ProcessBuffer(byte[] buffer)
    {
        Debug.Log("ProcessBuffer 성공");
        HeaderType header = (HeaderType)bufferCon.GetHeaderType(buffer);

        if (handlers.TryGetValue(header, out var handler))
        {
            handler.Invoke(buffer, packetData[header]);
        }
        else
        {
            Debug.LogWarning($"알 수 없는 메시지 타입: {header}");
        }
    }

    public void HandleLoginProcesse(byte[] buffer, Packet packet)
    {
        Debug.Log("HandleLoginProcesse 성공" + ServerConnect.Instance.currentState);
        byte[] result = new byte[256];
        result = packet.DeSerialzed(buffer);
        Debug.Log(Encoding.UTF8.GetString(result));

        if(ServerConnect.Instance.currentState == ConnectionState.LOGIN_SUCCESS)
        {
            SceneManager.LoadScene("MatchingScene");
            Debug.Log("로그인 성공");
        }
    }

    public void HandleMatchProcesse(byte[] buffer, Packet packet)
    {
        Debug.Log("HandleMatchProcesse 성공" + ServerConnect.Instance.currentState);
        byte[] result = new byte[256];
        result = packet.DeSerialzed(buffer);

        if (ServerConnect.Instance.currentState == ConnectionState.MATCH_FIND)
        {
            SceneManager.LoadScene("MatchMakingScene");
        }
        else if (ServerConnect.Instance.currentState == ConnectionState.MATCH_ACCEPT)
        {
            
        }
        else if (ServerConnect.Instance.currentState == ConnectionState.MATCH_REFUSE)
        {
            Debug.Log("매칭 거절");
        }
    }
    
    public void HandleIngameProcesse(byte[] buffer, Packet packet)
    {
        byte[] result = new byte[256];
        result = packet.DeSerialzed(buffer);
        Debug.Log("HandleIngameProcesse 성공" + ServerConnect.Instance.currentState);

        if (ServerConnect.Instance.currentState == ConnectionState.INGAME_INIT)
        {
            ServerConnect.Instance.myClientIndex = BitConverter.ToInt32(result);
            Debug.Log("clientInit : " + BitConverter.ToString(result));

            SceneManager.LoadScene("BattleScene");
        }
        else if (ServerConnect.Instance.currentState == ConnectionState.INGAME_MOVE)
        {
            
        }
        else if (ServerConnect.Instance.currentState == ConnectionState.MATCH_REFUSE)
        {
            Debug.Log("매칭 거절");
        }
    }
}
