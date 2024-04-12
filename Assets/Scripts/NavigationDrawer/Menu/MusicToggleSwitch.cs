using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;


public class MusicToggleSwitch : MonoBehaviour, IPointerDownHandler
{
	[SerializeField]
	private bool _isOn = true;

	public bool isOn
	{
		get
		{
			return _isOn;
		}
	}

	[SerializeField]
	private RectTransform toggleIndicator;

	[SerializeField]
	private Image backgroundImage;

	[SerializeField]
	private Color defaultColor;

	[SerializeField]
	private Color onColor;

	[SerializeField]
	private Color offColor;

	[SerializeField]
	private Text textStatus;

	private float offX;
	private float onX;

	[SerializeField]
	private string on, off;

	[SerializeField]
	private float tweenTime = 0.25f;

	public delegate void ValueChanged(bool value);
	public event ValueChanged valueChanged;

    [SerializeField]
    Sprite lightOnSprite, lightOffSprite, darkOnSprite, darkOffSprite;

    [SerializeField]
    Image SoundIcon;

    changeThemeMode themeMode;

    GameSettings gameSettings;
    // Start is called before the first frame update
    void Start()
    {
        offX = toggleIndicator.anchoredPosition.x;
        onX = offX * -1f;

        backgroundImage.DOColor(defaultColor, tweenTime);

        if (PlayerPrefs.GetInt("soundStatus") == 1)
        {
            Toggle(true);
        }
        else
        {
            Toggle(false);
        }
    }

     private void Awake()
     {
        themeMode = FindObjectOfType<changeThemeMode>();
        gameSettings = FindObjectOfType<GameSettings>();
    }

    private void OnEnable()
    {
    	Toggle(isOn);
    }

    private void Toggle(bool value)
    {
    	if(value != isOn)
    	{
    		_isOn = value;

    		ToggleColor(isOn);
    		MoveIndicator(isOn);
    		ToggleText(isOn);
            changeIcon(isOn);

    		if(valueChanged != null)
    		valueChanged(isOn);
    	}
    }

    private void changeIcon(bool value)
    {
        if(GameManager.gm.isDarkMode)
        {
            if(value)
            {
                SoundIcon.sprite = darkOnSprite;
                PlayerPrefs.SetInt("soundStatus", 1);
            }     
            else
            {
                SoundIcon.sprite = darkOffSprite;
                PlayerPrefs.SetInt("soundStatus", 0);
            }
                
        }
        else
        {
            if(value)
            {
                SoundIcon.sprite = lightOnSprite;
                PlayerPrefs.SetInt("soundStatus", 1);
            }
            else
            {
                SoundIcon.sprite = lightOffSprite;
                PlayerPrefs.SetInt("soundStatus", 0);
            }
                
        }
        
        GameManager.gm.isMusicOn = value;
        gameSettings.setSoundImage();
    }

    private void ToggleColor(bool value)
    {
    	if(value)
    		backgroundImage.DOColor(onColor, tweenTime);
    	else
    		backgroundImage.DOColor(offColor, tweenTime);
    }

    private void MoveIndicator(bool value)
    {
    	if(value)
    		toggleIndicator.DOAnchorPosX(onX, tweenTime);
    	else
    		toggleIndicator.DOAnchorPosX(offX, tweenTime);
    }

    private void ToggleText(bool value)
    {
    	if(value)
    		textStatus.DOText(on, tweenTime);
    	else
    		textStatus.DOText(off, tweenTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    	Toggle(!isOn);
    }
}
