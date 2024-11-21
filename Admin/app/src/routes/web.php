<?php

use App\Http\Controllers\AccountController;
use App\Http\Controllers\AuthController;
use App\Http\Controllers\UserController;
use App\Http\Controllers\ItemController;
use App\Http\Controllers\PersonalItemController;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\RegAccountController;
use App\Http\Middleware\AuthMiddleware;
use App\Http\Middleware\NoCacheMiddleware;
use App\Http\Controllers\MailController;
use App\Http\Controllers\FriendController;

Route::middleware(NoCacheMiddleware::class)->group(function () {
// ログイン画面
    Route::get('/', [AuthController::class, 'login'])->name('login');

// ログイン処理
    Route::post('auths/dologin', [AuthController::class, 'dologin'])->name('dologin');

// ログアウト処理
    Route::get('auths/dologout', [AuthController::class, 'dologout'])->name('dologout');

// プレイヤー一覧表示処理
    Route::get('accounts/showPlayer', [UserController::class, 'showPlayer'])->name('user.showPlayer');

// アイテム一覧表示処理
    Route::get('accounts/showItem', [ItemController::class, 'showItem'])->name('item.showItem');

// アイテム所持数一覧表示処理
    Route::get('accounts/showPersonalItem',
        [PersonalItemController::class, 'showPersonalItem'])->name('personalItem.showPersonalItem');

// 削除確認画面表示処理
    Route::post('accounts/confDestroy', [AccountController::class, 'confDestroy'])->name('accounts.confDestroy');

// 削除完了画面表示処理
    Route::post('accounts/successDestroy',
        [AccountController::class, 'successDestroy'])->name('accounts.successDestroy');

    Route::get('Mail/index', [MailController::class, 'index'])->name('mail.index');

    Route::get('User/showUserMail', [UserController::class, 'showUserMail'])->name('showUserMail');

    Route::post('Mail/createMail', [MailController::class, 'createMail'])->name('createMail');

    Route::get('Mail/disCreateMail', [MailController::class, 'disCreateMail'])->name('disCreateMail');

    Route::get('Follow/index', [FriendController::class, 'index'])->name('Follow.index');

// ユーザー一覧
    Route::get('accounts/index/{account_id?}', [AccountController::class, 'index'])->name('accounts.index');

    Route::prefix('accounts')->name('accounts.')->controller(AccountController::class)
        ->middleware(AuthMiddleware::class)->group(function () {
            Route::get('/', 'index')->name('index');                              // accounts.index   　 一覧表示画面
            Route::get('create', 'create')->name('create');                       // accounts.create  　 登録画面
            Route::post('store', 'store')->name('store');                         // accounts.store   　 登録処理
            Route::post('destroy', 'destroy')->name('destroy');                   // accounts.destroy 　 削除処理
            Route::post('disPassUpdate', 'disPassUpdate')->name('disPassUpdate'); // accounts.update     更新情報入力画面
            Route::post('passUpdate', 'passUpdate')->name('passUpdate');          // accounts.passUpdate 更新処理
            Route::post('deleteCansel', 'deleteCansel')->name('deleteCansel');
        });
});
