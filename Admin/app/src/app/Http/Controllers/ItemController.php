<?php

namespace App\Http\Controllers;

use App\Models\Item;
use Illuminate\Http\Request;

class ItemController extends Controller
{
    // アイテム一覧表示処理
    public function showItem(Request $request)
    {
        // sessionにloginが含まれていたら
        if ($request->session()->exists('login')) {

            $items = Item::all();

            // データを渡してviewを表示
            return view('items/index', ['items' => $items]);
        } else {
            // 含まれていなかったらログイン画面を表示
            return redirect('/');
        }
    }
}
