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

    private void OnBlockDestroy(BlockScript _)
    {
        _wallet.AddMoney(_reward);
    }
}