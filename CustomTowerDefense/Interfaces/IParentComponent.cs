using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Interfaces
{
    /// <summary>
    /// For parent components, including at least one child components
    /// </summary>
    public interface IParentComponent
    {
        List<GameComponent> ChildComponents { get; }
    }
}