using MagicOnion;
using MagicOnion.Server;
using RialTimeServer.Model.Context;
using RialTimeServer.Model.Entity;
using Shared.Interfaces.Services;

namespace RialTimeServer.StreamingHubs
{
    public class UserService : ServiceBase<IUserService>, IUserService
    {
        public async UnaryResult<int> RegistUserAsync()
        {
            using var context = new GameDbContext();

            //バリデーションチェック
            /*if(context.Users.Where(user => user.Name == name).Count() > 0)
            {
                throw new ReturnStatusException(Grpc.Core.StatusCode.InvalidArgument,"エラーメッセージを書く");
            }*/

            // テーブルにレコード追加
            User user = new User();
            user.Token = "";
            user.Name = Guid.NewGuid().ToString();
            user.Created_at = DateTime.Now;
            user.Updated_at = DateTime.Now;
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.Id;
        }

        public async UnaryResult<User> GetUserAsync(int userId)
        {
            using var context = new GameDbContext();

            User user = context.Users.Where(user => user.Id == userId).First();

            return user;
        }
    }
}
