using UnityEngine;

namespace NavigationDrawer.UI
{
    public class PopupOpener : MonoBehaviour
    {
        #region FIELDS

        [SerializeField]
        private GameObject _popupPrefab = default;

        #endregion

        #region PUBLIC_METHODS

        public virtual void OpenWindow()
        {
            _popupPrefab.SetActive(true);
            _popupPrefab.transform.localScale = Vector3.one;
            _popupPrefab.GetComponent<Popup>().Open();
        }

        #endregion
    }
}