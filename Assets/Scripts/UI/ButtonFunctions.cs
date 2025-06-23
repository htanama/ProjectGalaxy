using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor;
using TMPro;
using JetBrains.Annotations;
using System;
using System.Reflection;

/* 
 * Tried to split the menus and the buttons into separate classes, but unity is being hard-headed     
 */
public class ButtonFunctions : MonoBehaviour
{
    //SceneManagerScript sceneManager;
    //playerScript PlayerScript;

    [Header("===== MENUS =====")]
    //public Vector3 centerPos = Vector3.zero; 
    Vector3 onScreenPos;
    Vector3 offScreenPos;
    Vector3 headerOnScreenPos;
    Vector3 headerOffScreenPos;
    

    //[SerializeField] private float moveSpeed = 5f;  // Speed of movement
    //[SerializeField] private float fadeSpeed = 1f;  // Speed of fading

    // Header Bar //
    [SerializeField] GameObject headerBar;
    
    // Buttons //
    //public List<Transform> buttonPositions;
    public List<UnityEngine.UI.Button> buttons;
    public TextMeshProUGUI centerTitle;
    public GameObject titleGroup;
    public GameObject selectedMenu;

    // Background Screen //
    [SerializeField] GameObject backgroundScreen;
    private CanvasGroup backgroundGroup;
    public GameObject BackgroundScreen
    { get => backgroundScreen; set => backgroundScreen = value; }

    // Pause Menu //
    [SerializeField] GameObject pauseMenu;
    private CanvasGroup pauseGroup;    
    public GameObject PauseMenu
    { get => pauseMenu; set => pauseMenu = value; }
    
    // Title //
    GameObject pauseTitle;
    private TextMeshProUGUI titleText;
    public GameObject PauseTitle
    { get => pauseTitle; set => pauseTitle = value; }
    GameObject buttonsParent;

    // Menu Base //
    [SerializeField] GameObject baseMenuBackground;
    public GameObject MenuBase
    { get => baseMenuBackground; set => baseMenuBackground = value; }
    public TMP_FontAsset chakra;
    public TMP_FontAsset glitch;
    int currentButtonIndex = 2;

    // Settings Menu //
    [SerializeField] GameObject settingsMenu;
    public GameObject SettingsMenu
    { get => settingsMenu; set => settingsMenu = value; }

    // Controls Menu //
    [SerializeField] GameObject controlsMenu;
    public GameObject ControlsMenu
    { get => controlsMenu; set => controlsMenu = value; }

    // Inventory Menu //
    [SerializeField] GameObject inventoryMenu;
    public GameObject InventoryMenu
    { get => inventoryMenu; set => inventoryMenu = value; }

    // Save Menu //
    [SerializeField] GameObject saveMenu;
    private TextMeshProUGUI timeDateStamp;
    public GameObject SaveMenu
    { get => saveMenu; set => saveMenu = value; }
    public TextMeshProUGUI TimeDateStamp
    { get => timeDateStamp; set => timeDateStamp = value; }

    public GameObject WinScreen => winScreen;
    public GameObject LoseScreen => loseScreen;
    // Radial Menu //
    //RadialMenu radialMenu;
    //[SerializeField] GameObject radialMenuObject;


    // Credit Menu //
    [SerializeField] GameObject creditScreen;
    public GameObject CreditScreen
    { get => creditScreen; set => creditScreen = value; }

    // For Win //
    [SerializeField] GameObject winScreen;

    // For Lose //
    [SerializeField] GameObject loseScreen;

    // Loading //
    [SerializeField] GameObject loadingScreen;

    // Flags //

    [Header("===== Settings =====")]
    [SerializeField] private bool enableFlickering;

    [SerializeField] TextMeshProUGUI musicVolumeText;
    [SerializeField] TextMeshProUGUI sfxVolumeText;

    public UnityEngine.UI.Slider _musicSlider, _sfxSlider;
    public bool animationOff;

