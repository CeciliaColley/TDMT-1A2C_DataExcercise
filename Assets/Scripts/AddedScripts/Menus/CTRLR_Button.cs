using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    /// <summary>
    /// Controls the behavior of a menu button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class CTRL_Button : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private string _id;
        private Button _button;

        /// <summary>
        /// Event triggered when the button is clicked.
        /// </summary>
        public event Action<string> OnClick;

        private void Reset()
        {
            GameObject child;
            if (transform.childCount < 1)
            {
                child = new GameObject("Text (TMP)");
                child.transform.SetParent(transform);
            }
            else
            {
                child = transform.GetChild(0).gameObject;
            }

            if (!child.TryGetComponent<TMP_Text>(out text))
            {
                text = child.AddComponent<TextMeshProUGUI>();
            }
            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            text ??= GetComponent<TMP_Text>();
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        /// <summary>
        /// Sets up the button with a label, ID, and click event action.
        /// </summary>
        /// <param name="label">The label for the button.</param>
        /// <param name="id">The ID for the button.</param>
        /// <param name="onClick">The action to perform on button click.</param>
        public void Setup(string label, string id, Action<string> onClick)
        {
            text.SetText(label);
            _id = id;
            OnClick = onClick;
        }

        /// <summary>
        /// Handles the button click event.
        /// </summary>
        private void HandleButtonClick()
        {
            OnClick?.Invoke(_id);
        }
    }
}