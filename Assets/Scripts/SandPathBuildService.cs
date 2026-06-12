using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

public class SandPathBuildService : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private BlockDetector _blockDetector;
    [SerializeField] private BlockScript _blockPrefab;
    [SerializeField] private BlockScript _grassBlockPrefab;
    [SerializeField] private BlockScript _pathStart;
    [SerializeField] private BlockScript _pathEnd;
    [SerializeField] private BlockScript[] _pathFinish;
    [SerializeField] private BlockScript[] _predefinedBlocks;

    [SerializeField] private UnityEvent<BlockScript> _onBlockDestroy;
    [SerializeField] private UnityEvent _onBuildComplete;

    public event UnityAction<BlockScript> OnBlockDestroy
    {
        add => _onBlockDestroy.AddListener(value);
        remove => _onBlockDestroy.RemoveListener(value);
    }
    public event UnityAction OnBuildComplete
    {
        add => _onBuildComplete.AddListener(value);
        remove => _onBuildComplete.RemoveListener(value);
    }

    private BlockScript _currentBlock;
    private readonly List<BlockScript> _blocksInPath = new();

    private void Start() => InitBlocks();

    private void InitBlocks()
    {
        _currentBlock = _pathStart;
        _blocksInPath.AddRange(_predefinedBlocks);
    }
    
    public void DestroyBlock(BlockScript block)
    {
        _onBlockDestroy?.Invoke(block);
        Destroy(block.gameObject);
    }
    
    private void OnEnable()
    {
        _blockDetector.OnBlockClicked += OnBlockDetected;
    }
    private void OnDisable()
    {
        _blockDetector.OnBlockClicked -= OnBlockDetected;
    }

    public void ResetPath()
    {
        for (int i = _predefinedBlocks.Length; i < _blocksInPath.Count; i++)
        {
            var block = _blocksInPath[i];
            Instantiate(_grassBlockPrefab, block.transform.position, block.transform.localRotation, block.transform.parent);
            DestroyBlock(block);
        }

        _blocksInPath.Clear();
        InitBlocks();
    }

    public void ResetLastBlock()
    {
        if (_predefinedBlocks.Contains(_currentBlock))
            return;

        _blocksInPath.Remove(_currentBlock);
        Instantiate(_grassBlockPrefab,
            _currentBlock.transform.position,
            _currentBlock.transform.localRotation,
            _currentBlock.transform.parent);
        DestroyBlock(_currentBlock);

        _currentBlock = _blocksInPath.LastOrDefault();
    }

    private void OnBlockDetected(BlockScript block)
    {
        if (_gameManager.CurrentState != GameManager.GameState.BuildingPath)
            return;
        if (block == _currentBlock)
        {
            ResetLastBlock();
            return;
        }
        if (!block.CompareTag("GrassBlock") ||
            !_currentBlock.GetNeighbors().Contains(block) ||
            block.GetNeighbors().Count((o) => o.Type == BlockScript.BlockType.Sand) > 1)
            return;

        var t = block.transform;
        _currentBlock = Instantiate(_blockPrefab, t.position, t.localRotation, t.parent);
        _blocksInPath.Add(_currentBlock);
        if (_pathFinish.Contains(block))
        {
            _currentBlock = _pathEnd;
            _blocksInPath.Add(_currentBlock);
            _onBuildComplete?.Invoke();
            _gameManager.CurrentState = GameManager.GameState.BuildingTurrets;
        }

        DestroyBlock(block);
    }

    public IEnumerable<Vector3> GetPath() =>
        _blocksInPath.Select(b => b.transform.position + Vector3.up * 0.5f);
}
