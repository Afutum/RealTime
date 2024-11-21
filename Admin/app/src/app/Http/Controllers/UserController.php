<?php

namespace App\Http\Controllers;

use App\Models\Account;
use App\Models\User;
use App\Models\userMail;
use Barryvdh\Debugbar\Facades\Debugbar;
use Barryvdh\Debugbar\Twig\Extension\Debug;
use Illuminate\Http\Request;

class UserController extends Controller
{
    // プレイヤー一覧表示処理
    public function showPlayer(Request $request)
    {
        // テーブルからプレイヤー情報をすべて取得
        //$players = User::all();

        $players = User::paginate(10);

        //DebugBar::error('エラーだよ');

        // sessionにloginが含まれていたら
        if ($request->session()->exists('login')) {
            // プレイヤーの情報を渡してviewを表示
            return view('users/index', ['players' => $players]);
        } else {
            // 含まれていなかったらログイン画面を表示
            return redirect('/');
        }
    }

    // ユーザーメール一覧を表示
    public function showUserMail()
    {
        $userMails = userMail::select([
            'user_mails.id',
            'mail_id',
            'users.name as user_name',
            'is_received',
            'user_mails.created_at',
            'user_mails.updated_at'
        ])
            ->join('users', 'users.id', '=', 'user_mails.user_id')
            ->get();

        return view('users.userMail', ['userMails' => $userMails]);
    }
}
