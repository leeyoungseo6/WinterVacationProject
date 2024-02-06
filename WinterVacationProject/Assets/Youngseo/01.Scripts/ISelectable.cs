using UnityEngine;

namespace YS
{
    public interface ISelectable
    {
        public void MouseEnter();
        public void MouseExit();
        public void OnSelect();
        public Transform GetTransform();
    }
}