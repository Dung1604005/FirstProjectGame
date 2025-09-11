using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<MenuTab> menuTabs;
    [SerializeField] private List<MenuLayOutUI> menuLayOutUIs;

    private int currentTabIndex = 0;
    public int CurrentTabIndex => currentTabIndex;

    public void OpenMenu()
    {
        this.gameObject.SetActive(true);
        menuTabs[currentTabIndex].ActiveTab();
        menuLayOutUIs[currentTabIndex].TurnOn();
    }
    public void CloseMenu()
    {
        this.gameObject.SetActive(false);

        menuLayOutUIs[currentTabIndex].TurnOff();
    }
    public void SwitchTab()
    {
        int curTab = currentTabIndex;
        int nextTab = (currentTabIndex + 1) % menuLayOutUIs.Count;
        menuTabs[curTab].InactiveTab();
        menuLayOutUIs[curTab].TurnOff();
        menuTabs[nextTab].ActiveTab();
        menuLayOutUIs[nextTab].TurnOn();
        currentTabIndex = nextTab;
    }
}
