using UnityEngine;

namespace LS.Core
{
    public interface IInjectable
    {
        void Inject(IGameContext context);
    }
}
