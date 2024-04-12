using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class MqMessage
{
    public MqMessage(string message)
    {
        this.message = message;
    }

    public string message { get; set; }
}
