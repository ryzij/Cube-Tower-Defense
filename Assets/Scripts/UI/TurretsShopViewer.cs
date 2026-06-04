using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class TurretsShopViewer : MonoBehaviour
{
    [SerializeField] private TurretsShop _shop;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TurretButton _buttonPrefab;
    [SerializeField] private List<BlockScript> _ignoreBlocks;

    private Animator _animator;
    private bool _hidden = true;
    private readonly List<TurretButton> _buttons = new();

    private static readonly int _hideAnimation = Animator.StringToHash("HideTurretShopPanelAnimation");
    private static readonly int _showAnimation = Animator.StringToHash("ShowTurretShopPanelAnimation");

    private void Start()
    {
        foreach (var item in _shop.TurretShopItems)
        {
            var button = Instantiate(_buttonPrefab, transform);
            button.TurretItem = item;
            button.Icon = item.Icon;
            button.PriceText = item.Price.ToString();
            button.OnClick += () => _shop.BuyTurret(item);

            _buttons.Add(button);
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _wallet.OnMoneyChanged += OnMoneyChanged;
        _shop.OnBlockSelected += OnBlockSelected;
        _shop.OnBlockDeselected += OnBlockDeselected;
    }

    private void OnDisable()
    {
        _wallet.OnMoneyChanged -= OnMoneyChanged;
        _shop.OnBlockSelected -= OnBlockSelected;
        _shop.OnBlockDeselected -= OnBlockDeselected;
    }

    private void OnBlockSelected(BlockScript block)
    {
        if (!_hidden)
            return;

        // Чтобы анимация не воспроизводилась когда не надо
        if (_ignoreBlocks.Count > 0 && _ignoreBlocks.Contains(block))
        {
            _ignoreBlocks.Clear();
            return;
        }

        _animator.CrossFade(_showAnimation, 0.1f);
        _hidden = false;
    }

    private void OnBlockDeselected()
    {
        _animator.CrossFade(_hideAnimation, 0.1f);
        _hidden = true;
    }

    private void OnMoneyChanged(int money)
    {
        foreach (var button in _buttons)
            button.Interactable = money >= button.TurretItem.Price;
    }
}
