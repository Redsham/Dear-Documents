using Cysharp.Threading.Tasks;

namespace Gameplay.Items
{
    public interface IOnReturnHandler
    {
        UniTask OnReturn();
    }
}