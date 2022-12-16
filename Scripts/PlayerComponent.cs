using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    public UIManager uiManager;
    public int index;
    public Player player;

    public void UpdateUI() {
        uiManager.UpdateQuickPlayerViewer(this.player);
    }
}
