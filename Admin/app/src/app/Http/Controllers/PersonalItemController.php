<?php

namespace App\Http\Controllers;

use App\Models\PersonalItem;
use Illuminate\Http\Request;

class PersonalItemController extends Controller
{
    public function showPersonalItem(Request $request)
    {
        $personalItem = PersonalItem::select([
            'personal_items.id as id',
            'users.name as user_name',
            'items.item_name as item_name',
            'personal_item'
        ])
            ->join('users', 'users.id', '=', 'personal_items.user_id')
            ->join('items', 'items.id', '=', 'personal_items.item_id')
            ->get();

        // sessionにloginが含まれていたら
        if ($request->session()->exists('login')) {
            // 所持数一覧を表示
            return view('PersonalItems/index', ['items' => $personalItem]);
        } else {
            // 含まれていない場合
            // ログイン画面にリダイレクトする
            return redirect('/');
        }
    }
}
