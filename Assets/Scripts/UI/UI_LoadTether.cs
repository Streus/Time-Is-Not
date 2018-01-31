using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadTether : MonoBehaviour {

	public void LoadPoint(int state)
    {
        LevelStateManager.loadTetherPoint(state);
    }
}
