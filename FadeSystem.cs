using System.Collections;
using UnityEngine;

public class FadeSystem : MonoBehaviour
{
    //inspectorで書き込み　変数設定
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeSpeed = 2f;
    　//変数作成
    private System.Action _fadeEndCallback;

    public void FadeOut(System.Action fadeEndCallback)
    {
        _fadeEndCallback = fadeEndCallback;
        StartCoroutine(FadeOutCo());
    }

    private IEnumerator FadeOutCo()
    {
        //キャンバスグループのalpha値を0に設定
        _canvasGroup.alpha = 0;
        //キャンバスグループのalpha値が1以下の場合処理
        while (1f > _canvasGroup.alpha)
        {
            //Time.delotaTime=前回のUpdateから何秒経過したか
            //キャンバスグループのalpha値を増加させていく
            _canvasGroup.alpha += Time.deltaTime * _fadeSpeed;
            //キャンバスグループの値が1以上もしくは、1になった時
            if (1f < _canvasGroup.alpha) _canvasGroup.alpha = 1f;
            //処理が終わったらコルーチンから外れる
            yield return null;
        }

        if (_fadeEndCallback != null)
            _fadeEndCallback();
    }
}