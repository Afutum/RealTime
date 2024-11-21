<?php

namespace App\Http\Controllers;

use App\Models\Item;
use App\Models\Mail;
use App\Models\User;
use App\Models\userMail;
use Barryvdh\Debugbar\Facades\Debugbar;
use Faker\Core\Number;
use Illuminate\Http\Request;
use function Laravel\Prompts\select;

class MailController extends Controller
{
    // メール一覧表示
    public function index()
    {
        $mail = Mail::select([
            'mails.id',
            'text',
            'items.item_name as item_name',
            'mails.updated_at',
            'mails.created_at'
        ])
            ->Leftjoin('items', 'items.id', '=', 'mails.item_id')
            ->get();

        return view('mails.index', ['mails' => $mail]);
    }

    // 送信処理
    public function createMail(Request $request)
    {
        if (!User::where('id', '=', $request['id'])->exists()) {
            return redirect()->route('disCreateMail');
        }

        Mail::create([
            'text' => $request['text'],
            'item_id' => $request['item_id'],
            'item_cnt' => $request['cnt']
        ]);

        userMail::create([
            'mail_id' => Mail::max('id'),
            'user_id' => $request['id'],
            'is_received' => 0
        ]);

        return redirect()->route('disCreateMail', ['success' => 'valid']);
    }

    // 送信ページ表示
    public function disCreateMail(Request $request)
    {
        $item = Item::all();

        return view('mails.create', ['items' => $item, 'success' => $request['success']]);
    }
}
