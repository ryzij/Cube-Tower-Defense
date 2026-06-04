using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurretButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _priceText;

    public Sprite Icon
    {
        get
        {
            if (_icon.isActiveAndEnabled)
                return _icon.sprite;
            return null;
        }
        set
        {
            bool isActive = value != null;
            _icon.gameObject.SetActive(isActive);
            if (isActive)
                _icon.sprite = value;
        }
    }
    public string PriceText
    {
        get => _priceText.text;
        set => _priceText.text = value;
    }
    public bool Interactable
    {
        get => _button.interactable;
        set => _button.interactable = value;
    }
    public TurretShopItem TurretItem { get; set; }

    public event UnityAction OnClick
    {
        add => _button.onClick.AddListener(value);
        remove => _button.onClick.RemoveListener(value);
    }
}