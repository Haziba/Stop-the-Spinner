using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PlayerInfo
{
  public class DragBackpackCardHandler : MonoBehaviour
  {
      public static DragBackpackCardHandler Instance;

      [Header("References")]
      public Canvas canvas;                   // Assign your main Canvas here
      public RectTransform dragImage;        // Assign a RectTransform with Image (usually invisible by default)
      public GameObject dragImageComponent;      // The Image component on dragImage
      public GameObject Deck;

      private PlayerCardDetails currentItem;

      void Awake()
      {
          // Singleton pattern (optional but handy)
          if (Instance != null && Instance != this)
          {
              Destroy(gameObject);
          }
          else
          {
              Instance = this;
          }
      }

      public void StartDrag(PlayerCardDetails item)
      {
          currentItem = item;
          Debug.Log(item.CardName);
          dragImageComponent.GetComponent<CardHandler>().SetCardImage(item.CardName, item);
          dragImage.gameObject.SetActive(true);
      }

      public void UpdateDrag(Vector2 screenPosition)
      {
          if (dragImage == null || dragImage.parent == null)
              return;

          RectTransformUtility.ScreenPointToLocalPointInRectangle(
              dragImage.parent as RectTransform,
              screenPosition,
              canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
              out Vector2 localPoint
          );

          dragImage.localPosition = localPoint;
      }

      public bool EndDrag(PointerEventData eventData)
      {
          dragImage.gameObject.SetActive(false);

          if (eventData.pointerEnter != null)
          {
              Transform currentTransform = eventData.pointerEnter.transform;
              while (currentTransform != null)
              {
                Debug.Log(currentTransform.gameObject, Deck);
                  if (currentTransform.gameObject == Deck)
                  {
                    Player.Instance.AddCardToDeck(currentItem);
                    DeckListHandler.Instance.AddCard(currentItem);
                      return true;
                  }
                  currentTransform = currentTransform.parent;
              }
          }

          currentItem = null;
          return false;
      }
  }
}