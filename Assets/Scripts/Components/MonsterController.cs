using System;
using System.Linq;
using DG.Tweening;
using Libraries;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterController : MonoBehaviour
{
  MonsterName _name;

  public void Init(MonsterName name)
  {
    _name = name;
    
    SetEnemyImage();
  }

  public void UpdateState(AgentState state)
  {
    if (Monster().ExtraSprites == null)
      return;
    foreach (var extraSprite in Monster().ExtraSprites.Where(eS => !eS.Triggered).ToArray())
    {
      switch (extraSprite.Type)
      {
        case ExtraSpriteType.RemoveOverlay:
          switch (extraSprite.ResolutionType)
          {
            case ResolutionType.Armour:
              if (state.Armour() <= extraSprite.TriggerLimit)
              {
                var sprite = transform.Find(_name.ToString()).Find(extraSprite.Name);
                var sequence = sprite.DOJump(sprite.transform.position + Vector3.right * 0.6f * (Random.Range(0,2) == 0 ? -1 : 1) + Vector3.down, 1.5f, 1, 1f);
                DOTween.ToAlpha(() => sprite.gameObject.GetComponent<SpriteRenderer>().color,
                  x => sprite.gameObject.GetComponent<SpriteRenderer>().color = x,
                  0,
                  1f);
               
                extraSprite.Trigger();
              }
              break;
          }
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  Monster Monster()
  {
    return MonsterLibrary.Monsters[_name];
  }
  
  void SetEnemyImage()
  {
    HideAllEnemyImages();
    ShowEnemyImage();
  }

  void HideAllEnemyImages()
  {
    for(var i = 0; i < transform.childCount; i++){
      transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void ShowEnemyImage()
  {
    transform.Find(_name.ToString()).gameObject.SetActive(true);;
  }
}