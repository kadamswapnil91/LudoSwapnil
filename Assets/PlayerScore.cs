using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Android Merge fast ludo
public class PlayerScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstPlayerCount;
    [SerializeField] TextMeshProUGUI secondPlayerCount;
    [SerializeField] TextMeshProUGUI thirdPlayerCount;
    [SerializeField] TextMeshProUGUI fourthPlayerCount;
    // Start is called before the first frame update

        private void Awake()
    {
        firstPlayerCount.text = "";
        secondPlayerCount.text = "";
        thirdPlayerCount.text = "";
        fourthPlayerCount.text = "";
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         UpdateScore();
    }

        private void UpdateScore()
    {
        int firstPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(1);
        int secondPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(2);
        int thirdPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(3);
        int fourthPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(4);
        firstPlayerCount.text = firstPlayerScore.ToString();
        secondPlayerCount.text = secondPlayerScore.ToString();
        thirdPlayerCount.text = thirdPlayerScore.ToString();
        fourthPlayerCount.text = fourthPlayerScore.ToString();
    }

      public void updateColor(int toggleCount) {
        switch (toggleCount)
        {
            case 1:
                firstPlayerCount.color = Color.yellow;
                secondPlayerCount.color = Color.green;
                thirdPlayerCount.color = Color.red;
                fourthPlayerCount.color = new Color(0.0424f,0.9814f,1,1);
                break;
            case 2:
                firstPlayerCount.color = new Color(0.0424f, 0.9814f, 1, 1);
                secondPlayerCount.color = Color.yellow;
                thirdPlayerCount.color = Color.green;
                fourthPlayerCount.color = Color.red;
                break;
            case 3:
                firstPlayerCount.color = Color.red;
                secondPlayerCount.color = new Color(0.0424f, 0.9814f, 1, 1);
                thirdPlayerCount.color = Color.yellow;
                fourthPlayerCount.color = Color.green;
                break;
            case 4:
                firstPlayerCount.color = Color.green;
                secondPlayerCount.color = Color.red;
                thirdPlayerCount.color = new Color(0.0424f, 0.9814f, 1, 1);
                fourthPlayerCount.color = Color.yellow;
                break;
            default:
                firstPlayerCount.color = Color.green;
                secondPlayerCount.color = Color.red;
                thirdPlayerCount.color = new Color(0.0424f, 0.9814f, 1, 1);
                fourthPlayerCount.color = Color.yellow;
                break;

        }

    }

}