    public void Start()
    {
        //Debug
        animationOff = true;
        if(!GameManager.instance.IsWebGL && animationOff == false)
        {
            DOTween.useSafeMode = true;
        }

        onScreenPos = new Vector3(0, 0, 0);
        headerOnScreenPos = new Vector3(0, 277, 0);

        offScreenPos = baseMenuBackground.GetComponent<RectTransform>().anchoredPosition;
        headerOffScreenPos = headerBar.GetComponent<RectTransform>().anchoredPosition;

        //HEADER BUTTONS//
        //if (buttonPositions == null)
        //    buttonPositions = new List<Transform>();

        //if (buttons == null)
        //    buttons = new List<UnityEngine.UI.Button>();

        //for (int i = 0; i < buttons.Count; i++)
        //{
        //    buttons[i].transform.position = buttonPositions[i].position;
        //}

        //offScreenPos = settingsMenu.transform.position; 

        // Settings //
        //_musicSlider = _musicSlider.GetComponent<UnityEngine.UI.Slider>();
        //_sfxSlider = _musicSlider.GetComponent<UnityEngine.UI.Slider>();

        //enableFlickering = true;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NavigateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NavigateRight();
        }
    }
    public void NavigateLeft()
    {        
        currentButtonIndex = (currentButtonIndex - 1 + buttons.Count) % buttons.Count;
        buttons[currentButtonIndex].onClick.Invoke();      
    }

    public void NavigateRight()
    {
        currentButtonIndex = (currentButtonIndex + 1) % buttons.Count;
        buttons[currentButtonIndex].onClick.Invoke();
    }
    //Debug//
    // Background Screen //
    public void OpenBackgroundScreen()
    {
        backgroundScreen.SetActive(true);
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(ZoomFadeIn(backgroundScreen));
        }
        else if (animationOff)
        {

        }
        else
        {
            //DoTween
            ZoomInFade(backgroundScreen);
        }
    }
    public void CloseBackgroundScreen()
    {
        backgroundScreen.SetActive(false);
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(ZoomFadeOut(backgroundScreen));
        }
        else if (animationOff)
        {

        }
        else
        {
            //DoTween
            ZoomOutFade(backgroundScreen);
        }
    }
    // Pause Menu //
    public void OpenPauseMenuBase()
    {
       if(!backgroundScreen.activeSelf)
        { OpenBackgroundScreen(); }

        pauseMenu.SetActive(true);
        GameManager.instance.MenuActive = PauseMenu;

        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(ZoomFadeIn(pauseMenu));
        }
        else if (animationOff)
        {

        }
        else
        {
            //DoTween
            ZoomInFade(pauseMenu);
        }


    }
    public void ClosePauseMenuBase()
    {
        if(!baseMenuBackground.activeSelf)
        {
            CloseBackgroundScreen();
        }

        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(ZoomFadeOut(pauseMenu));
        }
        else if (animationOff)
        {

        }
        else
        {
            //DoTween
            ZoomOutFade(pauseMenu);
        }

        pauseMenu.SetActive(false);
        GameManager.instance.MenuActive = null;
    }
    // Menu Background //
    public void PauseSettingsButton()
    {
        GameManager.instance.MenuActive.SetActive(false);
        GameManager.instance.MenuActive = settingsMenu;
        baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = onScreenPos;
        baseMenuBackground.SetActive(true);
        
        GameManager.instance.MenuActive.SetActive(true);
        centerTitle.text = "Settings";
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(MoveMenuSelected(3));
        }
        else
        {
            MoveSelectedMenu(3);
        }
        ResetButtonFonts();
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().font = glitch;
    }
    public void MenuSettingsButton()
    {
        if(!baseMenuBackground.activeSelf)
        {
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = onScreenPos;
            baseMenuBackground.SetActive(true);
            if(!headerBar.activeSelf)
            {
                headerBar.SetActive(true);
            }
        }

        if(GameManager.instance.MenuActive != settingsMenu)
        { 
            GameManager.instance.MenuActive.SetActive(false);
            GameManager.instance.MenuActive = settingsMenu;
            GameManager.instance.MenuActive.SetActive(true);
            centerTitle.text = "Settings";
        }
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(MoveMenuSelected(3));
        }
        else
        {
            MoveSelectedMenu(3);
        }
        ResetButtonFonts();
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().font = glitch;

    }
    public void MenuCreditButton()
    {
        if (!baseMenuBackground.activeSelf)
        {
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = onScreenPos;
            baseMenuBackground.SetActive(true);
            if (!headerBar.activeSelf)
            {
                headerBar.SetActive(true);
            }
        }
        if (GameManager.instance.MenuActive != creditScreen)
        {
            GameManager.instance.MenuActive.SetActive(false);
            GameManager.instance.MenuActive = creditScreen;
            GameManager.instance.MenuActive.SetActive(true);
            centerTitle.text = "Credits";
        }
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(MoveMenuSelected(1));
        }
        else
        {
            MoveSelectedMenu(1);
        }
        ResetButtonFonts();
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().font = glitch;

    }
    public void MenuSaveButton()
    {
        if (!baseMenuBackground.activeSelf)
        {
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = onScreenPos;
            baseMenuBackground.SetActive(true);
            if (!headerBar.activeSelf)
            {
                headerBar.SetActive(true);
            }
        }
        if (GameManager.instance.MenuActive != saveMenu)
        {
            GameManager.instance.MenuActive.SetActive(false);
            GameManager.instance.MenuActive = saveMenu;
            GameManager.instance.MenuActive.SetActive(true);
            centerTitle.text = "Save/Load";
        }
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(MoveMenuSelected(2));
        }
        else
        {
            MoveSelectedMenu(2);
        }
        ResetButtonFonts();
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().font = glitch;

        //centerTitle.text = buttons[1].GetComponentInChildren<Text>().text;
        //buttons[1].gameObject.SetActive(false);
    }
    public void MenuControlsButton()
    {
        if (!baseMenuBackground.activeSelf)
        {
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = onScreenPos;
            baseMenuBackground.SetActive(true);
            if (!headerBar.activeSelf)
            {
                headerBar.SetActive(true);
            }
        }
        if (GameManager.instance.MenuActive != controlsMenu)
        {
            GameManager.instance.MenuActive.SetActive(false);
            GameManager.instance.MenuActive = controlsMenu;
            GameManager.instance.MenuActive.SetActive(true);
            centerTitle.text = "Controls";
        }
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(MoveMenuSelected(4));
        }
        else
        {
            MoveSelectedMenu(4);
        }
        ResetButtonFonts();
        buttons[3].GetComponentInChildren<TextMeshProUGUI>().font = glitch;
    }
    public void MenuInventoryButton()
    {
        if (!baseMenuBackground.activeSelf)
        {
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = onScreenPos;
            baseMenuBackground.SetActive(true);
            if (!headerBar.activeSelf)
            {
                headerBar.SetActive(true);
            }
        }
        if (GameManager.instance.MenuActive != inventoryMenu)
        {
            GameManager.instance.MenuActive.SetActive(false);
            GameManager.instance.MenuActive = inventoryMenu;
            GameManager.instance.MenuActive.SetActive(true);
            centerTitle.text = "Inventory";
        }
        
        if (GameManager.instance.IsWebGL)
        {
            StartCoroutine(MoveMenuSelected(5));
        }
        else
        {
            MoveSelectedMenu(5);
        }
        ResetButtonFonts();
        buttons[4].GetComponentInChildren<TextMeshProUGUI>().font = glitch;
    }

    public void CloseButton()
    {
        baseMenuBackground.SetActive(false);
        baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = offScreenPos;
        GameManager.instance.MenuActive = null;
        OpenPauseMenuBase();
    }
   
    public void MenuOpenCheck()
    {
        if(settingsMenu.activeSelf)
        {
            baseMenuBackground.SetActive(false); 
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = offScreenPos;
            
        }
        if (controlsMenu.activeSelf)
        {
            baseMenuBackground.SetActive(false); 
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = offScreenPos;
            
        }
        if (inventoryMenu.activeSelf)
        {

            baseMenuBackground.SetActive(false); 
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = offScreenPos;
            
        }
        if (saveMenu.activeSelf)
        {
            baseMenuBackground.SetActive(false); 
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = offScreenPos;
            
        }
        if (creditScreen.activeSelf)
        {

            baseMenuBackground.SetActive(false); 
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = offScreenPos;
            
        }
        if (baseMenuBackground.activeSelf)
        {
            baseMenuBackground.SetActive(false);
            baseMenuBackground.GetComponent<RectTransform>().anchoredPosition = offScreenPos;
        }
        if (pauseMenu.activeSelf)
        {
            ClosePauseMenuBase();
        }
        if (backgroundScreen.activeSelf)
        {
            CloseBackgroundScreen();
        }

        GameManager.instance.MenuActive = null;
    }
    public void ResetButtonFonts()
    {
        foreach (UnityEngine.UI.Button button in buttons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().font = chakra;
        }
    }

    // Animation by Platform //
    //desktop platform
    public void ZoomInFade(GameObject gameObject)
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        gameObject.transform.localScale = Vector3.zero;
        canvasGroup.DOFade(1f, 0.5f).SetUpdate(true);
        gameObject.transform.DOScale(1f, 0.5f).SetUpdate(true);
    }
    public void ZoomOutFade(GameObject gameObject)
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, 1f).SetUpdate(true);
        gameObject.transform.DOScale(0f, 1f).SetUpdate(true);
    }

    public void MoveSelectedMenu(int index)
    {
        RectTransform rectTransform = selectedMenu.GetComponent<RectTransform>();
        float duration = 2f;
        float startPos = rectTransform.anchoredPosition.x;
        
        float endPos = 5f;

        if (index == 1)
        {
            endPos = -645f;
        }
        else if (index == 2)
        {
            endPos = -315f;
        }
        else if (index == 4)
        {
            endPos = 330f;
        }
        else if (index == 5)
        {
            endPos = 660f;
        }
        else
        {
            endPos = 5f;
        }
        if (!animationOff)
        { 
            rectTransform.DOMoveX(endPos, duration).SetEase(Ease.InOutBack).SetUpdate(true);
        }
        if (animationOff)
        {
            rectTransform.anchoredPosition = new Vector2(endPos, rectTransform.anchoredPosition.y);
        }
    }




    //WebGL platform
    public IEnumerator ZoomFadeIn(GameObject gameObject)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();

        // Initial state
        rectTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        float duration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            // Scale from 0 to 1
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            // Fade from 0 to 1
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure the final state is exactly the target values
        rectTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
    }
    
    public IEnumerator ZoomFadeOut(GameObject gameObject)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        float duration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            // Scale from 1 to 0
            rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

            // Fade from 1 to 0
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure the final state is exactly the target values
        rectTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
    }
    public IEnumerator MoveMenuSelected(int index)
    {
        float timeElapsed = 0f;
        float duration = 2f;
        float startPos = selectedMenu.GetComponent<RectTransform>().anchoredPosition.x;
        RectTransform rectTransform = selectedMenu.GetComponent<RectTransform>();
        float endPos = 5f;

        if (index == 1)
        {
            endPos = -645f;
        }
        else if (index == 2)
        {
            endPos = -315f;
        }
        else if (index == 4)
        {
            endPos = 330f;
        }
        else if (index == 5)
        {
            endPos = 660f;
        }
        else
        {
            endPos = 5f;
        }
        if (animationOff)
        {
            rectTransform.anchoredPosition = new Vector2(endPos, rectTransform.anchoredPosition.y);
        }
        if(!animationOff)
        { 
            while (timeElapsed < duration)
            {
                float newX = Mathf.Lerp(startPos, endPos, timeElapsed / duration);
                rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);
                timeElapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    /* 
     * Will clean up, want to save notes for later just in case
     */
    //public IEnumerator OpenBackgroundScreen()
    //{
    //if(DOTween.TotalPlayingTweens() > 0)
    //    {

    //    }
    //    Debug.Log("Background Open Called");
    //    float duration = 0.25f;
    //    float timeElapsed = 0f;

    //    backgroundScreen.SetActive(true);
    //    backgroundGroup.alpha = 0f;
    //    backgroundScreen.transform.localScale = Vector3.zero;

    //    while (timeElapsed < duration)
    //    {
    //        // Scale
    //        backgroundScreen.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / 0.25f);

    //        // Fade
    //        backgroundGroup.alpha = Mathf.Lerp(0f, 0.98f, timeElapsed / 0.25f);

    //        timeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }
    //}

    //public IEnumerator OpenPauseMenu()
    //{
    //    Debug.Log("Open Pause Called");

    //    GameManager.instance.MenuActive = PauseMenu;

    //    if (!backgroundScreen.activeSelf)
    //    {
    //        StartCoroutine(OpenBackgroundScreen());
    //    }
    //    else if(backgroundGroup.alpha == 1f)
    //    {
    //        backgroundGroup.alpha = 0.98f;
    //    }

    //    GameManager.instance.MenuActive.SetActive(true);       
    //    pauseMenu.transform.localScale = Vector3.zero;
    //    pauseGroup.alpha = 0f;
    //    float duration = 0.5f;
    //    float timeElapsed = 0f;

    //    while (timeElapsed < duration)
    //    {
    //        pauseMenu.transform.localScale = Vector3.one * Mathf.Lerp(0f, 1f, timeElapsed / duration);

    //        pauseGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / duration);

    //        timeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    pauseTitle.SetActive(true);
    //    pauseTitle.transform.localScale = Vector3.zero;
    //    titleText.alpha = 0f;
    //    float titleDuration = 0.25f;
    //    float titleTimeElapsed = 0f;

    //    // Scale and fade title
    //    while (titleTimeElapsed < titleDuration)
    //    {
    //        pauseTitle.transform.localScale = Vector3.one * Mathf.Lerp(0f, 1f, titleTimeElapsed / titleDuration);
    //        titleText.alpha = Mathf.Lerp(0f, 1f, titleTimeElapsed / titleDuration);

    //        titleTimeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    //if(enableFlickering)
    //    //{
    //    //    StartCoroutine(GlitchText());
    //    //}

    //    float totalDelay = 0.5f;
    //    yield return new WaitForSecondsRealtime(totalDelay);

    //    for (int i = 0; i < buttonsParent.transform.childCount; i++)
    //    {
    //        GameObject button = buttonsParent.transform.GetChild(i).gameObject;

    //        button.SetActive(true);
    //        button.transform.localScale = Vector3.zero;

    //        // Scale animation
    //        float buttonScaleDuration = 0.15f;
    //        float buttonTimeElapsed = 0f;
    //        while (buttonTimeElapsed < buttonScaleDuration)
    //        {
    //            // Progress over time
    //            float timeProgress = buttonTimeElapsed / buttonScaleDuration;
    //            float overshotScale = Mathf.Lerp(1f, 1.2f, timeProgress);
    //            button.transform.localScale = Vector3.one * overshotScale;

    //            buttonTimeElapsed += Time.unscaledDeltaTime;
    //            yield return null;
    //        }
    //        button.transform.localScale = Vector3.one;
    //    }
    //}

    ////private IEnumerator GlitchText()
    ////{
    ////    float glitchDuration = Random.Range(0.1f, 0.3f); // Duration for each glitch
    ////    float timeElapsed = 0f;
    ////    Vector3 originalPosition = titleText.transform.localPosition;

    ////    while (true)
    ////    {
    ////        timeElapsed += Time.unscaledDeltaTime;

    ////        if (timeElapsed >= glitchDuration)
    ////        {
    ////            // Randomize position (to simulate text shifting/glitching)
    ////            titleText.transform.localPosition = originalPosition + new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0f);

    ////            // Randomly scale text to simulate glitchy distortion
    ////            titleText.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);

    ////            // Randomize opacity for more glitch effect
    ////            titleText.alpha = Random.Range(0f, 1f);

    ////            // Reset the timer and randomize the glitch duration for next glitch
    ////            timeElapsed = 0f;
    ////            glitchDuration = Random.Range(0.2f, 0.5f);
    ////        }

    ////        yield return null;
    ////    }
    ////}

    //public IEnumerator ClosePauseMenu()
    //{
    //    Debug.Log("Close Pause Called");

    //    for (int i = 0; i < buttonsParent.transform.childCount; i++)
    //    {
    //        GameObject button = buttonsParent.transform.GetChild(i).gameObject;

    //        // Scale animation for closing buttons
    //        float buttonScaleDuration = 0.15f;
    //        float buttonTimeElapsed = 0f;

    //        while (buttonTimeElapsed < buttonScaleDuration)
    //        {
    //            // Progress over time to scale down
    //            float timeProgress = buttonTimeElapsed / buttonScaleDuration;
    //            button.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timeProgress);

    //            buttonTimeElapsed += Time.unscaledDeltaTime;
    //            yield return null;
    //        }

    //        // Set the button inactive after scaling
    //        button.SetActive(false);
    //    }

    //    float titleCloseDuration = 0.25f;
    //    float titleCloseTimeElapsed = 0f;

    //    while (titleCloseTimeElapsed < titleCloseDuration)
    //    {
    //        pauseTitle.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, titleCloseTimeElapsed / titleCloseDuration);
    //        titleText.alpha = Mathf.Lerp(1f, 0f, titleCloseTimeElapsed / titleCloseDuration);

    //        titleCloseTimeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }
    //    pauseTitle.SetActive(false);

    //    float closeDuration = 0.5f;
    //    float closeTimeElapsed = 0f;

    //    // Scale down the pause menu
    //    while (closeTimeElapsed < closeDuration)
    //    {
    //        pauseMenu.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, closeTimeElapsed / closeDuration);
    //        pauseGroup.alpha = Mathf.Lerp(1f, 0f, closeTimeElapsed / closeDuration);

    //        closeTimeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    if(GameManager.instance.MenuActive != null && GameManager.instance.MenuActive != PauseMenu)
    //    {
    //        backgroundGroup.alpha = 1f;
    //    }
    //    else
    //    {
    //        StartCoroutine(CloseBackgroundScreen());
    //    }
    //}
    //public IEnumerator CloseBackgroundScreen()
    //{
    //    float duration = 0.25f;
    //    float timeElapsed = 0f;

    //    while (timeElapsed < duration)
    //    {
    //        // Scale down
    //        backgroundScreen.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timeElapsed / duration);

    //        // Fade out
    //        backgroundGroup.alpha = Mathf.Lerp(0.98f, 0f, timeElapsed / duration);

    //        timeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    backgroundScreen.SetActive(false);
    //}




    ////public void SettingsButton()
    ////{
    ////    if(GameManager.instance.MenuActive != null)
    ////    {

    ////    }
    ////    //if (GameManager.instance.MenuActive != null)
    ////    //{
    ////    //    //close first?
    ////    //    GameManager.instance.MenuActive.SetActive(false);
    ////    //}
    ////    //GameManager.instance.MenuActive = SettingsMenu;
    ////    //SettingsMenu.SetActive(true);
    ////    //PauseMenu.SetActive(false);
    ////    //backgroundGroup.alpha = 1f;
    ////    //RectTransform settingsTransform = SettingsMenu.GetComponent<RectTransform>();

    ////    //settingsTransform.DOAnchorPos(new Vector3(0, 0, 0), 0.25f)
    ////    //                 .SetEase(Ease.InOutQuad);        
    ////}

    //// Settings Menu Buttons //
    ///* Placeholder on/off until animations added */
    ////public void SettingsMenuButton()
    ////{
    ////    if(GameManager.instance.MenuActive != settingsMenu)
    ////    {
    ////        if (GameManager.instance.MenuActive != null)
    ////        {
    ////            GameManager.instance.MenuActive.SetActive(false);
    ////        }
    ////        GameManager.instance.MenuActive = settingsMenu;
    ////        GameManager.instance.MenuActive.SetActive(true);
    ////    }
    ////}
    ////public void ControlsMenuButton()
    ////{
    ////    if(GameManager.instance.MenuActive != controlsMenu)
    ////    { 
    ////        GameManager.instance.MenuActive.SetActive(false);
    ////        GameManager.instance.MenuActive = controlsMenu;
    ////        GameManager.instance.MenuActive.SetActive(true);
    ////    }
    ////    //controlsMenu.SetActive(true);
    ////    //controlsPos.DOAnchorPos(new Vector2(0, 0), 0.25f)
    ////    //           .SetEase(Ease.OutQuad)
    ////    //           .SetUpdate(true);
    ////}
    ////public void InventoryMenuButton()
    ////{
    ////    if (GameManager.instance.MenuActive != inventoryMenu)
    ////    {
    ////        GameManager.instance.MenuActive.SetActive(false);
    ////        GameManager.instance.MenuActive = inventoryMenu;
    ////        GameManager.instance.MenuActive.SetActive(true);
    ////    }
    ////}
    ////public IEnumerator SaveMenuButton()
    ////{
    ////    if (GameManager.instance.MenuActive != saveMenu)
    ////    {
    ////        if(GameManager.instance.MenuActive != null)
    ////        { 
    ////            GameManager.instance.MenuActive.SetActive(false); 
    ////        }
    ////        GameManager.instance.MenuActive = saveMenu;
    ////        GameManager.instance.MenuActive.SetActive(true);
    ////    }

    ////    backgroundGroup.alpha = 1f;
    ////    RectTransform saveTransform = saveMenu.GetComponent<RectTransform>();

    ////    Vector3 startPos = saveTransform.anchoredPosition;
    ////    Vector3 endPos = new Vector3(0, 0, 0);
    ////    float duration = 0.25f;
    ////    float elapsedTime = 0f;

    ////    while (elapsedTime < duration)
    ////    {
    ////        // Lerp the position over time
    ////        saveTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);

    ////        // Increase elapsed time by time passed
    ////        elapsedTime += Time.deltaTime;

    ////        // Wait for the next frame
    ////        yield return null;
    ////    }

    ////    // Ensure it ends exactly at the target position
    ////    saveTransform.anchoredPosition = endPos;

    ////    //saveTransform.DOAnchorPos(new Vector3(0, 0, 0), 0.25f)
    ////    //             .SetEase(Ease.InOutQuad);

    ////}

    ////public void CreditsButton()
    ////{
    ////    if (GameManager.instance.MenuActive != creditScreen)
    ////    {
    ////        if (GameManager.instance.MenuActive != null)
    ////        {
    ////            GameManager.instance.MenuActive.SetActive(false);
    ////        }
    ////        GameManager.instance.MenuActive = creditScreen;
    ////        GameManager.instance.MenuActive.SetActive(true);
    ////    }
    ////    backgroundGroup.alpha = 1f;
    ////    RectTransform creditsTransform = creditScreen.GetComponent<RectTransform>();

    ////    creditsTransform.DOAnchorPos(new Vector3(0, 0, 0), 0.25f)
    ////                     .SetEase(Ease.InOutQuad);
    ////}


    //// Screens //  
    //public void WinScreen()
    //{ 
    //    GameManager.instance.MenuActive = winScreen;
    //    winScreen.SetActive(true);
    //}
    //public void LoseScreen()
    //{
    //    GameManager.instance.MenuActive = loseScreen;
    //    loseScreen.SetActive(true);
    //}
    //// Maybe not needed...
    //public void LoadingScreen()
    //{

    //}
    ////public void SettingsMenuReset()
    ////{
    ////    RectTransform settingsTransform = settingsMenu.GetComponent<RectTransform>();
    ////    settingsTransform.DOAnchorPos(new Vector3(-1983, 0, 0), 0.25f).SetEase(Ease.InOutQuad);
    ////    settingsMenu.SetActive(false);
    ////}
    ////public IEnumerator SaveMenuReset()
    ////{
    ////    RectTransform saveTransform = saveMenu.GetComponent<RectTransform>();

    ////    Vector3 startPos = saveTransform.anchoredPosition;
    ////    Vector3 endPos = new Vector3(-1983, 0, 0);
    ////    float duration = 0.25f;
    ////    float elapsedTime = 0f;

    ////    // Move the position over time
    ////    while (elapsedTime < duration)
    ////    {
    ////        saveTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
    ////        elapsedTime += Time.deltaTime;
    ////        yield return null;
    ////    }

    ////    // Ensure it ends exactly at the target position
    ////    saveTransform.anchoredPosition = endPos;

    ////    // Deactivate the save menu after the movement
    ////    saveMenu.SetActive(false);
    ////}
    ////public void CreditsReset()
    ////{
    ////    RectTransform creditsTransform = creditScreen.GetComponent<RectTransform>();
    ////    creditsTransform.DOAnchorPos(new Vector3(-1983, 0, 0), 0.25f).SetEase(Ease.InOutQuad);
    ////    creditScreen.SetActive(false);
    ////}
    ////public void CloseAllMenus()
    ////{
    ////    DOTween.KillAll();
    ////    if (settingsMenu.activeSelf)
    ////    {
    ////        SettingsMenuReset();
    ////    }
    ////    else if(saveMenu.activeSelf)
    ////    {
    ////        StartCoroutine(SaveMenuReset());
    ////    }
    ////    else if(creditScreen.activeSelf)
    ////    {
    ////        CreditsReset();
    ////    }

    ////    if(GameManager.instance.MenuActive == pauseMenu || PauseMenu.activeSelf)
    ////    {
    ////        ClosePauseMenu();
    ////    }

    ////    if(backgroundScreen.activeSelf)
    ////    {
    ////        CloseBackgroundScreen();
    ////    }

    ////    //reset variable
    ////    GameManager.instance.MenuActive = null;
    ////}

    //// Settings //
    //    // Toggle //
    //    public void ToggleFlicker()
    //    {
    //        enableFlickering = !enableFlickering;
    //    }
    //    public void ToggleMusic()
    //    {
    //        //AudioManager.instance.ToggleMusic();
    //    }
    //    public void TogglePlayerSFX()
    //    {
    //        //AudioManager.instance.TogglePlayerSFX();
    //    }
    //    public void MuteAllSFX()
    //    {
    //        //AudioManager.instance.MuteAllSFX();
    //    }
    //    public void UnMuteAllSFX()
    //    {
    //        //AudioManager.instance.UnMuteAllSFX();
    //    }

    //    // Slider //
    //    public void MusicVolume()
    //    {
    //        //AudioManager.instance.MusicVolume(_musicSlider.value);

    //        musicVolumeText.text = (_musicSlider.value * 100).ToString("F0");
    //    }
    ////public void SFXAllVolume()
    ////{
    ////    AudioManager.instance.SFXAllVolume(_sfxSlider.value);

    ////    sfxVolumeText.text = (_sfxSlider.value * 100).ToString("F0");
    ////}
    //public void ResetButtonPositions()
    //{

    //}

    //public IEnumerator OpenBaseMenu(GameObject menuName, string title, string buttonName)
    //{
    //    if (backgroundScreen.activeSelf)
    //    {
    //        backgroundGroup.alpha = 1;
    //    }
    //    else if (!backgroundScreen.activeSelf)
    //    {
    //        StartCoroutine(OpenBackgroundScreen());
    //        backgroundGroup.alpha = 1;
    //    }

    //    if (headerBar.transform.position != Vector3.zero)
    //    {
    //        headerBar.transform.position = Vector3.zero;
    //    }
    //    if (menuName.transform.position != Vector3.zero)
    //    {
    //        menuName.transform.position = Vector3.zero;
    //    }

    //    GameObject menuButton = GameObject.Find(buttonName);
    //    StartCoroutine(CenterButton(title, menuButton));

    //    if (headerBar.activeSelf)
    //    {
    //        menuName.transform.GetChild(1).gameObject.SetActive(true);
    //    }
    //    else if(!headerBar.activeSelf)
    //    {
    //        headerBar.SetActive(true);
    //        menuName.transform.GetChild(0).gameObject.SetActive(true);
    //        menuName.transform.GetChild(1).gameObject.SetActive(true);
    //    }
    //    GameManager.instance.MenuActive = menuName;
    //    menuName.SetActive(true);

    //     yield return null;

    //}
    //public IEnumerator OpenSettingsMenu()
    //{
    //    if(backgroundScreen.activeSelf)
    //    {
    //        backgroundGroup.alpha = 1;
    //    }
    //    else if(!backgroundScreen.activeSelf)
    //    {
    //        StartCoroutine(OpenBackgroundScreen());
    //        backgroundGroup.alpha = 1;
    //    }

    //    GameManager.instance.MenuActive = settingsMenu;        
    //    settingsMenu.SetActive(true);
    //    settingsMenu.transform.localScale = Vector3.zero;
    //    CanvasGroup menuGroup = settingsMenu.GetComponent<CanvasGroup>();
    //    menuGroup.alpha = 0f;

    //    //scale and fade
    //    float duration = 0.25f;
    //    float timeElapsed = 0f;

    //    while(timeElapsed < duration)
    //    {
    //        settingsMenu.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / duration);
    //        menuGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / duration);

    //        timeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }       
    //        GameObject settingsButton = GameObject.Find("SettingsBtn");
    //        StartCoroutine(CenterButton("Settings", settingsButton));

    //    if (!headerBar.activeSelf)
    //    {
    //        headerBar.SetActive(true);
    //    }
    //}

    //public IEnumerator OpenCreditsMenu()
    //{
    //    StartCoroutine(OpenBaseMenu(creditScreen, "Credits", "CreditsBtn"));

    //    //if (backgroundScreen.activeSelf)
    //    //{
    //    //    backgroundGroup.alpha = 1;
    //    //}
    //    //else if (!backgroundScreen.activeSelf)
    //    //{
    //    //    StartCoroutine(OpenBackgroundScreen());
    //    //    backgroundGroup.alpha = 1;
    //    //}

    //    //if (headerBar.activeSelf)
    //    //{
    //    //    GameObject creditsButton = GameObject.Find("CreditsBtn");
    //    //    StartCoroutine(CenterButton("Credits", creditsButton));

    //    //    GameManager.instance.MenuActive = creditScreen;

    //    //}



    //    yield return null;
    //}
    //public IEnumerator OpenSaveMenu()
    //{
    //    StartCoroutine(OpenBaseMenu(saveMenu, "Save/Load", "Save_LoadBtn"));


    //    //if (backgroundScreen.activeSelf)
    //    //{
    //    //    backgroundGroup.alpha = 1;
    //    //}
    //    //else if (!backgroundScreen.activeSelf)
    //    //{
    //    //    StartCoroutine(OpenBackgroundScreen());
    //    //    backgroundGroup.alpha = 1;
    //    //}

    //    //if (headerBar.activeSelf)
    //    //{
    //    //    GameObject saveLoadButton = GameObject.Find("Save_LoadBtn");
    //    //    StartCoroutine(CenterButton("Save/Load", saveLoadButton));

    //    //    GameManager.instance.MenuActive = saveMenu;

    //    //}



    //    yield return null;
    //}
    //public IEnumerator OpenControlsMenu()
    //{
    //    StartCoroutine(OpenBaseMenu(controlsMenu, "Controls", "ControlsBtn"));

    //    //if (backgroundScreen.activeSelf)
    //    //{
    //    //    backgroundGroup.alpha = 1;
    //    //}
    //    //else if (!backgroundScreen.activeSelf)
    //    //{
    //    //    StartCoroutine(OpenBackgroundScreen());
    //    //    backgroundGroup.alpha = 1;
    //    //}


    //    //if (headerBar.activeSelf)
    //    //{
    //    //    GameObject controlsButton = GameObject.Find("ControlsBtn");
    //    //    StartCoroutine(CenterButton("Controls", controlsButton));

    //    //    GameManager.instance.MenuActive = controlsMenu;

    //    //}



    //    yield return null;
    //}
    //public IEnumerator OpenInventoryMenu()
    //{
    //    StartCoroutine(OpenBaseMenu(inventoryMenu, "Inventory", "InventoryBtn"));
    //    //if (backgroundScreen.activeSelf)
    //    //{
    //    //    backgroundGroup.alpha = 1;
    //    //}
    //    //else if (!backgroundScreen.activeSelf)
    //    //{
    //    //    StartCoroutine(OpenBackgroundScreen());
    //    //    backgroundGroup.alpha = 1;
    //    //}


    //    //if (headerBar.activeSelf)
    //    //{
    //    //    GameObject inventoryButton = GameObject.Find("InventoryBtn");
    //    //    StartCoroutine(CenterButton("Inventory", inventoryButton));

    //    //    GameManager.instance.MenuActive = inventoryMenu;

    //    //}



    //    yield return null;
    //}


    //public IEnumerator CloseMenu(GameObject menuName)
    //{
    //    //StopAllCoroutines();

    //    float duration = 0.25f;
    //    float timeElapsed = 0f;
    //    CanvasGroup menuGroup = menuName.GetComponent<CanvasGroup>();

    //    while (timeElapsed < duration)
    //    {
    //        menuName.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timeElapsed / duration);
    //        menuGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);

    //        timeElapsed += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    menuName.SetActive(false);
    //    menuName.transform.localPosition = offScreenPos;
    //}


}
