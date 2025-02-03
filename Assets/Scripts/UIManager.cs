using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Sliding GameObject UI")]
    public RectTransform mainMenu, addPlayerMenu, SpinMenu, GameMenu, SettingsMenu;

    private DS_SpinWheel spinWheel;

    // Start is called before the first frame update
    void Start()
    {
        spinWheel = GameObject.Find("Turner").GetComponent<DS_SpinWheel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void slideMainMenu()
    {
        mainMenu.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void slideAddPlayerUI()
    {
        mainMenu.DOAnchorPos(new Vector2(0, -2300), 0.25f);
        addPlayerMenu.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(0.25f);
    }

    public void slideSpinUI()
    {
        addPlayerMenu.DOAnchorPos(new Vector2(1300, 0), 0.25f);
        SpinMenu.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

        public void slideOutAddPlayerUIAddMenu()
    {
        //Slide out Add Player
        addPlayerMenu.DOAnchorPos(new Vector2(1300, 0), 0.25f);
        //Slide in Main Menu UI
        mainMenu.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(0.25f);
    }

    public void slideGameUI()
    {
        SpinMenu.DOAnchorPos(new Vector2(-1300, 0), 0.25f);
        GameMenu.DOAnchorPos(new Vector2(0, 0), 0.01f).SetDelay(0.25f);
    }

    public void slideMenuUI()
    {
        mainMenu.DOAnchorPos(new Vector2(0, 0), 0.25f);
        GameMenu.DOAnchorPos(new Vector2(1300, 0), 0.01f).SetDelay(0.25f);
    }

    // Slide in Add Player UI after gameplay
    public void AddPlayer()
    {
        GameMenu.DOAnchorPos(new Vector2(1300, 0), 0.25f);
        addPlayerMenu.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(0.25f);
    }

    public void AddPlayerUI()
    {
        SpinMenu.DOAnchorPos(new Vector2(-1300, 0), 0.25f);
        addPlayerMenu.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(0.25f);
    }

     public void undoSpinMenu()
    {       
        GameMenu.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(5.2f);
        Invoke("SpinMenuShift", 5.2f);
    }

    // Made into a method, so that it can be invoked while the tweening Anim plays from DS_SpinWheel.cs
    public void SpinMenuShift()
    {
        SpinMenu.DOAnchorPos(new Vector2(-1300, 0), 0.25f);
    }

    public void slideInSpinUI()
    {
        SpinMenu.DOAnchorPos(new Vector2(0, 0), 0.25f);
        GameMenu.DOAnchorPos(new Vector2(1300, 0), 0.25f);
    }

     public void slideSettingsUI()
    {
        SettingsMenu.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void undoSettingsUI()
    {
        SettingsMenu.DOAnchorPos(new Vector2(1300, 0), 0.25f);
    }
}
