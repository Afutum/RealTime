<?php

namespace App\Http\Controllers;

use App\Models\Account;
use App\Models\User;
use Barryvdh\Debugbar\Facades\Debugbar;
use Barryvdh\Debugbar\Twig\Extension\Debug;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Hash;
use function PHPUnit\Framework\returnArgument;

class AccountController extends Controller
{
    // アカウント一覧を表示する
    public function index(Request $request)
    {
        // テーブルからアカウント情報をすべて取得
        //$accounts = Account::All();

        $accounts = Account::paginate(3);
        // ログインしている名前をセッションから取得
        $loginName = $request->session()->get('name');

        // アカウント一覧を表示
        return view('accounts.index', ['accounts' => $accounts, 'error' => null, 'loginName' => $loginName]);

        //dd($request->account_id);

        /*DebugBar::info('あいうえお');
        DebugBar::error('エラーだよ');

        // セッションに指定のキーで値を保存
        $request->session()->put('aaa', 5);

        // セッションから指定のキーの値を取得
        $value = $request->session()->get('aaa');

        // セッションから指定したデータを削除
        //$request->session()->forget('aaa');

        // セッションの全てのデータを削除
        $request->session()->flush();*/

        // セッションに指定したキーが存在するか
        /*if ($request->session()->exists('aaa')) {
            var_dump($request['aaa']);
        }*/

        //$title = 'アカウント一覧';

        // カンマ区切りで、複数のパラメータを渡せる
        //return view('accounts/index', ['title' => $title, 'accounts' => $data]);
    }

    // アカウント作成処理
    public function store(Request $request)
    {
        // パスワードが一致しているかどうか
        $request->validate([
            'name' => ['required', 'min:4', 'max:20'],
            'password' => ['required', 'confirmed']
        ]);

        // 同じ名前があるかどうか
        //$isAccountExist = Account::where('name', '=', $request['name'])->exists();

        // 同じ名前がなかった場合
        /*if (!$isAccountExist) {
            // レコードを追加
            Account::create(['name' => $request['name'], 'password' => Hash::make($request['password'])]);

            // 成功時用
            return redirect()->route('accounts.create',
                ['success' => $request['name'] . 'を登録しました']);
        }*/

        $user = User::create(
            [
                'name' => $request['name'],
                'level' => 1,
                'exp' => 0,
                'life' => 5
            ]
        );

        return response()->json(['user_id' => $user->id]);

        // 失敗時用
        //return redirect()->route('accounts.create', ['error' => 'invalid']);
    }

    // アカウント作成画面表示処理
    public function create(Request $request)
    {
        // nullか$requestの中身を返す
        return view('RegAccounts/index',
            ['success' => $request['success'] ?? null, 'error' => $request['error'] ?? null]);
    }

    // 削除処理
    public function destroy(Request $request)
    {
        // idを取得してそのidの情報を削除する
        $accounts = Account::findOrFail($request['id']);
        $accounts->delete();

        $accounts = Account::paginate(3);

        // アカウント一覧を表示
        return view('accounts.index', ['accounts' => $accounts, 'error' => null]);
    }

    // 削除確認画面表示処理
    public function confDestroy(Request $request)
    {
        $accounts = Account::all();
        if (count($accounts) <= 1) {
            $error = 'アカウントは最低1つ必要です';
            $accounts = Account::paginate(3);
            return view('accounts.index', ['accounts' => $accounts, 'error' => $error]);
        }

        $accounts = Account::findOrFail($request['id']);

        // 削除確認画面を表示
        return view('destroy.index', ['account' => $accounts]);
    }

    // 削除完了画面表示処理
    public function successDestroy(Request $request)
    {
        $accounts = Account::findOrFail($request['id']);

        // 削除完了画面を表示
        return view('destroy.successDestroy', ['account' => $accounts]);
    }

    // パスワード更新処理
    public function passUpdate(Request $request)
    {
        // パスワードが一致しているかどうか
        $request->validate([
            'password' => ['required', 'confirmed']
        ]);

        // idを取得してそのidの情報を更新する
        $accounts = Account::findOrFail($request['id']);
        $accounts['password'] = Hash::make($request['password']);
        $accounts->save();

        // アカウント一覧を表示
        return view('updateAccounts.successUpdate', ['account' => $accounts]);
    }

    // 情報更新入力画面表示処理
    public function disPassUpdate(Request $request)
    {
        $accounts = Account::findOrFail($request['id']);

        // 更新入力画面を表示
        return view('updateAccounts.index', ['account' => $accounts, 'error' => null]);
    }

    public function deleteCansel(Request $request)
    {
        $accounts = Account::paginate(3);

        // アカウント一覧を表示
        return view('accounts.index', ['accounts' => $accounts, 'error' => null]);
    }
}
