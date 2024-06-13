using System.Collections;
using System.Collections.Generic;
using TikTokLiveUnity;
using UnityEngine;

public class Tiktok : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var userName = "mypias";

        TikTokLiveManager.Instance.OnLike += (liveClient, likeEvent) =>
        {   
            Debug.Log(message:$"Thank you likes! {likeEvent.Count} {likeEvent.Sender.NickName}");
        };

        await TikTokLiveManager.Instance.ConnectToStream(userName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
