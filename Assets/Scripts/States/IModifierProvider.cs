using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public interface IModifierProvider
    {
        IEnumerable<float>GetAdditiveModifier(Stat stat);
        IEnumerable<float> GetPercentageModifier(Stat stat);
    }
}
