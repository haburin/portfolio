using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class MovieViewType : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private System.Action _movieEndCallback = default;
    private bool _isStart = false;
    public void ShowMovie(System.Action movieEndCallback)
    {
        _movieEndCallback = movieEndCallback;
        StartCoroutine(DoMovie());
    }

    private void Update()
    {
        if(_isStart)
        {
            if(_videoPlayer.isPlaying == false)
            {
                if(null != _movieEndCallback)
                {
                    _movieEndCallback();
                }
            }
        }
    }

    private IEnumerator DoMovie()
    {
        yield return new WaitForSeconds(3f);
        if(null != _movieEndCallback)
        {
            _movieEndCallback();
        }
    }
}