using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TurretsShop : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private BlockDetector _blockDetector;
    [SerializeField] private Transform _turretsHolder;
    [SerializeField] private InputAction _deselectAction;
    [SerializeField] private TurretShopItem[] _turretShopItems;
    [SerializeField] private UnityEvent<BlockScript> _onBlockSelected;
    [SerializeField] private UnityEvent _onBlockDeselected;
    [SerializeField] private UnityEvent<TurretShopItem> _onTurretBought;

    public IReadOnlyCollection<TurretShopItem> TurretShopItems => _turretShopItems;

    public event UnityAction<BlockScript> OnBlockSelected
    {
        add => _onBlockSelected.AddListener(value);
        remove => _onBlockSelected.RemoveListener(value);
    }

    public event UnityAction OnBlockDeselected
    {
        add => _onBlockDeselected.AddListener(value);
        remove => _onBlockDeselected.RemoveListener(value);
    }

    public event UnityAction<TurretShopItem> OnTurretBought
    {
        add => _onTurretBought.AddListener(value);
        remove => _onTurretBought.RemoveListener(value);
    }

    private BlockScript _selectedBlock;

    private void OnEnable()
    {
        if (_deselectAction != null)
        {
            _deselectAction.Enable();
            _deselectAction.performed += DeselectActionPerformed;
        }

        _blockDetector.OnBlockClicked += OnBlockClicked;
    }
    private void OnDisable()
    {
        if (_deselectAction != null)
        {
            _deselectAction.performed -= DeselectActionPerformed;
            _deselectAction.Disable();
        }

        _blockDetector.OnBlockClicked -= OnBlockClicked;
    }

    private void Start()
    {
        if (_turretsHolder == null)
            _turretsHolder = transform;
    }

    private void OnBlockClicked(BlockScript block)
    {
        if (_selectedBlock == block)
        {
            DeselectBlock();
            return;
        }

        if (_gameManager.CurrentState != GameManager.GameState.BuildingPath &&
            block.CompareTag("GrassBlock"))
            SelectBlock(block);
    }
    
    public void BuyTurret(TurretShopItem item)
    {
        if (_selectedBlock == null || _wallet.Money < item.Price)
            return;

        _wallet.TakeMoney(item.Price);
        Instantiate(item.TurretPrefab, _selectedBlock.transform.position + Vector3.up, Quaternion.identity, _turretsHolder);
        _onTurretBought?.Invoke(item);
        DeselectBlock();
    }

    public void SelectBlock(BlockScript block)
    {
        _selectedBlock = block;
        _onBlockSelected?.Invoke(block);
    }

    public void DeselectBlock()
    {
        _selectedBlock = null;
        _onBlockDeselected?.Invoke();
    }

    private void DeselectActionPerformed(InputAction.CallbackContext _) =>
        DeselectBlock();
}
