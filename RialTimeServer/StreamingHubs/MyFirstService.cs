using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using MagicOnion;
using MagicOnion.Server;
using Shared.Interfaces.Services;

namespace Shared.Interfaces.StreamingHubs
{
    public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
    {
        // 『足し算API』二つの整数を引数で受け取り合計値を返す
        public async UnaryResult<int> SumAsync(int x,int y)
        {
            Console.WriteLine("Received:" + x + "," +  y);
            return x + y;
        }

        public async UnaryResult<int> SumAllAsync(int[] numList)
        {
            int sum = 0;

            for (int i = 0; i < numList.Length; i++)
            {
                Console.WriteLine("Received:" + numList[i]);
                sum += numList[i];
            }

            return sum;
        }

        public async UnaryResult<int[]> CalcForOperationAsync(int x, int y)
        {
            int[] numList = new int[4];

            Console.WriteLine("Received:" + x +"," + y);

            numList[0] = x + y;
            numList[1] = x - y;
            numList[2] = x * y;
            numList[3] = x / y;

            return numList;
        }

        public async UnaryResult<float> SumAllNumberAsync(Number numArray)
        {
            Console.WriteLine("Received:" + numArray.x + ',' + numArray.y);
            return numArray.x + numArray.y;
        }
    }
}
