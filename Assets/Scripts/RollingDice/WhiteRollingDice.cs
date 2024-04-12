using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteRollingDice : MonoBehaviour
{

	[SerializeField] int numberGot;
	[SerializeField] GameObject rollingDiceAnim;
	[SerializeField] SpriteRenderer numberedSpHoldere;
	[SerializeField] Sprite[] numberedSprites;

	Coroutine generateRandNumOnDice_Coroutine;
	bool canDiceRoll;

    // Start is called before the first frame update
    void Start()
    {
        canDiceRoll = true;
    }

    public void OnMouseDown()
	{
		print("clicked");
	  generateRandNumOnDice_Coroutine = StartCoroutine(GenerateRandomNumberOnDice_Enum());
	}


IEnumerator GenerateRandomNumberOnDice_Enum()
{
yield return new WaitForEndOfFrame();

if(canDiceRoll)
{
canDiceRoll = false;	
SoundManager.PlaySound("rollDice");
numberedSpHoldere.gameObject.SetActive(false);
rollingDiceAnim.SetActive(true);
yield return new WaitForSeconds(0.5f);

numberGot = UnityEngine.Random.Range(0, 6);
numberedSpHoldere.sprite = numberedSprites[numberGot];
numberGot += 1;

  GameManager.gm.numberOfStepsToMove = numberGot;
  GameManager.gm.dice = numberGot;

rollingDiceAnim.SetActive(false);	
numberedSpHoldere.gameObject.SetActive(true);
canDiceRoll = true;


yield return new WaitForEndOfFrame();

if(generateRandNumOnDice_Coroutine != null)
{
StopCoroutine(generateRandNumOnDice_Coroutine);
}

}

}

}
