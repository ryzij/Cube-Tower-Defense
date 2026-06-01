using System;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{
    [SerializeField] private BlockDetector _blockDetector;
    [SerializeField] private BlockMaterial[] _selectedMaterials;

    private Renderer _selectedBlockRenderer;
    private Material _oldMaterial;

    private void OnEnable()
    {
        _blockDetector.OnBlockDetected += OnBlockDetected;
    }

    private void OnDisable()
    {
        _blockDetector.OnBlockDetected -= OnBlockDetected;

        if (_selectedBlockRenderer != null)
        {
            _selectedBlockRenderer.material = _oldMaterial;
            _selectedBlockRenderer = null;
        }
    }

    private Material GetMaterialByBlockType(BlockScript.BlockType type)
    {
        foreach (var blockMaterial in _selectedMaterials)
        {
            if (blockMaterial.Type == type)
                return blockMaterial.Material;
        }

        return null;
    }

    private void OnBlockDetected(BlockScript block)
    {
        var selectedMaterial = GetMaterialByBlockType(block.Type);
        if (selectedMaterial == null)
            return;

        if (_selectedBlockRenderer != null)
            _selectedBlockRenderer.material = _oldMaterial;

       _selectedBlockRenderer = block.GetComponentInChildren<Renderer>();
        _oldMaterial = _selectedBlockRenderer.material;
        _selectedBlockRenderer.material = selectedMaterial;
    }

    [Serializable]
    private struct BlockMaterial
    {
        public Material Material;
        public BlockScript.BlockType Type;
    }
}
