using System;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;


public class ChirpMessage : MessageBase
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

    public override uint GetSenderID()
    {
        return id;

    }

    public override string GetSenderName()
    {
        return sender;
    }

    public override string GetText()
    {
        return message;
    }

    public override bool IsSimilarMessage(MessageBase other)
    {
        var m = other as ChirpMessage;
        return m != null && ((m.sender == sender && m.id == id) || m.message.Replace("#", "") == message.Replace("#", ""));
    }

    public override void Serialize(ColossalFramework.IO.DataSerializer s)
    {
        s.WriteSharedString(sender);
        s.WriteUInt32(id);
        s.WriteSharedString(message);
    }

    public override void Deserialize(ColossalFramework.IO.DataSerializer s)
    {
        sender = s.ReadSharedString();
        id = s.ReadUInt32();
        message = s.ReadSharedString();
    }

    public override void AfterDeserialize(ColossalFramework.IO.DataSerializer s)
    {
    }

}