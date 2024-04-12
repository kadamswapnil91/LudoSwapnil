using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerPieceEvent
{
    public int playerId;
    public int figureId;
    public int pos;
    public int dice_value;
    public int player_chance;
    public int sixCount;
}
