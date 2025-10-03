using UnityEngine;

public static class LayerMaskExtensionMethods
{
    public static bool IncludesLayer(this LayerMask _mask, int _layer)
    {
        return ((1 << _layer) & _mask) != 0;
    }

    public static bool ExcludesLayer(this LayerMask _mask, int _layer) => !IncludesLayer(_mask, _layer);
}