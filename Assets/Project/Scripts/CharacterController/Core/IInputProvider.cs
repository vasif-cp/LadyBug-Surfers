using UnityEngine;

namespace LS.Core
{
    public interface IInputProvider
    {
        float HorizontalInput { get; }
        float VerticalInput { get; }
    }
}
