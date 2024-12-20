using Cysharp.Net.Http;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MyFirstModel : MonoBehaviour
{
    const string ServerURL = "http://localhost:7000";

    async void Start()
    {
        /*int[] numList = new int[2];
        numList[0] = 100;
        numList[1] = 323;
        int result = await SumAll(numList);*/

        /*int[] result = await Allcalculation(323, 100);
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(result[i]);
        }*/

        Number number = new Number();

        number.x = 100; number.y = 100;

        float result = await SumAllNum(number);

        Debug.Log(result);
    }

    public async UniTask<int> Sum(int x,int y)
    {
        using var handler = new YetAnotherHttpHandler() {Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL,new GrpcChannelOptions() { HttpHandler = handler});
        var client = MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await client.SumAsync(x, y);
        return result;
    }

    public async UniTask<int> SumAll(int[] numList)
    {
        using var handler = new YetAnotherHttpHandler() { Http2Only = true};
        var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler});
        var client = MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await client.SumAllAsync(numList);
        return result;
    }

    public async UniTask<int[]> Allcalculation(int x,int y)
    {
        using var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        var client = MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await client.CalcForOperationAsync(x,y);
        return result;
    }

    public async UniTask<float> SumAllNum(Number numArray)
    {
        using var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        var client = MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await client.SumAllNumberAsync(numArray);
        return result;
    }
}
