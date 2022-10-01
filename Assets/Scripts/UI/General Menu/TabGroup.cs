using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour {
    public List<TabButton> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public Color textIdle;
    public Color textActive;

    private TabButton selectedTab;

    public void Subscribe(TabButton button) {
        if (tabButtons == null) tabButtons = new List<TabButton>();
        tabButtons.Add(button);
        ResetTabs();
    }

    public void OnTabEnter(TabButton button) {
        if (button == selectedTab) return;
        ResetTabs();
        button.background.color = tabHover;
    }

    public void OnTabExit(TabButton button) {
        if (button == selectedTab) return;
        ResetTabs();
    }

    public void OnTabSelected(TabButton button) {
        selectedTab = button;
        ResetTabs();
        button.background.color = tabActive;
        button.text.color = textActive;
        if (button.tab != null) button.tab.SetActive(true);
    }

    public void ResetTabs() {
        foreach (TabButton b in tabButtons) {
            if (b == selectedTab) continue;
            b.background.color = tabIdle;
            b.text.color = textIdle;
            if (b.tab != null) b.tab.SetActive(false);
        }
    }
}
