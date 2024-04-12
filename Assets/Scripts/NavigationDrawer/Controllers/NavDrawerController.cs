using NavigationDrawer.UI;
using UnityEngine;
using UnityEngine.UI;

namespace NavigationDrawer.Controller
{
    public class NavDrawerController : MonoBehaviour
    {
        #region FIELDS

        [SerializeField, Header("Nav panels")]
        public NavDrawerPanelController _navDrawerPanelController = default;

        [SerializeField]
        public NavDrawerPanel _navDrawerPanel = default;

       /* [SerializeField, Header("Nav buttons")]
        public Button _btnProfile = default;

        [SerializeField]
        public Button _btnRating = default;

        [SerializeField]
        public Button _btnHelp = default;

        [SerializeField]
        public Button _btnAbout = default;

        [SerializeField]
        public Button _btnTerms = default;*/

        #endregion

        #region UNITY_METHODS

        private void Start()
        {
           /* _btnProfile.onClick.AddListener(NavDrawerPanelOnProfile);
            _btnRating.onClick.AddListener(NavDrawerPanelOnRating);
            _btnHelp.onClick.AddListener(NavDrawerPanelOnHelp);
            _btnAbout.onClick.AddListener(NavDrawerPanelOnAbout);
            _btnTerms.onClick.AddListener(NavDrawerPanelOnTerms);*/
        }

        #endregion

        #region PUBLIC_METHODS

        public void InitNavDrawer()
        {
            _navDrawerPanel.Open();
        }

        public void CloseAllPanel()
        {
            _navDrawerPanelController.CloseAllPanel();
        }

        #endregion

        #region PRIVATE_METHODS

      /*  private void NavDrawerPanelOnProfile()
        {
            _navDrawerPanelController.OpenProfilePanel();
        }

        private void NavDrawerPanelOnRating()
        {
            _navDrawerPanelController.OpenRatingPanel();
        }

        private void NavDrawerPanelOnHelp()
        {
            _navDrawerPanelController.OpenHelpPanel();
        }

        private void NavDrawerPanelOnAbout()
        {
            _navDrawerPanelController.OpenAboutPanel();
        }

        private void NavDrawerPanelOnTerms()
        {
            _navDrawerPanelController.OpenTermsPanel();
        }*/

        #endregion
    }
}