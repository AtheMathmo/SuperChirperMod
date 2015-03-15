using System;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;


public class ChirpMessage : IChirperMessage
{
    private string message;
    private string sender;
    private uint id;

    public ChirpMessage(string sender, string message, uint id)
    {
        this.sender = sender;
        this.message = message;
        this.id = id;
    }

    public uint senderID
    {
        get { return id; }
    }

    public string senderName
    {
        get { return sender; }
    }

    public string text
    {
        get { return message; }
    }
}