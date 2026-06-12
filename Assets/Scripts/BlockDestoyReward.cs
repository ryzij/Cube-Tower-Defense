using UnityEngine;

public class BlockDestoyReward : MonoBehaviour
{
    [SerializeField] private SandPathBuildService _sandPathBuildService;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private int _reward = 10;

    private void OnEnable()
    {
        _sandPathBuildService.OnBlockDestroy += OnBlockDestroy;
    }

    private void OnDisable()
    {
        _sandPathBuildService.OnBlockDestroy -= OnBlockDestroy;
    }

    private void OnBlockDestroy(BlockScript block)
    {
        if (block.Type == BlockScript.BlockType.Grass)
            _wallet.AddMoney(_reward);
        else if (block.Type == BlockScript.BlockType.Sand)
            _wallet.TakeMoney(_reward);
    }
}