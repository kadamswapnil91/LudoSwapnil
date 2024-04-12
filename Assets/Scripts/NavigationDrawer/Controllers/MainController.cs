using System.Collections;
//using Loading;
using UnityEngine;

namespace NavigationDrawer.Controller
{
    public class MainController : MonoBehaviour
    {
        #region FIELDS

        [SerializeField, Header("Controllers")]
        private NavDrawerController _navDrawerController = default;

        #endregion

        #region UNITY_METHODS

        private IEnumerator Start()
        {
        /*    LoadingPanel.Instance.LoadingStart(ELoading.LoadData);

            

            LoadingPanel.Instance.LoadingStop();*/
            yield return StartCoroutine(InitAsync());
        }

        #endregion

        #region PRIVATE_METHODS

        private IEnumerator InitAsync()
        {
            yield return new WaitForSeconds(2.0f);

            Initialize();
        }

        private void Initialize()
        {
            _navDrawerController.InitNavDrawer();
        }

        #endregion
    }
}