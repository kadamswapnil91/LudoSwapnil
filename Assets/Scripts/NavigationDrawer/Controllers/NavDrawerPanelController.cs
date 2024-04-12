using System.Collections;
using NavigationDrawer.UI;
using UnityEngine;

namespace NavigationDrawer.Controller
{
    public class NavDrawerPanelController : MonoBehaviour
    {
        #region FIELDS

      /*  [SerializeField, Header("Panels")]
        private ProfilePanel _profilePanel = default;

        [SerializeField]
        private RatingPanel _ratingPanel = default;

        [SerializeField]
        private AboutPanel _aboutPanel = default;

        [SerializeField]
        private TermsPanel _termsPanel = default;

        [SerializeField]
        private HelpPanel _helpPanel = default;

        [SerializeField]
        private GameObject _btnMenu = default;*/

        #endregion

        #region PUBLIC_METHODS

      /*  public void OpenProfilePanel()
        {
            OpenWindow(_profilePanel.gameObject);
        }

        public void OpenRatingPanel()
        {
            OpenWindow(_ratingPanel.gameObject);
        }

        public void OpenHelpPanel()
        {
            OpenWindow(_helpPanel.gameObject);
        }

        public void OpenAboutPanel()
        {
            OpenWindow(_aboutPanel.gameObject);
        }

        public void OpenTermsPanel()
        {
            OpenWindow(_termsPanel.gameObject);
        }*/

        public void SetActiveBtnMenu(bool value)
        {
       //     _btnMenu.SetActive(value);
        }

        public void CloseAllPanel()
        {
          /*  if (_profilePanel.isActiveAndEnabled)
            {
                CloseWindow(_profilePanel.gameObject);
            }

            if (_ratingPanel.isActiveAndEnabled)
            {
                CloseWindow(_ratingPanel.gameObject);
            }

            if (_helpPanel.isActiveAndEnabled)
            {
                CloseWindow(_helpPanel.gameObject);
            }

            if (_termsPanel.isActiveAndEnabled)
            {
                CloseWindow(_termsPanel.gameObject);
            }

            if (_aboutPanel.isActiveAndEnabled)
            {
                CloseWindow(_aboutPanel.gameObject);
            }*/
        }

        #endregion

        #region PRIVATE_METHODS

        private static void CloseWindow(GameObject popup)
        {
            popup.GetComponent<Popup>().CloseWindow();
        }

        private void OpenWindow(GameObject popup)
        {
            StartCoroutine(OpenWindowAsync(popup));
        }

        private static IEnumerator OpenWindowAsync(GameObject popup)
        {
            yield return new WaitForSeconds(0.25f);

            popup.SetActive(true);
            popup.transform.localScale = Vector3.one;
            popup.GetComponent<Popup>().Open();
        }

        #endregion
    }
}