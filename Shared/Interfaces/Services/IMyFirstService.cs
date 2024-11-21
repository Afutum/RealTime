using MagicOnion;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.Services
{
    public interface IMyFirstService:IService<IMyFirstService>
    {
        //[ここにどのようなAPIを作るのか、関数形式で定義を作成する]

        /// <summary>
        /// 『足し算API』二つの整数を引数で受け取り合計値を返す
        /// </summary>
        /// <param name="x">足す数</param>
        /// <param name="y">足される数</param>
        /// <returns>xとyの合計値 result</returns>
        UnaryResult<int> SumAsync(int x,int y);
        /// <summary>
        /// 受け取った配列の合計を返す
        /// </summary>
        /// <param name="numList">計算したい数の配列</param>
        /// <returns>配列の合計値 result</returns>
        UnaryResult<int> SumAllAsync(int[] numList);
        /// <summary>
        /// x+yを[0]に、x-yを[1]に、x*yを[2]に、x/yを[3]に入れて配列で返す
        /// </summary>
        /// <param name="x">足す、引く、かける、割る数</param>
        /// <param name="y">足す、引く、かける、割られる数</param>
        /// <returns>xとyの合計値 result</returns>
        UnaryResult<int[]> CalcForOperationAsync(int x, int y);
        /// <summary>
        /// xとyの小数をフィールドに持つNumberクラスに渡して、x+yの結果を返す
        /// </summary>
        /// <param name="numArray">小数のフィールド</param>
        /// <returns></returns>
        UnaryResult<float> SumAllNumberAsync(Number numArray);
    }

    [MessagePackObject]
    public class Number
    {
        [Key(0)]
        public float x;
        [Key(1)]
        public float y;
    }
}
