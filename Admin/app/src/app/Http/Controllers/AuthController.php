<?php

namespace App\Http\Controllers;

use App\Models\Account;
use Barryvdh\Debugbar\Facades\Debugbar;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Hash;
use PHPUnit\Framework\Constraint\Count;

class AuthController extends Controller
{
    // ログイン画面表示
    public function login(Request $request)
    {
        // errorかnullを返す
        return view('accounts/login', ['error' => $request['error'] ?? null]);
    }

    // ログイン処理
    public function dologin(Request $request)
    {
        $validated = $request->validate([
            'name' => ['required', 'min:4', 'max:20'],
            'password' => ['required']
        ]);

        // 入力された名前を取得
        $accounts = Account::where('name', '=', $request['name'])->get();
        if ($accounts->count() > 0) {
            // ログインした名前をセッションに保存
            $request->session()->put('name', $request['name']);
            // ハッシュ化されたパスワードと入力されたパスワードがあっているか
            if (Hash::check($request['password'], $accounts[0]['password'])) {
                // 名前とパスが一致している場合
                // セッションに指定のキーで値を保存
                $request->session()->put('login', true);

                // ユーザー一覧にリダイレクトする
                return redirect()->route('accounts.index');
            }
        }

        return redirect()->route('login', ['error' => 'invalid']);
    }

    // ログアウト処理
    public function dologout(Request $request)
    {
        // セッションから指定したデータを削除
        $request->session()->forget('login');
        $request->session()->forget('loginName');

        // ログイン画面にリダイレクトする
        return view('accounts/login');
    }
}
