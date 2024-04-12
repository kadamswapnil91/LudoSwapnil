using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;


public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
	[SerializeField]
	private bool _isOn = false;

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

    changeThemeMode themeMode;

    // Start is called before the first frame update
    void Start()
    {
        offX = toggleIndicator.anchoredPosition.x;
        onX = offX * -1f;

        backgroundImage.DOColor(defaultColor, tweenTime);


        if (PlayerPrefs.GetInt("themeStatus") == 0)
        {
            _isOn = false;
            Toggle(true);
        }
        else
        {
            _isOn = true;
            Toggle(false);
        }
    }

     private void Awake()
     {
        themeMode = FindObjectOfType<changeThemeMode>();
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

            GameManager.gm.isDarkMode = isOn;
            themeMode.changeTheme();

    		if(valueChanged != null)
    		valueChanged(isOn);
    	}
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
    		toggleIndicator.DOAnchorPosX(offX, tweenTime);
    	else
    		toggleIndicator.DOAnchorPosX(onX, tweenTime);
    }

    private void ToggleText(bool value)
    {
    	if(value)
        {
            textStatus.DOText(on, tweenTime);
            PlayerPrefs.SetInt("themeStatus", 0);
        }	
    	else
        {
            textStatus.DOText(off, tweenTime);
            PlayerPrefs.SetInt("themeStatus", 1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    	Toggle(!isOn);
    }
}
