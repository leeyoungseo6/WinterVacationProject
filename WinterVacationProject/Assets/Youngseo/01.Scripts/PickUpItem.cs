using System.Linq;
using UnityEngine;

namespace YS
{
    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] private LayerMask _itemLayer;
        private ISelectable _prevItem;
        
        private void Update()
        {
            Point();
        }

        private void Point()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hit, 10, _itemLayer)) // 아이템이 감지된 경우
            {
                // 현재 가리키고 있는 아이템(currentItem)과 이전에 가리키던 아이템(_prevItem)이 같지 않다면
                // ( _prevItem이 null인 경우도 해당 )
                if (hit.transform.TryGetComponent(out ISelectable currentItem) && !Equals(currentItem, _prevItem))
                {
                    // _prevItem의 MouseExit 실행하고 currentItem의 MouseEnter 실행
                    _prevItem?.MouseExit();
                    currentItem.MouseEnter();
                    _prevItem = currentItem;
                }
            }
            else // 아무것도 감지되지 않은 경우
            {
                _prevItem?.MouseExit(); // _prevItem의 MouseExit을 실행하고
                _prevItem = null; // _prevItem은 비움
            }
        }
    }
}