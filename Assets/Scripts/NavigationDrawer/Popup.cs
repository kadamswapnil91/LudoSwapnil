using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NavigationDrawer.UI
{
    public class Popup : MonoBehaviour
    {
        #region FIELDS

        [SerializeField]
        private Sprite _popupBackground = default;

        [SerializeField]
        private Material _blur = default;

        [SerializeField]
        private Color _backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

        private const float DESTROY_TIME = 0.2f;
        private GameObject _background;

        #endregion

        #region PUBLIC_METHODS

        public void Open()
        {
            AddBackground();
        }

        public void CloseWindow()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.Play("Close");
            }

            RemoveBackground();
            StartCoroutine(RunPopupDeactiveAsync());
        }

        #endregion

        #region PRIVATE_METHODS

        private IEnumerator RunPopupDeactiveAsync()
        {
            yield return new WaitForSeconds(DESTROY_TIME);
            Destroy(_background);
            gameObject.SetActive(false);
        }

        private void AddBackground()
        {
            _background = new GameObject("PopupBackground");

            var image = _background.AddComponent<Image>();
            var sprite = _popupBackground;
            var material = _blur;
            var newColor = image.color;

            image.material = material;
            image.sprite = sprite;
            image.color = newColor;

            var canvas = GameObject.Find("UICanvas");
            _background.transform.localScale = new Vector3(1, 1, 1);
            _background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            _background.transform.SetParent(canvas.transform, false);
            _background.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        private void RemoveBackground()
        {
            var image = _background.GetComponent<Image>();
            if (image != null)
            {
                image.CrossFadeAlpha(0.0f, 0.2f, false);
            }
        }

        #endregion
    }
}