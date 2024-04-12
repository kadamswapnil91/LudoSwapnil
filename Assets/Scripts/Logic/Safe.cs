using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class Safe
{
	public Safe(int player_id, int figure_id, int count_steps, int position, int index) {
        this.player_id = player_id;
        this.figure_id = figure_id;
        this.count_steps = count_steps;
        this.position = position;
        this.index = index;
        this.isFastLudo = isFastLudo;
    }
    
     public int player_id { get; set; }
     public int figure_id { get; set; }
     public int count_steps { get; set; }
     public int position { get; set; }
     public int index { get; set; }
     public bool isFastLudo { get; set; }
}
