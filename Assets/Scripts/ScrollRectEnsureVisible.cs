using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectEnsureVisible : MonoBehaviour
{

    #region Public Variables

    public float AnimTime = 0.15f;
    public bool Snap = false;
    public RectTransform MaskTransform;

    #endregion

    #region Private Variables

    private ScrollRect _mScrollRect;
    private RectTransform _mScrollTransform;
    private RectTransform _mContent;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _mScrollRect = GetComponent<ScrollRect>();
        _mScrollTransform = _mScrollRect.transform as RectTransform;
        _mContent = _mScrollRect.content;
    }

    #endregion

    #region Public Methods

    public void CenterOnItem(RectTransform target)
    {
        // Item is here
        var itemCenterPositionInScroll = GetWorldPointInWidget(_mScrollTransform, GetWidgetWorldPoint(target));
        // But must be here
        var targetPositionInScroll = GetWorldPointInWidget(_mScrollTransform, GetWidgetWorldPoint(MaskTransform));
        // So it has to move this distance
        var difference = targetPositionInScroll - itemCenterPositionInScroll;
        difference.z = 0f;

        //clear axis data that is not enabled in the scrollrect
        if (!_mScrollRect.horizontal)
        {
            difference.x = 0f;
        }
        if (!_mScrollRect.vertical)
        {
            difference.y = 0f;
        }

        Debug.Log("Difference: " + difference);

        var normalizedDifference = new Vector2(
            difference.x / (_mContent.rect.size.x - _mScrollTransform.rect.size.x),
            difference.y / (_mContent.rect.size.y - _mScrollTransform.rect.size.y));

        Debug.Log("Normalized difference: " + normalizedDifference);

        var newNormalizedPosition = _mScrollRect.normalizedPosition - normalizedDifference;
        Debug.Log("New normalized position: " + newNormalizedPosition);
        if (_mScrollRect.movementType != ScrollRect.MovementType.Unrestricted)
        {
            newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
            newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
            Debug.Log("Clamped normalized position: " + newNormalizedPosition);
        }

       // if (Snap)
       /// {
            _mScrollRect.normalizedPosition = newNormalizedPosition;
        //}
        //else
        //{
       //     Go.to(_mScrollRect, AnimTime, new GoTweenConfig().vector2Prop("normalizedPosition", newNormalizedPosition));
       // }
    }

    #endregion

    #region Private Methods

    Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }

    Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }
    #endregion
}