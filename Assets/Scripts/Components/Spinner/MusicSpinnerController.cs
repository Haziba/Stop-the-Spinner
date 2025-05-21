using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math = System.Math;

public class MusicSpinnerController : MonoBehaviour, ISpinnerController
{
    private const float Threshold = 0.3f;
    public GameObject NotePrefab;
    public GameObject TimeLine;

    private int _notesCount = 3;
    private float _speed = 1.5f;
    private IList<GameObject> _notes = new List<GameObject>();
    private IList<float> _notePositions;
    private int _currentNote;
    private int _hits;
    private int _crits;

    private Vector2 _initialTimeLinePosition;
    private readonly Vector2 _topLeftCorner = new(-0.8f, 0.48f);
    private readonly Vector2 _bottomRightCorner = new(1.2f, -0.2f);

    AgentStatusEffects _statusEffects;

    float _intoxicatedSpeed;
    float _intoxicatedAccel;
    float _intoxicatedChangeCountdown;

    private bool _spinning;
    
    public SpinnerType SpinnerType => SpinnerType.Music;

    public void UpdateConfig(ISpinnerConfiguration config)
    {
        var conf = config as SpinnerMusicConfiguration;
        TimeLine.transform.position = _initialTimeLinePosition;
        _notesCount = conf.Notes;
        _spinning = false;
        foreach (var note in _notes)
            Destroy(note);
        _notes = new List<GameObject>();
        _notePositions = new List<float>();
        _currentNote = 0;
        _hits = 0;
        _crits = 0;

        for (var i = 0; i < _notesCount; i++)
        {
            var note = Instantiate(NotePrefab, gameObject.transform);

            var translation = new Vector2(
                _topLeftCorner.x + ((_bottomRightCorner.x - _topLeftCorner.x) / (_notesCount - 1)) * i,
                _bottomRightCorner.y + ((_topLeftCorner.y - _bottomRightCorner.y) / 3) * UnityEngine.Random.Range(0, 4)
            );

            note.transform.localPosition = translation;
            note.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
            _notes.Add(note);
            _notePositions.Add(note.transform.position.x);
        }
    }

    public void StartSpinning(AgentStatusEffects statusEffects)
    {
        _statusEffects = statusEffects;
        
        if(_statusEffects.HasFlag(AgentStatusEffects.Distracted) && !_statusEffects.HasFlag(AgentStatusEffects.Focused))
            _speed = 2.5f;
        else if (_statusEffects.HasFlag(AgentStatusEffects.Focused) &&
                 !_statusEffects.HasFlag(AgentStatusEffects.Distracted))
            _speed = 1f;
        else
            _speed = 1.5f;
        
        StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        yield return new WaitForSeconds(0.5f);
        _spinning = true;
    }

    public SpinnerResult StopSpinning()
    {
        var timelineX = TimeLine.transform.position.x;
        
        if (Math.Abs(timelineX - _notePositions[_currentNote]) < Threshold / 2)
        {
            _notes[_currentNote].GetComponent<SpriteRenderer>().color = Color.yellow;
            _currentNote++;
            _hits++;
            _crits++;
        } else if (Math.Abs(timelineX - _notePositions[_currentNote]) < Threshold)
        {
            _notes[_currentNote].GetComponent<SpriteRenderer>().color = Color.green;
            _currentNote++;
            _hits++;
        } else if (Math.Abs(timelineX - _notePositions[_currentNote]) > Threshold)
        {
            _notes[_currentNote].GetComponent<SpriteRenderer>().color = Color.red;
            _currentNote++;
        }
        
        if (_currentNote < _notesCount) return SpinnerResult.StillSpinning;
        if (_crits == _notesCount) return SpinnerResult.Crit;
        if (_hits == _notesCount) return SpinnerResult.Hit;
        return SpinnerResult.Miss;
    }

    public bool IsSpinning()
    {
        return _spinning;
    }

    public void Start()
    {
        _initialTimeLinePosition = TimeLine.transform.position;
    }

    public void Update()
    {
        if (!_spinning)
            return;
        
        var timelineX = TimeLine.transform.position.x;

        if (timelineX >= _bottomRightCorner.x + 1f)
            return;

        if (_statusEffects.HasFlag(AgentStatusEffects.Intoxicated))
        {
            _intoxicatedSpeed += _intoxicatedAccel * Time.deltaTime;
            _intoxicatedChangeCountdown -= Time.deltaTime;
            if (_intoxicatedChangeCountdown <= 0)
                SpinIntoxicationWheel();
            
            Debug.Log(_intoxicatedSpeed);
            
            TimeLine.transform.position += Vector3.right * (_intoxicatedSpeed * Time.deltaTime);
        }
        
        TimeLine.transform.position += Vector3.right * (_speed * Time.deltaTime);

        if (_currentNote >= _notesCount)
            return;

        if (timelineX > _notePositions[_currentNote] + Threshold)
        {
            _notes[_currentNote].GetComponent<SpriteRenderer>().color = Color.red;
            _currentNote++;
            return;
        }

        if (timelineX > _notePositions[_currentNote] - Threshold)
        {
            _notes[_currentNote].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    
    void SpinIntoxicationWheel()
    {
        _intoxicatedAccel = Random.Range(5f, 5f);//Random.Range(5f, 5f);
        if(UnityEngine.Random.Range(0, 2) == 0)
            _intoxicatedAccel *= -1;
        if(_intoxicatedSpeed >= 30f)
            _intoxicatedAccel = -Math.Abs(_intoxicatedAccel);
        if(_intoxicatedSpeed <= -_speed)
            _intoxicatedAccel = Math.Abs(_intoxicatedAccel);
        _intoxicatedChangeCountdown = 0.1f;
    }
}