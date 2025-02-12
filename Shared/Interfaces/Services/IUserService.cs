using MagicOnion;
using MessagePack;
using RialTimeServer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.Services
{
    public interface IUserService:IService<IUserService>
    {
        // ユーザー登録API
        UnaryResult<int> RegistUserAsync();

        // id指定でユーザー情報を取得するAPI
        UnaryResult<User> GetUserAsync(int userId);
    }
}
