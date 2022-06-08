using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : GenericSingleton<MenuManager>
{
    private Stack<Menu> menuStack = new Stack<Menu>();

    public void OpenMenu(Menu menuInstance)
    {
        if(menuInstance != null)
        {
            if (menuStack.Count > 0)
            {
                foreach (Menu menu in menuStack)
                {
                    menu.gameObject.SetActive(false);
                }
            }

            menuInstance.gameObject.SetActive(true);
            menuStack.Push(menuInstance);
        }    
    }

    public void CloseMenu()
    {
        if(menuStack.Count == 0)
        {
            return;
        }

        Menu topMenu = menuStack.Pop();
        topMenu.gameObject.SetActive(false);

        if (menuStack.Count > 0)
        {
            Menu nextMenu = menuStack.Peek();
            nextMenu.gameObject.SetActive(true);
        }
    }

    public void ClearMenuStack()
    {

    }
}
