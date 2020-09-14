using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class LocalHost : MonoBehaviour
{

    void Start()
    {
        //print(Network.player.ipAddress);
        print(LocalIPAddress());
        //print(NetworkManager.singleton.networkAddress);
        string hostName = System.Net.Dns.GetHostName();
        string localIP = System.Net.Dns.GetHostEntry(hostName).AddressList[0].ToString();

        print(hostName);
        print(localIP);
    }

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

}
