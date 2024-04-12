using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCameraPosition : MonoBehaviour
{
    float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        

    }


    IEnumerator ChangeObjectPosition(GameObject gameObject, float y)
    {

        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0);

        yield return new WaitForEndOfFrame();

    }
}
